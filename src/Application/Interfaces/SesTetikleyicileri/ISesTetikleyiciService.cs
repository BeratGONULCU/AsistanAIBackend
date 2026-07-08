using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Interfaces.SesTetikleyici;

public interface ISesTetikleyiciService
{
    Task<SesTetikleyiciResponse> CreateSesTetikleyici(CreateSesTetikleyiciRequest request,CancellationToken cancellationToken);
    Task<List<SesTetikleyiciResponse>> GetAll(CancellationToken cancellationToken);
    Task<bool> CountSesTetikleyicileri(CancellationToken cancellationToken);
}
