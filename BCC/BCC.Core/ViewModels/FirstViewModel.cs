using MvvmCross.Core.ViewModels;

namespace BCC.Core.ViewModels
{
    public class FirstViewModel
        : MvxViewModel
    {
        private string _name = "Hello MvvmCross";
        public string Name
        {
            get { return _name; }
            set {
                if (value != null && value != _name)
                {
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }
    }
}
