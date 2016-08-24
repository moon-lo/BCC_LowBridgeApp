using MvvmCross.Core.ViewModels;

namespace BCC.Core.ViewModels
{
    public class FirstViewModel
        : MvxViewModel
    {
        private string _name = getApplicationContext().getPackageName();;
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != null && value != _name)
                {
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }
    }
}
