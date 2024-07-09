using Microsoft.AspNetCore.Mvc;
using ASP.Data;
using ASP.Models;
using System.Collections.Generic;
using ASP.Abstraction;
using ASP.Dto;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;

namespace ASP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _memoryCache;
        public ProductController(IProductRepository productRepository, IMemoryCache memoryCache)
        { 
          _productRepository = productRepository;
          _memoryCache = memoryCache;
        }
        [HttpPost]
        public ActionResult<int> AddProduct(ProductDto productDto)
        {
            try
            {
                var id = _productRepository.AddProduct(productDto);
                return Ok(id);
            }
            catch 
            {
                return StatusCode(409);
            }


        }
        [HttpGet("get_all_product")]
        public ActionResult<IEnumerable<Product>> GetAllProducts()
        {
            return Ok(_productRepository.GetAllProducts());

        }

        [HttpDelete("delete_product")]
        public ActionResult DeleteProduct(int id)
        {
            try
            {
                _productRepository.DeleteProduct(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ошибка при удалении продукта.");
            }
        }
        [HttpGet("export_products_csv")]
        public IActionResult ExportProductsCsv()
        {
            var products = _productRepository.GetAllProducts();
            var builder = new StringBuilder();
            builder.AppendLine("Id,Name,Price,Description,ProductGroupId");

            foreach (var product in products)
            {
                builder.AppendLine($"{product.Id},{product.Name},{product.Price},{product.Description},{product.ProductGroupId}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "products.csv");
        }
        [HttpGet("cache_stats_file")]
        public IActionResult CacheStatsFile()
        {
            var cacheStats = new
            {
                Hits = _memoryCache.GetTotalHitCount(),
                Misses = _memoryCache.GetTotalMissCount(),
            };
            var cacheStatsContent = System.Text.Json.JsonSerializer.Serialize(cacheStats);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "cache_stats.json");

            System.IO.File.WriteAllText(filePath, cacheStatsContent);

            return Ok($"Файл статистики кэша создан: {Request.Scheme}://{Request.Host}/cache_stats.json");
        }
    }
    
}