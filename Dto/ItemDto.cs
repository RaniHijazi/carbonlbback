namespace StoreProject.Dto
{
    public class ItemDto
    {
        
        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }
        public string Color { get; set; }
        public bool AvailabilityStatus { get; set; }
        public string CategoryName { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
