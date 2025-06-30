using Chapeau25.Models;

public interface ILoginService
{
    Employee? Authenticate(string username, string password);
}