using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCC.Core.json
{
    public class BridgeData
    {
        public string ASSET_ID { get; set; }
        public string COUNTER { get; set; }
        public string Description { get; set; }
        public string Direction { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Signed_Clearance { get; set; }
        public double SIGNED_CLEARANCE_MAX { get; set; }
        public string Street_Name { get; set; }
        public string Structure_ID { get; set; }
        public string Suburb { get; set; }
    }
}
