using Microsoft.Data.SqlClient;
using Orders.Dal.Repo.Interfaces;
using Orders.Domain.Entity;
using System.Data;

namespace Orders.Dal.Repo.Implementations;

public class CustomersRepo : ICustomersRepo
{
    private readonly SqlConnection _connection;
    private readonly SqlTransaction? _transaction;

    public CustomersRepo(SqlConnection connection, SqlTransaction? transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }

    // Read-only, може працювати без транзакції
    public async Task<IEnumerable<Customers>> GetAllAsync()
    {
        var customers = new List<Customers>();

        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT Id, Name, Email FROM dbo.Customers";
        cmd.Transaction = _transaction;

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            customers.Add(new Customers
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Email = reader.GetString(reader.GetOrdinal("Email"))
            });
        }
        return customers;
    }

    public async Task<Customers?> GetByIdAsync(int id)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT Id, Name, Email FROM Customers WHERE Id = @Id";
        cmd.Parameters.Add(new SqlParameter("@Id", id));
        cmd.Transaction = _transaction;

        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Customers
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Email = reader.GetString(reader.GetOrdinal("Email"))
            };
        }
        return null;
    }

    // CRUD - обов'язково в транзакції
    public async Task<int> AddAsync(Customers customer)
    {
        if (_transaction == null)
            throw new InvalidOperationException("Add operation requires a transaction.");

        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"INSERT INTO Customers (Name, Email) VALUES (@Name, @Email);
                            SELECT CAST(SCOPE_IDENTITY() AS int)";
        cmd.Transaction = _transaction;

        cmd.Parameters.Add(new SqlParameter("@Name", customer.Name));
        cmd.Parameters.Add(new SqlParameter("@Email", customer.Email));

        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<bool> UpdateAsync(Customers customer)
    {
        if (_transaction == null)
            throw new InvalidOperationException("Update operation requires a transaction.");

        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "UPDATE Customers SET Name=@Name, Email=@Email WHERE Id=@Id";
        cmd.Transaction = _transaction;

        cmd.Parameters.Add(new SqlParameter("@Name", customer.Name));
        cmd.Parameters.Add(new SqlParameter("@Email", customer.Email));
        cmd.Parameters.Add(new SqlParameter("@Id", customer.Id));

        var rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (_transaction == null)
            throw new InvalidOperationException("Delete operation requires a transaction.");

        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "DELETE FROM Customers WHERE Id=@Id";
        cmd.Parameters.Add(new SqlParameter("@Id", id));
        cmd.Transaction = _transaction;

        var rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT COUNT(1) FROM Customers WHERE Email=@Email";
        cmd.Parameters.Add(new SqlParameter("@Email", email));
        cmd.Transaction = _transaction;

        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }
}
