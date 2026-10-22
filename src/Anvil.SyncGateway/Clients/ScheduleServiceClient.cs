using System.Net.Http.Json;
using Anvil.Contracts.Dtos;

namespace Anvil.SyncGateway.Clients;

public class ScheduleServiceClient
{
    private readonly HttpClient _http;

    public ScheduleServiceClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _http.BaseAddress = new Uri(config["ScheduleService:BaseUrl"]!);
    }

    public async Task<List<ScheduleDto>> GetForSyncAsync(Guid userId, DateTime? since)
    {
        var url = $"/api/schedules/sync?userId={userId}";
        if (since.HasValue)
            url += $"&since={since.Value:O}";

        var response = await _http.GetFromJsonAsync<List<ScheduleDto>>(url);
        return response ?? new List<ScheduleDto>();
    }
}