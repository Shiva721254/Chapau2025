using Chapeau25.Models;

public interface IEmployeeRepository
{
    Employee? GetByUsername(string username);
}