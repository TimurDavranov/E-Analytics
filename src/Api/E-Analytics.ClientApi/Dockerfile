#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Api/E-Analytics.ClientApi/EAnalytics.Client.csproj", "src/Api/E-Analytics.ClientApi/"]
COPY ["src/EA/EA.Application/EA.Application.csproj", "src/EA/EA.Application/"]
COPY ["src/EA/EA.Domain/EA.Domain.csproj", "src/EA/EA.Domain/"]
COPY ["src/Core/EAnalytics.Common/EAnalytics.Common.csproj", "src/Core/EAnalytics.Common/"]
COPY ["src/EA/EA.Infrastructure/EA.Infrastructure.csproj", "src/EA/EA.Infrastructure/"]
COPY ["src/EA/EA.Presentation/EA.Presentation.csproj", "src/EA/EA.Presentation/"]
COPY ["src/OL/OL.Application/OL.Application.csproj", "src/OL/OL.Application/"]
COPY ["src/OL/OL.Infrastructure/OL.Infrastructure.csproj", "src/OL/OL.Infrastructure/"]
COPY ["src/OL/OL.Domain/OL.Domain.csproj", "src/OL/OL.Domain/"]
RUN dotnet restore "src/Api/E-Analytics.ClientApi/EAnalytics.Client.csproj"
COPY . .
WORKDIR "/src/src/Api/E-Analytics.ClientApi"
RUN dotnet build "EAnalytics.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EAnalytics.Client.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EAnalytics.Client.dll"]