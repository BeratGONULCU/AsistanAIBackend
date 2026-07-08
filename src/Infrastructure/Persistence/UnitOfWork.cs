using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Application.Interfaces.Repositories;
using GeminiAsistanBackend.Domain.Entities;
using GeminiAsistanBackend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace GeminiAsistanBackend.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _currentTransaction;

    private IGenericRepository<CihazKomutu>? _cihazKomutlari;
    private IIslemLogRepository _islemLoglari;
    private ISesTetikleyiciRepository _sesTetikleyicileri;
    private IGenericRepository<TetikleyiciKomut>? _tetikleyiciKomutlar;
    private IGenericRepository<EgitimDataset>? _egitimDataset;
    private ICihazKomutuRepository _cihazKomutu;
    private IGenericRepository<AsistanYanit> _asistanYanitlar;

    private readonly ILogger<SesTetikleyiciRepository> _sesTetikleyiciLogger;

    public UnitOfWork(AppDbContext context, ILogger<SesTetikleyiciRepository> sesTetikleyiciLogger)
    {
        _context = context;
        _sesTetikleyiciLogger = sesTetikleyiciLogger;
    }

    public IGenericRepository<CihazKomutu> CihazKomutlari =>
        _cihazKomutlari ??= new GenericRepository<CihazKomutu>(_context);

    public ICihazKomutuRepository CihazKomutu => 
        _cihazKomutu  ??= new CihazKomutuRepository(_context);
    
    public IIslemLogRepository IslemLoglari => 
        _islemLoglari ??= new IslemLogRepository(_context);

    public ISesTetikleyiciRepository SesTetikleyicileri =>
        _sesTetikleyicileri ??= new SesTetikleyiciRepository(_context,_sesTetikleyiciLogger);

    public IGenericRepository<TetikleyiciKomut> TetikleyiciKomutlar =>
        _tetikleyiciKomutlar ??= new GenericRepository<TetikleyiciKomut>(_context);

    public IGenericRepository<EgitimDataset> EgitimDataset => 
        _egitimDataset ??= new GenericRepository<EgitimDataset>(_context);

    public IGenericRepository<AsistanYanit> AsistanYanit =>
        _asistanYanitlar ??= new GenericRepository<AsistanYanit>(_context);

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