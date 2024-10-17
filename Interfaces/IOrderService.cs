// File : IOrderService.cs
// Date : 08/10/2024
// This interface defines the operations for managing orders and cart items in the WebApi application. It includes methods for       retrieving cart items, creating orders, and managing order statuses.

using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<CartItemDto>> GetCartItemsByCustomerId(Guid id);

        Task CreateCartItem(CartItem cartItem);

        Task<string> RemoveCartItem(Guid customerId, string cartItemId);

        Task CreateOrderStatus(Status status);

        Task<IEnumerable<Status>> GetActiveStatuses();

        Task<string> CreateOrder(Order order);

        Task<IEnumerable<OrderDto>> GetCustomerOrdersById(Guid id);

        Task<string> DeleteOrder(Guid customerId, string orderId);
    }
}
