using Microsoft.AspNetCore.Mvc;
using StoreProject.Dto;
using StoreProject.Interfaces;

namespace StoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreRepository _storeRepository;

        public StoreController(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            await _storeRepository.CreateCategory(categoryDto);
            return Ok("Category created successfully");
        }

        
        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _storeRepository.GetAllCategories();
            if (categories == null)
                return NotFound("No categories found.");
            return Ok(categories);
        }

        
        [HttpPost("items")]
        public async Task<IActionResult> CreateItem([FromForm] ItemDto itemDto)
        {
            try
            {
                await _storeRepository.CreateItem(itemDto);
                return Ok("Item created successfully");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetAllItemsInStore()
        {
            var items = await _storeRepository.GetAllItemsInStore();
            if (items == null)
                return NotFound("No items found.");
            return Ok(items);
        }

        
        [HttpGet("items/category/{categoryName}")]
        public async Task<IActionResult> GetItemsByCategory(string categoryName)
        {
            var items = await _storeRepository.GetAllCategoryItems(categoryName);
            if (items == null)
                return NotFound($"No items found in category '{categoryName}'.");
            return Ok(items);
        }

       
        [HttpGet("items/filter/color")]
        public async Task<IActionResult> FilterItemsByColor([FromQuery] string color)
        {
            var items = await _storeRepository.FilterItemsByColor( color);
            if (items == null)
                return NotFound("No items found with the specified criteria.");
            return Ok(items);
        }

        
        [HttpGet("items/filter/price-descending/{categoryName}")]
        public async Task<IActionResult> FilterItemsByPriceDescending(string categoryName)
        {
            var items = await _storeRepository.FilterItemsByPriceDescending(categoryName);
            if (items == null)
                return NotFound("No items found with the specified criteria.");
            return Ok(items);
        }
        [HttpGet("items/filter/price-ascending/{categoryName}")]
        public async Task<IActionResult> FilterItemsByPriceAscending(string categoryName)
        {
            var items = await _storeRepository.FilterItemsByPriceAescending(categoryName);
            if (items == null)
                return NotFound("No items found with the specified criteria.");
            return Ok(items);
        }

        [HttpGet("items/search")]
        public async Task<IActionResult> SearchItems([FromQuery] string searchTerm)
        {
            var items = await _storeRepository.SearchItems(searchTerm);
            if (items == null)
                return NotFound("No items matched your search.");
            return Ok(items);
        }

       
        [HttpPut("items/{itemId}/price")]
        public async Task<IActionResult> UpdateItemPrice(int itemId, [FromBody] decimal newPrice)
        {
            var updated = await _storeRepository.UpdateItemPrice(itemId, newPrice);
            if (!updated)
                return NotFound("Item not found.");
            return Ok("Price updated successfully.");
        }

       
        [HttpPut("items/{itemId}/name")]
        public async Task<IActionResult> UpdateItemName(int itemId, [FromBody] string newName)
        {
            var updated = await _storeRepository.UpdateItemName(itemId, newName);
            if (!updated)
                return NotFound("Item not found.");
            return Ok("Name updated successfully.");
        }

        
        [HttpDelete("categories/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var deleted = await _storeRepository.DeleteCategory(categoryId);
            if (!deleted)
                return NotFound("Category not found.");
            return Ok("Category deleted successfully.");
        }

       
        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            var deleted = await _storeRepository.DeleteItem(itemId);
            if (!deleted)
                return NotFound("Item not found.");
            return Ok("Item deleted successfully.");
        }

        [HttpGet("items/filter/price")]
        public async Task<IActionResult> FilterItemsByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            var items = await _storeRepository.FilterItemsByPriceRange(minPrice, maxPrice);
            if (items == null || !items.Any())
                return NotFound("No items found in the specified price range.");

            return Ok(items);
        }


        [HttpGet("bestsellers")]
        public async Task<ActionResult<List<GetItemDto>>> GetBestsellerItems()
        {
            var items = await _storeRepository.GetBestsellerItems();
            if (items == null || !items.Any())
            {
                return NotFound("No bestseller items found.");
            }
            return Ok(items);
        }

     
        [HttpGet("special-offers")]
        public async Task<ActionResult<List<GetItemDto>>> GetSpecialOfferItems()
        {
            var items = await _storeRepository.GetSpecialOfferItems();
            if (items == null || !items.Any())
            {
                return NotFound("No special offer items found.");
            }
            return Ok(items);
        }

     
        [HttpGet("new-arrivals")]
        public async Task<ActionResult<List<GetItemDto>>> GetNewArrivalItems()
        {
            var items = await _storeRepository.GetNewArrivalItems();
            if (items == null || !items.Any())
            {
                return NotFound("No new arrival items found.");
            }
            return Ok(items);
        }

        [HttpPut("set-bestseller/{itemId}")]
        public async Task<IActionResult> SetItemAsBestseller(int itemId)
        {
            var result = await _storeRepository.SetItemAsBestseller(itemId);
            if (!result)
                return NotFound($"Item with ID {itemId} not found.");

            return Ok("Item marked as Bestseller.");
        }

        [HttpPut("set-special-offer/{itemId}")]
        public async Task<IActionResult> SetItemAsSpecialOffer(int itemId)
        {
            var result = await _storeRepository.SetItemAsSpecialOffer(itemId);
            if (!result)
                return NotFound($"Item with ID {itemId} not found.");

            return Ok("Item marked as Special Offer.");
        }

        [HttpPut("set-new-arrival/{itemId}")]
        public async Task<IActionResult> SetItemAsNewArrival(int itemId)
        {
            var result = await _storeRepository.SetItemAsNewArrival(itemId);
            if (!result)
                return NotFound($"Item with ID {itemId} not found.");

            return Ok("Item marked as New Arrival.");
        }

        [HttpGet("items/filter/label")]
        public async Task<IActionResult> FilterItemsByLabel([FromQuery] string label)
        {
            if (string.IsNullOrWhiteSpace(label))
                return BadRequest("Label parameter is required.");

            var items = await _storeRepository.FilterItemsByLabel(label);

            if (items == null || !items.Any())
                return NotFound($"No items found for the label: {label}.");

            return Ok(items);
        }


    }
}
