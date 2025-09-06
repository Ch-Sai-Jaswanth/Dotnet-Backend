using BikeDealersProject.AuthModels;
using BikeDealersProject.Models;
namespace BikeDealersProject.Services
{
    public interface IAuthService
    {
        Task<(int, string)> RegisterAsync(RegisterModel model);
        Task<(int, string, IEnumerable<string>)> LoginAsync(LoginModel model);
    }

    public interface IBikeService
    {
        Task<int> AddBike(BikeStore bike);
        Task<int> AddBikesBulk(List<BikeStore> bikes);
        Task<BikeStore> UpdateBike(int id, BikeStore bike);
        Task<int> DeleteBike(int id);
        Task<List<BikeStore>> GetBikes();
        Task<BikeStore> FindBikeById(int id);
        Task<BikeStore> FindBikeByName(string name);
        Task<bool> Exists(int id);
    }
    public interface IDealerService
    {
        Task<int> AddDealer(Dealer dealer);
        Task<int> AddDealersBulk(List<Dealer> dealers);
        Task<List<Dealer>> GetDealers();
        Task<Dealer> FindDealerById(int id);
        Task<Dealer> FindDealerByName(string name);
        Task<Dealer> UpdateDealer(int id, Dealer dealer);
        Task<int> DeleteDealer(int id);
        Task<bool> Exists(int id);
    }
    public interface IDealerMasterService
    {
        Task<int> AddDealerMaster(DealerMaster dm);
        Task<int> AddDealerMastersBulk(List<DealerMaster> dealerMasters);
        Task<List<DealerMaster>> GetDMs();
        Task<DealerMaster> FindDMById(int id);
        Task<DealerMaster> UpdateDM(int id, DealerMaster dm);
        Task<int> DeleteDM(int id);
        Task<bool> Exists(int id);
    }
}
