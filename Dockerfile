
# Imatge base per a l'entorn d'execució
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# Instalar librerías necesarias para SQL Server
RUN apt-get update && apt-get install -y libgssapi-krb5-2


# Imatge per a compilar el projecte
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


# Copiem el fitxer .csproj i restaurem dependències
COPY KarmaWebAPI.csproj .
RUN dotnet restore "KarmaWebAPI.csproj"


# Copiem la resta del projecte
COPY . .
RUN dotnet build "KarmaWebAPI.csproj" -c Release -o /app/build


# Publicació del projecte
FROM build AS publish
RUN dotnet publish "KarmaWebAPI.csproj" -c Release -o /app/publish


# Imatge final per a executar l'aplicació
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KarmaWebAPI.dll"]
