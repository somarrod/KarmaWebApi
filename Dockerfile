
# Imatge base per a l'entorn d'execuci�
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# Instalar librer�as necesarias para SQL Server
RUN apt-get update && apt-get install -y libgssapi-krb5-2


# Imatge per a compilar el projecte
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


# Copiem el fitxer .csproj i restaurem depend�ncies
COPY KarmaWebAPI.csproj .
RUN dotnet restore "KarmaWebAPI.csproj"


# Copiem la resta del projecte
COPY . .
RUN dotnet build "KarmaWebAPI.csproj" -c Release -o /app/build


# Publicaci� del projecte
FROM build AS publish
RUN dotnet publish "KarmaWebAPI.csproj" -c Release -o /app/publish


# Imatge final per a executar l'aplicaci�
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KarmaWebAPI.dll"]
