using BikeDealersProject.Models;
using Microsoft.AspNetCore.Mvc;
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
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 1. Add the DealerMaster record
                    _context.DealerMasters.Add(dm);
                    var result = await _context.SaveChangesAsync(); // Save to get the DealerMasterId if it's database-generated

                    // 2. Update the dealer's inventory
                    await UpdateDealerInventoryOnAdd(dm);

                    // 3. Commit the transaction if everything was successful
                    await transaction.CommitAsync();
                    return result;
                }
                catch (Exception)
                {
                    // 4. Roll back the transaction if anything fails
                    await transaction.RollbackAsync();
                    throw; // Re-throw the exception to be handled by the controller
                }
            }
            //_context.DealerMasters.Add(dm);
            //return await _context.SaveChangesAsync();
        }

        public async Task<int> AddDealerMastersBulk(List<DealerMaster> dealerMasters)
        {
            await _context.DealerMasters.AddRangeAsync(dealerMasters);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteDM(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 1. Find the record first so we know how many bikes to subtract
                    DealerMaster dm = await _context.DealerMasters.FindAsync(id);
                    if (dm == null) return 0;

                    // 2. Update the inventory BEFORE deleting the record
                    // (We need the data from the record to know what to subtract)
                    await UpdateDealerInventoryOnDelete(dm.DealerMasterId);

                    // 3. Now remove the record and save
                    _context.DealerMasters.Remove(dm);
                    var result = await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return result;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            //DealerMaster dm = await _context.DealerMasters.FindAsync(id);
            //if (dm == null) return 0;
            //_context.DealerMasters.Remove(dm);
            //return await _context.SaveChangesAsync();
        }

        public async Task<DealerMaster> FindDMById(int id)
        {
            DealerMaster dm = await _context.DealerMasters.FirstOrDefaultAsync(d => d.DealerMasterId == id);
            return dm;
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.DealerMasters.AnyAsync(d => d.DealerMasterId == id);
        }

        public async Task<List<DealerMaster>> GetDMs()
        {
            List<DealerMaster> lst =  await _context.DealerMasters.ToListAsync();
            return lst;
        }

        public async Task<DealerMaster> UpdateDM(int id, DealerMaster dm)
        {
            DealerMaster dd = await _context.DealerMasters.FindAsync(id);
            if (dd == null) return null;
            dd.DealerId = dm.DealerId;
            dd.BikeId = dm.BikeId;
            dd.BikesDelivered = dm.BikesDelivered;
            dd.DeliveryDate = dm.DeliveryDate;
            await _context.SaveChangesAsync();
            return dd;
        }
        public async Task<int> UpdateDealerInventoryOnAdd(DealerMaster dm)
        {
            // Find the dealer associated with this DealerMaster
            var dealer = await _context.Dealers.FindAsync(dm.DealerId);
            if (dealer == null)
            {
                throw new ArgumentException($"Dealer with ID {dm.DealerId} not found.");
            }

            // If Inventory is null, initialize it to 0 before adding
            dealer.Inventory ??= 0;
            dealer.Inventory += dm.BikesDelivered ?? 0; // Add the bikes delivered

            _context.Dealers.Update(dealer);
            return await _context.SaveChangesAsync();
        }

        // ** NEW METHOD: Updates inventory when a DealerMaster is DELETED
        public async Task<int> UpdateDealerInventoryOnDelete(int dealerMasterId)
        {
            // Find the DealerMaster record to get the data
            var dm = await _context.DealerMasters.FindAsync(dealerMasterId);
            if (dm == null)
            {
                throw new ArgumentException($"DealerMaster record with ID {dealerMasterId} not found.");
            }

            // Find the associated dealer
            var dealer = await _context.Dealers.FindAsync(dm.DealerId);
            if (dealer == null)
            {
                throw new ArgumentException($"Dealer with ID {dm.DealerId} not found.");
            }

            // Subtract the bikes that were delivered in this record
            // Ensure inventory doesn't go negative (logic depends on your business rules)
            if (dealer.Inventory.HasValue && dm.BikesDelivered.HasValue)
            {
                dealer.Inventory = Math.Max(0, dealer.Inventory.Value - dm.BikesDelivered.Value);
            }

            _context.Dealers.Update(dealer);
            return await _context.SaveChangesAsync();
        }
    }
}
