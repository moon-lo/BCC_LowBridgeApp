using BCC.Core.Models;
using MvvmCross.Core.ViewModels;
using System.Collections.ObjectModel;

namespace BCC.Core.ViewModels
{
    public class HamburgerViewModel
        : MvxViewModel
    {

        public ObservableCollection<MenuItem> MenuItems { get; private set; }

        private MvxCommand<MenuItem> itemSelectedCommand;

        public IMvxCommand ItemSelectedCommand
        {
            get
            {
                itemSelectedCommand = itemSelectedCommand ?? new MvxCommand<MenuItem>(MenuAction);
                return itemSelectedCommand;
            }
        }

        public void MenuAction(MenuItem item)
        {
            ShowViewModel(item.ViewModelType);
        }

        public void MenuViewModel()
        {
            MenuItems = new ObservableCollection<MenuItem>();

            MenuItems.Add(new MenuItem() { Title = "Settings", ViewModelType = typeof(HamburgerViewModel) });
            MenuItems.Add(new MenuItem() { Title = "Help", ViewModelType = typeof(HamburgerViewModel) });
            MenuItems.Add(new MenuItem() { Title = "About, terms & privacy", ViewModelType = typeof(HamburgerViewModel) });
        }
    }

}

