FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

WORKDIR /src
COPY ["/DeclaredPersonsAnalyzer/DeclaredPersonsAnalyzer.csproj", "DeclaredPersonsAnalyzer/"]
COPY ["/Application/Application.csproj", "Application/"]
COPY ["/Domain/Domain.csproj", "Domain/"]
COPY ["/Shared/Shared.csproj", "Shared/"]
COPY ["/Adapters/Adapters.Shared/Adapters.Shared.csproj", "Adapters/Adapters.Shared/"]
COPY ["/Adapters/DeclaredPersonsAdapter/DeclaredPersonsAdapter.Application/DeclaredPersonsAdapter.Application.csproj", "Adapters/DeclaredPersonsAdapter/DeclaredPersonsAdapter.Application/"]
COPY ["/Adapters/DeclaredPersonsAdapter/DeclaredPersonsAdapter.Infrastructure/DeclaredPersonsAdapter.Infrastructure.csproj", "Adapters/DeclaredPersonsAdapter/DeclaredPersonsAdapter.Infrastructure/"]
COPY ["/Infrastructure.Shared/Infrastructure.Shared.csproj", "Infrastructure.Shared/"]
COPY ["/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "DeclaredPersonsAnalyzer/DeclaredPersonsAnalyzer.csproj"
COPY . .
WORKDIR "/src/DeclaredPersonsAnalyzer"
RUN dotnet build "DeclaredPersonsAnalyzer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DeclaredPersonsAnalyzer.csproj" -c Release -o /app/publish

# RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish  ./

ENTRYPOINT ["dotnet", "declared-persons-analyser.dll"]

# FROM build AS publish
# RUN dotnet publish "DeclaredPersonsAnalyzer.csproj" -c Release -o /app/publish

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["declared-persons-analyser.exe"]
