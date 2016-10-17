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

        public async Task UpdateVehicle(AddVehicle addVehicle)
        {
            try
            {
                    await conn.UpdateAsync(addVehicle);
            }
            catch (SQLiteException ex)
            {
            }
        }

        public async Task<bool> DeleteVehicle(AddVehicle addVehicle)
        {
            try
            {
                await conn.DeleteAsync(addVehicle);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Task<List<AddVehicle>> GetAllAddVehicles()
        {
            return conn.Table<AddVehicle>().ToListAsync();
        }
    }
}