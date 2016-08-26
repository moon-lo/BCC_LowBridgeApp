using System;
using Android.App;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using MvvmCross.Droid.Views;
using Android.Util;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using System.Collections.Generic;
using System.Linq;

namespace BCC.Droid.Views
{
    [Activity(Label = "View for FirstViewModel")]
    public class FirstView : MvxActivity, ILocationListener
    {
        double _latitude;
        double _longitude;
        LocationManager _locationManager;
        string _locationProvider;

        public void OnLocationChanged(Location location)
        {
            //save and load from previous location
            _latitude = location.Latitude;
            _longitude = location.Longitude;

            LatLng position = new LatLng(_latitude, _longitude);
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(position);
            builder.Zoom(16);
            builder.Bearing(0);
            builder.Tilt(0);
            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            GoogleMap map = null;

            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);

            OnMapReadyClass readyCallback = new OnMapReadyClass();

            readyCallback.MapReadyAction += delegate (GoogleMap googleMap)
            {
                map = googleMap;
            };

            mapFrag.GetMapAsync(readyCallback);
            map = mapFrag.Map;//fix this tempoary

            if (map != null)
            {
                map.MoveCamera(cameraUpdate);
            }

        }

        public class OnMapReadyClass : Java.Lang.Object, IOnMapReadyCallback
        {
            public GoogleMap Map { get; private set; }
            public event Action<GoogleMap> MapReadyAction;

            public void OnMapReady(GoogleMap googleMap)
            {
                Map = googleMap;

                if (MapReadyAction != null)
                    MapReadyAction(Map);
            }
        }

        public void OnProviderDisabled(string provider)
        {

        }

        public void OnProviderEnabled(string provider)
        {

        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {

        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ActionBar.Hide();
            SetContentView(Resource.Layout.FirstView);
            InitializeLocationManager();
        }
        protected override void OnResume()
        {

            base.OnResume();

            Criteria locationCriteria = new Criteria();

            locationCriteria.Accuracy = Accuracy.Coarse;
            locationCriteria.PowerRequirement = Power.Medium;

            _locationProvider = _locationManager.GetBestProvider(locationCriteria, true);
            _locationManager.RequestLocationUpdates(_locationProvider, 500, 0, this);


        }
        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }

        void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
        }

    }
}
