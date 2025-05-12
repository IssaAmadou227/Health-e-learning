namespace FutureDocteur.API.DataBase.Repository.Contract
{
    public interface IEmail
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
    }
}
