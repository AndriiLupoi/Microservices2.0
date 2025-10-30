using Dapper;
using Microsoft.Data.SqlClient;
using Orders.Dal.Repo.Interfaces;
using Orders.Domain.Entity;
using System.Data;

namespace Orders.Dal.Repo.Implementations;

public class OrderItemsRepo : IOrderItemsRepo
{
    private readonly IDbConnection? _connection;
    private readonly IDbTransaction? _transaction;

    public OrderItemsRepo(IDbConnection? connection, IDbTransaction? transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }

    private async Task EnsureOpenConnectionAsync()
    {
        if (_connection is SqlConnection sqlConn && sqlConn.State != ConnectionState.Open)
            await sqlConn.OpenAsync();
    }

    public async Task<IEnumerable<OrderItems>> GetAllAsync()
    {
        if (_connection == null) return Enumerable.Empty<OrderItems>();
        await EnsureOpenConnectionAsync();
        return await _connection.QueryAsync<OrderItems>("SELECT * FROM OrderItems", transaction: _transaction);
    }

    public async Task<OrderItems?> GetByIdAsync(int id)
    {
        if (_connection == null) return null;
        await EnsureOpenConnectionAsync();
        return await _connection.QuerySingleOrDefaultAsync<OrderItems>(
            "SELECT * FROM OrderItems WHERE Id=@Id",
            new { Id = id },
            _transaction
        );
    }

    public async Task<int> AddAsync(OrderItems item)
    {
        if (_connection == null) return 0;
        await EnsureOpenConnectionAsync();

        const string sql = "INSERT INTO OrderItems (OrderId, ProductId, Quantity) VALUES (@OrderId, @ProductId, @Quantity)";
        return await _connection.ExecuteAsync(sql, item, _transaction);
    }

    public async Task<int> UpdateAsync(OrderItems item)
    {
        if (_connection == null) return 0;
        await EnsureOpenConnectionAsync();

        const string sql = "UPDATE OrderItems SET Quantity=@Quantity WHERE Id=@Id";
        return await _connection.ExecuteAsync(sql, item, _transaction);
    }

    public async Task<int> DeleteAsync(int id)
    {
        if (_connection == null) return 0;
        await EnsureOpenConnectionAsync();

        const string sql = "DELETE FROM OrderItems WHERE Id=@Id";
        return await _connection.ExecuteAsync(sql, new { Id = id }, _transaction);
    }
}
