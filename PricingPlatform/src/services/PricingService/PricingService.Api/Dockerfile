FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
# ปรับให้เป็น 8080 เพื่อให้เป็นมาตรฐานเดียวกันกับทุก Service ภายใน Docker
EXPOSE 8080 

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/services/PricingService/PricingService.Api/PricingService.Api.csproj", "src/services/PricingService/PricingService.Api/"]
COPY ["src/services/PricingService/PricingService.Application/PricingService.Application.csproj", "src/services/PricingService/PricingService.Application/"]
COPY ["src/services/PricingService/PricingService.Domain/PricingService.Domain.csproj", "src/services/PricingService/PricingService.Domain/"]
COPY ["src/services/PricingService/PricingService.Infrastructure/PricingService.Infrastructure.csproj", "src/services/PricingService/PricingService.Infrastructure/"]

RUN dotnet restore "src/services/PricingService/PricingService.Api/PricingService.Api.csproj"
COPY . .
WORKDIR "/src/src/services/PricingService/PricingService.Api"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PricingService.Api.dll"]