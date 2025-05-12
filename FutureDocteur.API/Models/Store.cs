namespace FutureDocteur.API.Models
{
    public class Store : BaseModel
    {
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public string CoverPhotoUrl { get; set; }
        public string Description { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public Guid OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        public ICollection<Product> Products { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
