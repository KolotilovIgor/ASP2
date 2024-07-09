using ASP.Abstraction;
using ASP.Data;
using ASP.Dto;
using ASP.Models;
using AutoMapper;
using ASP.Repository;
using Microsoft.Extensions.Caching.Memory;


namespace ASP.Repository
{
    public class ProductGroupRepository : IProductGroupRepository
    {
        private readonly StorageContext _storageContext;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        public ProductGroupRepository(StorageContext storageContext, IMapper mapper, IMemoryCache memoryCache)
        {
            _storageContext = storageContext;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public int AddProductGroup(ProductGroupDto productGroupDto)
        {
            if (_storageContext.ProductGroups.Any(p => p.Name == productGroupDto.Name))
                throw new Exception("Уже есть с таким именем");

            var entity = _mapper.Map<ProductGroup>(productGroupDto);
            _storageContext.ProductGroups.Add(entity);
            _storageContext.SaveChanges();
            _memoryCache.Remove("productgroups");
            return entity.Id;
        }
        public IEnumerable<ProductGroupDto> GetAllProductGroups()
        {
            if (_memoryCache.TryGetValue("productgroups", out List<ProductGroupDto> listDto)) return listDto;
            listDto = _storageContext.ProductGroups.Select(_mapper.Map<ProductGroupDto>).ToList();
            _memoryCache.Set("productgroups", listDto, TimeSpan.FromMinutes(30));
            return listDto;
        }

        public void DeleteProductGroup(int id)
        {
            var productGroup = _storageContext.ProductGroups.Find(id);
            if (productGroup == null)
            {
                throw new KeyNotFoundException("Группа продуктов не найдена.");
            }
            _storageContext.ProductGroups.Remove(productGroup);
            _storageContext.SaveChanges();
            _memoryCache.Remove("productgroups");
        }

    }

}
