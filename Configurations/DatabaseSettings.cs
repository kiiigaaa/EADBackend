// File Name : ApiConstant.cs
// Date : 08/10/2024
// This class defines the settings required for connecting to the database in the WebApi application. It includes properties for the connection string, database name, and collection names for various data entities such as products, categories, roles, users, cart items, status, and orders.

namespace WebApi.Configurations
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;

        public string DatabaseName { get; set; } = string.Empty;

        public string ProductCollectionName { get; set; } = string.Empty;

        public string CategoryCollectionName { get; set; } = string.Empty;

        public string RolesCollectionName { get; set; } = string.Empty;

        public string UsersCollectionName { get; set; } = string.Empty;

        public string CartItemCollectionName { get; set; } = string.Empty;

        public string StatusCollectionName { get; set; } = string.Empty;

        public string OrderCollectionName { get; set; } = string.Empty;
    }
}
