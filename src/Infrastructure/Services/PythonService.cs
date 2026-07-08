using GeminiAsistanBackend.Application.Interfaces.Python;
using System.Diagnostics;
using System.Text.Json;

public class PythonService : IPythonService
{
    private readonly string _pythonPath = @"C:\Users\berat\Desktop\python-gemini-asistan\.venv\Scripts\python.exe";
    private readonly string _scriptPath = @"C:\Users\berat\Desktop\python-gemini-asistan\Asistan\trigger.py";
    private readonly string _workingDirectory = @"C:\Users\berat\Desktop\python-gemini-asistan\Asistan";

    public async Task<string> RunMainScriptAsync(object payload)
    {
        var jsonPayload = JsonSerializer.Serialize(payload);

        var startInfo = new ProcessStartInfo
        {
            FileName = _pythonPath,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = _workingDirectory
        };

        startInfo.ArgumentList.Add(_scriptPath);
        startInfo.ArgumentList.Add(jsonPayload);

        using var process = new Process
        {
            StartInfo = startInfo
        };

        process.Start();

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        var output = await outputTask;
        var error = await errorTask;

        if (process.ExitCode != 0)
        {
            throw new Exception($"Python hata verdi: {error}");
        }

        return output.Trim();
    }
}