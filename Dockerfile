#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
RUN apt-get update
RUN apt-get install -y curl
RUN curl -sL https://deb.nodesource.com/setup_14.x | bash -
RUN apt-get install -y nodejs
RUN npm install -g yarn
WORKDIR /src

COPY ["test3.csproj", ""]
RUN dotnet restore "./test3.csproj"
COPY . .
WORKDIR "/src/."

RUN dotnet build "test3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "test3.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "test3.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet test3.dll