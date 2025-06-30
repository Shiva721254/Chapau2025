using Chapeau25.Models;
using Microsoft.Data.SqlClient;

public class EmployeeRepository() : IEmployeeRepository
{
    public Employee? GetByUsername(string username)
    {
        var connection = Chapeau25.ExtentionMethods.DatabaseHelper.GetConnection();
        connection.Open();
        var cmd = new SqlCommand("SELECT employee_id, Username, Password, Role FROM Employee WHERE Username = @username", connection);
        cmd.Parameters.AddWithValue("@username", username);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Employee
            {
                Id = (int)reader["employee_id"],
                Username = reader["Username"] as string,
                HashedPassword = reader["Password"] as string,
                Role = reader["Role"] as string
            };
        }
        return null;
    }
}