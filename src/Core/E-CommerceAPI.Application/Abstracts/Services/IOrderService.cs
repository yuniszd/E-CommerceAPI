using E_CommerceAPI.Domain.Entities;

namespace E_CommerceAPI.Application.Abstracts.Services;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(Guid productId);
    Task<List<Order>> GetMyOrdersAsync();
    Task<List<Order>> GetMySalesAsync();
    Task<Order?> GetOrderByIdAsync(Guid id);
}
