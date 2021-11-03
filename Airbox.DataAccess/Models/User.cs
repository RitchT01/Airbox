using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airbox.DataAccess.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Location> Locations { get; set; }
    }
}
