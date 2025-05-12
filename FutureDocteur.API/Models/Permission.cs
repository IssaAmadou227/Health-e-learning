namespace FutureDocteur.API.Models
{
    public class Permission:BaseModel
    {
        public string Name { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
