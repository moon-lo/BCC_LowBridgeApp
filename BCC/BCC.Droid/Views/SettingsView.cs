using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BCC.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.FullFragging.Fragments;
using MvvmCross.Droid.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using MvvmCross.Droid.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Platform;

namespace BCC.Droid.Views
{
    [MvxFragment(typeof(FirstViewModel), Resource.Id.frameLayout)]
    [Register("bcc.droid.SettingView")]
    public class SettingsView : MvxFragment<SettingsViewModel>
    {
        FragmentManager settingsFrag;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
            View view = inflater.Inflate(Resource.Layout.SettingView, container, false);

            //Setting up the layout for the toolbar 
            /*var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Settings";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.back); */

            return view;
            
        }

        
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            /*var backButton = (Button) GetView().findViewById(Resource.Id.buttonCloseFragment);
            
            backButton.Click += delegate
            {
                settingsFrag = getSupportFragmentManager();
                settingsFrag.beginTransaction();
                settingsFrag.remove(settingsFrag);
                settingsFrag.commit();
            };*/
        }
        /// <summary>
        /// This detects if any of the buttons in the toolbar have been pressed
        /// </summary>
        /// <param name="item">the item that was pressed</param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnResume()
        {
            base.OnResume();
        }


    }
}