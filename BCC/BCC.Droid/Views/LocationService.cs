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
            //start maps
            return StartCommandResult.Sticky;
        }

        /// <summary>
        /// Sets up location tracking and returns the binder
        /// </summary>
        /// <param name="intent"></param>
        /// <returns>the locationservicebinder</returns>
        public override IBinder OnBind(Intent intent)
        {
            binder = new LocationServiceBinder(this);
            _locationManager = (LocationManager)GetSystemService(LocationService);

            Criteria locationCriteria = new Criteria();
            locationCriteria.Accuracy = Accuracy.Coarse;
            locationCriteria.PowerRequirement = Power.Low;

            _locationProvider = _locationManager.GetBestProvider(locationCriteria, true);
            _locationManager.RequestLocationUpdates(_locationProvider, 100, 0, this);
            return binder;

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
                binder.activity.updateMap(location);

            }
            else
            {
                if (warning)
                {

                    //go through the currentNotifiedBridges and see if it is in notified bridges, 
                    //if it is in both do nothing, if it is in current but not notified remove it from the push
                    //if it is in notified only add it to the push
                }
                Toast.MakeText(this, "closed", ToastLength.Long).Show();
            }
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