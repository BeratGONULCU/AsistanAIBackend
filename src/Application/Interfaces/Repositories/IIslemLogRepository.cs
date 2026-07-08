using GeminiAsistanBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Interfaces.Repositories;

public interface IIslemLogRepository : IGenericRepository<Domain.Entities.IslemLog> 
{
    // komutId değeri ile log getirme
    // komutId değeri ile log add
    Task<Domain.Entities.IslemLog> getLogbyId(int id);
    Task<IEnumerable<Domain.Entities.IslemLog>> getLogs();
    Task<List<Domain.Entities.IslemLog?>> GetByMetin(string metin, CancellationToken cancellationToken=default);
    Task<IEnumerable<Domain.Entities.IslemLog>> GetLogsByDate(CancellationToken cancellationToken=default);
    Task<IEnumerable<Domain.Entities.IslemLog>> GetIslemLogByDurum(string durum, CancellationToken cancellationToken = default);

}

