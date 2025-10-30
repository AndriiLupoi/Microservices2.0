using Microsoft.Data.SqlClient;
using Orders.Dal.Repo.Interfaces;
using Orders.Dal.Repo.Implementations;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Orders.Dal.UOW;

public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly string _connectionString;
    private SqlConnection? _connection;
    private SqlTransaction? _transaction;
    private bool _disposed;

    public IOrdersRepo Orders { get; private set; }
    public IOrderItemsRepo OrderItems { get; private set; }
    public ICustomersRepo Customers { get; private set; }
    public IProductsRepo Products { get; private set; }

    public UnitOfWork(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("OrdersDb")
            ?? throw new InvalidOperationException("Connection string 'OrdersDb' not found.");

        // відкрите підключення для read-only
        _connection = new SqlConnection(_connectionString);
        _connection.Open();

        Orders = new OrdersRepo(_connection, null);
        OrderItems = new OrderItemsRepo(_connection, null);
        Customers = new CustomersRepo(_connection, null);
        Products = new ProductsRepo(_connection, null);
    }

    public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        if (_connection == null) _connection = new SqlConnection(_connectionString);
        if (_connection.State != ConnectionState.Open)
            await _connection.OpenAsync();

        _transaction = _connection.BeginTransaction(isolationLevel);

        // пересоздаємо репозиторії з транзакцією
        Orders = new OrdersRepo(_connection, _transaction);
        OrderItems = new OrderItemsRepo(_connection, _transaction);
        Customers = new CustomersRepo(_connection, _transaction);
        Products = new ProductsRepo(_connection, _transaction);
    }

    public async Task CommitAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Transaction not started.");

        try
        {
            await Task.Run(() => _transaction.Commit());
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
        finally
        {
            await DisposeAsync();
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction == null) return;

        try
        {
            await Task.Run(() => _transaction.Rollback());
        }
        finally
        {
            await DisposeAsync();
        }
    }

    public void Dispose()
    {
        if (_disposed) return;

        _transaction?.Dispose();
        _connection?.Dispose();
        _disposed = true;
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        if (_transaction != null) await Task.Run(() => _transaction.Dispose());
        if (_connection != null) await _connection.DisposeAsync();

        _disposed = true;
    }
}
