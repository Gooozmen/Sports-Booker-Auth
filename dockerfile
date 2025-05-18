# ---------- Stage 1: Build ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar configuraciones de NuGet y CPM primero
COPY src/nuget.config .
COPY src/Directory.Packages.props .

# Copiar solución y proyectos (.csproj) para cache eficiente
COPY src/sports-booker-auth.sln .
COPY src/Shared/*.csproj ./Shared/
COPY src/Application/*.csproj ./Application/
COPY src/Domain/*.csproj ./Domain/
COPY src/Infrastructure/*.csproj ./Infrastructure/
COPY src/Presentation/*.csproj ./Presentation/

# Restaurar dependencias
RUN dotnet restore ./Presentation/Presentation.csproj

# Copiar el código completo
COPY src/Shared ./Shared
COPY src/Application ./Application
COPY src/Domain ./Domain
COPY src/Infrastructure ./Infrastructure
COPY src/Presentation ./Presentation

# Publicar la app
WORKDIR /src/Presentation
RUN dotnet publish -c Release -o /app/publish

# ---------- Stage 2: Runtime ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "CourtBooker.Auth.Presentation.dll"]
