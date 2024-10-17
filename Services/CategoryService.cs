// File Name: CategoryService.cs
// Date : 08/10/2024
// This service class provides methods for managing categories, including
// fetching, creating, updating, and deleting categories in a MongoDB collection.

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApi.Configurations;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categories;
        public IOptions<DatabaseSettings> _databaseSettings;

        public CategoryService(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings;

            // Initialize MongoDb client
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

            // Connect to the MongoDb database
            var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _categories = mongoDb.GetCollection<Category>(databaseSettings.Value.CategoryCollectionName);
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            try
            {
                var categories = await _categories.Find(_ => true).ToListAsync();

                if (categories != null)
                {
                    return categories;
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

        public async Task<IEnumerable<Category>> GetActiveCategories()
        {
            try
            {
                var categories = await _categories.Find(p => p.IsActive == true).ToListAsync();

                if (categories != null)
                {
                    return categories;
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

        public async Task<Category> GetCategoryById(string id)
        {
            try
            {
                var categoryDetails = await _categories.Find(p => p.Id == id && p.IsActive == true).FirstOrDefaultAsync();

                if (categoryDetails != null)
                {
                    return categoryDetails;
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

        public async Task CreateCategory(Category category)
        {
            try
            {
                category.CreatedOn = DateTime.UtcNow;
                category.ModifiedOn = DateTime.UtcNow;
                category.IsActive = true;
                await _categories.InsertOneAsync(category);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> UpdateCategory(Category category)
        {
            try
            {
                var categoryDetails = await _categories.Find(p => p.Id == category.Id).FirstOrDefaultAsync();

                if (categoryDetails != null)
                {
                    category.ModifiedOn = DateTime.UtcNow;
                    await _categories.FindOneAndReplaceAsync(p => p.Id == category.Id, category);
                    return "success";
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

        public async Task<string> UpdateCategoryStatus(Category category)
        {
            try
            {
                var filter = Builders<Category>.Filter.Eq(p => p.Id, category.Id);

                if (filter == null)
                {
                    return "not-found";
                }
                else
                {
                    var update = Builders<Category>.Update
                    .Set(p => p.IsActive, category.IsActive)
                    .Set(p => p.ModifiedOn, DateTime.UtcNow)
                    .Set(p => p.ModifiedBy, category.ModifiedBy);

                    var result = await _categories.UpdateOneAsync(filter, update);

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

        public async Task<string> DeleteCategory(string id, Guid userId)
        {
            try
            {
                var filter = Builders<Category>.Filter.Eq(p => p.Id, id);

                if (filter == null)
                {
                    return "not-found";
                }
                else
                {
                    var update = Builders<Category>.Update
                    .Set(p => p.IsActive, false)
                    .Set(p => p.ModifiedOn, DateTime.UtcNow)
                    .Set(p => p.ModifiedBy, userId);

                    var result = await _categories.UpdateOneAsync(filter, update);

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
