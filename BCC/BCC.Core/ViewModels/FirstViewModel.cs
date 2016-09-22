using BCC.Core.json;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BCC.Core.ViewModels
{
    /// <summary>
    /// Author: N9452982, Michael Devenish
    /// </summary>
    public class FirstViewModel
        : MvxViewModel
    {
        private ObservableCollection<LocationAutoCompleteResult.Result> locations;
        public ObservableCollection<LocationAutoCompleteResult.Result> Locations
        {
            get { return locations; }
            set { SetProperty(ref locations, value); }
        }
        private string unitCode;
        public string UnitCode
        {
            get { return unitCode; }
            set
            {
                string prevalue = unitCode;
                SetProperty(ref unitCode, value);
                RaisePropertyChanged(() => Locations);
                if (value != null && value != "")
                {
                    SearchLocations(value);
                }
            }
        }
        private IView view;
        public IView View { get; set; }


        public List<BridgeData> GetBridges(Stream file)
        {
            BridgeService service = new BridgeService();
            return  service.GetLocations(file);
        }

        /// <summary>
        /// Searches for locations containing the search terms and adds the results to the Locations list
        /// </summary>
        /// <param name="searchTerm"></param>
        private async void SearchLocations(string searchTerm)
        {
            locationService locationService = new locationService();
            var locationResults = await locationService.GetLocations(searchTerm);
            Locations.Clear();
            foreach (var item in locationResults)
            {
                Locations.Add(item);
            }
        }

        public ICommand ButtonCommand { get; private set; }
        public ICommand OpenSearch { get; private set; }
        public ICommand VehicleButton { get; private set; }
        public ICommand SelectUnitCommand { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public FirstViewModel()
        {
            Locations = new ObservableCollection<LocationAutoCompleteResult.Result>();
            OpenSearch = new MvxCommand(() =>
            {
                if (Locations.Count > 0)
                    View.ShowSearch();
            });

            SelectUnitCommand = new MvxCommand<LocationAutoCompleteResult.Result>(location =>
            {
                View.GoTo(location);
                UnitCode = location.formatted_address;
            });
            VehicleButton = new MvxCommand(() => ShowViewModel<VehicleProfilesViewModel>());
        }
    }
}
