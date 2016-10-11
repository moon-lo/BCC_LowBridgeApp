using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using BCC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BCC.Core.ViewModels
{
    public class AllAddVehiclesViewModel : MvxViewModel
    {
        public List<AddVehicle> AllAddVehicles { get; set; }

        public ICommand NavBack
        {
            get
            {
                return new MvxCommand(() => Close(this));
            }
        }

        
        public void Init()
        {
            Task<List<AddVehicle>> result = Mvx.Resolve<Repository>().GetAllAddVehicles();
            result.Wait();
            AllAddVehicles = result.Result;
        }
    }
}