﻿using BCC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCC.Core.ViewModels
{
    public interface IVehicle
    {
        void DeleteItem(AddVehicle name);

        void EditVisibility(bool state);
    }
}
