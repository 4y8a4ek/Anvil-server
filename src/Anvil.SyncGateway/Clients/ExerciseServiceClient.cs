using System.Net.Http.Json;
using Anvil.Contracts.Dtos;

namespace Anvil.SyncGateway.Clients;

public class ExerciseServiceClient
{
    private readonly HttpClient _http;

    public ExerciseServiceClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _http.BaseAddress = new Uri(config["ExerciseService:BaseUrl"]!);
    }

    public async Task<List<ExerciseDto>> GetForSyncAsync(Guid userId, DateTime? since)
    {
        // В реальном сценарии userId используется для фильтрации
        // через ScheduleService, но для заготовки синхронизации
        // параметр передаётся напрямую.
        var url = $"/api/exercises/sync?userId={userId}";
        if (since.HasValue)
            url += $"&since={since.Value:O}";

        var response = await _http.GetFromJsonAsync<List<ExerciseDto>>(url);
        return response ?? new List<ExerciseDto>();
    }
}