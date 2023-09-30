#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/OL.Parser.Worker/OL.Parser.Worker.csproj", "src/Services/OL.Parser.Worker/"]
RUN dotnet restore "src/Services/OL.Parser.Worker/OL.Parser.Worker.csproj"
COPY . .
WORKDIR "/src/src/Services/OL.Parser.Worker"
RUN dotnet build "OL.Parser.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OL.Parser.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OL.Parser.Worker.dll"]