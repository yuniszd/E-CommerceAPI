using E_CommerceAPI.Application.Abstracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_CommerceAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // ✅ POST /api/orders
        [HttpPost]
        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> CreateOrder([FromBody] int productId)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(productId);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ GET /api/orders/my
        [HttpGet("my")]
        public async Task<IActionResult> GetMyOrders()
        {
            var orders = await _orderService.GetMyOrdersAsync();
            return Ok(orders);
        }

        // ✅ GET /api/orders/my-sales
        [HttpGet("my-sales")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetMySales()
        {
            var orders = await _orderService.GetMySalesAsync();
            return Ok(orders);
        }

        // ✅ GET /api/orders/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound("Order not found or access denied.");

            return Ok(order);
        }
    }
}
