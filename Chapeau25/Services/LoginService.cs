using Chapeau25.Models;
using System.Security.Cryptography;
using System.Text;

public class LoginService(IEmployeeRepository employeeRepository) : ILoginService
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;

    public Employee? Authenticate(string username, string password)
    {
        var employee = _employeeRepository.GetByUsername(username);
        if (employee == null) return null;

        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        var hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();

        if (employee.HashedPassword == hashString)
            return employee;

        return null;
    }
}