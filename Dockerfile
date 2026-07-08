FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY GeminiAsistanBackend.sln ./
COPY src/api/GeminiAsistanBackend.Api.csproj src/api/
COPY src/application/GeminiAsistanBackend.Application.csproj src/application/
COPY src/domain/GeminiAsistanBackend.Domain.csproj src/domain/
COPY src/infrastructure/GeminiAsistanBackend.Infrastructure.csproj src/infrastructure/

RUN dotnet restore GeminiAsistanBackend.sln

COPY . .

RUN dotnet publish src/api/GeminiAsistanBackend.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5131
ENTRYPOINT ["dotnet", "GeminiAsistanBackend.Api.dll"]