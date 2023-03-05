#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["A.I.S.A.Mastodon.Compose/A.I.S.A.Mastodon.Container.csproj", "A.I.S.A.Mastodon.Compose/"]
COPY ["A.I.S.A/A.I.S.A.csproj", "A.I.S.A/"]
RUN dotnet restore "A.I.S.A.Mastodon.Compose/A.I.S.A.Mastodon.Container.csproj"
COPY . .
WORKDIR "/src/A.I.S.A.Mastodon.Compose"
RUN dotnet build "A.I.S.A.Mastodon.Container.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "A.I.S.A.Mastodon.Container.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "A.I.S.A.Mastodon.Container.dll"]