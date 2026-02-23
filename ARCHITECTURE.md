# Anaya E-Commerce Component Architecture Refactoring

## Overview
This document describes the refactored component architecture for the Anaya E-commerce application. The refactoring organizes both frontend and backend code into a more maintainable, scalable, and testable structure following industry best practices.

## Architecture Principles
- **Separation of Concerns**: Each layer has a specific responsibility
- **Single Responsibility Principle**: Classes and components have one reason to change
- **Dependency Injection**: Services are injected rather than created locally
- **Loose Coupling**: Components depend on abstractions rather than concrete implementations
- **DRY (Don't Repeat Yourself)**: Code reuse through shared components and services

---

## Frontend Architecture (Angular)

### Directory Structure
```
src/app/
├── core/                          # Application-wide services, guards, interceptors
│   ├── interceptors/
│   └── services/
│       ├── notification.service.ts
│       ├── loading.service.ts
│       └── index.ts
│
├── shared/                        # Shared across features
│   ├── components/
│   │   └── search-bar.component.ts
│   ├── models/
│   │   ├── product.ts
│   │   ├── category.ts
│   │   ├── cart-item.ts
│   │   └── index.ts
│   └── pipes/
│
├── features/                      # Feature modules
│   ├── products/
│   │   ├── services/
│   │   │   └── product.service.ts
│   │   ├── components/
│   │   │   └── product-card.component.ts
│   │   └── products.module.ts (optional)
│   │
│   ├── category/
│   │   ├── services/
│   │   │   └── category.service.ts
│   │   ├── components/
│   │   │   └── category-filter.component.ts
│   │   └── category.module.ts (optional)
│   │
│   ├── cart/
│   │   ├── services/
│   │   │   └── cart.service.ts
│   │   └── cart.module.ts (optional)
│   │
│   └── wishlist/
│       ├── services/
│       │   └── wishlist.service.ts
│       └── wishlist.module.ts (optional)
│
└── app.ts (refactored root component)
```

### Layer Responsibilities

#### 1. **Core Services** (`/core/services/`)
Application-wide, singleton services
- `NotificationService`: Centralized notification management
- `LoadingService`: Global loading state management
- Future: Guards, interceptors, authentication

#### 2. **Shared Models** (`/shared/models/`)
Shared data structures used across the app
- `Product`: Product interface with filtering options
- `Category`: Category interface
- `CartItem`: Cart item model extending Product
- Barrel export (`index.ts`) for clean imports

#### 3. **Shared Components** (`/shared/components/`)
Reusable components used in multiple features
- `SearchBarComponent`: Product search input
- Future: Loading spinner, error message, pagination, etc.

#### 4. **Feature Services** (`/features/*/services/`)
Feature-specific business logic layer
- **ProductService**: All product-related API calls
- **CategoryService**: Category API operations
- **CartService**: State management for shopping cart (using signals)
- **WishlistService**: State management for wishlist items

#### 5. **Feature Components** (`/features/*/components/`)
Feature-specific UI components
- **ProductCardComponent**: Displays individual product
- **CategoryFilterComponent**: Category filter menu
- Future: Cart page, wishlist page, product details page

#### 6. **Root Component** (`app.ts`)
Orchestrates features and manages global state/flows
- Coordinates between services
- Manages data loading
- Handles filter and search logic
- Routes user actions to appropriate services

### Key Design Patterns

**Signals-based State Management**
```typescript
// Services manage state using signals
products = signal<Product[]>([]);
cart = signal<CartItem[]>([]);

// Components use signals for reactivity
isInWishlist = signal(false);
```

**Service Composition**
```typescript
// Components receive services through constructor injection
constructor(
  private productService: ProductService,
  private cartService: CartService,
  private notificationService: NotificationService
)
```

**Event-driven Communication**
```typescript
// Components emit events for user actions
@Output() addToCart = new EventEmitter<Product>();
@Output() toggleWishlist = new EventEmitter<Product>();
```

---

## Backend Architecture (.NET/C#)

### Directory Structure
```
BackEnd/AnayaCore/
├── Controllers/
│   ├── ProductController.cs (original - to be deprecated)
│   ├── ProductControllerRefactored.cs (new)
│   ├── CategoryController.cs (original - to be deprecated)
│   ├── CategoryControllerRefactored.cs (new)
│   └── CartController.cs
│
├── Services/
│   ├── IProductService.cs (interface)
│   ├── ProductService.cs (implementation)
│   ├── ICategoryService.cs (interface)
│   ├── CategoryService.cs (implementation)
│   └── ICartService.cs (interface)
│
├── Repositories/
│   ├── IProductRepository.cs (interface)
│   ├── ProductRepository.cs (in-memory implementation)
│   ├── ICategoryRepository.cs (interface)
│   └── CategoryRepository.cs (in-memory implementation)
│
├── Models/
│   ├── Product.cs
│   ├── Category.cs
│   ├── Customer.cs
│   ├── Cart.cs
│   └── ... (domain models)
│
├── Program.cs (configuration)
└── appsettings.json
```

### Layer Responsibilities

#### 1. **Controllers** (`/Controllers/`)
HTTP request/response handling
- Validate incoming requests
- Call services for business logic
- Return appropriate HTTP responses
- Log operations

**Pattern: dependency injection of services**
```csharp
public ProductControllerRefactored(
    IProductService productService,
    ILogger<ProductControllerRefactored> logger)
```

#### 2. **Services** (`/Services/`)
Business logic layer
- Implement business rules and validation
- Orchestrate data operations
- Handle cross-cutting concerns (logging, validation)
- Independent of HTTP concerns

**Interface-based design**
```csharp
public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    // ... more methods
}
```

#### 3. **Repositories** (`/Repositories/`)
Data access layer
- Encapsulate data retrieval logic
- Currently in-memory implementation
- Ready for database integration
- Provide `Async` methods for scalability

**Repository Pattern**
```csharp
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    // ... more methods
}
```

#### 4. **Models** (`/Models/`)
Domain models
- `Product.cs`: Product entity
- `Category.cs`: Category entity
- Data validation attributes
- Database-agnostic (ready for EF Core)

### Architecture Benefits

```
┌─────────────────────┐
│  HTTP Requests      │
└──────────┬──────────┘
           │
      ┌────▼──────┐
      │ Controller│  ← Validates request, orchestrates response
      └────┬──────┘
           │
      ┌────▼────────┐
      │  Service    │  ← Business logic, validation, rules
      └────┬────────┘
           │
      ┌────▼─────────────┐
      │  Repository      │  ← Data access, queries
      └────┬─────────────┘
           │
      ┌────▼─────────────┐
      │  Database        │  ← Persistence (future)
      └──────────────────┘
```

### Async/Await Pattern
All service and repository methods are async:
```csharp
public async Task<IEnumerable<Product>> GetAllProductsAsync()
{
    return await _productRepository.GetAllProductsAsync();
}
```

---

## Integration Guide

### Program.cs Setup (Backend)
```csharp
// Register repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Register services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Add logging
builder.Services.AddLogging();
```

### Dependency Injection Configuration
Services are wired up in `Program.cs` using dependency injection:
- Controllers receive services through constructor
- Services receive repositories through constructor
- All dependencies are managed by the DI container

---

## Migration Path

### Phase 1: Parallel Implementation (Current)
- New refactored controllers run alongside old ones
- Old endpoints remain functional
- Gradual migration of client code

### Phase 2: Client Migration
- Update frontend to use new endpoints
- Remove old controller routes
- Complete testing

### Phase 3: Database Integration
- Replace in-memory repositories with EF Core
- Add database migrations
- Update service validation logic

### Phase 4: Advanced Features
- Add caching layer
- Implement audit logging
- Add performance monitoring

---

## Best Practices Applied

1. **Separation of Concerns**: Each layer has a specific responsibility
2. **Dependency Inversion**: Depend on abstractions (interfaces)
3. **Single Responsibility**: Services focus on business logic
4. **DRY Principle**: Shared code in base classes, utilities
5. **Async Operations**: All I/O is non-blocking
6. **Error Handling**: Proper exception handling and logging
7. **Validation**: Input validation at service layer
8. **Documentation**: XML comments on public members

---

## Future Enhancements

### Frontend
- [ ] Feature-specific routing modules
- [ ] State management library (NgRx)
- [ ] Error handling component
- [ ] Loading skeleton screens
- [ ] Interceptors for auth/error handling
- [ ] Unit tests for services and components
- [ ] E2E tests

### Backend
- [ ] Entity Framework Core integration
- [ ] Database layer with migrations
- [ ] Unit of Work pattern
- [ ] Caching strategy (Redis)
- [ ] API versioning
- [ ] CORS configuration
- [ ] Authentication/Authorization
- [ ] API documentation (Swagger)
- [ ] Unit and Integration tests
- [ ] Docker containerization

---

## Component Communication Flow

```
User Interaction (Click)
        ↓
Component Method Called
        ↓
Service Method Invoked (via DI)
        ↓
HTTP Request to API
        ↓
Controller Route Handler
        ↓
Service Business Logic
        ↓
Repository Data Access
        ↓
Response JSON
        ↓
Frontend Service processes Response
        ↓
Component Signal Updated
        ↓
Template Re-renders (Angular Change Detection)
        ↓
User Sees Updated UI
```

---

## Testing Strategy

### Frontend Unit Tests
- Test services in isolation
- Mock HTTP calls
- Test component input/output
- Verify signal changes

### Backend Unit Tests
- Test service business logic
- Mock repositories
- Test validation rules

### Integration Tests
- Test service + repository interaction
- Test API endpoints
- Test error scenarios

---

## Summary

This refactored architecture provides:
✅ **Scale**: Easy to add new features
✅ **Maintain**: Clear code organization
✅ **Test**: Isolated, testable layers
✅ **Reuse**: Shared components and services
✅ **Understand**: Clear separation of concerns
✅ **Flexibility**: Easy to swap implementations (e.g., database)
