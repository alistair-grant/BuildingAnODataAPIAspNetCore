using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirVinyl.Entities
{
    public class RecordStore
    {
        [Key]
        public int RecordStoreId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public Address StoreAddress { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
