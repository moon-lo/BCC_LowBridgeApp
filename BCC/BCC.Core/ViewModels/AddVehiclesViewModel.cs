using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using BCC.Core.Models;
using System.Windows.Input;
using MvvmCross.Plugins.Messenger;

//Author Scott Fletcher N9017097
namespace BCC.Core.ViewModels
{
    public class AddVehiclesViewModel : MvxViewModel
    {
        AddVehicle _addVehicle;

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
                    if (_addVehicle.IsValid())
                    {AddVehicle tempveh = new AddVehicle();
                        tempveh.ProfileName = ProfileName;
                        tempveh.VehicleName = VehicleName;
                        tempveh.RegNumber = RegNumber;
                        tempveh.VehicleHeight = VehicleHeight;
                        tempveh.VehicleSelection = 0;
                        Mvx.Resolve<Repository>().CreateAddVehicle(_addVehicle).Wait();
                        IMvxMessenger messenger = Mvx.Resolve<IMvxMessenger>();
                        var message = new ViewModelCommunication(this, "reload");
                        messenger.Publish(message);
                        Close(this);
                    }
                });
            }
        }


        public void Init(AddVehicle addVehicle = null)
        {
            _addVehicle = addVehicle == null ? new AddVehicle() : addVehicle;
            RaiseAllPropertiesChanged();
        }



    }
}
