# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar los proyectos
COPY OrchestratorMicroService.API/OrchestratorMicroService.API.csproj ./OrchestratorMicroService.API/
COPY OrchestratorMicroService.Application/OrchestratorMicroService.Application.csproj ./OrchestratorMicroService.Application/
COPY OrchestratorMicroService.Domain/OrchestratorMicroService.Domain.csproj ./OrchestratorMicroService.Domain/
COPY OrchestratorMicroService.Infrastructure/OrchestratorMicroService.Infrastructure.csproj ./OrchestratorMicroService.Infrastructure/

# Restaurar paquetes
RUN dotnet restore ./OrchestratorMicroService.API/OrchestratorMicroService.API.csproj

# Copiar el resto del contenido
COPY . .

# Compilar
RUN dotnet build ./OrchestratorMicroService.API/OrchestratorMicroService.API.csproj -c Release --no-restore

# Publicar
RUN dotnet publish ./OrchestratorMicroService.API/OrchestratorMicroService.API.csproj -c Release -o /app/publish --no-restore

# Etapa 2: Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "OrchestratorMicroService.API.dll"]
