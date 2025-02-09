using Core.Data;
using Core.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Design.Internal;
using System.Data;
using System.Data.Common;

namespace Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly Context _context;
    private IDbContextTransaction _transaction;
    private string _transactionKey;

    public UnitOfWork(Context context)
    {
        _context = context;
    }

    public bool HasTransaction => _transaction != null;

    public IDbTransaction Transaction => HasTransaction ? _transaction.GetDbTransaction() : null;

    public async Task<int> SaveChangesAsync()
    {
        var result = await _context.SaveChangesAsync();

        _context.DetachedAll();

        return result;
    }

    public async Task BeginTransactionAsync(string transactionKey)
    {
        if (_transaction is not null) return;

        if (_context.Database.CurrentTransaction is null)
            _transaction ??= await _context.Database.BeginTransactionAsync();
        else
            _transaction ??= _context.Database.CurrentTransaction;

        if (string.IsNullOrWhiteSpace(_transactionKey))
            _transactionKey = transactionKey;
    }

    public async Task TransactionCommitAsync(string transactionKey)
    {
        if (transactionKey != _transactionKey) return;

        await SaveChangesAsync();
        await _transaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
    }

    public DatabaseModel DatabaseMetadata
    {
        get
        {
            var serviceCollection = new ServiceCollection();

#pragma warning disable
            new NpgsqlDesignTimeServices().ConfigureDesignTimeServices(serviceCollection);

            var databaseModelFactory = serviceCollection.BuildServiceProvider().GetService<IDatabaseModelFactory>();

            return databaseModelFactory.Create(_context.Database.GetDbConnection(), new DatabaseModelFactoryOptions());
        }
    }

    public void SetDbConnection(IDbConnection connection)
    {
        if (_context.Database.GetDbConnection() != null) _context.Database.CloseConnection();

        _context.Database.SetDbConnection((DbConnection)connection);
    }

    public void SetTransaction(IDbTransaction transaction)
    {
        _context.Database.UseTransaction((DbTransaction)transaction);
        _transaction = _context.Database.CurrentTransaction;
    }
}