#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["A.I.S.A.WebApp/A.I.S.A.WebApp.csproj", "A.I.S.A.WebApp/"]
RUN dotnet restore "A.I.S.A.WebApp/A.I.S.A.WebApp.csproj"
COPY . .
WORKDIR "/src/A.I.S.A.WebApp"
RUN dotnet build "A.I.S.A.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "A.I.S.A.WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "A.I.S.A.WebApp.dll"]