# PricingPlatform
A microservices-based pricing engine with rule-driven pipeline execution, API Gateway, and observability.

---

## 🧠 Architecture
Microservices + DDD + Clean architecture

## 📦 Services

### 1. ApiGateway
- Reverse proxy (YARP)

### 2. PricingService
- Handles quote calculation
- Calls Pricing Engine
- Exposes `/quotes`

### 3. RulesService
- CRUD rules
- Provides active rule set
- Webhook integration

### 4. PricingPlatform.Engine
- Core pricing logic
- Rule evaluators:
  - WeightTier
  - RemoteAreaSurcharge
  - TimeWindowPromotion
- Compiled pipeline execution

### ▶️ How to Run Doker
docker-compose up --build

!! Doker พัง http://localhost:5000 !! ตอนนี้มีปัญหา เรื่องการ Map ระหว่าง Gateway กับ PricingService และ RulesService
!! ใช้ run แต่ละ project ไปก่อน

dotnet run --project src/services/PricingService
dotnet run --project src/services/RulesService

### 🌐 Endpoints
PricingService 
  http://localhost:5113/Swagger/index.html
RulesService 
  http://localhost:5178/Swagger/index.html

### 📥 Example Request
# PricingService
POST /pricing/quote
{
  "basePrice": 100,
  "weight": 5,
  "zone": "A"
}
  
Response
{
  "price": 160
}

POST /quotes/bulk
{อัพโหลดไฟล์ csv}

Response
{
  "jobid": "9f18739d-27b6-455b-8f52-440265692040"
}

# RulesService
POST /rules
{
  "type": 2,
  "configJson": "{\"zones\":[\"A\",\"B\"],\"surchargeType\":\"Percent\",\"value\":20}",
  "priority": 10,
  "effectiveFrom": "2026-01-01T00:00:00Z",
  "effectiveTo": "2026-12-31T23:59:59Z"
}

Response
{
  "id": "1A18222d-27b6-665b-8f52-440265692022"
}

## 🧪 Run Tests
Unit Tests
  dotnet test tests/Unit

Integration Tests
  dotnet test tests/Integration

## ⚙️ Key Features
  Rule-based pricing engine
  Compiled pipeline execution (performance optimized)
  Additive / multiplier model
  Override pricing support
  Microservices architecture
  Rate limiting (API Gateway)
  Health checks per service


