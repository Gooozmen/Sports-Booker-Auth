# ---------- Stage 1: Build ----------
    FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
    WORKDIR /src
    
    # Copiar configuraciones de NuGet y CPM primero
    COPY src/nuget.config ./src/
    COPY src/Directory.Packages.props ./src/
    
    # Copiar solución y proyectos (.csproj) para cache eficiente
    COPY src/sports-booker-auth.sln ./src/
    COPY src/Shared/*.csproj ./src/Shared/
    COPY src/Application/*.csproj ./src/Application/
    COPY src/Domain/*.csproj ./src/Domain/
    COPY src/Infrastructure/*.csproj ./src/Infrastructure/
    COPY src/Presentation/*.csproj ./src/Presentation/
    
    # Restaurar dependencias
    RUN dotnet restore src/Presentation/Presentation.csproj
    
    # Copiar el código completo sin sobrescribir los .csproj
    COPY src/Shared ./src/Shared
    COPY src/Application ./src/Application
    COPY src/Domain ./src/Domain
    COPY src/Infrastructure ./src/Infrastructure
    COPY src/Presentation ./src/Presentation
    
    # Publicar la app
    WORKDIR src/Presentation/
    RUN dotnet publish -c Release -o /app/publish

    
    # ---------- Stage 2: Runtime ----------
    FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS runtime
    WORKDIR /app
    COPY --from=build /app/publish .
    
    EXPOSE 80
    ENTRYPOINT ["dotnet", "Presentation.dll"]
    