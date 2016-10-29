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
using Android.Preferences;

namespace BCC.Droid.Views
{
    [MvxFragment(typeof(FirstViewModel), Resource.Id.frameLayout)]
    [Register("bcc.droid.SettingView")]
    public class SettingsView : MvxFragment<SettingsViewModel>
    {
        FragmentManager settingsFrag;
        ImageButton btn;
        EditText newDist;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
            View view = inflater.Inflate(Resource.Layout.SettingView, container, false);

            btn = (ImageButton) view.FindViewById(Resource.Id.confirmButton);
            newDist = view.FindViewById<EditText>(Resource.Id.inputDist);
            newDist.Text = GetDist().ToString();

            btn.Click += (object sender, EventArgs e) => {
                CheckDist(sender, e);
            };

            return view;
            
        }

        
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            
        }
        
        public override void OnResume()
        {
            base.OnResume();
        }

        private void CheckDist(object sender, EventArgs e) {
            int dist;
            if (int.TryParse(newDist.Text, out dist)) {
                if (dist >= 1)
                {
                    ChangeDist(dist);
                    Toast.MakeText(Application.Context, "Detection distance updated.", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(Application.Context, "Please enter a valid distance.", ToastLength.Short).Show();
                }
            }
            
        }

        public void ChangeDist(int newDist)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutInt("distance", newDist);
            editor.Apply();
        }

        public int GetDist() {
            int dist = 100;
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            dist = prefs.GetInt("distance", 100);
            return dist;
        }


    }
}