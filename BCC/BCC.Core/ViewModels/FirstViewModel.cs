using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                Locations.Clear();
                if (value != null && value != "" && value.Length > prevalue.Length)
                {
                    SearchLocations(value);
                }
            }
        }
        private IView view;
        public IView View { get; set; }

        /// <summary>
        /// Searches for locations containing the search terms and adds the results to the Locations list
        /// </summary>
        /// <param name="searchTerm"></param>
        private async void SearchLocations(string searchTerm)
        {
            locationService locationService = new locationService();
            var locationResults = await locationService.GetLocations(searchTerm);
            foreach (var item in locationResults)
            {
                Locations.Add(item);
            }
        }

        public ICommand ButtonCommand { get; private set; }
        public ICommand VehicleButton { get; private set; }
        public ICommand SelectUnitCommand { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public FirstViewModel()
        {
            Locations = new ObservableCollection<LocationAutoCompleteResult.Result>();
            SelectUnitCommand = new MvxCommand<LocationAutoCompleteResult.Result>(location =>
            {
                View.GoBack(location);
                //search
                UnitCode = location.formatted_address;
            });
            VehicleButton = new MvxCommand(() => ShowViewModel<VehicleProfilesViewModel>());
        }
    }
}
