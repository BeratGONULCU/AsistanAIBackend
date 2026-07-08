using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Interfaces.Python;

public interface IPythonService
{
    //Task<string> RunMainScriptAsync(string arguments = "");
    Task<string> RunMainScriptAsync(object payload);
}