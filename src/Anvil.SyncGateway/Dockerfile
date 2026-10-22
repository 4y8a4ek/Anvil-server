# syntax=docker/dockerfile:1.7

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Anvil.SyncGateway/Anvil.SyncGateway.csproj", "src/Anvil.SyncGateway/"]
COPY ["src/Anvil.Contracts/Anvil.Contracts.csproj", "src/Anvil.Contracts/"]

RUN --mount=type=cache,target=/root/.nuget/packages \
    --mount=type=cache,target=/tmp/NuGetScratchroot \
    dotnet restore "src/Anvil.SyncGateway/Anvil.SyncGateway.csproj"

COPY . .
WORKDIR "/src/src/Anvil.SyncGateway"

RUN --mount=type=cache,target=/root/.nuget/packages \
    --mount=type=cache,target=/tmp/NuGetScratchroot \
    dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Anvil.SyncGateway.dll"]