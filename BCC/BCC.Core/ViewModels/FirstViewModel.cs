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
                SetProperty(ref unitCode, value);
                RaisePropertyChanged(() => Locations);
                Locations.Clear();
                if (value != null && value != "")
                {
                    SearchLocations(value);
                }

            }
        }

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
        public ICommand SelectUnitCommand { get; private set; }
        public FirstViewModel()
        {
            Locations = new ObservableCollection<LocationAutoCompleteResult.Result>();
            SelectUnitCommand = new MvxCommand<LocationAutoCompleteResult.Result>(unit =>
            {
                //search

            });
        }


    }


}
