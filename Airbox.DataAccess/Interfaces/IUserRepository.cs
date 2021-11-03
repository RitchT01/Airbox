using Airbox.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airbox.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task AddLocationAsync(int userId, Location location);

        Task<Location> GetCurrentLocationAsync(int userId);

        Task<IEnumerable<Location>> GetLocationsAsync(int userId);

        Task<IEnumerable<User>> GetUsersAsync();
    }
}
