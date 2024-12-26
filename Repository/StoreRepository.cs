using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using StoreProject.Data;
using StoreProject.Dto;
using StoreProject.Interfaces;
using StoreProject.Models;
using Microsoft.AspNetCore.Hosting;


namespace StoreProject.Repository
{
    public class StoreRepository : IStoreRepository
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StoreRepository(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }


        public async Task CreateCategory(CategoryDto dto)
        {
            var Category = new Category
            {
                Name = dto.Name,
            };
            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();

        }

        public async Task<List<CategoryDto>?> GetAllCategories()
        {
            var AllCategories = await _context.Categories.Select(c => new CategoryDto
            {
                Name = c.Name,

            }).ToListAsync();

            return AllCategories.Any() ? AllCategories : null;
        }



        public async Task CreateItem(ItemDto dto)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == dto.CategoryName);
            if (category == null)
            {
                throw new InvalidOperationException($"Category with name '{dto.CategoryName}' not found.");
            }

            var item = new Item
            {
                Name = dto.Name,
                Description = dto.Description,
                AvailabilityStatus = dto.AvailabilityStatus,
                Color = dto.Color,
                Price = dto.Price,
                CategoryId = category.Id,
                ImageUrl = null,
            };

            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                var webRootPath = _webHostEnvironment.WebRootPath; 
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.ImageFile.FileName);
                var filePath = Path.Combine(webRootPath, "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                item.ImageUrl = $"/images/{fileName}";
            }

            _context.Items.Add(item);
            await _context.SaveChangesAsync();
        }


        public async Task<List<GetItemDto>?> GetAllItemsInStore()
        {
            var AllItemsInStore = await _context.Items.Select(
                i => new GetItemDto
                {
                    id = i.Id, 
                    Name = i.Name,
                    Color=i.Color,
                    Price=i.Price,
                    AvailabilityStatus = i.AvailabilityStatus,  
                    Description = i.Description,
                    CategoryName=i.Category.Name,
                    ImageUrl=i.ImageUrl,


                }).ToListAsync();

            return AllItemsInStore.Any() ? AllItemsInStore : null;


        }

        public async Task<List<GetItemDto>?> GetAllCategoryItems(String CategoryName)
        {
            var Items = await _context.Items.Where(i => i.Category.Name == CategoryName)
                .Select(i => new GetItemDto
                {
                    id = i.Id,  
                    Name = i.Name,
                    Color = i.Color,
                    Price = i.Price,
                    AvailabilityStatus = i.AvailabilityStatus,
                    Description = i.Description,
                    CategoryName = i.Category.Name ,
                    ImageUrl = i.ImageUrl,
                })
        .ToListAsync();

            return Items.Any() ? Items : null;

        }

        public async Task<List<GetItemDto>?> FilterItemsByColor( string color)
        {
            var items = await _context.Items
                .Where(i => i.Color == color)
                .Select(i => new GetItemDto
                {
                    Name = i.Name,
                    Color = i.Color,
                    Price = i.Price,
                    AvailabilityStatus = i.AvailabilityStatus,
                    Description = i.Description,
                    CategoryName = i.Category.Name,
                    ImageUrl = i.ImageUrl,
                })
                .ToListAsync();

            return items.Any() ? items : null;
        }



        public async Task<List<GetItemDto>?> FilterItemsByPriceDescending(string categoryName)
        {
            var items = await _context.Items
                .Where(i => i.Category.Name == categoryName)
                .OrderByDescending(i => i.Price) 
                .Select(i => new GetItemDto
                {
                    Name = i.Name,
                    Color = i.Color,
                    Price = i.Price,
                    AvailabilityStatus = i.AvailabilityStatus,
                    Description = i.Description,
                    CategoryName = i.Category.Name,
                    ImageUrl = i.ImageUrl,
                })
                .ToListAsync();

            return items.Any() ? items : null;
        }


        public async Task<List<GetItemDto>?> FilterItemsByPriceAescending(string categoryName)
        {
            var items = await _context.Items
                .Where(i => i.Category.Name == categoryName)
                .OrderBy(i => i.Price) 
                .Select(i => new GetItemDto
                {
                    Name = i.Name,
                    Color = i.Color,
                    Price = i.Price,
                    AvailabilityStatus = i.AvailabilityStatus,
                    Description = i.Description,
                    CategoryName = i.Category.Name,
                    ImageUrl = i.ImageUrl,
                })
                .ToListAsync();

            return items.Any() ? items : null;
        }

        public async Task<List<GetItemDto>?> SearchItems(string searchTerm)
        {
            decimal parsedPrice;

            var items = await _context.Items
                .Where(i =>
                    i.Name.Contains(searchTerm) || 
                    i.Category.Name.Contains(searchTerm) || 
                    (decimal.TryParse(searchTerm, out parsedPrice) && i.Price == parsedPrice) || 
                    i.Color.Contains(searchTerm)) 
                .Select(i => new GetItemDto
                {
                    Name = i.Name,
                    Color = i.Color,
                    Price = i.Price,
                    AvailabilityStatus = i.AvailabilityStatus,
                    Description = i.Description,
                    CategoryName = i.Category.Name,
                    ImageUrl = i.ImageUrl,
                })
                .ToListAsync();

            return items.Any() ? items : null;
        }

        public async Task<bool> UpdateItemPrice(int itemId, decimal newPrice)
        {
            var item = await _context.Items.FindAsync(itemId);

            if (item == null)
                return false; 

            item.Price = newPrice;

            await _context.SaveChangesAsync();
            return true; 
        }


        public async Task<bool> UpdateItemName(int itemId, string newName)
        {
            var item = await _context.Items.FindAsync(itemId);

            if (item == null)
                return false; 

            item.Name = newName;

            
            await _context.SaveChangesAsync();
            return true; 
        }

        public async Task<bool> UpdateItemDescription(int itemId, string newDescription)
        {
            var item = await _context.Items.FindAsync(itemId);

            if (item == null)
                return false;

            item.Description = newDescription;


            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateItemAvailabilityStatus(int itemId, bool newStatus)
        {
            var item = await _context.Items.FindAsync(itemId);

            if (item == null)
                return false;

            item.AvailabilityStatus = newStatus;

            await _context.SaveChangesAsync();
            return true; 
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);

            if (category == null)
                return false; 

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true; 
        }

        public async Task<bool> DeleteItem(int itemId)
        {
            var item = await _context.Items.FindAsync(itemId);

            if (item == null)
                return false; 

            _context.Items.Remove(item); 
            await _context.SaveChangesAsync();

            return true; 
        }

        public async Task<List<GetItemDto>?> FilterItemsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var items = await _context.Items
                .Where(item => item.Price >= minPrice && item.Price <= maxPrice)
                .Select(i => new GetItemDto
                {

                    id = i.Id,
                    Name = i.Name,
                    Color = i.Color,
                    Price = i.Price,
                    AvailabilityStatus = i.AvailabilityStatus,
                    Description = i.Description,
                    CategoryName = i.Category.Name,
                    ImageUrl = i.ImageUrl,
                })
                .ToListAsync();
            return items.Any() ? items : null;
        }



        public async Task<List<GetItemDto>?> GetBestsellerItems()
        {
            var items = await _context.Items
                .Where(i => i.Bestseller) 
                .Select(i => new GetItemDto
                {

                    id = i.Id,
                    Name = i.Name,
                    Color = i.Color,
                    Price = i.Price,
                    AvailabilityStatus = i.AvailabilityStatus,
                    Description = i.Description,
                    CategoryName = i.Category.Name,
                    ImageUrl = i.ImageUrl,
                })
                .ToListAsync();

            return items.Any() ? items : null;
        }



        public async Task<List<GetItemDto>?> GetSpecialOfferItems()
        {
            var items = await _context.Items
                .Where(i => i.SpecialOffer) 
                .Select(i => new GetItemDto
                {
                    id = i.Id,
                    Name = i.Name,
                    Color = i.Color,
                    Price = i.Price,
                    AvailabilityStatus = i.AvailabilityStatus,
                    Description = i.Description,
                    CategoryName = i.Category.Name,
                    ImageUrl = i.ImageUrl,
                })
                .ToListAsync();

            return items.Any() ? items : null;
        }

        public async Task<List<GetItemDto>?> GetNewArrivalItems()
        {
            var items = await _context.Items
                .Where(i => i.NewArrival) 
                .Select(i => new GetItemDto
                {
                    id=i.Id,
                    Name = i.Name,
                    Color = i.Color,
                    Price = i.Price,
                    AvailabilityStatus = i.AvailabilityStatus,
                    Description = i.Description,
                    CategoryName = i.Category.Name,
                    ImageUrl = i.ImageUrl,
                })
                .ToListAsync();

            return items.Any() ? items : null;
        }


        public async Task<bool> SetItemAsBestseller(int itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
                return false;

            item.Bestseller = true;
            await _context.SaveChangesAsync();
            return true; 
        }

        public async Task<bool> SetItemAsSpecialOffer(int itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
                return false; 

            item.SpecialOffer = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetItemAsNewArrival(int itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
                return false; 

            item.NewArrival = true;
            await _context.SaveChangesAsync();
            return true; 
        }

        public async Task<List<GetItemDto>?> FilterItemsByLabel(string label)
        {
            IQueryable<Item> query = label.ToLower() switch
            {
                "bestseller" => _context.Items.Where(i => i.Bestseller),
                "specialoffer" => _context.Items.Where(i => i.SpecialOffer),
                "newarrival" => _context.Items.Where(i => i.NewArrival),
                _ => null
            };

            if (query == null)
                return null;

            var items = await query
                .Select(i => new GetItemDto
                {
                    id = i.Id,
                    Name = i.Name,
                    Color = i.Color,
                    Price = i.Price,
                    AvailabilityStatus = i.AvailabilityStatus,
                    Description = i.Description,
                    CategoryName = i.Category.Name,
                    ImageUrl = i.ImageUrl,
                })
                .ToListAsync();

            return items.Any() ? items : null;
        }










    }
}
