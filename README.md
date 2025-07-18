# Ambev Developer Evaluation - Sales API

## Description
Complete REST API for sales management implementing DDD (Domain-Driven Design) patterns with Clean Architecture. Built as part of Ambev's technical evaluation process.

## Features
- ✅ Complete CRUD operations for sales and users
- ✅ Quantity-based discount rules implementation
- ✅ Domain events (SaleCreated, SaleModified, SaleCancelled, ItemCancelled)
- ✅ JWT authentication and authorization
- ✅ MongoDB audit logging
- ✅ PostgreSQL main database
- ✅ Redis caching support
- ✅ Health checks and monitoring
- ✅ Comprehensive test coverage

## Business Rules

### Sales Discount Rules
- **4-9 identical items**: 10% discount
- **10-20 identical items**: 20% discount
- **Maximum 20 items** per product per sale
- **No discount** for quantities below 4 items

## Architecture

### Project Structure
```
src/
├── Ambev.DeveloperEvaluation.WebApi/      # API Controllers and Configuration
├── Ambev.DeveloperEvaluation.Application/ # Use Cases and Commands/Queries
├── Ambev.DeveloperEvaluation.Domain/      # Business Logic and Entities
├── Ambev.DeveloperEvaluation.ORM/         # Data Access and Repositories
├── Ambev.DeveloperEvaluation.Common/      # Shared Components
└── Ambev.DeveloperEvaluation.IoC/         # Dependency Injection

tests/
├── Ambev.DeveloperEvaluation.Unit/        # Unit Tests
├── Ambev.DeveloperEvaluation.Integration/ # Integration Tests
└── Ambev.DeveloperEvaluation.Functional/  # Functional Tests
```

### Design Patterns
- Domain-Driven Design (DDD)
- Clean Architecture
- CQRS with MediatR
- Repository Pattern
- External Identities Pattern
- Domain Events

## Prerequisites

- .NET 8.0 SDK or higher
- Docker and Docker Compose
- Visual Studio 2022+ or VS Code (recommended)

## Quick Start

### 1. Clone Repository
```bash
git clone https://github.com/your-username/ambev-developer-evaluation.git
cd ambev-developer-evaluation
```

### 2. Run with Docker Compose (Recommended)
```bash
# Start all services (API, PostgreSQL, MongoDB, Redis)
docker-compose up -d

# The API will be available at:
# - HTTP: http://localhost:8080
# - Swagger: http://localhost:8080/swagger
```

### 3. Manual Setup (Development)

#### Start Database Services
```bash
# Start only database services
docker-compose up -d ambev.developerevaluation.database ambev.developerevaluation.nosql ambev.developerevaluation.cache
```

#### Run the API
```bash
# Restore dependencies
dotnet restore

# Run database migrations
dotnet ef database update --project src/Ambev.DeveloperEvaluation.WebApi

# Start the API
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi

# API will be available at:
# - HTTPS: https://localhost:7181
# - HTTP: http://localhost:5119
# - Swagger: https://localhost:7181/swagger
```

## Database Configuration

### PostgreSQL (Main Database)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=developer_evaluation;Username=developer;Password=ev@luAt10n"
  }
}
```

### MongoDB (Audit Logs)
```json
{
  "MongoSettings": {
    "ConnectionString": "mongodb://developer:ev@luAt10n@localhost:27017",
    "Database": "DeveloperEvaluation_Audit"
  }
}
```

## Testing

### Run All Tests
```bash
dotnet test
```

### Run Tests by Category
```bash
# Unit tests only
dotnet test --filter Category=Unit

# Integration tests only
dotnet test --filter Category=Integration

# Functional tests only
dotnet test --filter Category=Functional
```

### Generate Coverage Report
```bash
# Windows
.\coverage-report.bat

# Linux/macOS
./coverage-report.sh

# View report
open TestResults/CoverageReport/index.html
```

## API Endpoints

### Authentication
- `POST /api/auth` - Authenticate user and get JWT token

### Users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create new user
- `DELETE /api/users/{id}` - Delete user

### Sales
- `GET /api/sales` - List sales (with pagination and filtering)
- `GET /api/sales/{id}` - Get sale by ID
- `POST /api/sales` - Create new sale
- `PUT /api/sales/{id}` - Update sale
- `DELETE /api/sales/{id}` - Cancel sale

### Example: Create Sale
```json
POST /api/sales
{
  "saleNumber": "SALE-2025-001",
  "date": "2025-01-15T14:30:00Z",
  "customerId": "123e4567-e89b-12d3-a456-426614174000",
  "branch": "Downtown Branch",
  "items": [
    {
      "productId": "123e4567-e89b-12d3-a456-426614174001",
      "quantity": 5,
      "unitPrice": 10.00
    }
  ]
}
```

### Example Response
```json
{
  "success": true,
  "message": "Sale created successfully",
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174002",
    "totalAmount": 45.00
  }
}
```

## Environment Variables

```bash
# Database
ConnectionStrings__DefaultConnection=Host=localhost;Database=developer_evaluation;Username=developer;Password=ev@luAt10n

# JWT
Jwt__SecretKey=YourSuperSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong

# MongoDB
MongoSettings__ConnectionString=mongodb://developer:ev@luAt10n@localhost:27017
MongoSettings__Database=DeveloperEvaluation_Audit

# Environment
ASPNETCORE_ENVIRONMENT=Development|Staging|Production

# Logging
Serilog__MinimumLevel=Information
```

## Development Tools

### Database Migrations
```bash
# Add new migration
dotnet ef migrations add MigrationName --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi

# Update database
dotnet ef database update --project src/Ambev.DeveloperEvaluation.WebApi

# Remove last migration
dotnet ef migrations remove --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi
```

### Build and Publish
```bash
# Build solution
dotnet build

# Publish for production
dotnet publish -c Release -o publish

# Run published version
cd publish && dotnet Ambev.DeveloperEvaluation.WebApi.dll
```

## Health Checks

The API includes comprehensive health checks:

- `/health` - Overall application health
- `/health/live` - Liveness probe (for Kubernetes)
- `/health/ready` - Readiness probe (for Kubernetes)

## Monitoring and Logs

### Structured Logging
The application uses Serilog with structured logging:

```json
{
  "timestamp": "2025-01-15T14:30:00.000Z",
  "level": "Information",
  "message": "Sale created successfully",
  "properties": {
    "correlationId": "abc-123-def",
    "saleId": "123e4567-e89b-12d3-a456-426614174002",
    "customerId": "123e4567-e89b-12d3-a456-426614174000"
  }
}
```

### Domain Events
All business operations trigger domain events:
- **SaleCreated**: When a sale is successfully created
- **SaleModified**: When sale details are updated
- **SaleCancelled**: When a sale is cancelled
- **ItemCancelled**: When individual items are cancelled


**Developed for Ambev Technical Assessment**
