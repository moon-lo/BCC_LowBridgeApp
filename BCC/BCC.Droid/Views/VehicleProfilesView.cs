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
using MvvmCross.Droid.Support.V7.AppCompat;
using BCC.Core.ViewModels;

namespace BCC.Droid.Views
{
    [Activity(Label = "Vehicle Profiles")]

    public class VehicleProfilesView : MvxAppCompatActivity<VehicleProfilesViewModel>
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.VehicleProfiles);
            // var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            // SetSupportActionBar(toolbar);
            //SupportActionBar.Title = "Vehicle Profiles";
            //SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetHomeButtonEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);
            //SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.back);
            //SupportActionBar.Show();


        }
    }
}

