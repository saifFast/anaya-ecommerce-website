import { Component, OnInit, signal } from '@angular/core';
import { ProductService } from './features/products/services/product.service';
import { CategoryService } from './features/category/services/category.service';
import { CartService } from './features/cart/services/cart.service';
import { WishlistService } from './features/wishlist/services/wishlist.service';
import { NotificationService } from './core/services/notification.service';
import { Product, Category } from './shared/models';

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

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService,
    public cartService: CartService,
    public wishlistService: WishlistService,
    private notificationService: NotificationService
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
    this.cartService.addToCart(product);
    this.notificationService.success(`${product.name} added to cart!`);
  }

  /**
   * Toggle product in wishlist
   */
  toggleWishlist(product: Product): void {
    this.wishlistService.toggleWishlist(product.id);
    const isNowInWishlist = this.wishlistService.isInWishlist(product.id);
    const message = isNowInWishlist
      ? `${product.name} added to wishlist!`
      : `${product.name} removed from wishlist`;
    this.notificationService.success(message);
  }

  /**
   * Clear all filters
   */
  clearAllFilters(): void {
    this.selectedCategory.set(null);
    this.searchTerm.set('');
    this.loadProducts();
  }
}
