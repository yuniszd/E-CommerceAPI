using E_CommerceAPI.Application.Abstracts.Services;
using E_CommerceAPI.Application.DTOs.ProductDTOs;
using E_CommerceAPI.Domain.Entities;
using E_CommerceAPI.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_CommerceAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Guid? categoryId, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] string search)
        {
            var products = await _service.GetAllAsync(categoryId, minPrice, maxPrice, search);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _service.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            if (!created)
                return Forbid();

            return Ok("Product created successfully.");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductCreateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (!result)
                return Forbid();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return Forbid();

            return NoContent();
        }

        [HttpGet("my")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetMyProducts()
        {
            var products = await _service.GetMyProductsAsync();
            return Ok(products);
        }
    }
}
