
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

        
        public AddVehicle()
        {
            ProfileName = string.Empty;
            VehicleName = string.Empty; ;
            RegNumber = string.Empty;
            VehicleHeight = string.Empty;
        }

        public bool IsValid()
        {
            return (!String.IsNullOrWhiteSpace(ProfileName));
        }
    }
}
    
