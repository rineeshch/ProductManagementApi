# Product Management API

A .NET Core Web API for managing products with CRUD operations and stock management functionality.

## Features

- CRUD operations for products
- Unique 6-digit product ID generation
- Stock management (increment/decrement)
- Entity Framework Core with SQL Server
- RESTful API design

## Prerequisites

- .NET 6.0 SDK or later
- SQL Server LocalDB (included with Visual Studio)
- Visual Studio 2022 or VS Code

## Getting Started

1. Clone the repository
```bash
git clone [repository-url]
cd ProductManagementApi
```

2. Update Database
```bash
dotnet ef database update --project ProductManagement.Infrastructure --startup-project ProductManagement.Api
```

3. Run the application
```bash
dotnet run --project ProductManagement.Api
```

The API will be available at `http://localhost:5059`

## API Endpoints

### Products

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create new product |
| PUT | `/api/products/{id}` | Update product |
| DELETE | `/api/products/{id}` | Delete product |
| PUT | `/api/products/decrement-stock/{id}/{quantity}` | Decrease stock |
| PUT | `/api/products/add-to-stock/{id}/{quantity}` | Increase stock |

## Sample Requests

### Create Product
```json
POST /api/products
{
  "name": "Test Product",
  "price": 99.99,
  "stockAvailable": 50
}
```

## Database Configuration

The application uses SQL Server LocalDB. Connection string can be modified in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProductManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

## Project Structure

- **ProductManagement.Api**: Web API controllers and configuration
- **ProductManagement.Core**: Domain entities and interfaces
- **ProductManagement.Infrastructure**: Data access and repository implementations

## Error Handling

- 404: Resource not found
- 400: Bad request (validation errors)
- 500: Internal server error

## Development Notes

- Product IDs are automatically generated as unique 6-digit numbers
- Stock validation is performed before decrement operations
- Entity Framework Core is used with Code First approach

### Sample Request:
```json
POST /api/products
{
  "name": "Test Product",
  "price": 99.99,
  "stockAvailable": 50
}
```
