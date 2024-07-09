using ASP.Abstraction;
using ASP.Data;
using ASP.Dto;
using ASP.Models;
using ASP.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductGroupController : ControllerBase
    {
        private readonly IProductGroupRepository _productGroupRepository;

        public ProductGroupController(IProductGroupRepository productGroupRepository)
        {
            _productGroupRepository = productGroupRepository;
        }

        [HttpPost]
        public ActionResult<int> AddProductGroup(ProductGroupDto productGroupDto)
        {
            try
            {
                var id = _productGroupRepository.AddProductGroup(productGroupDto);
                return Ok(id);
            }
            catch
            {
                return StatusCode(409);
            }
        }

        [HttpGet("get_all_group")]
        public ActionResult<IEnumerable<ProductGroup>> GetAllProductGroups()
        {
            return Ok(_productGroupRepository.GetAllProductGroups());
        }

        [HttpDelete("delete_group")]
        public ActionResult DeleteProductGroup(int id)
        {
            try
            {
                _productGroupRepository.DeleteProductGroup(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ошибка при удалении группы продуктов.");
            }
        }
    }
    
}
