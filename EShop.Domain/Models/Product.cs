namespace EShopService.Models
{
    public class Product: BaseModel
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Ean { get; set; } = string.Empty;
        public int Stock { get; set; } = 0;
        public string sku { get; set; } = string.Empty;
        public Category Category { get; set; } = default!;
       

        

    }
}
