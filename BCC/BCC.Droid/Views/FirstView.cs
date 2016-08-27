using System;
using Android.App;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using MvvmCross.Droid.Views;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Views;

namespace BCC.Droid.Views
{
    /// <summary>
    /// author: Michael Devenish
    /// </summary>
    [Activity(Label = "View for FirstViewModel")]
    public class FirstView : MvxActivity, ILocationListener
    {

        //todo
        //when user scrolls, detect scroll, disable _locationManager.RemoveUpdates(this); and change the icon
        //when user presses center button call _locationManager.RequestLocationUpdates(_locationProvider, 500, 0, this); and move to last known location
        //save and load from previous location

        private LocationManager _locationManager;
        private string _locationProvider;
        private Marker marker = null;

        #region gps
        /// <summary>
        /// Called when the users location is updated, it moves the user to their new location and places a marker to show them exactly 
        /// where they are
        /// </summary>
        /// <param name="location">the users location</param>
        public void OnLocationChanged(Location location)
        {
            GoogleMap map = null;

            //get the map fragment and setup the checks to see if the map is ready
            var frag = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map);
            var mapReadyCallback = new OnMapReadyClass();

            //get the map if ready
            mapReadyCallback.MapReady += (sender, args) =>
            {
                map = ((OnMapReadyClass)sender).Map;//receive the Map object

                CameraUpdate cameraUpdate = GetNewCameraPosition(location);
                SetupMarker(location, map);

                //set map paramaters
                map.UiSettings.MapToolbarEnabled = false;
                map.UiSettings.CompassEnabled = false;

                //move the camera
                if (map != null)
                    map.MoveCamera(cameraUpdate);

            };
            //call the above code when map ready
            frag.GetMapAsync(mapReadyCallback);

        }

        private void SetupMarker(Location location, GoogleMap map)
        {
            //remove the old marker
            if (marker != null)
                marker.Remove();

            //set up and place a marker on the new location
            MarkerOptions userMarker = new MarkerOptions();
            userMarker.SetPosition(new LatLng(location.Latitude, location.Longitude));
            userMarker.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));
            userMarker.SetTitle("Your Location");
            marker = map.AddMarker(userMarker);
        }

        /// <summary>
        /// uses the suplied location class to create a CameraUpdate class 
        /// </summary>
        /// <param name="location">the location to updfate to</param>
        /// <returns>a new camera status</returns>
        private static CameraUpdate GetNewCameraPosition(Location location)
        {
            LatLng position = new LatLng(location.Latitude, location.Longitude);
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(position);
            builder.Zoom(15);
            builder.Bearing(0);
            builder.Tilt(0);

            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            return cameraUpdate;
        }

        /// <summary>
        /// the class than handles checking when the map is ready
        /// </summary>
        public class OnMapReadyClass : Java.Lang.Object, IOnMapReadyCallback
        {
            public GoogleMap Map { get; private set; }
            public event EventHandler MapReady;

            public void OnMapReady(GoogleMap googleMap)
            {
                Map = googleMap;
                var handler = MapReady;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }

        public void OnProviderDisabled(string provider) { }
        public void OnProviderEnabled(string provider) { }
        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras) { }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ActionBar.Hide();
            SetContentView(Resource.Layout.FirstView);
            _locationManager = (LocationManager)GetSystemService(LocationService);
        }
        protected override void OnResume()
        {

            base.OnResume();

            Criteria locationCriteria = new Criteria();

            locationCriteria.Accuracy = Accuracy.Coarse;
            locationCriteria.PowerRequirement = Power.Medium;

            _locationProvider = _locationManager.GetBestProvider(locationCriteria, true);
            _locationManager.RequestLocationUpdates(_locationProvider, 100, 0, this);



        }
        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }
    }
}
