﻿using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using BCC.Core.Models;
using System.Windows.Input;

//Author Scott Fletcher N9017097
namespace BCC.Core.ViewModels
{
    public class AddVehiclesViewModel : MvxViewModel
    {
        AddVehicle _addVehicle;

        public string ProfileName
        {
            get { return _addVehicle.ProfileName; }
            set { _addVehicle.ProfileName = value;
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
                    {
                        Mvx.Resolve<Repository>().CreateAddVehicle(_addVehicle).Wait();
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
