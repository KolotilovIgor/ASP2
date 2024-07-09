namespace ASP.Dto
{
    public class ProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int? ProductGroupId { get; set; }
        public object Id { get; internal set; }
    }
}
