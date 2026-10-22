using Anvil.SyncGateway.Clients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ScheduleServiceClient>();
builder.Services.AddHttpClient<ExerciseServiceClient>();
builder.Services.AddSingleton<UserServiceClient>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapControllers();
app.Run();