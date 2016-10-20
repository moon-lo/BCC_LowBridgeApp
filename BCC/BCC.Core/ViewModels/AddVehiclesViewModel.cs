using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using BCC.Core.Models;
using System.Windows.Input;
using MvvmCross.Plugins.Messenger;
using System.Collections.Generic;
using System.Threading.Tasks;

//Author Scott Fletcher N9017097
namespace BCC.Core.ViewModels
{
    public class AddVehiclesViewModel : MvxViewModel
    {
        AddVehicle _addVehicle;
        bool external = false;

        public string ProfileName
        {
            get { return _addVehicle.ProfileName; }
            set
            {
                _addVehicle.ProfileName = value;
                RaisePropertyChanged(() => ProfileName);
            }
        }

        public string VehicleName
        {
            get { return _addVehicle.VehicleName; }
            set
            {
                _addVehicle.VehicleName = value;
                RaisePropertyChanged(() => VehicleName);
            }
        }

        public string RegNumber
        {
            get { return _addVehicle.RegNumber; }
            set
            {
                _addVehicle.RegNumber = value;
                RaisePropertyChanged(() => RegNumber);
            }
        }

        public string VehicleHeight
        {
            get { return _addVehicle.VehicleHeight; }
            set
            {
                _addVehicle.VehicleHeight = value;
                RaisePropertyChanged(() => VehicleHeight);
            }


        }
        public int VehicleSelection
        {
            get { return _addVehicle.VehicleSelection; }
            set
            {
                _addVehicle.VehicleSelection = value;
                RaisePropertyChanged(() => VehicleSelection);
            }
        }
        public ICommand NavBack
        {
            get
            {
                return new MvxCommand(() => Close(this));
            }
        }

        public ICommand SaveAddVehicle
        {
            get
            {
                return new MvxCommand(() =>
                {
                    double result;
                    if (_addVehicle.IsValid() && double.TryParse(VehicleHeight, out result))
                        if (ProcessVehicle(_addVehicle))
                        {
                            Mvx.Resolve<IMvxMessenger>().Publish(new ViewModelCommunication(this, "reload"));
                            Close(this);
                        }
                        else Mvx.Resolve<IMvxMessenger>().Publish(new ViewModelCommunication(this, "string"));
                });
            }
        }

        /// <summary>
        /// deals with updating/creating a supplied vehicle
        /// </summary>
        /// <param name="addVehicle">the vehilce to update/create</param>
        /// <returns>if successful</returns>
        private bool ProcessVehicle(AddVehicle addVehicle)
        {
            Task<List<AddVehicle>> vehicles = Mvx.Resolve<Repository>().GetAllAddVehicles();
            vehicles.Wait();
            bool close = true;
            if (!external)
            {
                foreach (AddVehicle vehicle in new List<AddVehicle>(vehicles.Result))
                    if (vehicle.ProfileName == addVehicle.ProfileName)
                    {
                        close = false;
                        Mvx.Resolve<IMvxMessenger>().Publish(new ViewModelCommunication(this, "contains"));
                    }
                if (close) Mvx.Resolve<Repository>().CreateAddVehicle(addVehicle).Wait();
            }
            else update();
            return close;
        }

        /// <summary>
        /// when called updates the current vehicle profile in the database
        /// </summary>
        private async void update()
        {
            await Mvx.Resolve<Repository>().UpdateVehicle(_addVehicle);
        }

        /// <summary>
        /// Initalises the function by looking for vehicles related to the supplied string, if the string is not found
        /// it initalises a new vehicle
        /// </summary>
        /// <param name="val"></param>
        public void Init(string val)
        {
            Task<List<AddVehicle>> result = Mvx.Resolve<Repository>().GetAllAddVehicles();
            result.Wait();
            foreach (AddVehicle vehicle in new List<AddVehicle>(result.Result))
            {
                if (vehicle.ProfileName == val)
                {
                    _addVehicle = vehicle;
                    external = true;
                }
            }
            if (_addVehicle == null) _addVehicle = new AddVehicle();
        }
    }
}
