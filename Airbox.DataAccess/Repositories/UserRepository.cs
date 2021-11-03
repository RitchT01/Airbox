using Airbox.DataAccess.Interfaces;
using Airbox.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airbox.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AirboxContext _context;
        
        public UserRepository(AirboxContext context)
        {
            _context = context;
        }

        public async Task AddLocationAsync(int userId, Location location)
        {
            var user = await GetUserAsync(userId).ConfigureAwait(false);

            user.Locations.Add(location);

            await _context.SaveChangesAsync();
        }

        public async Task<Location> GetCurrentLocationAsync(int userId)
        {
            var user = await GetUserAsync(userId).ConfigureAwait(false);

            var currentLocation = user.Locations.LastOrDefault();

            return currentLocation;
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync(int userId)
        {
            var user = await GetUserAsync(userId).ConfigureAwait(false);

            return user.Locations;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var users = await _context.Users.Include(u => u.Locations).ToListAsync().ConfigureAwait(false);

            return users;
        }


        private async Task<User> GetUserAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Locations)
                .FirstOrDefaultAsync(u => u.Id == userId).ConfigureAwait(false);
        }
    }
}
