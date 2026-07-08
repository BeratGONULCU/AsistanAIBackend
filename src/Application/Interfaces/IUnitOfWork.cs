using GeminiAsistanBackend.Application.Interfaces.Repositories;
using GeminiAsistanBackend.Domain.Entities;

namespace GeminiAsistanBackend.Application.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    IGenericRepository<CihazKomutu> CihazKomutlari {  get; }
    //IGenericRepository<GeminiAsistanBackend.Domain.Entities.IslemLog> IslemLoglariGeneric {  get; }
    IIslemLogRepository IslemLoglari { get; }
    ICihazKomutuRepository CihazKomutu { get; }
    IGenericRepository<AsistanYanit> AsistanYanit { get; }

    //IGenericRepository<SesTetikleyicisi> SesTetikleyicileri { get; }
    ISesTetikleyiciRepository SesTetikleyicileri { get;  }
    IGenericRepository<TetikleyiciKomut> TetikleyiciKomutlar { get; }
    IGenericRepository<EgitimDataset> EgitimDataset { get; }
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken=default);
}