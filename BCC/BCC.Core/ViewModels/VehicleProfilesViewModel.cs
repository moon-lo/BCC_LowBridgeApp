using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BCC.Core.ViewModels
{
    public class VehicleProfilesViewModel : MvxViewModel
    {

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
