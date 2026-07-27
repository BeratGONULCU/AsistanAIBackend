using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeminiAsistanBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeminiAsistanBackend.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<CihazKomutu> CihazKomutlari {  get; }
    DbSet<Domain.Entities.IslemLog> IslemLoglari { get; }
    DbSet<Domain.Entities.SesTetikleyicisi> SesTetikleyicileri { get; }
    DbSet<Domain.Entities.TetikleyiciKomut> TetikleyiciKomutlar { get; }
    DbSet<Domain.Entities.EgitimDataset> EgitimDataset { get; }
    DbSet<RedmineEgitimDataset> RedmineEgitimDataset { get; }
    DbSet<AsistanYanit> AsistanYanit { get; }
    DbSet<AsistanSettings> AsistanSettings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
