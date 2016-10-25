using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BCC.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.FullFragging.Fragments;
using MvvmCross.Droid.Shared.Attributes;

namespace BCC.Droid.Views
{
    [MvxFragment(typeof(FirstViewModel), Resource.Id.frameLayout)]
    [Register("mvvmcrossdemo.droid.SettingView")]
    public class SettingsView : MvxFragment<SettingsViewModel>
    {
        FragmentManager settingsFrag;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
            View view = inflater.Inflate(Resource.Layout.SettingView, container, false);
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

        public override void OnResume()
        {
            base.OnResume();
        }


    }
}