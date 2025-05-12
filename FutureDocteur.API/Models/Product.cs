namespace FutureDocteur.API.Models
{
    public class Product : BaseModel
    {
        public string Name { get; set; }                    // Nom du produit
        public string Description { get; set; }             // Description détaillée
        public decimal Price { get; set; }                  // Prix
        public bool IsAvailable { get; set; } = true;       // Disponibilité

        public int Stock { get; set; }                      // Quantité disponible

        public ICollection<string> PhotoUrls { get; set; }  // Photos du produit

        public Guid StoreId { get; set; }                   // Boutique associée
        public Store Store { get; set; }                    // Navigation vers la boutique

        public string Category { get; set; }                // Catégorie simple (optionnelle)
    }
}
