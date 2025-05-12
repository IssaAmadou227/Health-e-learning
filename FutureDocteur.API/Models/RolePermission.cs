using System.Data;

namespace FutureDocteur.API.Models
{
    public class RolePermission:BaseModel
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
