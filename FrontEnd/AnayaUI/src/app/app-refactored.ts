import { Component, OnInit, signal } from '@angular/core';
import { ProductService } from './features/products/services/product.service';
import { CategoryService } from './features/category/services/category.service';
import { CartService } from './features/cart/services/cart.service';
import { WishlistService } from './features/wishlist/services/wishlist.service';
import { NotificationService, LoadingService } from './core/services';
import { Product, Category } from './shared/models';

/**
 * App Root Component (Refactored)
 * Main application component with better separation of concerns
 */
@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('AnayaUI');

  // State signals
  products = signal<Product[]>([]);
  categories = signal<Category[]>([]);
  selectedCategory = signal<number | null>(null);
  searchTerm = signal('');

  // Computed-like signals via constructor
  loading = signal(false);
  error = signal<string | null>(null);

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService,
    public cartService: CartService,
    public wishlistService: WishlistService,
    private notificationService: NotificationService,
    private loadingService: LoadingService
  ) {
    // Subscribe to loading state changes
    // In a real app, you might use effect() here
  }

  ngOnInit(): void {
    this.loadInitialData();
  }

  /**
   * Load categories and products on initialization
   */
  private loadInitialData(): void {
    this.loadCategories();
    this.loadProducts();
  }

  /**
   * Load all categories
   */
  private loadCategories(): void {
    this.loadingService.start();
    this.categoryService.getAllCategories().subscribe({
      next: (data) => {
        this.categories.set(data);
        this.error.set(null);
        this.loadingService.stop();
      },
      error: (err) => {
        console.error('Error loading categories:', err);
        this.error.set('Failed to load categories');
        this.notificationService.error('Failed to load categories');
        this.loadingService.stop();
      }
    });
  }

  /**
   * Load products based on current filters
   */
  loadProducts(): void {
    this.loadingService.start();
    const searchTerm = this.searchTerm();
    const categoryId = this.selectedCategory();

    let request: any;

    if (searchTerm.trim()) {
      request = this.productService.searchProducts(searchTerm);
    } else if (categoryId) {
      request = this.productService.getProductsByCategory(categoryId);
    } else {
      request = this.productService.getAllProducts();
    }

    request.subscribe({
      next: (data: Product[]) => {
        this.products.set(data);
        this.error.set(null);
        this.loadingService.stop();
      },
      error: (err: any) => {
        console.error('Error loading products:', err);
        this.error.set('Failed to load products');
        this.notificationService.error('Failed to load products');
        this.loadingService.stop();
      }
    });
  }

  /**
   * Handle category filter change
   */
  onCategoryChange(categoryId: number | null): void {
    this.selectedCategory.set(categoryId);
    this.searchTerm.set('');
    this.loadProducts();
  }

  /**
   * Handle search input
   */
  onSearch(term: string): void {
    this.searchTerm.set(term);
    this.selectedCategory.set(null);
    if (term.trim()) {
      this.loadProducts();
    }
  }

  /**
   * Handle search clear
   */
  onClearSearch(): void {
    this.searchTerm.set('');
    this.selectedCategory.set(null);
    this.loadProducts();
  }

  /**
   * Handle add to cart
   */
  onAddToCart(product: Product): void {
    this.cartService.addToCart(product);
    this.notificationService.success(`${product.name} added to cart!`);
  }

  /**
   * Handle toggle wishlist
   */
  onToggleWishlist(product: Product): void {
    this.wishlistService.toggleWishlist(product.id);
    const isNowInWishlist = this.wishlistService.isInWishlist(product.id);
    const message = isNowInWishlist
      ? `${product.name} added to wishlist!`
      : `${product.name} removed from wishlist`;
    this.notificationService.success(message);
  }

  /**
   * Check if product is in wishlist
   */
  isInWishlist(productId: number): boolean {
    return this.wishlistService.isInWishlist(productId);
  }

  /**
   * Get category name by ID
   */
  getCategoryName(categoryId: number | null): string {
    if (!categoryId) return 'Uncategorized';
    const category = this.categories().find(c => c.id === categoryId);
    return category ? category.name : 'Unknown';
  }
}
