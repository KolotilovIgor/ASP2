using ASP.Abstraction;
using ASP.Data;
using ASP.Dto;
using ASP.Models;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;

namespace ASP.Repository
{
    public class ProductRepository(StorageContext storageContext, IMapper _mapper, IMemoryCache _memoryCache) : IProductRepository
    {
        public int AddProduct(ProductDto productDto)
        {
            if (storageContext.Products.Any(p => p.Name == productDto.Name))
                throw new Exception("Уже есть с таким именем");

            var entity = _mapper.Map<Product>(productDto);
            storageContext.Products.Add(entity);
            storageContext.SaveChanges();
            _memoryCache.Remove("products");
            return entity.Id;
        }
        public IEnumerable<ProductDto> GetAllProducts()
        {
            if (_memoryCache.TryGetValue("products", out List<ProductDto> listDto)) return listDto;
            listDto = storageContext.Products.Select(_mapper.Map<ProductDto>).ToList();
            _memoryCache.Set("products", listDto, TimeSpan.FromMinutes(30));
            return listDto;
        }

        public void DeleteProduct(int id)
        {
            var product = storageContext.Products.Find(id);
            if (product == null)
            {
                throw new KeyNotFoundException("Продукт не найден.");
            }
            storageContext.Products.Remove(product);
            storageContext.SaveChanges();
            _memoryCache.Remove("products");
        }
    }
}
