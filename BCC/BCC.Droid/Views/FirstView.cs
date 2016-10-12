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
using System.IO;
using EmbeddedResources;
using System.Reflection;
using BCC.Core.json;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V4.Content;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.FullFragging.Fragments;
using Android.Support.V4.Widget;
using Android.Support.V7.App;

namespace BCC.Droid.Views
{
    /// <summary>
    /// author: N9452982,  Michael Devenish
    /// </summary>
    [Activity(Label = "View for FirstViewModel")]
    public class FirstView : MvxCachingFragmentCompatActivity<FirstViewModel>, IOnMapReadyCallback, ITextWatcher, IView
    {


        private Location currentLocation = null;
        private Marker marker = null;
        private Marker searchMarker = null;
        private List<Marker> bridgeMarkers;
        public List<BridgeData> bridges;
        public event EventHandler MapReady;

        private bool softwareUpdate = true;
        private bool disablePositioning = false;
        private bool crosshairStatus = true;
        private bool firstUpdate = true;
        private bool visibleSearch = false;

        private double vehicleHeight = 1;
        public bool isBound = false;
        public LocationServiceBinder binder;
        public bool isConfigurationChange = false;
        LocationServiceConnection locationServiceConnection;

        MvxFragment[] fragments = { new SettingsView(), new HelpView(), new AboutView() };
        string[] titles = { "Settings", "Help", "About, terms & privacy" };
        ActionBarDrawerToggle drawerToggle;
        ListView drawerListView;
        DrawerLayout drawerLayout;

        public GoogleMap Map { get; private set; }

        #region gps

        /// <summary>
        /// handles updating the map when it is ready
        /// </summary>
        /// <param name="googleMap">the map to update</param>
        public void OnMapReady(GoogleMap googleMap)
        {
            Map = googleMap;
            var handler = MapReady;
            Map.CameraChange += (sender, e) => UpdateCamera(sender, e);

            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called whenever the camera is modified, Checks if it was done by software or human and modifies internal states 
        /// of the class depending
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateCamera(object sender, GoogleMap.CameraChangeEventArgs e)
        {
            {
                if (softwareUpdate)
                {
                    if (!firstUpdate)
                        softwareUpdate = false;
                    else firstUpdate = false;
                    if (!crosshairStatus)
                    {
                        FindViewById<ImageButton>(Resource.Id.focusButton).SetImageResource(Resource.Drawable.gps_blue);
                        crosshairStatus = true;
                    }
                }
                else
                {
                    FindViewById<ImageButton>(Resource.Id.focusButton).SetImageResource(Resource.Drawable.crosshair);
                    crosshairStatus = false;
                    disablePositioning = true;
                }
            };
        }

        /// <summary>
        /// places a marker on the supplied map at the supplied location, it removes old marker if one is supplied
        /// </summary>
        /// <param name="location">the location of the marker</param>
        /// <param name="map">the map it is being placed on</param>
        /// <param name="marker">the old marker</param>
        /// <returns>the new marker</returns>
        private Marker SetupMarker(Location location, GoogleMap map, Marker marker, string title, string snippet, float color)
        {
            //remove the old marker
            if (marker != null) marker.Remove();

            //set up and place a marker on the new location
            MarkerOptions userMarker = new MarkerOptions();
            userMarker.SetPosition(new LatLng(location.Latitude, location.Longitude));
            userMarker.SetIcon(BitmapDescriptorFactory.DefaultMarker(color));
            userMarker.SetTitle(title);
            if (snippet != "")
                userMarker.SetSnippet(snippet);
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
        /// Generate a location object from latitude and longatude
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lon">longatude</param>
        /// <returns></returns>
        private static Location GenerateLocation(double lat, double lon)
        {
            Location tempLocation = new Location("");
            tempLocation.Latitude = lat;
            tempLocation.Longitude = lon;
            return tempLocation;
        }

        /// <summary>
        /// Updates the map with the supplied location
        /// </summary>
        public void updateMap(Location location)
        {
            binder.GetService().CurrentLocation = location;
            this.MapReady = (sender, args) =>
            {
                CameraUpdate cameraUpdate = null;

                if (!disablePositioning)
                    cameraUpdate = GetNewCameraPosition(location);

                marker = SetupMarker(location, Map, marker, "Your Location", "", BitmapDescriptorFactory.HueCyan);
                if (Map != null && !disablePositioning)
                {
                    softwareUpdate = true;
                    Map.AnimateCamera(cameraUpdate);
                }
            };
            //calls the mapready event
            FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
        }


        /// <summary>
        /// Initalises all of the things required by the map
        /// </summary>
        private void SetupMap(List<BridgeData> bridges)
        {
            this.MapReady = (sender, args) =>
            {
                AddBridgesToBridgeMarkersList(bridges, Map);
                Map.UiSettings.MapToolbarEnabled = false;
                Map.UiSettings.CompassEnabled = false;
            };

            //call the above code when map ready
            FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);

            //set event handeler for crosshair button
            FindViewById<ImageButton>(Resource.Id.focusButton).Click += delegate
            {
                disablePositioning = false;
                if (binder.GetService().LocationManager.GetLastKnownLocation(binder.GetService().LocationProvider) != null)
                    updateMap(binder.GetService().LocationManager.GetLastKnownLocation(binder.GetService().LocationProvider));
            };

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
            var res = Resource.Id.beginning;
            FindViewById<MvxListView>(Resource.Id.searching).BringToFront();
            FindViewById<EditText>(Resource.Id.searchText).AddTextChangedListener(this);
        }

        /// <summary>
        /// places a marker and goes to the supplied location
        /// </summary>
        /// <param name="location"></param>
        public void GoTo(LocationAutoCompleteResult.Result location)
        {
            softwareUpdate = false;
            disablePositioning = true;

            //modifies the mapready event
            this.MapReady = (sender, args) => NavigateTo(location);

            //calls the mapready event
            FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
        }

        /// <summary>
        /// the navigating code for GoTo
        /// </summary>
        /// <param name="location">The Location to navigate to</param>
        /// <returns></returns>
        private GoogleMap NavigateTo(LocationAutoCompleteResult.Result location)
        {
            Location tempLocation = GenerateLocation(location.geometry.location.lat, location.geometry.location.lng);

            CameraUpdate cameraUpdate = null;
            cameraUpdate = GetNewCameraPosition(tempLocation);
            searchMarker = SetupMarker(tempLocation, Map, searchMarker, location.formatted_address, "", BitmapDescriptorFactory.HueCyan);
            if (Map != null) Map.AnimateCamera(cameraUpdate);

            visibleSearch = false;
            FindViewById<EditText>(Resource.Id.searchText).ClearFocus();
            FindViewById<MvxListView>(Resource.Id.searching).Visibility = ViewStates.Invisible;
            return Map;
        }

        /// <summary>
        /// Shows the search box
        /// </summary>
        public void ShowSearch()
        {
            visibleSearch = true;
            FindViewById<MvxListView>(Resource.Id.searching).Visibility = ViewStates.Visible;
        }

        #endregion
        #region bridge markers
        /// <summary>
        /// Adds the items in the supplied list to the map as markers, warning remove all old bridges before calling
        /// </summary>
        /// <param name="bridges">the bridges to add</param>
        /// <param name="map">the map it is being added to</param>
        private void AddBridgesToBridgeMarkersList(List<BridgeData> bridges, GoogleMap map)
        {
            foreach (BridgeData bridge in bridges)
            {
                Location bridgeLocation = GenerateLocation(bridge.Latitude, bridge.Longitude);
                bridgeMarkers.Add(SetupMarker(bridgeLocation, map, null, bridge.Description, "Height:" + bridge.Signed_Clearance + "m", BitmapDescriptorFactory.HueRed));
            }
        }

        /// <summary>
        /// Hides bridges that are taller than a listed ammount from the map
        /// </summary>
        /// <param name="height">the min bridge height</param>
        public void HideBridgesOnHeight(double height)
        {
            if (isBound)
                binder.GetService().Bridges = new List<BridgeData>(bridges);
            for (int i = 0; i < bridgeMarkers.Count; i++)
            {
                if (bridges[i].Signed_Clearance > height)
                {
                    if (isBound)
                        binder.GetService().Bridges.Remove(bridges[i]);
                    bridgeMarkers[i].Visible = false;
                }
                else bridgeMarkers[i].Visible = true;
            }
        }

        /// <summary>
        /// Loads the bridges from a local file
        /// </summary>
        /// <param name="viewModel">the current viewmodel</param>
        private void LoadBridges(FirstViewModel viewModel)
        {
            Stream location = ResourceLoader.GetEmbeddedResourceStream(Assembly.GetAssembly(typeof(ResourceLoader)), "lowBridge_2016-04-06.json");
            bridges = viewModel.GetBridges(location);
        }

        #endregion
        #region nav menu
        public void SetupNavBar()
        {
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            drawerListView = FindViewById<ListView>(Resource.Id.drawerListView);
            drawerListView.ItemClick += (s, e) => ShowFragmentAt(e.Position);
            drawerListView.Adapter = new ArrayAdapter<string>(
                this,
                Android.Resource.Layout.SimpleListItem1,
                titles);

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawerLayout);

            var tm = FragmentManager.BeginTransaction();
            foreach (var item in fragments)
            {
                tm.Add(item, item.ToString());
            }
            ShowFragmentAt(0);
        }

        public void OpenDrawer()
        {
            drawerLayout.OpenDrawer(drawerListView);
        }

        void ShowFragmentAt(int position)
        {
            FragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.frameLayout, fragments[position])
                .Commit();

            Title = titles[position];
            drawerLayout.CloseDrawer(drawerListView);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (drawerToggle.OnOptionsItemSelected(item))
            {
                return true;
            }
            return base.OnOptionsItemSelected(item);
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
            SetContentView(Resource.Layout.FirstView);
            bridgeMarkers = new List<Marker>();

            var viewModel = DataContext as FirstViewModel;
            LoadBridges(viewModel);
            viewModel.View = this;

            SetupSearch();
            SetupMap(bridges);

            drawerListView = FindViewById<ListView>(Resource.Id.drawerListView);
            if (drawerListView != null)
            {
                drawerListView.ItemClick += (s, e) => ShowFragmentAt(e.Position);
                drawerListView.Adapter = new ArrayAdapter<string>(
                    this,
                    Android.Resource.Layout.SimpleListItem1,
                    titles);
            }

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawerLayout);
            drawerToggle = new ActionBarDrawerToggle(
                this,
                drawerLayout,
                Resource.String.OpenDrawerString,
                Resource.String.CloseDrawerString);

            drawerLayout.AddDrawerListener(drawerToggle);
            var tm = FragmentManager.BeginTransaction();
            foreach (var item in fragments)
            {
                tm.Add(item, item.ToString());
            }
            ShowFragmentAt(1);


        }

        protected override void OnStart()
        {
            base.OnStart();

            var demoServiceIntent = new Intent(this, typeof(LocationService));
            locationServiceConnection = new LocationServiceConnection(this);
            BindService(demoServiceIntent, locationServiceConnection, Bind.AutoCreate);
            StartService(demoServiceIntent);
        }


        /// <summary>
        /// when the app resumes it sets up all the map things
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            if (isBound)
                binder.GetService().inForeground = true;

        }

        /// <summary>
        /// pauses all of the map things
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
            if (isBound)
                binder.GetService().inForeground = false;
        }

        /// <summary>
        /// pauses all of the map things
        /// </summary>
        protected override void OnDestroy()
        {

            base.OnDestroy();
            if (!isConfigurationChange)
            {
                if (isBound)
                {
                    binder.Close();
                    UnbindService(locationServiceConnection);
                    isBound = false;
                }
            }

        }

        #endregion
        #region unused
        [Obsolete("Method is unused")]
        public void BeforeTextChanged(ICharSequence s, int start, int count, int after) { }
        [Obsolete("Method is unused")]
        public void OnTextChanged(ICharSequence s, int start, int before, int count) { }

        //DrawActivity is merged with FirstView

        /*public class DrawerActivity : MvxCachingFragmentCompatActivity<FirstViewModel>
        {
            MvxFragment[] fragments = { new SettingsView(), new HelpView(), new AboutView() };
            string[] titles = { "Settings", "Help", "About, terms & privacy" };
            ActionBarDrawerToggle drawerToggle;

            ListView drawerListView;

            DrawerLayout drawerLayout;

            protected override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.FirstView);

                SupportActionBar.SetDisplayHomeAsUpEnabled(true);

                drawerListView = FindViewById<ListView>(Resource.Id.drawerListView);
                drawerListView.ItemClick += (s, e) => ShowFragmentAt(e.Position);
                drawerListView.Adapter = new ArrayAdapter<string>(
                    this,
                    Android.Resource.Layout.SimpleListItem1,
                    titles);

                drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawerLayout);
                drawerToggle = new ActionBarDrawerToggle(
                    this,
                    drawerLayout,
                    Resource.String.OpenDrawerString,
                    Resource.String.CloseDrawerString);

                drawerLayout.AddDrawerListener(drawerToggle);
                var tm = FragmentManager.BeginTransaction();
                foreach (var item in fragments)
                {
                    tm.Add(item, item.ToString());
                }
                ShowFragmentAt(0);
            }

            void ShowFragmentAt(int position)
            {
                FragmentManager
                    .BeginTransaction()
                    .Replace(Resource.Id.frameLayout, fragments[position])
                    .Commit();

                Title = titles[position];
                drawerLayout.CloseDrawer(drawerListView);
            }

            public override bool OnOptionsItemSelected(IMenuItem item)
            {
                if (drawerToggle.OnOptionsItemSelected(item))
                {
                    return true;
                }
                return base.OnOptionsItemSelected(item);
            }
        }*/
        #endregion
    }
    class LocationServiceConnection : Java.Lang.Object, IServiceConnection
    {
        FirstView activity;
        LocationServiceBinder binder;

        public LocationServiceBinder Binder
        {
            get
            {
                return binder;
            }
        }

        public LocationServiceConnection(FirstView activity)
        {
            this.activity = activity;
        }

        /// <summary>
        /// When the service is connected it sets variables in the main process 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="service"></param>
        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            var demoServiceBinder = service as LocationServiceBinder;
            if (demoServiceBinder != null)
            {
                var binder = (LocationServiceBinder)service;
                ((LocationServiceBinder)service).activity = activity;
                activity.binder = binder;
                activity.isBound = true;
                activity.HideBridgesOnHeight(0);//add code further down the chan when height is checked to show

                // keep instance for preservation across configuration changes
                this.binder = (LocationServiceBinder)service;
                activity.binder.GetService().SetupLocationTracking();

            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            activity.isBound = false;
        }

    }

}
