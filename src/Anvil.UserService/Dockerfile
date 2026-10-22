# syntax=docker/dockerfile:1.7

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Anvil.UserService/Anvil.UserService.csproj", "src/Anvil.UserService/"]
COPY ["src/Anvil.Contracts/Anvil.Contracts.csproj", "src/Anvil.Contracts/"]

RUN --mount=type=cache,target=/root/.nuget/packages \
    --mount=type=cache,target=/tmp/NuGetScratchroot \
    dotnet restore "src/Anvil.UserService/Anvil.UserService.csproj"

COPY . .
WORKDIR "/src/src/Anvil.UserService"

RUN --mount=type=cache,target=/root/.nuget/packages \
    --mount=type=cache,target=/tmp/NuGetScratchroot \
    dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Anvil.UserService.dll"]