using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using BCC.Core.ViewModels;
using MvvmCross.Droid.FullFragging.Fragments;
using MvvmCross.Droid.Shared.Attributes;

namespace BCC.Droid.Views
{
    [MvxFragment(typeof(ScanViewModel), Resource.Id.frameLayout)]
    [Register("mvvmcrossdemo.droid.ScanView")]
    public class ScanView : MvxFragment<ScanViewModel>
    {
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
            View view = inflater.Inflate(Resource.Layout.ScanView, container, false);
            return view;
        }
    }
}