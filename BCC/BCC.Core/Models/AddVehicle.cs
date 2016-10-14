
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

        [NotNull]
        public string VehicleName { get; set; }

        [NotNull]
        public string RegNumber { get; set; }

        [NotNull]
        public string VehicleHeight { get; set; }

        [NotNull]
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

