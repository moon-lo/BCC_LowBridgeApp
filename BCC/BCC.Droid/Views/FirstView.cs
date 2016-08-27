using System;
using Android.App;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using MvvmCross.Droid.Views;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Views;
using Android.Widget;

namespace BCC.Droid.Views
{
    /// <summary>
    /// author: Michael Devenish
    /// </summary>
    [Activity(Label = "View for FirstViewModel")]
    public class FirstView : MvxActivity, ILocationListener, IOnMapReadyCallback
    {

        //todo
        //when user scrolls, detect scroll, disable _locationManager.RemoveUpdates(this); and change the icon
        //when user presses center button call _locationManager.RequestLocationUpdates(_locationProvider, 500, 0, this); and move to last known location
        //save and load from previous location

        private LocationManager _locationManager;
        private string _locationProvider;
        private Marker marker = null;

        public GoogleMap Map { get; private set; }
        public event EventHandler MapReady;
        private string test = "test";

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

            //get the map if ready
            this.MapReady += (sender, args) =>
            {
                map = Map;//receive the Map object

                CameraUpdate cameraUpdate = GetNewCameraPosition(location);
                marker = SetupMarker(location, map, marker);

                if (map != null) map.MoveCamera(cameraUpdate);
            };
            //call the above code when map ready
            frag.GetMapAsync(this);

        }

        /// <summary>
        /// places a marker on the supplied map at the supplied location, it removes old marker if one is supplied
        /// </summary>
        /// <param name="location">the location of the marker</param>
        /// <param name="map">the map it is being placed on</param>
        /// <param name="marker">the old marker</param>
        /// <returns>the new marker</returns>
        private Marker SetupMarker(Location location, GoogleMap map, Marker marker)
        {
            //remove the old marker
            if (marker != null)
                marker.Remove();

            //set up and place a marker on the new location
            MarkerOptions userMarker = new MarkerOptions();
            userMarker.SetPosition(new LatLng(location.Latitude, location.Longitude));
            userMarker.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));
            userMarker.SetTitle("Your Location");
            return map.AddMarker(userMarker);
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



        public void OnMapReady(GoogleMap googleMap)
        {
            Map = googleMap;
            var handler = MapReady;
            Map.UiSettings.MapToolbarEnabled = false;
            Map.UiSettings.CompassEnabled = false;

            googleMap.CameraChange += (sender, e) => {
                Map.UiSettings.CompassEnabled = false;
                Toast.MakeText(this, test, ToastLength.Short).Show();
                
                //we now have a way to detect when the map has been moved, now to check if it was done by the software or human
                //then disable the required stuff
            };

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
        

        //unused
        public void OnProviderDisabled(string provider) { }
        public void OnProviderEnabled(string provider) { }
        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras) { }

        #endregion
        /// <summary>
        /// the function that handles the creation of the view
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ActionBar.Hide();
            SetContentView(Resource.Layout.FirstView);
            _locationManager = (LocationManager)GetSystemService(LocationService);

            var frag = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map);
        }

        /// <summary>
        /// when the app resumes it sets up all the map things
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();

            Criteria locationCriteria = new Criteria();

            locationCriteria.Accuracy = Accuracy.Coarse;
            locationCriteria.PowerRequirement = Power.Medium;

            _locationProvider = _locationManager.GetBestProvider(locationCriteria, true);
            _locationManager.RequestLocationUpdates(_locationProvider, 100, 0, this);
        }

        /// <summary>
        /// pauses all of the map things
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }
    }
}
