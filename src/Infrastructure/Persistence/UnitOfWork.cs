using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Application.Interfaces.Repositories;
using GeminiAsistanBackend.Domain.Entities;
using GeminiAsistanBackend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace GeminiAsistanBackend.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _currentTransaction;

    private IGenericRepository<CihazKomutu>? _cihazKomutlari;
    private IGenericRepository<IslemLog>? _islemLoglari;
    private IGenericRepository<SesTetikleyicisi>? _sesTetikleyicileri;
    private IGenericRepository<TetikleyiciKomut>? _tetikleyiciKomutlar;
    private IGenericRepository<EgitimDataset>? _egitimDataset;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<CihazKomutu> CihazKomutlari =>
        _cihazKomutlari ??= new GenericRepository<CihazKomutu>(_context);

    public IGenericRepository<IslemLog> IslemLoglari =>
        _islemLoglari ??= new GenericRepository<IslemLog>(_context);

    public IGenericRepository<SesTetikleyicisi> SesTetikleyicileri =>
        _sesTetikleyicileri ??= new GenericRepository<SesTetikleyicisi>(_context);

    public IGenericRepository<TetikleyiciKomut> TetikleyiciKomutlar =>
        _tetikleyiciKomutlar ??= new GenericRepository<TetikleyiciKomut>(_context);

    public IGenericRepository<EgitimDataset> EgitimDataset => 
        _egitimDataset ??= new GenericRepository<EgitimDataset>(_context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.CommitAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}