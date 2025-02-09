using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System.Data;

namespace Core.Domain.Base;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync(string transactionKey);
    Task TransactionCommitAsync(string transactionKey);
    Task RollbackTransactionAsync();
    bool HasTransaction { get; }
    IDbTransaction Transaction { get; }
    DatabaseModel DatabaseMetadata { get; }
    void SetDbConnection(IDbConnection connection);
    void SetTransaction(IDbTransaction transaction);
}