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
using MvvmCross.Droid.Views;
using Android.Support.V4.Widget;
using Java.Lang;


namespace BCC.Droid.Views
{
    public class Hamburger : Activity
    {
        private static string[] hamburgerItemTitles = { "Current Vehicle", "Settings", "Help", "About, terms & privacy" };
        private DrawerLayout drawerLayout;
        private ListView hambList;

        public class NavigationDrawerActivity : Activity
        {
            protected override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.Hamburger);



                var mDrawerLayout = (DrawerLayout)FindViewById(Resource.Id.drawer_layout);
                var mDrawerList = (ListView)FindViewById(Resource.Id.left_drawer);

                // Set the adapter for the list view
                mDrawerList.Adapter = new ArrayAdapter<string>(this, Resource.Layout.abc_list_menu_item_layout, hamburgerItemTitles);
                // Set the list's click listener
                //mDrawerList.OnItemClickListener = new DrawerItemClickListener();

            }
        }
    }
}

    