#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ConferencePlanner.Backend/ConferencePlanner.Backend.csproj", "ConferencePlanner.Backend/"]
COPY ["ConferencePlanner.ConferenceDTO/ConferencePlanner.ConferenceDTO.csproj", "ConferencePlanner.ConferenceDTO/"]
RUN dotnet restore "ConferencePlanner.Backend/ConferencePlanner.Backend.csproj"
COPY . .
WORKDIR "/src/ConferencePlanner.Backend"
RUN dotnet build "ConferencePlanner.Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConferencePlanner.Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConferencePlanner.Backend.dll"]