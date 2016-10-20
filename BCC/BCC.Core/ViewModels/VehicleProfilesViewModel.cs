using BCC.Core.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SQLite;


namespace BCC.Core.ViewModels
{
    public class VehicleProfilesViewModel : MvxViewModel
    {
        private AddVehicle currentVehicle;
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

        private int vehicleSelection;
        public int VehicleSelection
        {
            get { return vehicleSelection; }
            set
            {
                int prevalue = vehicleSelection;
                SetProperty(ref vehicleSelection, value);
            }
        }

        private void OnUpdateMessage(ViewModelCommunication locationMessage)
        {
            if (locationMessage.Msg == "reload")
                UpdateList();
        }

        private readonly MvxSubscriptionToken _token;

        public IVehicle View { get; set; }

        public ICommand DeleteCommand { get; set; }
        public ICommand AlterVehicle { get; set; }
        public ICommand SelectUnitCommand { get; private set; }
        public ICommand NavigateCreateAddVehicle { get; private set; }

        public VehicleProfilesViewModel()
        {
            _token = Mvx.Resolve<IMvxMessenger>().Subscribe<ViewModelCommunication>(OnUpdateMessage);

            DeleteCommand = new MvxCommand<AddVehicle>(vehicle => View.DeleteItem(vehicle));
            SelectUnitCommand = new MvxCommand<AddVehicle>(vehicle => SwitchVehicle(vehicle));
            AlterVehicle = new MvxCommand(() => ShowViewModel<AddVehiclesViewModel>(new { val = currentVehicle.ProfileName }));
            NavigateCreateAddVehicle = new MvxCommand(() => ShowViewModel<AddVehiclesViewModel>());
        }

        /// <summary>
        /// Switches the currentVehicle to the supplied vehicle
        /// </summary>
        /// <param name="vehicle">the vehicle to switch to</param>
        private async void SwitchVehicle(AddVehicle vehicle)
        {
            //switch title
            CurrVehicle = vehicle.ProfileName;
            CurrHeight = vehicle.VehicleHeight;

            View.EditVisibility(true);

            //switch bools in the database
            if (currentVehicle != null)
            {
                currentVehicle.VehicleSelection = 0;
                await Mvx.Resolve<Repository>().UpdateVehicle(currentVehicle);
            }
            vehicle.VehicleSelection = 1;
            await Mvx.Resolve<Repository>().UpdateVehicle(vehicle);
            currentVehicle = vehicle;

            //send message to notify main window
            Mvx.Resolve<IMvxMessenger>().Publish(new ViewModelCommunication(this, "vehicleChanged"));
        }

        /// <summary>
        /// Deletes the current vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        public async void DeleteVehicle(AddVehicle vehicle)
        {
            if (vehicle == currentVehicle) View.EditVisibility(false);
            bool result = await Mvx.Resolve<Repository>().DeleteVehicle(vehicle);
            Mvx.Resolve<IMvxMessenger>().Publish(new ViewModelCommunication(this, "reload"));
        }

        /// <summary>
        /// reloads the mvxlistview using the database and sets the current vehicle
        /// </summary>
        public void UpdateList()
        {
            bool visible = false;
            Task<List<AddVehicle>> result = Mvx.Resolve<Repository>().GetAllAddVehicles();
            result.Wait();
            AllAddVehicles = new ObservableCollection<AddVehicle>(result.Result);

            foreach (AddVehicle vehicle in AllAddVehicles)
                if (vehicle.VehicleSelection == 1)
                {
                    CurrVehicle = vehicle.ProfileName;
                    CurrHeight = vehicle.VehicleHeight;
                    currentVehicle = vehicle;
                    visible = true;
                    View.EditVisibility(true);
                }
            if (!visible)
                View.EditVisibility(false);

            RaisePropertyChanged(() => AllAddVehicles);
        }
    }
}
