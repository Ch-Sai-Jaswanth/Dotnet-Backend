using BikeDealersProject.Models;
using BikeDealersProject.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;

namespace BikeDealersProject.Services
{
    public class BikeService : IBikeService
    {
        private readonly BikeDealerMgmtContext _context;
        public BikeService(BikeDealerMgmtContext context)
        {
            _context = context;
        }
        public async Task<int> AddBike(BikeStore bike)
        {
            _context.Add(bike);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddBikesBulk(List<BikeStore> bikes)
        {
            _context.BikeStores.AddRangeAsync(bikes);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteBike(int id)
        {
            var bike = await _context.BikeStores.FindAsync(id);
            if (bike == null) return 0;
            _context.BikeStores.Remove(bike);
            return await _context.SaveChangesAsync();
        }

        public async Task<BikeStore> FindBikeById(int id)
        {
            BikeStore bike = await _context.BikeStores.FirstOrDefaultAsync(b => b.BikeId == id);
            return bike;
        }

        public async Task<BikeStore> FindBikeByName(string name)
        {
            BikeStore bike = await _context.BikeStores.FirstOrDefaultAsync(b => b.ModelName == name);
            return bike;
        }

        public async Task<List<BikeStore>> GetBikes()
        {
            List<BikeStore> lst = await _context.BikeStores.ToListAsync();
            return lst;
        }

        public async Task<BikeStore> UpdateBike(int id, BikeStore bike)
        {
            BikeStore bk = await _context.BikeStores.FindAsync(id);
            if (bk == null) return null;
            bk.ModelName = bike.ModelName;
            bk.ModelYear = bike.ModelYear;
            bk.EngineCc = bike.EngineCc;
            bk.Manufacturer = bike.Manufacturer;
            await _context.SaveChangesAsync();
            return bk;
        }
    }
}