using E_CommerceAPI.Application.Abstracts.Services;
using E_CommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_CommerceAPI.Persistence.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrderService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Order> CreateOrderAsync(Guid productId)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            throw new Exception("Product not found.");


        var order = new Order
        {
            ProductId = productId,
            BuyerId = userId,
            OrderDate = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<List<Order>> GetMyOrdersAsync()
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _context.Orders
            .Include(o => o.Product)
            .Where(o => o.BuyerId == userId)
            .ToListAsync();
    }

    public async Task<List<Order>> GetMySalesAsync()
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _context.Orders
            .Include(o => o.Product)
            .Where(o => o.Product.OwnerId == userId)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(Guid id)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var order = await _context.Orders
            .Include(o => o.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return null;

        if (order.BuyerId != userId && order.Product.OwnerId != userId)
            return null; 

        return order;
    }
}

