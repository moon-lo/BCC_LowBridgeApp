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
using Java.Lang;

namespace BCC.Droid.Views
{
    [Service]
    public class LocationService : Service
    {
        LocationServiceBinder binder;
        public bool inForeground = true;
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            //start maps
            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            binder = new LocationServiceBinder(this);
            BroadcastMapUpdate();
            return binder;

        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            //close location service
        }
        private void BroadcastMapUpdate()
        {
            Intent BroadcastIntent = new Intent(this, typeof(FirstView.LocationBroadcastReceiver));
            BroadcastIntent.SetAction(FirstView.LocationBroadcastReceiver.MAP_UPDATE);
            BroadcastIntent.AddCategory(Intent.CategoryDefault);
            SendBroadcast(BroadcastIntent);
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
        public void Close()
        {
            service.OnDestroy();
        }
    }

}