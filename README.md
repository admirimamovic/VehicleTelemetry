# Vehicle Telemetry Receiver API

[![.NET 8](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/download)

RESTful API for receiving and retrieving vehicle telemetry data from sensors. Built with Clean Architecture and production-ready features.

---

## 🚀 Features

- ✅ **POST** telemetry data from vehicle sensors
- ✅ **GET** latest telemetry by device ID
- ✅ **GET** paginated telemetry records
- ✅ Global error handling middleware
- ✅ Background cloud upload simulation
- ✅ Health check endpoint
- ✅ Comprehensive unit tests
- ✅ Swagger/OpenAPI documentation

---

## 📋 Tech Stack

- **.NET 8** - Latest framework
- **ASP.NET Core Web API** - RESTful services
- **Entity Framework Core 8.0.25** - ORM
- **SQLite** - Database
- **xUnit** - Unit testing
- **Moq** - Mocking framework
- **Swagger** - API documentation

---

## 🏗️ Architecture
VehicleTelemetry/ 
├── API/Controllers/            # Web API Endpoints 
├── BL/Services/                # Business Logic 
├── DL/ 
    ├── DTOs/                   # Data Transfer Objects 
    └── Entities/               # Domain Models   
        └── DB/                 # DbContext 
├── IoC/                        # Dependency Injection 
    └── Middlewares/            # Global Error Handling

---

## 🔧 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 or VS Code

### Run Application

Clone repository
git clone <repository-url> cd VehicleTelemetry

Restore dependencies
dotnet restore

Run application
dotnet run --project VehicleTelemetry


### Access Swagger UI

https://localhost:5001


### Run Tests

dotnet test

---

## 📡 API Endpoints

### POST Create Telemetry

POST /api/v1/telemetry Content-Type: application/json

{ 
    "deviceId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", 
    "timestamp": "2024-01-15T10:30:00Z", 
    "engineRPM": 3000, 
    "fuelLevelPercentage": 75.5, 
    "latitude": 45.2671, 
    "longitude": 19.8335
}


**Response:** `201 Created`

---

### GET Latest Telemetry

GET /api/v1/telemetry/{deviceId}/latest


**Responses:**
- `200 OK` - Telemetry record found
- `404 Not Found` - No data for device

---

### GET Paginated Telemetry


GET /api/v1/telemetry?pageNumber=1&pageSize=10


**Response:** `200 OK`

{
    "items": [...], 
    "pageNumber": 1, 
    "pageSize": 10, 
    "totalCount": 245, 
    "totalPages": 25, 
    "hasPreviousPage": false, 
    "hasNextPage": true
}



---

### GET Health Check

GET /hc

---

---

## 🔄 Background Services

**CloudUploadService** - Simulates cloud upload every 60 seconds
- Fetches latest 5 records
- Logs upload activity
- Runs as IHostedService

---

## ⚙️ Configuration

**appsettings.json**

{ 
"ConnectionStrings": { 
    "DefaultConnection": "Data Source=telemetry.db" 
}, 
"Logging": { 
    "LogLevel": 
    { 
        "Default": "Information" 
    } 
} 
}

---

## 🛡️ Best Practices

- ✅ Clean Architecture
- ✅ Async/Await for all I/O
- ✅ Dependency Injection
- ✅ Global error handling
- ✅ DTOs for API contracts
- ✅ Unit testing with In-Memory DB
- ✅ Structured logging
- ✅ API versioning (`/api/v1/`)

---

## 📝 Documentation

Interactive API documentation:

If you choose http, urls are: 
http://localhost:5191/swagger 
and 
http://localhost:5191/swagger/v1/swagger.json


If you choose IIS Express, urls are: 
http://localhost:44315/swagger 
and 
https://localhost:44315/swagger/v1/swagger.json

