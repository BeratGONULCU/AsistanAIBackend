using GeminiAsistanBackend.Application.DTOs.RedMineDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Interfaces.RedMineTask;

public interface IRedmineService 
{
    Task<RedMineDataResponse> GetMyTasksAsync(string token,CancellationToken cancellationToken);
    Task<RedMineDataResponse> GetclosedTasksAsync(string token, CancellationToken cancellationToken);
}
