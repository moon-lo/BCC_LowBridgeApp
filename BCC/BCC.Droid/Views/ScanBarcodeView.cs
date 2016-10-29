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
using MvvmCross.Droid.FullFragging.Views;
using ZXing.Mobile;

namespace BCC.Droid.Views
{
    public class ScanBarcodeView : MvxActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ScanView);
            // Create your application here
            //ZXing.Mobile.MobileBarcodeScanner;
            
        }
    }
}