// File: ICategoryService.cs
// Date : 08/10/2024
// This interface defines the operations for managing
// categories in the WebApi application. It includes methods 
// for retrieving, creating, updating, and deleting category data.

using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategories();

        Task<IEnumerable<Category>> GetActiveCategories();

        Task<Category> GetCategoryById(string id);

        Task CreateCategory(Category category);

        Task<string> UpdateCategory(Category category);

        Task<string> UpdateCategoryStatus(Category category);

        Task<string> DeleteCategory(string id, Guid userId);
    }
}
