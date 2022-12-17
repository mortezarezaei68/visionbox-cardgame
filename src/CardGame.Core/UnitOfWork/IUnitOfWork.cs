using Microsoft.EntityFrameworkCore.Storage;

namespace CardGame.Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        bool HasActiveTransaction { get; }
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task CommitAsync(IDbContextTransaction transaction);

        void RollbackTransaction();
    }
}