import { Injectable, signal, computed } from '@angular/core';

/**
 * Wishlist Service
 * Manages user's wishlist (favorite products)
 */
@Injectable({
  providedIn: 'root'
})
export class WishlistService {
  private wishlistItems = signal<number[]>([]);

  // Computed properties
  items = computed(() => this.wishlistItems());
  itemCount = computed(() => this.wishlistItems().length);

  /**
   * Get all wishlist items (product IDs)
   */
  getWishlist(): number[] {
    return this.wishlistItems();
  }

  /**
   * Add product to wishlist
   */
  addToWishlist(productId: number): void {
    const items = this.wishlistItems();
    if (!items.includes(productId)) {
      this.wishlistItems.set([...items, productId]);
    }
  }

  /**
   * Remove product from wishlist
   */
  removeFromWishlist(productId: number): void {
    const items = this.wishlistItems();
    this.wishlistItems.set(items.filter(id => id !== productId));
  }

  /**
   * Toggle product in wishlist
   */
  toggleWishlist(productId: number): void {
    if (this.isInWishlist(productId)) {
      this.removeFromWishlist(productId);
    } else {
      this.addToWishlist(productId);
    }
  }

  /**
   * Check if product is in wishlist
   */
  isInWishlist(productId: number): boolean {
    return this.wishlistItems().includes(productId);
  }

  /**
   * Clear entire wishlist
   */
  clearWishlist(): void {
    this.wishlistItems.set([]);
  }
}
