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
using MvvmCross.Droid.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using BCC.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Platform;
using BCC.Core.Models;
using BCC.Core;
using System.Threading.Tasks;

namespace BCC.Droid.Views
{
    [Activity(Label = "Vehicle Profiles")]

    public class VehicleProfilesView : MvxAppCompatActivity<VehicleProfilesViewModel>, IVehicle
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.VehicleProfiles);

            var viewModel = DataContext as VehicleProfilesViewModel;
            viewModel.View = this;
            viewModel.UpdateList();


            //Setting up the layout for the toolbar 
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Vehicle Profiles";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.back);

            SelectScannedVehicle();

        }

        /// <summary>
        /// This detects if any of the buttons in the toolbar have been pressed
        /// </summary>
        /// <param name="item">the item that was pressed</param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();//go back
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        /// <summary>
        /// asks the user if they want to delete the supplied vehicle, if they press Delete then delete
        /// </summary>
        /// <param name="name">the vehicle to delete</param>
        public void DeleteItem(AddVehicle name)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this)
                .SetTitle("Confirm delete")
                .SetMessage("Do you wish to delete " + name.ProfileName)
                .SetPositiveButton("Delete", (senderAlert, args) =>
                {
                    var viewModel = DataContext as VehicleProfilesViewModel;
                    viewModel.DeleteVehicle(name);
                })
                .SetNegativeButton("Cancel", (senderAlert, args) => { });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        /// <summary>
        /// Edits the visibility of the edit button 
        /// </summary>
        /// <param name="state">visible or invisible</param>
        public void EditVisibility(bool state)
        {
            if (state) FindViewById<ImageButton>(Resource.Id.editButton).Visibility = ViewStates.Visible;
            else FindViewById<ImageButton>(Resource.Id.editButton).Visibility = ViewStates.Invisible;
        }

        private void SelectScannedVehicle()
        {
            string vName = Intent.GetStringExtra("vName");
            string vRegNo = Intent.GetStringExtra("vRegNo");
            string vHeight = Intent.GetStringExtra("vHeight");

            if (vName != null && vRegNo != null && vHeight != null)
            {
                AddVehicle vehicle = new AddVehicle();
                vehicle.ProfileName = vName;
                vehicle.VehicleName = vName;
                vehicle.RegNumber = vRegNo;
                vehicle.VehicleHeight = vHeight;

                Task<List<AddVehicle>> result = Mvx.Resolve<Repository>().GetAllAddVehicles();

                AddVehicle CurrVehicle = null;
                foreach (AddVehicle vehicleSearch in result.Result)
                    if (vehicleSearch.VehicleSelection == 1)
                        CurrVehicle = vehicleSearch;

                var viewModel = DataContext as VehicleProfilesViewModel;
                viewModel.SwitchVehicle(vehicle, CurrVehicle);

                //Finish();
            }
        }
    }



}

