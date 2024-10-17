// File: IProductService.cs
// Date : 08/10/2024
// This interface defines the operations for managing products in the WebApi application. It includes methods for retrieving, creating, updating, and deleting product data.

using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();

        Task<IEnumerable<Product>> GetProductsByVendorID(Guid id);

        Task<Product> GetProductById(string id);

        Task CreateProduct(Product product);

        Task<string> UpdateProduct(Product product);

        Task<string> DeleteProduct(string id, Guid userId);
    }
}
