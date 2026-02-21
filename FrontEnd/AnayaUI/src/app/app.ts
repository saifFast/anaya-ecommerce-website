import { Component, OnInit, signal } from '@angular/core';
import { ProductService } from './services/product.service';
import { CategoryService } from './services/category.service';
import { Product } from './models/product';
import { Category } from './models/category';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('AnayaUI');
  
  products = signal<Product[]>([]);
  categories = signal<Category[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);
  selectedCategory = signal<number | null>(null);
  searchTerm = signal('');
  cart = signal<Product[]>([]);
  wishlist = signal<number[]>([]);

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService
  ) {}

  ngOnInit(): void {
    this.loadCategories();
    this.loadProducts();
  }

  /**
   * Load all categories
   */
  loadCategories(): void {
    this.loading.set(true);
    this.categoryService.getAllCategories().subscribe({
      next: (data) => {
        this.categories.set(data);
        this.error.set(null);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error loading categories:', err);
        this.error.set('Failed to load categories');
        this.loading.set(false);
      }
    });
  }

  /**
   * Load all products or filtered products
   */
  loadProducts(): void {
    this.loading.set(true);
    const searchTerm = this.searchTerm();
    
    if (searchTerm.trim()) {
      // Search products if search term is provided
      this.productService.searchProducts(searchTerm).subscribe({
        next: (data) => {
          this.products.set(data);
          this.error.set(null);
          this.loading.set(false);
        },
        error: (err) => {
          console.error('Error searching products:', err);
          this.error.set('Failed to search products');
          this.loading.set(false);
        }
      });
    } else if (this.selectedCategory()) {
      // Get products by category if category is selected
      this.productService.getProductsByCategory(this.selectedCategory()!).subscribe({
        next: (data) => {
          this.products.set(data);
          this.error.set(null);
          this.loading.set(false);
        },
        error: (err) => {
          console.error('Error loading products by category:', err);
          this.error.set('Failed to load products');
          this.loading.set(false);
        }
      });
    } else {
      // Get all products
      this.productService.getAllProducts().subscribe({
        next: (data) => {
          this.products.set(data);
          this.error.set(null);
          this.loading.set(false);
        },
        error: (err) => {
          console.error('Error loading products:', err);
          this.error.set('Failed to load products');
          this.loading.set(false);
        }
      });
    }
  }

  /**
   * Filter products by category
   */
  filterByCategory(categoryId: number | null): void {
    this.selectedCategory.set(categoryId);
    this.searchTerm.set('');
    this.loadProducts();
  }

  /**
   * Search products by term
   */
  onSearch(term: string): void {
    this.searchTerm.set(term);
    this.selectedCategory.set(null);
    this.loadProducts();
  }

  /**
   * Get category name by ID
   */
  getCategoryName(categoryId: number | null): string {
    if (!categoryId) return 'Uncategorized';
    const category = this.categories().find(c => c.id === categoryId);
    return category ? category.name : 'Unknown';
  }

  /**
   * Add product to cart
   */
  addToCart(product: Product): void {
    const currentCart = this.cart();
    const existingItem = currentCart.find(item => item.id === product.id);
    
    if (existingItem) {
      existingItem.quantity = (existingItem.quantity || 1) + 1;
    } else {
      product.quantity = 1;
      currentCart.push(product);
    }
    
    this.cart.set([...currentCart]);
    this.showNotification(`${product.name} added to cart!`);
  }

  /**
   * Toggle product in wishlist
   */
  toggleWishlist(product: Product): void {
    const wishlistItems = this.wishlist();
    const index = wishlistItems.indexOf(product.id);
    
    if (index > -1) {
      wishlistItems.splice(index, 1);
      this.showNotification(`${product.name} removed from wishlist`);
    } else {
      wishlistItems.push(product.id);
      this.showNotification(`${product.name} added to wishlist!`);
    }
    
    this.wishlist.set([...wishlistItems]);
  }

  /**
   * Clear all filters
   */
  clearAllFilters(): void {
    this.selectedCategory.set(null);
    this.searchTerm.set('');
    this.loadProducts();
  }

  /**
   * Show notification message
   */
  private showNotification(message: string): void {
    console.log('✨ ' + message);
  }
}
