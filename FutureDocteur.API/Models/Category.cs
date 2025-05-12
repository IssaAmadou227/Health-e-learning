namespace FutureDocteur.API.Models
{
    public class Category:BaseModel
    {
        public string? Name { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
