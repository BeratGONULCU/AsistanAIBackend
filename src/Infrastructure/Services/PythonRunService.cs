using GeminiAsistanBackend.Application.Interfaces.Python;
using System.Diagnostics;

public class PythonRunService : IPythonRunService
{
    private Process? _pythonProcess;
    private readonly object _processLock = new();

    private readonly string _pythonPath =
        @"C:\Users\berat\Desktop\python-gemini-asistan\.venv\Scripts\python.exe";

    private readonly string _scriptPath =
        @"C:\Users\berat\Desktop\python-gemini-asistan\Asistan\main.py";

    private readonly string _workingDirectory =
        @"C:\Users\berat\Desktop\python-gemini-asistan\Asistan";

    public Task<string> RunPythonMainScriptAsync()
    {
        lock (_processLock)
        {
            if (_pythonProcess is { HasExited: false })
            {
                return Task.FromResult(
                    $"Python zaten çalışıyor. PID: {_pythonProcess.Id}"
                );
            }

            _pythonProcess?.Dispose();

            var startInfo = new ProcessStartInfo
            {
                FileName = _pythonPath,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = _workingDirectory
            };

            startInfo.ArgumentList.Add("-u");
            startInfo.ArgumentList.Add(_scriptPath);

            _pythonProcess = new Process
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true
            };

            if (!_pythonProcess.Start())
            {
                _pythonProcess.Dispose();
                _pythonProcess = null;

                throw new InvalidOperationException(
                    "Python işlemi başlatılamadı."
                );
            }

            return Task.FromResult(
                $"Python main.py başlatıldı. PID: {_pythonProcess.Id}"
            );
        }
    }

    public async Task<string> StopPythonMainScriptAsync()
    {
        const int port = 8766;

        var powerShellCommand = """
        $connections = @(
            Get-NetTCPConnection `
                -LocalPort __PORT__ `
                -State Listen `
                -ErrorAction SilentlyContinue
        )

        if ($connections.Count -eq 0) {
            Write-Output "NOT_FOUND"
            exit 0
        }

        foreach ($connection in $connections) {
            $processId = $connection.OwningProcess
            $targetProcess = Get-Process `
                -Id $processId `
                -ErrorAction SilentlyContinue

            if ($null -ne $targetProcess) {
                Write-Output "STOPPING:$($processId):$($targetProcess.ProcessName)"

                Stop-Process `
                    -Id $processId `
                    -Force `
                    -ErrorAction Stop
            }
        }
        """.Replace("__PORT__", port.ToString());

        var startInfo = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        startInfo.ArgumentList.Add("-NoProfile");
        startInfo.ArgumentList.Add("-NonInteractive");
        startInfo.ArgumentList.Add("-Command");
        startInfo.ArgumentList.Add(powerShellCommand);

        using var process = new Process
        {
            StartInfo = startInfo
        };

        if (!process.Start())
        {
            throw new InvalidOperationException(
                "Python kapatma işlemi başlatılamadı."
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
                $"Python kapatılamadı: {error.Trim()}"
            );
        }

        lock (_processLock)
        {
            _pythonProcess?.Dispose();
            _pythonProcess = null;
        }

        if (output.Contains("NOT_FOUND"))
        {
            return "8766 portunda çalışan Python servisi bulunamadı.";
        }

        return $"Python servisi kapatıldı. {output.Trim()}";
    }
}