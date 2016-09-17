using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCC.Core.ViewModels
{
    public interface IView
    {
        void GoTo(LocationAutoCompleteResult.Result location);
        void ShowSearch();
    }
}
