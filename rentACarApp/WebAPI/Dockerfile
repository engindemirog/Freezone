#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["rentACarApp/WebAPI/WebAPI.csproj", "rentACarApp/WebAPI/"]
COPY ["corePackages/Freezone.Core.Application/Freezone.Core.Application.csproj", "corePackages/Freezone.Core.Application/"]
COPY ["corePackages/Freezone.Core.CrossCuttingConcerns/Freezone.Core.CrossCuttingConcerns.csproj", "corePackages/Freezone.Core.CrossCuttingConcerns/"]
COPY ["corePackages/Freezone.Core.Persistence/Freezone.Core.Persistence.csproj", "corePackages/Freezone.Core.Persistence/"]
COPY ["rentACarApp/Application/Application.csproj", "rentACarApp/Application/"]
COPY ["rentACarApp/Domain/Domain.csproj", "rentACarApp/Domain/"]
COPY ["rentACarApp/Persistence/Persistence.csproj", "rentACarApp/Persistence/"]
RUN dotnet restore "rentACarApp/WebAPI/WebAPI.csproj"
COPY . .
WORKDIR "/src/rentACarApp/WebAPI"
RUN dotnet build "WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]