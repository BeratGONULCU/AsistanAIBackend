using System.Linq.Expressions;

namespace GeminiAsistanBackend.Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> Query();
    // bu istenilen Query yazmak için kullanılır.

    Task<T?> GetByIdAsync(int id,CancellationToken cancellationToken = default);

    // list yerine bunu kullanma sebebi: dışarı veri dönememesini sağlamak. kapsülleme mantığı
    // ekleme-silme olsaydı list daha iyi 
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    // Bu metot, veritabanında belirli bir şarta uyan en az bir tane bile kayıt var mı, yok mu
    // unique değer kontrolü --> email vs.
    // AnyAsync ve Find methodlarının farkı dönüş tipleri
    Task<bool> AnyAsync(Expression<Func<T,bool>> expression, CancellationToken cancellationToken=default);

    // bu kısım sorguya açık haldedir. sonuna istenilen sorgu yazılır en son ToListAsync ile listenir. 
    // mesela : _repository.Find(x => x.tip == "komut") 
    IQueryable<T> Find(Expression<Func<T, bool>> expression);
    Task AddAsync(T entity, CancellationToken cancellationToken=default);
    void Update(T entity);
    void Delete(T entity);
}