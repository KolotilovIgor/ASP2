using ASP.Dto;
using ASP.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Abstraction
{
    public interface IProductRepository
    {
        IEnumerable<ProductDto> GetAllProducts();
        int AddProduct(ProductDto productDto);
        void DeleteProduct(int id);
    }
}
