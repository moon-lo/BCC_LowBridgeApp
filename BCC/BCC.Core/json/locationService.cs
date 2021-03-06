﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BCC.Core.ViewModels
{
    public class locationService
    {
        /// <summary>
        /// Takes a search term and uses googles geocoding api to find relavant locations and returns 
        /// them as a List
        /// </summary>
        /// <param name="searchTerm">the term to search</param>
        /// <returns>the search results</returns>
        public async Task<List<LocationAutoCompleteResult.Result>> GetLocations(string searchTerm)
        {
            WebRequest request = WebRequest.CreateHttp(string.Format("{0}?key={1}&address={2}&region=au",
                LocationSearch.LocationsEndpoints,
                LocationSearch.ApiKey, searchTerm));
            string responseValue = string.Empty;

            using (var response = await request.GetResponseAsync())
            {
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                responseValue = await reader.ReadToEndAsync();
                            }
                        }
                    }
                }
            }
            var dsResponse = JsonConvert.DeserializeObject<LocationAutoCompleteResult.RootObject>(responseValue);
            List<LocationAutoCompleteResult.Result> result = dsResponse.results;
            return result;
        }

    }
}
