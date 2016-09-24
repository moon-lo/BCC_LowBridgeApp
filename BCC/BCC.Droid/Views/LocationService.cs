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
using Android.Util;

namespace BCC.Droid.Views
{
    [Service]
    public class LocationService : Service
    {
        LocationServiceBinder binder;
        public bool inForeground = true;
        private int test = 0;
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {

            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            binder = new LocationServiceBinder(this);
            test = 7;
            return binder;
        }

        public void setTest(int i)
        {
            test = i;
        }
        public int returnTest()
        {
            return test;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }


    }
    public class LocationServiceBinder : Binder
    {
        LocationService service;

        public LocationServiceBinder(LocationService mnservice)
        {
            this.service = mnservice;
        }

        public LocationService GetService()
        {
            return service;
        }
    }

}