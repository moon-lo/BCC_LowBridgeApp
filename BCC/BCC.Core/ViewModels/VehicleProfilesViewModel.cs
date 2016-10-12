using BCC.Core.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BCC.Core.ViewModels
{
    public class VehicleProfilesViewModel : MvxViewModel
    {


        private ObservableCollection<AddVehicle> allAddVehicles;
        public ObservableCollection<AddVehicle> AllAddVehicles
        {
            get { return allAddVehicles; }
            set { SetProperty(ref allAddVehicles, value); }
        }

        private string currVehicle;
        public string CurrVehicle
        {
            get { return currVehicle; }
            set { SetProperty(ref currVehicle, "Vehicle: " + value); }
        }
        private string currHeight;
        public string CurrHeight
        {
            get { return currHeight; }
            set { SetProperty(ref currHeight, "Height: " + value + "m"); }
        }

        private string vehicle_name;
        public string Vehicle_name
        {
            get { return vehicle_name; }
            set
            {
                string prevalue = vehicle_name;
                SetProperty(ref vehicle_name, value);
            }
        }
        private string vehicle_height;
        public string Vehicle_height
        {
            get { return vehicle_name; }
            set
            {
                string prevalue = vehicle_name;
                SetProperty(ref vehicle_name, value);
            }
        }

        public ICommand SelectUnitCommand { get; private set; }
        public VehicleProfilesViewModel()
        {
            AddVehicle tempveh = new AddVehicle();
            tempveh.ProfileName = "car1";
            tempveh.VehicleName = "potato";
            tempveh.RegNumber = "770LXY";
            tempveh.VehicleHeight = "100.1";
            //Repository repo = new Repository();
            Task<List<AddVehicle>> result = Mvx.Resolve<Repository>().GetAllAddVehicles();
            result.Wait();
            AllAddVehicles = new ObservableCollection<AddVehicle>(result.Result);
            AllAddVehicles.Add(tempveh);

            SelectUnitCommand = new MvxCommand<AddVehicle>(vehicle =>
            {
                CurrVehicle = vehicle.ProfileName;
                CurrHeight = vehicle.VehicleHeight;
                //place code to switch bools in the database here
            });
        }

        public ICommand NavigateCreateAddVehicle
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<AddVehiclesViewModel>());
            }
        }

        public ICommand NavigateAllAddVehicle
        {
            get
            {

                return new MvxCommand(() => ShowViewModel<AllAddVehiclesViewModel>());

            }
        }
    }
}
