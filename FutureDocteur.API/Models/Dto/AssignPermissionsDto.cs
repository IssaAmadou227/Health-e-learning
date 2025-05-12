namespace FutureDocteur.API.Models.Dto
{
    public class AssignPermissionsDto
    {
        public Guid RoleId { get; set; }
        public List<Guid> PermissionIds { get; set; }
    }
}
