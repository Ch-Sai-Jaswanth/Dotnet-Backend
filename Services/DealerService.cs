using BikeDealersProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeDealersProject.Services
{
    public class DealerService : IDealerService
    {
        private readonly BikeDealerMgmtContext _context;
        public DealerService(BikeDealerMgmtContext context)
        {
            _context = context;
        }

        public async Task<int> AddDealer(Dealer dealer)
        {
            _context.Dealers.Add(dealer);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddDealersBulk(List<Dealer> dealers)
        {
            _context.Dealers.AddRangeAsync(dealers);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteDealer(int id)
        {
            Dealer dl = await _context.Dealers.FindAsync(id);
            if (dl == null) return 0;
            _context.Dealers.Remove(dl);
            return await _context.SaveChangesAsync();
        }

        public async Task<Dealer> FindDealerById(int id)
        {
            Dealer dl = await _context.Dealers.FirstOrDefaultAsync(d => d.DealerId == id);
            return dl;
        }

        public async Task<Dealer> FindDealerByName(string name)
        {
            Dealer dl = await _context.Dealers.FirstOrDefaultAsync(d => d.DealerName == name);
            return dl;
        }

        public async Task<List<Dealer>> GetDealers()
        {
            List<Dealer> lst = await _context.Dealers.ToListAsync();
            return lst;
        }

        public async Task<Dealer> UpdateDealer(int id, Dealer dealer)
        {
            Dealer dl = await _context.Dealers.FindAsync(id);
            if (dl == null) return null;
            dl.DealerName = dealer.DealerName;
            dl.Address = dealer.Address;
            dl.City = dealer.City;
            dl.State = dealer.State;
            dl.ZipCode = dealer.ZipCode;
            dl.StorageCapacity = dealer.StorageCapacity;
            dl.Inventory = dealer.Inventory;
            await _context.SaveChangesAsync();
            return dl;
        }
    }
}
