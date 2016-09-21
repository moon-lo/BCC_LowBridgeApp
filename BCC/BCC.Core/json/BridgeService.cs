using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCC.Core.json
{
    class BridgeService
    {
        /// <summary>
        /// Takes a search term and uses googles geocoding api to find relavant locations and returns 
        /// them as a List
        /// </summary>
        /// <param name="searchTerm">the term to search</param>
        /// <returns>the search results</returns>
        public List<BridgeData> GetLocations(Stream file)
        {
            List<BridgeData> items = new List<BridgeData>();
            using (StreamReader r = new StreamReader(file))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<BridgeData>>(json);
            }
            return items;
        }
    }
}
