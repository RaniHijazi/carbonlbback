namespace StoreProject.Dto
{
    public class GetItemDto
    {
        public int id { get; set; }
        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }
        public string Color { get; set; }
        public bool AvailabilityStatus { get; set; }
        public string CategoryName { get; set; }
        public string? ImageUrl { get; set; }
    }
    
}
