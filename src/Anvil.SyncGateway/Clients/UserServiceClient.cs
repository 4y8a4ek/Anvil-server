using Anvil.Contracts.Protos;
using Grpc.Net.Client;

namespace Anvil.SyncGateway.Clients;

public class UserServiceClient
{
    private readonly UserService.UserServiceClient _client;

    public UserServiceClient(IConfiguration config)
    {
        var channel = GrpcChannel.ForAddress(config["UserService:GrpcUrl"]!);
        _client = new UserService.UserServiceClient(channel);
    }

    public async Task<bool> UserExistsAsync(Guid userId)
    {
        var response = await _client.CheckUserExistsAsync(
            new UserRequest { Id = userId.ToString() });
        return response.Exists;
    }
}