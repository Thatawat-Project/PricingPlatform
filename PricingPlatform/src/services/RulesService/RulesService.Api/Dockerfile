FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
# ปรับให้ตรงกับพอร์ตจริงที่ .NET ใช้ภายใน Container (Internal Port)
EXPOSE 8080 

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/services/RulesService/RulesService.Api/RulesService.Api.csproj", "src/services/RulesService/RulesService.Api/"]
COPY ["src/services/RulesService/RulesService.Application/RulesService.Application.csproj", "src/services/RulesService/RulesService.Application/"]
COPY ["src/services/RulesService/RulesService.Domain/RulesService.Domain.csproj", "src/services/RulesService/RulesService.Domain/"]
COPY ["src/services/RulesService/RulesService.Infrastructure/RulesService.Infrastructure.csproj", "src/services/RulesService/RulesService.Infrastructure/"]
COPY ["src/services/shared/PricingPlatform.Contracts/PricingPlatform.Contracts/PricingPlatform.Contracts.csproj", "src/services/shared/PricingPlatform.Contracts/PricingPlatform.Contracts/"]
COPY ["src/services/shared/PricingPlatform.Infrastructure.Logging/PricingPlatform.Infrastructure.Logging/PricingPlatform.Infrastructure.Logging.csproj", "src/services/shared/PricingPlatform.Infrastructure.Logging/PricingPlatform.Infrastructure.Logging/"]

RUN dotnet restore "src/services/RulesService/RulesService.Api/RulesService.Api.csproj"
COPY . .
WORKDIR "/src/src/services/RulesService/RulesService.Api"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RulesService.Api.dll"]