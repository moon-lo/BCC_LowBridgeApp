using BCC.Core.json;
using BCC.Core.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
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

        private double currentVehicleHeight;
        public double CurrentVehicleHeight
        {
            get { return currentVehicleHeight; }
        }
        private IView view;
        public IView View { get; set; }

        public List<BridgeData> GetBridges(Stream file)
        {
            BridgeService service = new BridgeService();
            return service.GetLocations(file);
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

        public void UpdateHeight()
        {

            Repository repo = new Repository(View.LoadFile("profiles.db3"));
            Task<List<AddVehicle>> result = Mvx.Resolve<Repository>().GetAllAddVehicles();
            result.Wait();

            if (result.Result.Count == 0)
                currentVehicleHeight = 0;
            else foreach (AddVehicle vehicle in result.Result)
                    if (vehicle.VehicleSelection == 1)
                        currentVehicleHeight = double.Parse(vehicle.VehicleHeight);
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

            ButtonCommand = new MvxCommand(() =>
            {
                View.OpenDrawer();
            });

            SelectUnitCommand = new MvxCommand<LocationAutoCompleteResult.Result>(location =>
            {
                View.GoTo(location);
                UnitCode = location.formatted_address;
            });
            VehicleButton = new MvxCommand(() => ShowViewModel<VehicleProfilesViewModel>());
        }

        #region Nav menu
        readonly Type[] menuItemTypes =
        {
            typeof(SettingsViewModel),
            typeof(HelpMapViewModel),
            typeof(AboutViewModel)
        };

        public IEnumerable<string> MenuItems { get; private set; }
            = new[] { "Settings", "Help", "About, terms & privacy" };

        public void ShowDefaultMenuItem()
        {
            NavigateTo(0);
        }
        public void NavigateTo(int position)
        {
            ShowViewModel(menuItemTypes[position]);
        }
        #endregion
    }

    public class MenuItem : Tuple<string, Type>
    {
        public MenuItem(string displayName, Type viewModelType) : base(displayName, viewModelType) { }

        public string DisplayName
        {
            get { return Item1; }
        }
        public Type ViewModelType
        {
            get { return Item2; }
        }
    }
}
