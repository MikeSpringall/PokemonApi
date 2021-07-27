FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
EXPOSE 80

COPY /PokemonAPI/ ./

RUN dotnet publish PokemonAPI.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "PokemonAPI.dll"]