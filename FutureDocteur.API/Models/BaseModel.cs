namespace FutureDocteur.API.Models
{
    public class BaseModel
    {
        public Guid Id { get; set; }
        public string DisplayName {  get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
