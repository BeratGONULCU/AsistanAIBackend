using GeminiAsistanBackend.Application.DTOs.RedMineDto;
using GeminiAsistanBackend.Application.Interfaces.RedMineTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Infrastructure.Services;

public class RedmineService : IRedmineService
{
    public readonly HttpClient _httpclient;

    public RedmineService(HttpClient httpclient)
    {
        _httpclient = httpclient;
    }

    public async Task<RedMineDataResponse> GetMyTasksAsync(string token, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get,
            "https://redmine.niltekyazilim.com/issues.json?assigned_to_id=me&status_id=open");

        request.Headers.Add("X-Redmine-API-Key", token);

        var response = await _httpclient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<RedMineDataResponse>(json!);
    }

    public async Task<RedMineDataResponse> GetclosedTasksAsync(string token,CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get,
            "https://redmine.niltekyazilim.com/issues.json?assigned_to_id=me&status_id=open");

        request.Headers.Add("X-redmine-API-KEY", token);

        var response = await _httpclient.SendAsync(request,cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<RedMineDataResponse>(json);

        if (result?.Issues == null)
            return result;

        result.Issues = result.Issues
            .Where(x => x.ClosedOn != null)
            .ToList();

        return result;
    }

}
