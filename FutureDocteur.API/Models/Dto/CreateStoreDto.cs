using System.ComponentModel.DataAnnotations;

namespace FutureDocteur.API.Models.Dto
{
    public class CreateStoreDto
    {
        public string LogoUrl { get; set; }
        public List<string> ProductsPhotoPath { get; set; }
        public string PhotoCovert { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        public string Address { get; set; }
        public Guid OwnerId { get; set; }
    }
}
