using BCC.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

//Author Scott Fletcher N9017097
namespace BCC.Core
{
    public class Repository
    {
        private readonly SQLiteAsyncConnection conn;

        public Repository(string dbPath)
        {
            conn = new SQLiteAsyncConnection(dbPath);
            conn.CreateTableAsync<AddVehicle>().Wait();
        }

        public async Task CreateAddVehicle(AddVehicle addVehicle)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(addVehicle.ProfileName))
                    throw new Exception("Profile Name is Required");

                
                var result = await conn.InsertAsync(addVehicle).ConfigureAwait(continueOnCapturedContext: false);
                
            }
            catch (Exception ex)
            {
                
            }
        }

        public Task<List<AddVehicle>> GetAllAddVehicles()
        {
            
            return conn.Table<AddVehicle>().ToListAsync();
        }
    }
}