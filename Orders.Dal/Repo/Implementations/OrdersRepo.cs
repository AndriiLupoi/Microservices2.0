using Dapper;
using Microsoft.Data.SqlClient;
using Orders.Dal.Repo.Interfaces;
using Orders.Domain.Entity;
using System.Data;

namespace Orders.Dal.Repo.Implementations;

public class OrdersRepo : IOrdersRepo
{
    private readonly IDbConnection? _connection;
    private readonly IDbTransaction? _transaction;

    public OrdersRepo(IDbConnection? connection, IDbTransaction? transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }

    private async Task EnsureOpenConnectionAsync()
    {
        if (_connection is SqlConnection sqlConn && sqlConn.State != ConnectionState.Open)
            await sqlConn.OpenAsync();
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        if (_connection == null) return Enumerable.Empty<Order>();
        await EnsureOpenConnectionAsync();
        return await _connection.QueryAsync<Order>("SELECT * FROM Orders", transaction: _transaction);
    }

    public async Task<Order?> GetByIdAsync(int orderId)
    {
        if (_connection == null) return null;
        await EnsureOpenConnectionAsync();
        return await _connection.QueryFirstOrDefaultAsync<Order>(
            "SELECT * FROM Orders WHERE Id=@Id",
            new { Id = orderId },
            transaction: _transaction
        );
    }

    public async Task<int> AddAsync(Order order)
    {
        if (_connection == null) return 0;
        await EnsureOpenConnectionAsync();

        const string sql = @"
            INSERT INTO Orders (CustomerId, Status, CreatedAt)
            VALUES (@CustomerId, @Status, @CreatedAt);
            SELECT CAST(SCOPE_IDENTITY() as int);";

        return await _connection.ExecuteScalarAsync<int>(sql, order, _transaction);
    }

    public async Task<bool> UpdateAsync(Order order)
    {
        if (_connection == null) return false;
        await EnsureOpenConnectionAsync();

        const string sql = "UPDATE Orders SET CustomerId=@CustomerId, Status=@Status WHERE Id=@Id";
        var rows = await _connection.ExecuteAsync(sql, order, _transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int orderId)
    {
        if (_connection == null) return false;
        await EnsureOpenConnectionAsync();

        const string sql = "DELETE FROM Orders WHERE Id=@Id";
        var rows = await _connection.ExecuteAsync(sql, new { Id = orderId }, _transaction);
        return rows > 0;
    }

    public async Task<int> CountByCustomerAsync(int customerId)
    {
        if (_connection == null) return 0;
        await EnsureOpenConnectionAsync();

        const string sql = "SELECT COUNT(*) FROM Orders WHERE CustomerId=@CustomerId";
        return await _connection.ExecuteScalarAsync<int>(sql, new { CustomerId = customerId }, _transaction);
    }
}
