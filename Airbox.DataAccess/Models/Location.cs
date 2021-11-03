using System;
using System.ComponentModel.DataAnnotations;

namespace Airbox.DataAccess.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }
    }
}
