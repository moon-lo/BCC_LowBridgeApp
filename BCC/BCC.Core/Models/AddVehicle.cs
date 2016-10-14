
using SQLite;
using System;

namespace BCC.Core.Models
{
    //Author Scott Fletcher N9017097
    [Table(nameof(AddVehicle))]
    public class AddVehicle
    {
        [PrimaryKey,]
        public string ProfileName { get; set; }

        public string VehicleName { get; set; }

        public string RegNumber { get; set; }

        public string VehicleHeight { get; set; }

        public int VehicleSelection { get; set; }


        public AddVehicle()
        {
        }

        public bool IsValid()
        {
            return (!String.IsNullOrWhiteSpace(ProfileName));
        }
    }
}

