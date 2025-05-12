using Microsoft.AspNetCore.Identity;

namespace FutureDocteur.API.Models
{
    public class ApplicationUser:IdentityUser<Guid>
    {
        public string LastName {  get; set; }
        public string FirstName {  get; set; }
        public ICollection<Store> Shops { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public string? EmailVerificationCode { get; set; }
        public DateTime? CodeExpiration { get; set; }
    }
}
