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

            //Setting up the layout for the toolbar 
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Vehicle Profiles";//all you really need to change is this
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.back);

        }

        /// <summary>
        /// This detects if any of the buttons in the toolbar have been pressed
        /// </summary>
        /// <param name="item">the item that was pressed</param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)//this is the case for the home button (the only button that can be left of the text in the header)
            {
                case Android.Resource.Id.Home:
                    Finish();//go back
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

    }
   
}

