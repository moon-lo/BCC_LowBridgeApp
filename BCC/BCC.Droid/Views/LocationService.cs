using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Java.Lang;
using Android.Locations;
using BCC.Core.json;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;

namespace BCC.Droid.Views
{
    [Service]
    public class LocationService : Service, ILocationListener
    {
        LocationServiceBinder binder;
        public Location currentLocation;
        private LocationManager _locationManager;
        private string _locationProvider;
        public bool inForeground = true;
        private bool showingActiveAlert = false;
        private List<BridgeData> bridges;
        private List<BridgeData> notifiedBridges = new List<BridgeData>();//the bridges that the user is close to
        private Notification.Builder awayNotification;
        const int notificationId = 0;
        private bool awayNotificationShown = false;

        static string[] PERMISSIONS_LOCATION = { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation };
        const int RequestLocationId = 0;

        //have a global for the push notification
        //modify the push when you are close to a bridge if it is already shown
        //if it is not shown create one
        //

        public List<BridgeData> Bridges { get { return bridges; } set { bridges = value; } }
        public Location CurrentLocation { get { return currentLocation; } set { currentLocation = value; } }
        public LocationManager LocationManager { get { return _locationManager; } }
        public string LocationProvider { get { return _locationProvider; } }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            return StartCommandResult.Sticky;
        }

        /// <summary>
        /// Sets up all of the required functions and returns a binder
        /// </summary>
        /// <param name="intent"></param>
        /// <returns>the locationservicebinder</returns>
        public override IBinder OnBind(Intent intent)
        {
            binder = new LocationServiceBinder(this);

            SetupWarningPushNotification();

            return binder;

        }

        /// <summary>
        /// Sets up the required objects to follow the user
        /// </summary>
        public void SetupLocationTracking()
        {

            _locationManager = (LocationManager)GetSystemService(LocationService);

            _locationProvider = _locationManager.GetBestProvider(RequestLocation(), true);
            _locationManager.RequestLocationUpdates(_locationProvider, 100, 1, this);
        }

        private Criteria RequestLocation()
        {
            Criteria locationCriteria = new Criteria();
            if ((int)Build.VERSION.SdkInt < 23)
                locationCriteria = SetAccurate(locationCriteria, _locationManager.IsProviderEnabled(LocationManager.GpsProvider));

            else
            {
                //Check to see if any permission in our group is available, if one, then all are
                const string permission = Manifest.Permission.AccessFineLocation;
                if (CheckSelfPermission(permission) == (int)Permission.Granted)
                    locationCriteria = SetAccurate(locationCriteria, _locationManager.IsProviderEnabled(LocationManager.GpsProvider));
                else
                {
                    ActivityCompat.RequestPermissions(binder.activity, PERMISSIONS_LOCATION, RequestLocationId);
                    locationCriteria = SetAccurate(locationCriteria, CheckSelfPermission(permission) == (int)Permission.Granted
                        && _locationManager.IsProviderEnabled(LocationManager.GpsProvider));
                }
            }
            return locationCriteria;
        }

        private Criteria SetAccurate(Criteria locationCriteria, bool avaliable)
        {
            if (avaliable)
            {
                locationCriteria.Accuracy = Accuracy.Coarse;
                locationCriteria.PowerRequirement = Power.Low;
            }
            else
            {
                locationCriteria.Accuracy = Accuracy.Coarse;
                locationCriteria.PowerRequirement = Power.Low;
            }
            return locationCriteria;
        }

        /// <summary>
        /// Sets up a push notification that is used to warn the user when they are close
        /// to a dangerous bridge
        /// </summary>
        private void SetupWarningPushNotification()
        {
            Intent resultIntent = new Intent(this, typeof(FirstView));

            Android.App.TaskStackBuilder stackBuilder = Android.App.TaskStackBuilder.Create(this);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(FirstView)));
            stackBuilder.AddNextIntent(resultIntent);

            PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);


            awayNotification = new Notification.Builder(this)
                        .SetContentTitle("Approaching dangerous bridge")
                        .SetContentIntent(resultPendingIntent)
                        .SetContentText("Hello World! This is my first notification!")
                        .SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
                        .SetSmallIcon(Resource.Drawable.cars)
                        .SetPriority(2);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            //close location service
        }

        /// <summary>
        /// When the users location is changed this is called
        /// </summary>
        /// <param name="location">the users location</param>
        public void OnLocationChanged(Location location)
        {
            CurrentLocation = location;
            List<BridgeData> currentNotifiedBridges = new List<BridgeData>(notifiedBridges);
            bool warning = true;
            notifiedBridges = new List<BridgeData>();

            warning = CalculateDistanceFromBridges(location, warning, notifiedBridges);
            if (inForeground)
            {
                if (warning && !showingActiveAlert)
                    ShowActiveWarning();
                binder.activity.updateMap(location);

            }
            else if (warning && (!awayNotificationShown || !currentNotifiedBridges.All(notifiedBridges.Contains)))
                ShowBackgroundWarning();
        }

        /// <summary>
        /// sends a push notification telling the user they are close to a bridge 
        /// </summary>
        private void ShowBackgroundWarning()
        {
            awayNotificationShown = true;
            string result = "The bridges you are close to are:\n";
            foreach (BridgeData bridge in notifiedBridges)
                result += bridge.Description + "\n";

            awayNotification.SetContentText(result);
            // Publish the notification
            (GetSystemService(Context.NotificationService) as NotificationManager).Notify(notificationId, awayNotification.Build());
        }

        /// <summary>
        /// sends a popup notification telling the user they are close to a bridge 
        /// </summary>
        private void ShowActiveWarning()
        {
            showingActiveAlert = true;
            AlertDialog.Builder alert = new AlertDialog.Builder(binder.activity);
            alert.SetTitle("Alert!");
            alert.SetMessage("You are approaching a bridge that is too short for your vehicle!");//change this to mention what bridges
            alert.SetPositiveButton("Ok", (senderAlert, args) =>
            {
                showingActiveAlert = false;
            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }

        /// <summary>
        /// returns true if the user is close to any bridges and places the bridges 
        /// that are close in a list
        /// </summary>
        /// <param name="location">the user location</param>
        /// <param name="warning">the current state of if there is any warnings</param>
        /// <param name="warningLocations">the bridges that are in range</param>
        /// <returns></returns>
        private bool CalculateDistanceFromBridges(Location location, bool warning, List<BridgeData> warningLocations)
        {
            foreach (BridgeData bridge in bridges)
            {
                Location bridgeLoc = new Location("");
                bridgeLoc.Latitude = bridge.Latitude;
                bridgeLoc.Longitude = bridge.Longitude;
                if (location.DistanceTo(bridgeLoc) < 100)
                {
                    warning = true;
                    warningLocations.Add(bridge);
                }
            }

            return warning;
        }
        #region unused
        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    public class LocationServiceBinder : Binder
    {
        public FirstView activity;
        LocationService service;

        public LocationServiceBinder(LocationService mnservice)
        {
            this.service = mnservice;
        }

        public LocationService GetService()
        {
            return service;
        }
        public void Close()
        {
            service.OnDestroy();
        }
    }

}