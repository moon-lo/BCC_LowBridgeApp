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
using Android.Text;
using Java.Lang;
using MvvmCross.Platform;
using MvvmCross.Binding.Droid.Views;
using BCC.Core.ViewModels;

namespace BCC.Droid.Views
{
    /// <summary>
    /// author: N9452982,  Michael Devenish
    /// </summary>
    [Activity(Label = "View for FirstViewModel")]
    public class FirstView : MvxActivity, ILocationListener, IOnMapReadyCallback, ITextWatcher, IView
    {

        private LocationManager _locationManager;
        private string _locationProvider;
        private Marker marker = null;
        private Marker searchMarker = null;
        public event EventHandler MapReady;
        private bool disablePositioning = false;
        private int softwareUpdate = 1;
        private bool visibleSearch = false;

        public GoogleMap Map { get; private set; }

        #region gps
        /// <summary>
        /// Called when the users location is updated, it moves the user to their new location and places a marker to show them exactly 
        /// where they are
        /// </summary>
        /// <param name="location">the users location</param>
        public void OnLocationChanged(Location location)
        {
            GoogleMap map = null;

            //get the map if ready
            this.MapReady += (sender, args) =>
            {
                map = Map;//receive the Map object
                CameraUpdate cameraUpdate = null;

                if (!disablePositioning)
                {
                    softwareUpdate++;
                    cameraUpdate = GetNewCameraPosition(location);
                }

                marker = SetupMarker(location, map, marker, "Your Location");
                if (map != null && !disablePositioning) map.AnimateCamera(cameraUpdate);
            };
            //call the above code when map ready
            FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);

        }

        /// <summary>
        /// places a marker on the supplied map at the supplied location, it removes old marker if one is supplied
        /// </summary>
        /// <param name="location">the location of the marker</param>
        /// <param name="map">the map it is being placed on</param>
        /// <param name="marker">the old marker</param>
        /// <returns>the new marker</returns>
        private Marker SetupMarker(Location location, GoogleMap map, Marker marker, string title)
        {
            //remove the old marker
            if (marker != null) marker.Remove();

            //set up and place a marker on the new location
            MarkerOptions userMarker = new MarkerOptions();
            userMarker.SetPosition(new LatLng(location.Latitude, location.Longitude));
            userMarker.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));
            userMarker.SetTitle(title);
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


        /// <summary>
        /// handles updating the map when it is ready
        /// </summary>
        /// <param name="googleMap">the map to update</param>
        public void OnMapReady(GoogleMap googleMap)
        {
            Map = googleMap;
            var handler = MapReady;
            Map.CameraChange += (sender, e) =>
            {
                if (softwareUpdate > 0)
                {
                    softwareUpdate--;
                }
                else
                {
                    FindViewById<ImageButton>(Resource.Id.focusButton).SetImageResource(Resource.Drawable.crosshair);
                    disablePositioning = true;
                }
            };
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Initalises all of the things required by the map
        /// </summary>
        private void SetupMap()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);

            GoogleMap map = null;
            this.MapReady += (sender, args) =>
            {
                map = Map;
                Map.UiSettings.MapToolbarEnabled = false;
                Map.UiSettings.CompassEnabled = false;
            };
            //call the above code when map ready
            FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);

            //set event handeler for crosshair button
            FindViewById<ImageButton>(Resource.Id.focusButton).Click += delegate
            {
                disablePositioning = false;
                FindViewById<ImageButton>(Resource.Id.focusButton).SetImageResource(Resource.Drawable.gps_blue);
                if (_locationManager.GetLastKnownLocation(_locationProvider) != null)
                    OnLocationChanged(_locationManager.GetLastKnownLocation(_locationProvider));
            };
        }

        /// <summary>
        /// Deals with resuming all of the stuff required from the map when the app resumes
        /// </summary>
        private void MapResume()
        {
            Criteria locationCriteria = new Criteria();
            locationCriteria.Accuracy = Accuracy.Coarse;
            locationCriteria.PowerRequirement = Power.Low;

            _locationProvider = _locationManager.GetBestProvider(locationCriteria, true);
            _locationManager.RequestLocationUpdates(_locationProvider, 100, 0, this);
        }

        #endregion
        #region searching

        /// <summary>
        /// makes the search results visible when text is entered into search
        /// </summary>
        /// <param name="s"></param>
        public void AfterTextChanged(IEditable s)
        {
            ShowSearch();
        }

        /// <summary>
        /// When back is pressed it hides the results if they are visible before doing anything else
        /// </summary>
        public override void OnBackPressed()
        {
            if (!visibleSearch)
                base.OnBackPressed();
            else
            {
                visibleSearch = false;
                FindViewById<MvxListView>(Resource.Id.searching).Visibility = ViewStates.Invisible;
            }
        }

        /// <summary>
        /// Sets up all of the required things for the search
        /// </summary>
        private void SetupSearch()
        {
            FindViewById<MvxListView>(Resource.Id.searching).BringToFront();
            FindViewById<EditText>(Resource.Id.searchText).AddTextChangedListener(this);
        }

        #endregion
        #region main functions

        /// <summary>
        /// the function that handles the creation of the view
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ActionBar.Hide();
            SetContentView(Resource.Layout.FirstView);

            var viewModel = DataContext as FirstViewModel;
            viewModel.View = this;

            SetupSearch();
            SetupMap();
        }

        /// <summary>
        /// when the app resumes it sets up all the map things
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            MapResume();
        }

        /// <summary>
        /// pauses all of the map things
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }

        public void GoTo(LocationAutoCompleteResult.Result location)
        {
            disablePositioning = true;

            GoogleMap map = null;
            Location tempLocation = new Location("");
            tempLocation.Latitude = location.geometry.location.lat;
            tempLocation.Longitude = location.geometry.location.lng;

            //get the map if ready
            this.MapReady += (sender, args) =>
            {
                map = Map;//receive the Map object
                CameraUpdate cameraUpdate = null;
                cameraUpdate = GetNewCameraPosition(tempLocation);
                searchMarker = SetupMarker(tempLocation, map, searchMarker, location.formatted_address);
                if (map != null) map.AnimateCamera(cameraUpdate);

                visibleSearch = false;
                FindViewById<EditText>(Resource.Id.searchText).ClearFocus();
                FindViewById<MvxListView>(Resource.Id.searching).Visibility = ViewStates.Invisible;
                FindViewById<ImageButton>(Resource.Id.focusButton).SetImageResource(Resource.Drawable.crosshair);
            };
            //call the above code when map ready
            FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
        }

        public void ShowSearch()
        {
            visibleSearch = true;
            FindViewById<MvxListView>(Resource.Id.searching).Visibility = ViewStates.Visible;
        }

        #endregion
        #region unused
        [Obsolete("Method is unused")]
        public void OnProviderDisabled(string provider) { }
        [Obsolete("Method is unused")]
        public void OnProviderEnabled(string provider) { }
        [Obsolete("Method is unused")]
        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras) { }
        [Obsolete("Method is unused")]
        public void BeforeTextChanged(ICharSequence s, int start, int count, int after) { }
        [Obsolete("Method is unused")]
        public void OnTextChanged(ICharSequence s, int start, int before, int count) { }

        #endregion
    }
}
