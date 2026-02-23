# Testing Strategy & Examples

## Overview

This document provides testing strategies and example test cases for the refactored components architecture.

---

## Frontend Testing

### Service Testing (Jasmine/Karma)

#### 1. ProductService Tests

**Location**: `src/app/features/products/services/product.service.spec.ts`

```typescript
describe('ProductService', () => {
  let service: ProductService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ProductService]
    });
    service = TestBed.inject(ProductService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('getAllProducts', () => {
    it('should fetch all products', (done) => {
      const mockProducts: Product[] = [
        { id: 1, name: 'Product 1', description: 'Desc', categoryId: 1, price: 100, imageUrl: 'url' },
        { id: 2, name: 'Product 2', description: 'Desc', categoryId: 2, price: 200, imageUrl: 'url' }
      ];

      service.getAllProducts().subscribe(products => {
        expect(products.length).toBe(2);
        expect(products[0].name).toBe('Product 1');
        done();
      });

      const req = httpMock.expectOne('http://localhost:5209/api/product');
      expect(req.request.method).toBe('GET');
      req.flush(mockProducts);
    });
  });

  describe('getProductById', () => {
    it('should fetch product by ID', (done) => {
      const mockProduct: Product = { id: 1, name: 'Product 1', description: 'Desc', categoryId: 1, price: 100, imageUrl: 'url' };

      service.getProductById(1).subscribe(product => {
        expect(product.id).toBe(1);
        expect(product.name).toBe('Product 1');
        done();
      });

      const req = httpMock.expectOne('http://localhost:5209/api/product/1');
      expect(req.request.method).toBe('GET');
      req.flush(mockProduct);
    });
  });

  describe('searchProducts', () => {
    it('should search products by term', (done) => {
      const mockProducts: Product[] = [
        { id: 1, name: 'MacBook', description: 'Laptop', categoryId: 1, price: 1999, imageUrl: 'url' }
      ];

      service.searchProducts('MacBook').subscribe(products => {
        expect(products.length).toBe(1);
        expect(products[0].name).toBe('MacBook');
        done();
      });

      const req = httpMock.expectOne('http://localhost:5209/api/product/search?term=MacBook');
      expect(req.request.method).toBe('GET');
      req.flush(mockProducts);
    });
  });

  describe('createProduct', () => {
    it('should create a new product', (done) => {
      const newProduct: Product = { id: 0, name: 'New Product', description: 'Desc', categoryId: 1, price: 50, imageUrl: 'url' };
      const createdProduct: Product = { ...newProduct, id: 28 };

      service.createProduct(newProduct).subscribe(product => {
        expect(product.id).toBe(28);
        done();
      });

      const req = httpMock.expectOne('http://localhost:5209/api/product');
      expect(req.request.method).toBe('POST');
      req.flush(createdProduct);
    });
  });
});
```

#### 2. CartService Tests

**Location**: `src/app/features/cart/services/cart.service.spec.ts`

```typescript
describe('CartService', () => {
  let service: CartService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CartService]
    });
    service = TestBed.inject(CartService);
  });

  describe('addToCart', () => {
    it('should add a product to cart', () => {
      const product: Product = { id: 1, name: 'Product', description: 'Desc', categoryId: 1, price: 100, imageUrl: 'url' };
      
      service.addToCart(product);
      
      expect(service.items().length).toBe(1);
      expect(service.items()[0].quantity).toBe(1);
    });

    it('should increment quantity if product already in cart', () => {
      const product: Product = { id: 1, name: 'Product', description: 'Desc', categoryId: 1, price: 100, imageUrl: 'url' };
      
      service.addToCart(product);
      service.addToCart(product);
      
      expect(service.items().length).toBe(1);
      expect(service.items()[0].quantity).toBe(2);
    });
  });

  describe('removeFromCart', () => {
    it('should remove product from cart', () => {
      const product: Product = { id: 1, name: 'Product', description: 'Desc', categoryId: 1, price: 100, imageUrl: 'url' };
      
      service.addToCart(product);
      service.removeFromCart(1);
      
      expect(service.items().length).toBe(0);
    });
  });

  describe('updateQuantity', () => {
    it('should update product quantity', () => {
      const product: Product = { id: 1, name: 'Product', description: 'Desc', categoryId: 1, price: 100, imageUrl: 'url' };
      
      service.addToCart(product);
      service.updateQuantity(1, 5);
      
      expect(service.items()[0].quantity).toBe(5);
    });

    it('should remove product if quantity becomes zero', () => {
      const product: Product = { id: 1, name: 'Product', description: 'Desc', categoryId: 1, price: 100, imageUrl: 'url' };
      
      service.addToCart(product);
      service.updateQuantity(1, 0);
      
      expect(service.items().length).toBe(0);
    });
  });

  describe('totalPrice', () => {
    it('should calculate total price correctly', () => {
      const product1: Product = { id: 1, name: 'Product 1', description: 'Desc', categoryId: 1, price: 100, imageUrl: 'url' };
      const product2: Product = { id: 2, name: 'Product 2', description: 'Desc', categoryId: 1, price: 50, imageUrl: 'url' };
      
      service.addToCart(product1);
      service.addToCart(product2);
      
      expect(service.totalPrice()).toBe(150);
    });
  });

  describe('isInCart', () => {
    it('should return true if product is in cart', () => {
      const product: Product = { id: 1, name: 'Product', description: 'Desc', categoryId: 1, price: 100, imageUrl: 'url' };
      
      service.addToCart(product);
      
      expect(service.isInCart(1)).toBe(true);
      expect(service.isInCart(2)).toBe(false);
    });
  });
});
```

#### 3. WishlistService Tests

**Location**: `src/app/features/wishlist/services/wishlist.service.spec.ts`

```typescript
describe('WishlistService', () => {
  let service: WishlistService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WishlistService]
    });
    service = TestBed.inject(WishlistService);
  });

  describe('addToWishlist', () => {
    it('should add product ID to wishlist', () => {
      service.addToWishlist(1);
      expect(service.items()).toContain(1);
    });

    it('should not add duplicate product IDs', () => {
      service.addToWishlist(1);
      service.addToWishlist(1);
      expect(service.items().length).toBe(1);
    });
  });

  describe('removeFromWishlist', () => {
    it('should remove product ID from wishlist', () => {
      service.addToWishlist(1);
      service.removeFromWishlist(1);
      expect(service.items().length).toBe(0);
    });
  });

  describe('toggleWishlist', () => {
    it('should add product if not in wishlist', () => {
      service.toggleWishlist(1);
      expect(service.isInWishlist(1)).toBe(true);
    });

    it('should remove product if already in wishlist', () => {
      service.addToWishlist(1);
      service.toggleWishlist(1);
      expect(service.isInWishlist(1)).toBe(false);
    });
  });

  describe('isInWishlist', () => {
    it('should return correct wishlist status', () => {
      service.addToWishlist(1);
      expect(service.isInWishlist(1)).toBe(true);
      expect(service.isInWishlist(2)).toBe(false);
    });
  });
});
```

### Component Testing

#### ProductCardComponent Tests

**Location**: `src/app/features/products/components/product-card.component.spec.ts`

```typescript
describe('ProductCardComponent', () => {
  let component: ProductCardComponent;
  let fixture: ComponentFixture<ProductCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProductCardComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(ProductCardComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should emit addToCart event when button clicked', () => {
    const product: Product = { id: 1, name: 'Test', description: 'Test', categoryId: 1, price: 100, imageUrl: 'url' };
    component.product = product;
    
    spyOn(component.addToCart, 'emit');
    component.onAddToCart();
    
    expect(component.addToCart.emit).toHaveBeenCalledWith(product);
  });

  it('should emit toggleWishlist event when wishlist button clicked', () => {
    const product: Product = { id: 1, name: 'Test', description: 'Test', categoryId: 1, price: 100, imageUrl: 'url' };
    component.product = product;
    
    spyOn(component.toggleWishlist, 'emit');
    component.onToggleWishlist();
    
    expect(component.toggleWishlist.emit).toHaveBeenCalledWith(product);
  });
});
```

---

## Backend Testing

### Service Testing (NUnit)

#### ProductService Tests

**Location**: `BackEnd/AnayaCore/Services/ProductServiceTests.cs`

```csharp
using NUnit.Framework;
using Moq;
using AnayaCore.Models;
using AnayaCore.Repositories;

namespace AnayaCore.Tests.Services
{
    [TestFixture]
    public class ProductServiceTests
    {
        private IProductService _productService;
        private Mock<IProductRepository> _repositoryMock;
        private Mock<ILogger<ProductService>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _loggerMock = new Mock<ILogger<ProductService>>();
            _productService = new ProductService(_repositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAllProductsAsync_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 100 },
                new Product { Id = 2, Name = "Product 2", Price = 200 }
            };
            _repositoryMock.Setup(r => r.GetAllProductsAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            _repositoryMock.Verify(r => r.GetAllProductsAsync(), Times.Once);
        }

        [Test]
        public async Task GetProductByIdAsync_ReturnsProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Price = 100 };
            _repositoryMock.Setup(r => r.GetProductByIdAsync(1))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(1);

            // Assert
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Product 1"));
        }

        [Test]
        public async Task CreateProductAsync_WithValidData_CreatesProduct()
        {
            // Arrange
            var product = new Product { Name = "New Product", Price = 150 };
            var createdProduct = new Product { Id = 3, Name = "New Product", Price = 150 };
            
            _repositoryMock.Setup(r => r.CreateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(createdProduct);

            // Act
            var result = await _productService.CreateProductAsync(product);

            // Assert
            Assert.That(result.Id, Is.EqualTo(3));
            _repositoryMock.Verify(r => r.CreateProductAsync(It.IsAny<Product>()), Times.Once);
        }

        [Test]
        public async Task CreateProductAsync_WithEmptyName_ThrowsException()
        {
            // Arrange
            var product = new Product { Name = "", Price = 100 };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(
                async () => await _productService.CreateProductAsync(product));
        }

        [Test]
        public async Task CreateProductAsync_WithNegativePrice_ThrowsException()
        {
            // Arrange
            var product = new Product { Name = "Product", Price = -100 };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(
                async () => await _productService.CreateProductAsync(product));
        }

        [Test]
        public async Task SearchProductsAsync_ReturnsMatchingProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "MacBook", PricePrice = 1999 }
            };
            _repositoryMock.Setup(r => r.SearchProductsAsync("MacBook"))
                .ReturnsAsync(products);

            // Act
            var result = await _productService.SearchProductsAsync("MacBook");

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Name, Contains.Substring("MacBook"));
        }
    }
}
```

#### CategoryService Tests

**Location**: `BackEnd/AnayaCore/Services/CategoryServiceTests.cs`

```csharp
[TestFixture]
public class CategoryServiceTests
{
    private ICategoryService _categoryService;
    private Mock<ICategoryRepository> _repositoryMock;
    private Mock<ILogger<CategoryService>> _loggerMock;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<ICategoryRepository>();
        _loggerMock = new Mock<ILogger<CategoryService>>();
        _categoryService = new CategoryService(_repositoryMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task GetAllCategoriesAsync_ReturnsAllCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Accessories" }
        };
        _repositoryMock.Setup(r => r.GetAllCategoriesAsync())
            .ReturnsAsync(categories);

        // Act
        var result = await _categoryService.GetAllCategoriesAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task CreateCategoryAsync_WithValidData_CreatesCategory()
    {
        // Arrange
        var category = new Category { Name = "New Category" };
        var createdCategory = new Category { Id = 7, Name = "New Category" };
        
        _repositoryMock.Setup(r => r.CreateCategoryAsync(It.IsAny<Category>()))
            .ReturnsAsync(createdCategory);

        // Act
        var result = await _categoryService.CreateCategoryAsync(category);

        // Assert
        Assert.That(result.Id, Is.EqualTo(7));
    }
}
```

### Controller Testing

#### ProductControllerRefactored Tests

**Location**: `BackEnd/AnayaCore/Controllers/ProductControllerRefactoredTests.cs`

```csharp
[TestFixture]
public class ProductControllerRefactoredTests
{
    private ProductControllerRefactored _controller;
    private Mock<IProductService> _serviceMock;
    private Mock<ILogger<ProductControllerRefactored>> _loggerMock;

    [SetUp]
    public void Setup()
    {
        _serviceMock = new Mock<IProductService>();
        _loggerMock = new Mock<ILogger<ProductControllerRefactored>>();
        _controller = new ProductControllerRefactored(_serviceMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task GetAllProducts_ReturnsOkResult()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1" }
        };
        _serviceMock.Setup(s => s.GetAllProductsAsync())
            .ReturnsAsync(products);

        // Act
        var result = await _controller.GetAllProducts() as OkObjectResult;

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.EqualTo(products));
    }

    [Test]
    public async Task GetProductById_WithValidId_ReturnsProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Product 1" };
        _serviceMock.Setup(s => s.GetProductByIdAsync(1))
            .ReturnsAsync(product);

        // Act
        var result = await _controller.GetProductById(1) as OkObjectResult;

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.EqualTo(product));
    }

    [Test]
    public async Task GetProductById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetProductByIdAsync(999))
            .ReturnsAsync((Product)null);

        // Act
        var result = await _controller.GetProductById(999) as NotFoundObjectResult;

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(404));
    }
}
```

---

## Integration Testing

### Frontend E2E Tests (Cypress/Playwright)

**Location**: `cypress/e2e/products.cy.ts`

```typescript
describe('Products Page E2E Tests', () => {
  beforeEach(() => {
    cy.visit('http://localhost:4200');
  });

  it('should load products on page load', () => {
    cy.get('app-product-card').should('have.length.greaterThan', 0);
  });

  it('should search products', () => {
    cy.get('input[placeholder="Search products..."]').type('MacBook');
    cy.get('button').contains('Search').click();
    cy.get('app-product-card').first().should('contain', 'MacBook');
  });

  it('should filter by category', () => {
    cy.get('button').contains('Electronics').click();
    cy.get('app-product-card').should('have.length.greaterThan', 0);
  });

  it('should add product to cart', () => {
    cy.get('app-product-card').first().find('button').contains('Add to Cart').click();
    cy.get('.notification').should('contain', 'added to cart');
  });

  it('should add product to wishlist', () => {
    cy.get('app-product-card').first().find('button').contains('❤').click();
    cy.get('.notification').should('contain', 'added to wishlist');
  });
});
```

### Backend Integration Tests

**Location**: `BackEnd/AnayaCore/IntegrationTests/ProductApiTests.cs`

```csharp
[TestFixture]
public class ProductApiIntegrationTests
{
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;

    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task GetAllProducts_ReturnsOkStatus()
    {
        // Act
        var response = await _client.GetAsync("/api/product");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }

    [Test]
    public async Task CreateProduct_WithValidData_ReturnsCreatedStatus()
    {
        // Arrange
        var product = new Product { Name = "New Product", Price = 100, CategoryId = 1 };
        var content = new StringContent(
            JsonSerializer.Serialize(product),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await _client.PostAsync("/api/product", content);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created));
    }
}
```

---

## Test Coverage Goals

| Component/Service | Target Coverage | Priority |
|---|---|---|
| ProductService | 85%+ | High |
| CategoryService | 85%+ | High |
| CartService | 95%+ | Critical |
| WishlistService | 95%+ | Critical |
| ProductCardComponent | 80%+ | High |
| ProductController | 90%+ | Critical |
| CategoryController | 90%+ | Critical |

---

## Running Tests

### Frontend
```bash
# Run tests
ng test

# Run tests with coverage
ng test --code-coverage

# Run E2E tests
npx cypress open
```

### Backend
```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test /p:CollectCoverage=true

# Run specific test class
dotnet test --filter "ClassName"
```

---

## Continuous Integration

### GitHub Actions Example
```yaml
name: Tests
on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      - name: Setup .NET
        uses: actions/setup-dotnet@v1
      - name: Run backend tests
        run: dotnet test
      - name: Setup Node.js
        uses: actions/setup-node@v2
      - name: Install frontend dependencies
        run: npm install
      - name: Run frontend tests
        run: npm test
```

---

## Best Practices

1. **Test Service Logic**: Test services independently from API calls
2. **Mock Dependencies**: Use mocks for HTTP calls and external services
3. **Test Edge Cases**: Include negative tests and boundary conditions
4. **Meaningful Names**: Use descriptive test names
5. **Arrange-Act-Assert**: Use AAA pattern in tests
6. **Test Behavior**: Test what components do, not how they do it
7. **DRY Principles**: Share test setup and utilities
8. **Keep Tests Fast**: Avoid real API calls in unit tests

---

**Note**: Copy the test skeletons above and customize them for your specific implementation. These serve as templates for proper test structure.
