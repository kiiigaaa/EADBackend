// OrderService.cs
// Date : 08/10/2024
// This class provides services related to order management, including operations 
// for handling cart items, creating orders, and managing order statuses.
// It interacts with MongoDB collections for data storage and retrieval.

using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApi.Configurations;
using WebApi.Dto;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMongoCollection<CartItem> _cartItems;
        private readonly IMongoCollection<Status> _status;
        private readonly IMongoCollection<Order> _orders;
        private readonly IMongoCollection<Product> _products;
        public IOptions<DatabaseSettings> _databaseSettings;

        public OrderService(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings;

            // Initialize MongoDb client
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

            // Connect to the MongoDb database
            var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _cartItems = mongoDb.GetCollection<CartItem>(databaseSettings.Value.CartItemCollectionName);
            _status = mongoDb.GetCollection<Status>(databaseSettings.Value.StatusCollectionName);
            _products = mongoDb.GetCollection<Product>(databaseSettings.Value.ProductCollectionName);
            _orders = mongoDb.GetCollection<Order>(databaseSettings.Value.OrderCollectionName);
        }

        public async Task<IEnumerable<CartItemDto>> GetCartItemsByCustomerId(Guid id)
        {
            try
            {
                // Convert Guid to string for matching
                var customerIdString = id.ToString();

                var pipeline = new BsonDocument[]
                {
                    new("$match", new BsonDocument
                    {
                        { "CustomerId", customerIdString },
                        { "IsActive", true }
                    }),
                    new("$lookup", new BsonDocument
                    {
                        { "from", "Products" },
                        { "localField", "ProductId" },
                        { "foreignField", "_id" },
                        { "as", "product_details" }
                    }),
                    new("$unwind", "$product_details"),
                    new("$project", new BsonDocument
                    {
                        { "_id", 1 },
                        { "CustomerId", 1 },
                        { "ProductId", 1 },
                        { "Quantity", 1 },
                        { "CreatedBy", 1 },
                        { "CreatedOn", 1 },
                        { "ModifiedBy", 1 },
                        { "ModifiedOn", 1 },
                        { "IsActive", 1 },
                        { "Price", "$product_details.Price" },
                        { "ProductName", "$product_details.Name" },
                        { "ImageUrls", "$product_details.ImageUrls" }
                    })
                };

                var cartItems = await _cartItems.Aggregate<CartItemDto>(pipeline).ToListAsync();


                if (cartItems != null)
                {
                    return cartItems;
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

        public async Task CreateCartItem(CartItem cartItem)
        {
            try
            {
                cartItem.CreatedOn = DateTime.UtcNow;
                cartItem.ModifiedOn = DateTime.UtcNow;
                cartItem.IsActive = true;
                await _cartItems.InsertOneAsync(cartItem);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> RemoveCartItem(Guid customerId, string cartItemId)
        {
            try
            {
                var filter = Builders<CartItem>.Filter.Eq(p => p.Id, cartItemId);

                if (filter == null)
                {
                    return "not-found";
                }
                else
                {
                    var update = Builders<CartItem>.Update
                    .Set(p => p.IsActive, false)
                    .Set(p => p.ModifiedOn, DateTime.UtcNow)
                    .Set(p => p.ModifiedBy, customerId);

                    var result = await _cartItems.UpdateOneAsync(filter, update);

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateOrderStatus(Status status)
        {
            try
            {
                status.CreatedOn = DateTime.UtcNow;
                status.ModifiedOn = DateTime.UtcNow;
                status.IsActive = true;
                await _status.InsertOneAsync(status);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Status>> GetActiveStatuses()
        {
            try
            {
                var statuses = await _status.Find(p => p.IsActive == true).ToListAsync();

                if (statuses != null)
                {
                    return statuses;
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

        public async Task<string> CreateOrder(Order order)
        {
            var status = await _status.Find(s => s.Name == "Processing").FirstOrDefaultAsync();

            order.CreatedOn = DateTime.UtcNow;
            order.CurrentStatusId = status.Id;
            order.ModifiedOn = DateTime.UtcNow;
            order.IsActive = true;
            await _orders.InsertOneAsync(order);

            //foreach (var item in order.Products)
            //{
            //    var product = await _products.Find(p => p.Id == item.ProductId).FirstOrDefaultAsync();
            //    var update = Builders<Product>.UpdatGetCustomerOrdersByIde.Set(p => p.Quantity, product.Quantity - item.Quantity);
            //    await _products.UpdateOneAsync(p => p.Id == item.ProductId, update);
            //}
            return "order-created-successfully";
        }

        public async Task<IEnumerable<OrderDto>> GetCustomerOrdersById(Guid id)
        {
            try
            {
                // Convert Guid to string for matching
                var customerIdString = id.ToString();

                var pipeline = new[]
                {
                    new("$match", new BsonDocument
                    {
                        { "CustomerId", customerIdString },
                        { "IsActive", true }
                    }),
                    new BsonDocument("$lookup", new BsonDocument
                    {
                        { "from", "Statuses" },
                        { "localField", "CurrentStatusId" },
                        { "foreignField", "_id" },
                        { "as", "status" }
                    }),
                    new BsonDocument("$unwind", new BsonDocument("path", "$status")),
                    new BsonDocument("$project", new BsonDocument
                    {
                        { "OrderId", 1 },
                        { "CustomerId", 1 },
                        { "ItemCount", 1 },
                        { "Products", 1 },
                        { "TotalPrice", 1 },
                        { "CreatedBy", 1 },
                        { "CreatedOn", 1 },
                        { "ModifiedBy", 1 },
                        { "ModifiedOn", 1 },
                        { "IsActive", 1 },
                        { "Status", "$status.Name" },
                        { "StatusDescription", "$status.Description" },
                    })
                };

                var customerOrders = await _orders.Aggregate<OrderDto>(pipeline).ToListAsync();


                if (customerOrders != null)
                {
                    return customerOrders;
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

        public async Task<string> DeleteOrder(Guid customerId, string orderId)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(p => p.Id, orderId);

                if (filter == null)
                {
                    return "not-found";
                }
                else
                {
                    var update = Builders<Order>.Update
                    .Set(p => p.IsActive, false)
                    .Set(p => p.ModifiedOn, DateTime.UtcNow)
                    .Set(p => p.ModifiedBy, customerId);

                    var result = await _orders.UpdateOneAsync(filter, update);

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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
