using Airbox.DataAccess.Interfaces;
using Airbox.DataAccess.Models;
using Airbox.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Airbox.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // api/user/addlocation/1
        // requestBody...
        //{
        //    "DateTime" : "2021-11-03T12:00:00",
        //    "Longitude" : 16.089764,
        //    "Latitude" : 89.078695
        //}
        [HttpPost("addLocation/{userId}")]
        public async Task<ActionResult> AddLocation(string userId, [FromBody] Location location)
        {
            if (!ValidateLatitide(location.Latitude) || !ValidateLongitude(location.Longitude))
            {
                return BadRequest();
            }

            await _userRepository.AddLocationAsync(int.Parse(userId), location).ConfigureAwait(false);
            return NoContent();
        }

        // api/user/currentlocation/1
        [HttpGet("currentLocation/{userId}")]
        public async Task<ActionResult<Location>> GetCurrentLocation(string userId)
        {
            var location = await _userRepository.GetCurrentLocationAsync(int.Parse(userId)).ConfigureAwait(false);
            if (location == null)
            {
                return NotFound();
            }

            return location;
        }

        // api/user/locationhistory/1
        [HttpGet("locationHistory/{userId}")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocationHistory(string userId)
        {
            var locations = await _userRepository.GetLocationsAsync(int.Parse(userId)).ConfigureAwait(false);
            if (locations == null)
            {
                return NotFound();
            }

            return locations.ToList();
        }

        // api/user/getCurrentLocationAllUsers
        [HttpGet("getCurrentLocationAllUsers")]
        public async Task<ActionResult<IEnumerable<UserLocation>>> GetCurrentLocationAllUsers()
        {
            var users = await _userRepository.GetUsersAsync().ConfigureAwait(false);
            if (users == null)
            {
                return NotFound();
            }

            List<UserLocation> userLocations = new List<UserLocation>();
            foreach (var user in users)
            {
                userLocations.Add(new UserLocation
                {
                    Name = user.Name,
                    location = user.Locations.LastOrDefault()
                });
            }

            return userLocations;
        }

        // api/user/getCurrentLocationAllUsersWithinArea?latitudemin=-31.87656&latitudemax=34.76543&longitudemin=12.654&longitudemax=78.90876
        [HttpGet("getCurrentLocationAllUsersWithinArea")]
        public async Task<ActionResult<IEnumerable<UserLocation>>> GetCurrentLocationAllUsersWithinArea([FromQuery] SearchArea searchArea)
        {
            if (!SearchAreaIsValid(searchArea))
            {
                return BadRequest();
            }

            var users = await _userRepository.GetUsersAsync().ConfigureAwait(false);
            if (users == null)
            {
                return NotFound();
            }

            List<UserLocation> userLocations = new List<UserLocation>();
            foreach (var user in users)
            {
                var currentLocation = user.Locations.LastOrDefault();
                if (WithinArea(searchArea, currentLocation))
                {
                    userLocations.Add(new UserLocation
                    {
                        Name = user.Name,
                        location = currentLocation
                    });
                }
            }

            return userLocations;
        }

        private static bool ValidateLatitide(double latitude)
        {
            var regex = @"^(\+|-)?(?:90(?:(?:\.0{1,6})?)|(?:[0-9]|[1-8][0-9])(?:(?:\.[0-9]{1,6})?))$";
            bool isValid = Regex.IsMatch(latitude.ToString(), regex, RegexOptions.IgnoreCase);
            return isValid;
        }

        private static bool ValidateLongitude(double longitude)
        {
            var regex = @"^(\+|-)?(?:180(?:(?:\.0{1,6})?)|(?:[0-9]|[1-9][0-9]|1[0-7][0-9])(?:(?:\.[0-9]{1,6})?))$";
            bool isValid = Regex.IsMatch(longitude.ToString(), regex, RegexOptions.IgnoreCase);
            return isValid;
        }

        private bool WithinArea(SearchArea coordinates, Location location)
        {
            if (location.Longitude < coordinates.LongitudeMin || location.Longitude > coordinates.LongitudeMax)
            {
                return false;
            }

            if (location.Latitude < coordinates.LatitudeMin || location.Latitude > coordinates.LatitudeMax)
            {
                return false;
            }

            return true;
        }

        private bool SearchAreaIsValid(SearchArea searchArea)
        {
            if (!ValidateLongitude(searchArea.LongitudeMin)
                || !ValidateLongitude(searchArea.LatitudeMax)
                || !ValidateLatitide(searchArea.LatitudeMin)
                || !ValidateLatitide(searchArea.LatitudeMax))
            {
                return false;
            }

            return true;
        }
    }
}
