# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar el .csproj y restaurar
COPY RateOffersAPI3.csproj ./
RUN dotnet restore RateOffersAPI3.csproj

# Copiar el resto del contenido
COPY . .

# Compilar
RUN dotnet build RateOffersAPI3.csproj -c Release --no-restore

# Publicar
RUN dotnet publish RateOffersAPI3.csproj -c Release -o /app/publish --no-restore

# Etapa 2: Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "RateOffersAPI3.dll"]
