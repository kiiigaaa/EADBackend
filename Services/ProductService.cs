// FileName: ProductService.cs
// Date : 08/10/2024
// This service class provides methods for managing products in the MongoDB database,
// including CRUD operations and aggregation queries. It implements the IProductService interface.

using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApi.Configurations;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _products;
        public IOptions<DatabaseSettings> _databaseSettings;

        public ProductService(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings;

            // Initialize MongoDb client
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

            // Connect to the MongoDb database
            var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _products = mongoDb.GetCollection<Product>(databaseSettings.Value.ProductCollectionName);
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            try
            {
                // Pipeline for aggregation query
                var pipeline = new BsonDocument[]
                {
                    new("$match", new BsonDocument("IsActive", true)),
                    new("$lookup", new BsonDocument
                    {
                        { "from", "Categories" },
                        { "localField", "CategoryId" },
                        { "foreignField", "_id" },
                        { "as", "product_category" }
                    }),
                    new("$unwind", "$product_category"),
                    new("$project", new BsonDocument
                    {
                        { "_id", 1 },
                        { "Name", 1 },
                        { "Description", 1 },
                        { "Price", 1 },
                        { "Quantity", 1 },
                        { "ImageUrls", 1 },
                        { "CreatedBy", 1 },
                        { "CreatedOn", 1 },
                        { "ModifiedBy", 1 },
                        { "ModifiedOn", 1 },
                        { "IsActive", 1 },
                        { "CategoryId", 1 },
                        { "CategoryName", "$product_category.CategoryName" },
                    })
                };

                var products = await _products.Aggregate<Product>(pipeline).ToListAsync();

                if (products != null)
                {
                    return products;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetProductsByVendorID(Guid id)
        {
            try
            {
                // Convert Guid to string for matching
                var customerIdString = id.ToString();

                var pipeline = new BsonDocument[]
                {
                    new("$match", new BsonDocument
                    {
                        { "CreatedBy", customerIdString },
                        { "IsActive", true }
                    }),
                    new("$lookup", new BsonDocument
                    {
                        { "from", "Categories" },
                        { "localField", "CategoryId" },
                        { "foreignField", "_id" },
                        { "as", "product_category" }
                    }),
                    new("$unwind", "$product_category"),
                    new("$project", new BsonDocument
                    {
                        { "_id", 1 },
                        { "Name", 1 },
                        { "Description", 1 },
                        { "Price", 1 },
                        { "Quantity", 1 },
                        { "ImageUrls", 1 },
                        { "CreatedBy", 1 },
                        { "CreatedOn", 1 },
                        { "ModifiedBy", 1 },
                        { "ModifiedOn", 1 },
                        { "IsActive", 1 },
                        { "CategoryId", 1 },
                        { "CategoryName", "$product_category.CategoryName" },
                    })
                };

                var products = await _products.Aggregate<Product>(pipeline).ToListAsync();

                if (products != null)
                {
                    return products;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Product> GetProductById(string id)
        {
            try
            {
                var productDetails = await _products.Find(p => p.Id == id && p.IsActive == true).FirstOrDefaultAsync();

                if (productDetails != null)
                {
                    return productDetails;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CreateProduct(Product product)
        {
            try
            {
                product.CreatedOn = DateTime.UtcNow;
                product.ModifiedOn = DateTime.UtcNow;
                product.IsActive = true;
                product.CategoryName = null;
                await _products.InsertOneAsync(product);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> UpdateProduct(Product product)
        {
            try
            {
                var productDetails = await _products.Find(p => p.Id == product.Id).FirstOrDefaultAsync();

                if (productDetails != null)
                {
                    var updatedProductDetails = Builders<Product>.Update
                    .Set(p => p.Name, product.Name)
                    .Set(p => p.Description, product.Description)
                    .Set(p => p.Price, product.Price)
                    .Set(p => p.Quantity, product.Quantity)
                    .Set(p => p.ImageUrls, product.ImageUrls)
                    .Set(p => p.ModifiedBy, product.ModifiedBy)
                    .Set(p => p.ModifiedOn, DateTime.UtcNow)
                    .Set(p => p.CategoryId, product.CategoryId);

                    var result = await _products.UpdateOneAsync(p => p.Id == product.Id, updatedProductDetails);

                    if (result.ModifiedCount > 0)
                    {
                        return "success";
                    }
                    else
                    {
                        return "failed";
                    }
                }
                else
                {
                    return "not-found";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> DeleteProduct(string id, Guid userId)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(p => p.Id, id);

                if (filter == null)
                {
                    return "not-found";
                }
                else
                {
                    var update = Builders<Product>.Update
                    .Set(p => p.IsActive, false)
                    .Set(p => p.ModifiedOn, DateTime.UtcNow)
                    .Set(p => p.ModifiedBy, userId);

                    var result = await _products.UpdateOneAsync(filter, update);

                    if (result.ModifiedCount > 0)
                    {
                        return "success";
                    }
                    else
                    {
                        return "failed";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}