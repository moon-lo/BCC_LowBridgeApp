using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;

namespace BCC.Droid.Views
{
    [Activity(Label = "View for FirstViewModel")]
    public class FirstView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ActionBar.Hide();
            SetContentView(Resource.Layout.FirstView);
        }
    }
}
