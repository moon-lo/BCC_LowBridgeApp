﻿using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using BCC.Core.ViewModels;
using MvvmCross.Droid.FullFragging.Fragments;
using MvvmCross.Droid.Shared.Attributes;

namespace BCC.Droid.Views
{
    [MvxFragment(typeof(FirstViewModel), Resource.Id.frameLayout)]
    [Register("bcc.droid.AboutView")]
    public class AboutView : MvxFragment<AboutViewModel>
    {
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
            View view = inflater.Inflate(Resource.Layout.AboutView, container, false);
            return view;
        }
    }
}