using GeminiAsistanBackend.Application.Interfaces.Python;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

public sealed class PythonService : IPythonService
{
    private readonly string _pythonPath =
        @"C:\Users\berat\Desktop\python-gemini-asistan\.venv\Scripts\python.exe";

    private readonly string _scriptPath =
        @"C:\Users\berat\Desktop\python-gemini-asistan\Asistan\trigger.py";

    private readonly string _workingDirectory =
        @"C:\Users\berat\Desktop\python-gemini-asistan\Asistan";

    public async Task<string> RunMainScriptAsync(object payload)
    {
        if (!File.Exists(_pythonPath))
        {
            throw new FileNotFoundException(
                "Python executable bulunamadı.",
                _pythonPath
            );
        }

        if (!File.Exists(_scriptPath))
        {
            throw new FileNotFoundException(
                "trigger.py bulunamadı.",
                _scriptPath
            );
        }

        if (!Directory.Exists(_workingDirectory))
        {
            throw new DirectoryNotFoundException(
                $"Çalışma dizini bulunamadı: {_workingDirectory}"
            );
        }

        var jsonPayload = JsonSerializer.Serialize(
            payload,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }
        );

        var startInfo = new ProcessStartInfo
        {
            FileName = _pythonPath,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8,
            WorkingDirectory = _workingDirectory
        };

        /*
         * Python tarafında bu değer okunarak backend'e ikinci kez
         * kayıt atılması engellenebilir.
         */
        startInfo.Environment["ASISTAN_SAVE_TO_BACKEND"] = "false";

        startInfo.ArgumentList.Add(_scriptPath);
        startInfo.ArgumentList.Add(jsonPayload);

        using var process = new Process
        {
            StartInfo = startInfo
        };

        if (!process.Start())
        {
            throw new InvalidOperationException(
                "Python process başlatılamadı."
            );
        }

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        var output = await outputTask;
        var error = await errorTask;

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"Python hata verdi. ExitCode: {process.ExitCode}. Hata: {error}"
            );
        }

        if (string.IsNullOrWhiteSpace(output))
        {
            throw new InvalidOperationException(
                $"Python boş cevap döndürdü. Stderr: {error}"
            );
        }

        return output.Trim();
    }
}