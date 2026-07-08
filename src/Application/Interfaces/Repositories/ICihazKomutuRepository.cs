using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using GeminiAsistanBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Interfaces.Repositories;

public interface ICihazKomutuRepository : IGenericRepository<CihazKomutuResponse>
{
    Task<List<CihazKomutu>> GetAllByDomain(string domain, CancellationToken cancellationToken);
}
