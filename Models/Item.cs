using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StoreProject.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string?Description { get; set; }

        public bool Bestseller { get; set; } = false;

        public bool SpecialOffer { get; set; } = false;

        public bool NewArrival { get; set; } = false;

        public decimal Price { get; set; }
        public string Color { get; set; }
        public string? ImageUrl { get; set; }
        public bool AvailabilityStatus { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }  
    }
}
