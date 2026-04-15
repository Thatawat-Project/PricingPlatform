FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/services/PricingService/PricingService.Api/PricingService.Api.csproj", "src/services/PricingService/PricingService.Api/"]
COPY ["src/services/PricingService/PricingService.Application/PricingService.Application.csproj", "src/services/PricingService/PricingService.Application/"]
COPY ["src/services/PricingService/PricingService.Domain/PricingService.Domain.csproj", "src/services/PricingService/PricingService.Domain/"]
COPY ["src/services/PricingService/PricingService.Infrastructure/PricingService.Infrastructure.csproj", "src/services/PricingService/PricingService.Infrastructure/"]

# ← เพิ่มตรงนี้
COPY ["src/services/shared/PricingPlatform.Contracts/PricingPlatform.Contracts/PricingPlatform.Contracts.csproj", "src/services/shared/PricingPlatform.Contracts/PricingPlatform.Contracts/"]
COPY ["src/services/shared/PricingPlatform.Engine/PricingPlatform.Engine/PricingPlatform.Engine.csproj", "src/services/shared/PricingPlatform.Engine/PricingPlatform.Engine/"]
COPY ["src/services/shared/PricingPlatform.Infrastructure.Logging/PricingPlatform.Infrastructure.Logging/PricingPlatform.Infrastructure.Logging.csproj", "src/services/shared/PricingPlatform.Infrastructure.Logging/PricingPlatform.Infrastructure.Logging/"]
COPY ["src/services/shared/PricingPlatform.SharedKernel/PricingPlatform.SharedKernel/PricingPlatform.SharedKernel.csproj", "src/services/shared/PricingPlatform.SharedKernel/PricingPlatform.SharedKernel/"]

RUN dotnet restore "src/services/PricingService/PricingService.Api/PricingService.Api.csproj"
COPY . .
WORKDIR "/src/src/services/PricingService/PricingService.Api"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PricingService.Api.dll"]