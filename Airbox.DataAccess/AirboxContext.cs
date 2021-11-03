using Airbox.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Airbox.DataAccess
{
    public class AirboxContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Location> Locations { get; set; }

        public AirboxContext(DbContextOptions<AirboxContext> options) : base(options)
        {
            SeedData();
        }

        private void SeedData()
        {
            if (Users.Any())
            {
                return;
            }

            var police = new User
            {
                Name = "Police",
                Locations = new List<Location>
                {
                    new Location
                    {
                        DateTime = new DateTime(2021, 10, 31, 12, 0, 0),
                        Longitude = 17.000000,
                        Latitude = -13.000000
                    },
                    new Location
                    {
                        DateTime = new DateTime(2021, 11, 1, 12, 0, 0),
                        Longitude = 15.000000,
                        Latitude = -18.000000
                    }
                }
            };

            var coastGuard = new User
            {
                Name = "CoastGuard",
                Locations = new List<Location>
                {
                    new Location
                    {
                        DateTime = new DateTime(2021, 10, 31, 8, 0, 0),
                        Longitude = 25.000000,
                        Latitude = -3.000000
                    },
                    new Location
                    {
                        DateTime = new DateTime(2021, 11, 1, 15, 0, 0),
                        Longitude = 46.000000,
                        Latitude = 6.000000
                    },
                    new Location
                    {
                        DateTime = new DateTime(2021, 11, 2, 13, 0, 0),
                        Longitude = -6.000000,
                        Latitude = -12.000000
                    }
                }
            };

            var ambulance = new User
            {
                Name = "Ambulance",
                Locations = new List<Location>
                {
                    new Location
                    {
                        DateTime = new DateTime(2021, 10, 31, 20, 0, 0),
                        Longitude = 25.000000,
                        Latitude = -3.000000
                    }
                }
            };

            Users.AddRange(police, coastGuard, ambulance);

            SaveChanges();
        }
    }
}
