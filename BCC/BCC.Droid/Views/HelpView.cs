using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using BCC.Core.ViewModels;
using MvvmCross.Droid.FullFragging.Fragments;
using MvvmCross.Droid.Shared.Attributes;

namespace BCC.Droid.Views
{
    [MvxFragment(typeof(HelpMapViewModel), Resource.Id.frameLayout)]
    [Register("mvvmcrossdemo.droid.HelpView")]
    public class HelpView : MvxFragment<HelpMapViewModel>
    {
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
            View view = inflater.Inflate(Resource.Layout.HelpView, container, false);
            return view;
        }
    }
}