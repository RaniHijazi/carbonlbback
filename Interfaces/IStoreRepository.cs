using StoreProject.Dto;
using StoreProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreProject.Interfaces
{
    public interface IStoreRepository
    {
        Task CreateCategory(CategoryDto dto);
        Task<List<CategoryDto>?> GetAllCategories();

        Task CreateItem(ItemDto dto);
        Task<List<GetItemDto>?> GetAllItemsInStore();
        Task<List<GetItemDto>?> GetAllCategoryItems(string categoryName);
        Task<List<GetItemDto>?> FilterItemsByColor(string color);
        Task<List<GetItemDto>?> FilterItemsByPriceDescending(string categoryName);
        Task<List<GetItemDto>?> FilterItemsByPriceAescending(string categoryName);
        Task<List<GetItemDto>?> SearchItems(string searchTerm);
        Task<List<GetItemDto>?> FilterItemsByPriceRange(decimal minPrice, decimal maxPrice);

        Task<List<GetItemDto>?> FilterItemsByLabel(string label);
        Task<bool> UpdateItemPrice(int itemId, decimal newPrice);
        Task<bool> UpdateItemName(int itemId, string newName);
        Task<bool> UpdateItemDescription(int itemId, string newDescription);
        Task<bool> UpdateItemAvailabilityStatus(int itemId, bool newStatus);

        Task<bool> DeleteCategory(int categoryId);
        Task<bool> DeleteItem(int itemId);
        Task<List<GetItemDto>?> GetBestsellerItems();
        Task<List<GetItemDto>?> GetSpecialOfferItems();
        Task<List<GetItemDto>?> GetNewArrivalItems();

        Task<bool> SetItemAsBestseller(int itemId);
        Task<bool> SetItemAsSpecialOffer(int itemId);
        Task<bool> SetItemAsNewArrival(int itemId);

    }
}
