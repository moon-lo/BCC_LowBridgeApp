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
            set
            {
                SetProperty(ref currVehicle, "Vehicle: " + value);
                RaisePropertyChanged(() => CurrVehicle);
            }
        }
        private string currHeight;
        public string CurrHeight
        {
            get { return currHeight; }
            set
            {
                SetProperty(ref currHeight, "Height: " + value + "m");
                RaisePropertyChanged(() => CurrHeight);
            }
        }

        private string profileName;
        public string ProfileName
        {
            get { return profileName; }
            set
            {
                string prevalue = profileName;
                SetProperty(ref profileName, value);
            }
        }
        private string vehicleHeight;
        public string VehicleHeight
        {
            get { return vehicleHeight; }
            set
            {
                string prevalue = vehicleHeight;
                SetProperty(ref vehicleHeight, value);
            }
        }

        public ICommand SelectUnitCommand { get; private set; }
        public ICommand NavigateCreateAddVehicle { get; private set; }

        public VehicleProfilesViewModel()
        {
            UpdateList();

            SelectUnitCommand = new MvxCommand<AddVehicle>(vehicle =>
            {
                CurrVehicle = vehicle.ProfileName;
                CurrHeight = vehicle.VehicleHeight;
                //place code to switch bools in the database here
            });
            NavigateCreateAddVehicle = new MvxCommand(() =>
            {
                ShowViewModel<AddVehiclesViewModel>();
            });

        }

        private void UpdateList()
        {
            AddVehicle tempveh = new AddVehicle();//tempoary
            tempveh.ProfileName = "car1";//tempoary
            tempveh.VehicleName = "potato";//tempoary
            tempveh.RegNumber = "770LXY";//tempoary
            tempveh.VehicleHeight = "100.1";//tempoary
            //Repository repo = new Repository(); //set this up
            Task<List<AddVehicle>> result = Mvx.Resolve<Repository>().GetAllAddVehicles();
            result.Wait();
            AllAddVehicles = new ObservableCollection<AddVehicle>(result.Result);
            AllAddVehicles.Add(tempveh);//tempoary
            RaisePropertyChanged(() => AllAddVehicles);
        }
    }
}
