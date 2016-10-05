using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Views;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;


namespace BCC.Droid.Views
{
    /// <summary>
    /// Author: Lok Sum (Moon) Lo n9050159
    /// </summary>

    [Activity]

    public class MainActivity : AppCompatActivity
    {

        string[] _titles = { "Current vehicle", "Settings", "Help", "About & Terms and Privacy" };

        ActionBarDrawerToggle _drawerToggle;

        ListView _drawerListView;

        DrawerLayout _drawerLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Hamburger);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            _drawerListView = FindViewById<ListView>(Resource.Id.drawerListView);
            
            _drawerListView.Adapter = new ArrayAdapter<string>(this, global::Android.Resource.Layout.SimpleListItem1, _titles);

            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawerLayout);

            _drawerToggle = new ActionBarDrawerToggle(this, _drawerLayout, Resource.String.OpenDrawerString, Resource.String.CloseDrawerString);

            _drawerLayout.SetDrawerListener(_drawerToggle);

        }
    }
}

