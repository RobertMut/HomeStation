#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR "/home/app"
EXPOSE $HTTP_PORT
EXPOSE $SSL_PORT
EXPOSE $MQTT__Port

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS with-node
RUN apt-get update
RUN apt-get install curl
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash
RUN apt-get -y install nodejs
RUN npm install -g @angular/cli

FROM with-node AS build
ARG BUILD_CONFIGURATION=Release
COPY . "/home/app/src"
WORKDIR "/home/app/src"
RUN ls -la /home/app/src
RUN dotnet restore "./HomeStation.Web/HomeStation.Web.csproj"
RUN dotnet build "./HomeStation.Web/HomeStation.Web.csproj" -c $BUILD_CONFIGURATION -o /home/app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HomeStation.Web/HomeStation.Web.csproj" -c $BUILD_CONFIGURATION -o /home/app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR "/home/app/publish"
COPY --from=publish /home/app/publish .
ENTRYPOINT ["dotnet", "HomeStation.Web.dll"]
