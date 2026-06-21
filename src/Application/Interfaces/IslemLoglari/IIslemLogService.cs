using GeminiAsistanBackend.Application.DTOs.IslemLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Interfaces.IslemLog;

public interface IIslemLogService
{
    Task<IslemLogResponse> GetAllAsync(CancellationToken cancellationToken);
    Task<IslemLogResponse> CreateAsync(IslemLogRequest request, CancellationToken cancellationToken);
}
