using Anvil.UserService.Data;
using Anvil.UserService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddGrpc();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    // Seed test user
    if (!await db.Users.AnyAsync())
    {
        db.Users.Add(new Anvil.UserService.Models.User
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Test User",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        });
        await db.SaveChangesAsync();
    }
}

app.MapGrpcService<UserGrpcService>();
app.MapGet("/", () => "UserService gRPC is running");

app.Run();