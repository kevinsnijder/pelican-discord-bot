#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:10.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src
COPY ["src/pelican-discord-bot.csproj", "src/"]
COPY ["pelican/pelican.csproj", "pelican/"]
COPY ["discord/discord.csproj", "discord/"]
RUN dotnet restore "src/pelican-discord-bot.csproj"
COPY . .
WORKDIR "/src/src"
RUN dotnet build "pelican-discord-bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "pelican-discord-bot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "pelican-discord-bot.dll"]