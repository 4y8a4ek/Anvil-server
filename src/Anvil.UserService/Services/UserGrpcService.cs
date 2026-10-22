using Anvil.Contracts.Protos;
using Anvil.UserService.Data;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Anvil.UserService.Services;

public class UserGrpcService : Anvil.Contracts.Protos.UserService.UserServiceBase
{
    private readonly AppDbContext _db;

    public UserGrpcService(AppDbContext db) => _db = db;

    public override async Task<UserExistsResponse> CheckUserExists(
        UserRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var id))
            return new UserExistsResponse { Exists = false };

        var exists = await _db.Users
            .AnyAsync(u => u.Id == id && !u.IsDeleted, context.CancellationToken);

        return new UserExistsResponse { Exists = exists };
    }
}