using GeminiAsistanBackend.Application.Interfaces.Python;
using System.Diagnostics;

public class PythonRunService : IPythonRunService
{
    private readonly string _pythonPath = @"C:\Users\berat\Desktop\python-gemini-asistan\.venv\Scripts\python.exe";
    //private readonly string _scriptPath = @"C:\Users\berat\Desktop\python-gemini-asistan\Asistan\trigger.py";
    private readonly string _scriptPath = @"C:\Users\berat\Desktop\python-gemini-asistan\Asistan\main.py";
    private readonly string _workingDirectory = @"C:\Users\berat\Desktop\python-gemini-asistan\Asistan";

    public Task<string> RunPythonMainScriptAsync()
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = _pythonPath,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = _workingDirectory
        };

        startInfo.ArgumentList.Add(_scriptPath);

        var process = new Process
        {
            StartInfo = startInfo
        };

        process.Start();

        return Task.FromResult("Python main.py arka planda başlatıldı.");
    }
}