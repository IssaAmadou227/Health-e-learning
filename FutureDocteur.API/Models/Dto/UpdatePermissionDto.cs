namespace FutureDocteur.API.Models.Dto
{
    public class UpdatePermissionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
