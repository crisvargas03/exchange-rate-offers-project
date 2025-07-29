# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar el .csproj y restaurar
COPY RateOffersAPI1.csproj ./
RUN dotnet restore RateOffersAPI1.csproj

# Copiar el resto del contenido
COPY . .

# Compilar
RUN dotnet build RateOffersAPI1.csproj -c Release --no-restore

# Publicar
RUN dotnet publish RateOffersAPI1.csproj -c Release -o /app/publish --no-restore

# Etapa 2: Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "RateOffersAPI1.dll"]
