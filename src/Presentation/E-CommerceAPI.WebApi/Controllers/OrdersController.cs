using E_CommerceAPI.Application.Abstracts.Services;
using E_CommerceAPI.Application.Shared;
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

        [HttpPost]
        [Authorize(Policy = Permissions.Orders.Create)]
        public async Task<IActionResult> CreateOrder([FromBody] Guid productId)
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

        [HttpGet("my")]
        [Authorize(Policy = Permissions.Orders.GetMy)]
        public async Task<IActionResult> GetMyOrders()
        {
            var orders = await _orderService.GetMyOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("my-sales")]
        [Authorize(Policy = Permissions.Orders.GetMy)]
        public async Task<IActionResult> GetMySales()
        {
            var orders = await _orderService.GetMySalesAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Permissions.Orders.GetMySales)]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound("Order not found or access denied.");

            return Ok(order);
        }
    }
}
