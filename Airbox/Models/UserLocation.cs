using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Airbox.DataAccess.Models;

namespace Airbox.Models
{
    public class UserLocation
    {
        public string Name { get; set; }

        public Location location { get; set; }
    }
}
