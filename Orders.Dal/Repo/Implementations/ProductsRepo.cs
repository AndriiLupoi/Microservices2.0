using Dapper;
using Microsoft.Data.SqlClient;
using Orders.Dal.Repo.Interfaces;
using Orders.Domain.Entity;
using System.Data;

namespace Orders.Dal.Repo.Implementations;

public class ProductsRepo : IProductsRepo
{
    private readonly IDbConnection? _connection;
    private readonly IDbTransaction? _transaction;

    public ProductsRepo(IDbConnection? connection, IDbTransaction? transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }

    private async Task EnsureOpenConnectionAsync()
    {
        if (_connection is SqlConnection sqlConn && sqlConn.State != ConnectionState.Open)
            await sqlConn.OpenAsync();
    }

    public async Task<IEnumerable<Products>> GetAllAsync()
    {
        if (_connection == null) return Enumerable.Empty<Products>();
        await EnsureOpenConnectionAsync();

        return await _connection.QueryAsync<Products>("SELECT * FROM Products", transaction: _transaction);
    }

    public async Task<Products?> GetByIdAsync(int id)
    {
        if (_connection == null) return null;
        await EnsureOpenConnectionAsync();

        return await _connection.QueryFirstOrDefaultAsync<Products>(
            "SELECT * FROM Products WHERE Id=@Id",
            new { Id = id },
            _transaction
        );
    }

    public async Task<int> AddAsync(Products product)
    {
        if (_connection == null) return 0;
        await EnsureOpenConnectionAsync();

        const string sql = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price); SELECT CAST(SCOPE_IDENTITY() as int)";
        return await _connection.ExecuteScalarAsync<int>(sql, product, _transaction);
    }

    public async Task<bool> UpdateAsync(Products product)
    {
        if (_connection == null) return false;
        await EnsureOpenConnectionAsync();

        const string sql = "UPDATE Products SET Name=@Name, Price=@Price WHERE Id=@Id";
        var rows = await _connection.ExecuteAsync(sql, product, _transaction);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (_connection == null) return false;
        await EnsureOpenConnectionAsync();

        const string sql = "DELETE FROM Products WHERE Id=@Id";
        var rows = await _connection.ExecuteAsync(sql, new { Id = id }, _transaction);
        return rows > 0;
    }
}
