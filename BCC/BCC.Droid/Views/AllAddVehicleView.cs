using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;

namespace BCC.Droid.Views
{
    [Activity(Label = "AllAddVehicle", NoHistory = true)]
    public class AllAddVehicleView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.addvehiclelist);
        }
    }
}