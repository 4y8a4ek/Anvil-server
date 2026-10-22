using Anvil.Contracts.Dtos;
using Anvil.SyncGateway.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Anvil.SyncGateway.Controllers;

[ApiController]
[Route("api/sync")]
public class SyncController : ControllerBase
{
    private readonly UserServiceClient _userClient;
    private readonly ScheduleServiceClient _scheduleClient;
    private readonly ExerciseServiceClient _exerciseClient;

    public SyncController(
        UserServiceClient userClient,
        ScheduleServiceClient scheduleClient,
        ExerciseServiceClient exerciseClient)
    {
        _userClient = userClient;
        _scheduleClient = scheduleClient;
        _exerciseClient = exerciseClient;
    }

    [HttpPost]
    public async Task<ActionResult<SyncResponse>> Sync([FromBody] SyncRequest request)
    {
        // 1. Проверка пользователя через gRPC
        var userExists = await _userClient.UserExistsAsync(request.UserId);
        if (!userExists)
            return NotFound("User not found");

        // 2. Параллельный запрос данных из сервисов
        var schedulesTask = _scheduleClient.GetForSyncAsync(
            request.UserId, request.LastSyncTimestamp);
        var exercisesTask = _exerciseClient.GetForSyncAsync(
            request.UserId, request.LastSyncTimestamp);

        await Task.WhenAll(schedulesTask, exercisesTask);

        // 3. Сборка ответа
        var response = new SyncResponse
        {
            CurrentServerTimestamp = DateTime.UtcNow,
            Schedules = await schedulesTask,
            Exercises = await exercisesTask
        };

        return Ok(response);
    }
}