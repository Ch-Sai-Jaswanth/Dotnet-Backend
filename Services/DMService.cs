using BikeDealersProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeDealersProject.Services
{
    public class DMService : IDealerMasterService
    {
        private readonly BikeDealerMgmtContext _context;
        public DMService()
        {
            
        }
        public DMService(BikeDealerMgmtContext context)
        {
            _context = context;
        }

        public async Task<int> AddDealerMaster(DealerMaster dm)
        {
            _context.DealerMasters.Add(dm);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddDealerMastersBulk(List<DealerMaster> dealerMasters)
        {
            await _context.DealerMasters.AddRangeAsync(dealerMasters);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteDM(int id)
        {
            DealerMaster dm = await _context.DealerMasters.FindAsync(id);
            if (dm == null) return 0;
            _context.DealerMasters.Remove(dm);
            return await _context.SaveChangesAsync();
        }

        public async Task<DealerMaster> FindDMById(int id)
        {
            DealerMaster dm = await _context.DealerMasters.FirstOrDefaultAsync(d => d.DealerMasterId == id);
            return dm;
        }

        public async Task<List<DealerMaster>> GetDMs()
        {
            List<DealerMaster> lst =  await _context.DealerMasters.ToListAsync();
            return lst;
        }

        public async Task<int> UpdateDM(int id, DealerMaster dm)
        {
            DealerMaster dd = await _context.DealerMasters.FindAsync(id);
            if (dd == null) return 0;
            dd.DealerId = dm.DealerId;
            dd.BikeId = dm.BikeId;
            dd.BikesDelivered = dm.BikesDelivered;
            dd.DeliveryDate = dm.DeliveryDate;
            return await _context.SaveChangesAsync();
        }
    }
}
