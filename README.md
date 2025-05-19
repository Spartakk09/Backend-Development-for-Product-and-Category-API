# Backend Development for Product & Category API

## Project Overview
This project implements a RESTful API for managing **Products** and **Categories** using a **Code-First** approach with Entity Framework Core and SQLite.  
Each product can belong to **2 or 3 categories**. The API supports **pagination** for listing products and categories, ensuring efficient data retrieval for large datasets.

---

## Technology Stack
- **.NET Core 7** or later  
- **C#**  
- **Entity Framework Core** (Code-First, SQLite)  
- **Swagger** (for API documentation)  
- **Fluent Validation** (for request validation)  

---

## Features & API Endpoints

### Category API
- **Create a category**  
- **Get a paginated list of all categories**  
- **Get category details by ID**  
- **Update a category**  
- **Delete a category**

### Product API
- **Create a product with multiple categories (2 or 3)**  
- **Get a paginated list of all products with assigned categories**  
- **Get product details by ID**  
- **Update a product and its categories**  
- **Delete a product**

---

## Development Approach
- **Repository & Service pattern** for separation of concerns and maintainability  
- **DTOs** (Data Transfer Objects) for request and response models  
- **Fluent Validation** to validate API requests and ensure data integrity  
- **Pagination** implemented efficiently using EF Core's query capabilities  
- **Error handling and logging** for robust API behavior  
- **SQLite database** with EF Core migrations for easy setup and schema evolution  
- **Swagger** integrated for interactive API documentation and testing

---

## Getting Started

### Prerequisites
- [.NET SDK 7.0 or later](https://dotnet.microsoft.com/download)  
- SQLite (optional, can run in-memory for testing)  
- API testing tools like Swagger UI

### Installation & Setup
1. Clone the repository  
   ```bash
   https://github.com/Spartakk09/Backend-Development-for-Product-and-Category-API.git
````

2. Navigate to the project directory

   ```bash
   cd Backend-Development-for-Product-Category-API
   ```
3. Restore dependencies

   ```bash
   dotnet restore
   ```
4. Update database connection string in `appsettings.json` (if needed)
5. Apply EF Core migrations

   ```bash
   dotnet ef database update
   ```
6. Run the API

   ```bash
   dotnet run
   ```

---


## Project Structure

```
Backend-Development-for-Product-Category-API/
│
├── Controllers/          # API controllers for Products and Categories
├── Data/                 # EF Core DbContext and migrations
├── DTOs/                 # Request and response models
├── Models/               # Entity models
├── Repositories/         # Repository implementations
├── Services/             # Business logic implementations
├── Validators/           # Fluent Validation classes
├── appsettings.json      # Configuration file
├── Program.cs            # Application entry point
└── README.md             # Project documentation
```

---
