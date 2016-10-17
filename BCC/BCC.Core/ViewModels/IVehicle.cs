using BCC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCC.Core.ViewModels
{
    public interface IVehicle
    {
        string LoadFile(string file);
        void DeleteItem(AddVehicle name);
    }
}
