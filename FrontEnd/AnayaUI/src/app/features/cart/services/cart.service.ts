import { Injectable, signal, computed } from '@angular/core';
import { CartItem, Product } from '../../shared/models';

/**
 * Cart Service
 * Manages shopping cart state and operations
 */
@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartItems = signal<CartItem[]>([]);

  // Computed properties
  items = computed(() => this.cartItems());
  totalItems = computed(() =>
    this.cartItems().reduce((sum, item) => sum + item.quantity, 0)
  );
  totalPrice = computed(() =>
    this.cartItems().reduce((sum, item) => sum + item.price * item.quantity, 0)
  );

  /**
   * Get all cart items
   */
  getCart() {
    return this.cartItems();
  }

  /**
   * Add product to cart
   */
  addToCart(product: Product): void {
    const cart = this.cartItems();
    const existingItem = cart.find(item => item.id === product.id);

    if (existingItem) {
      existingItem.quantity++;
    } else {
      const cartItem: CartItem = {
        ...product,
        quantity: 1
      };
      cart.push(cartItem);
    }

    this.cartItems.set([...cart]);
  }

  /**
   * Remove product from cart
   */
  removeFromCart(productId: number): void {
    const cart = this.cartItems();
    this.cartItems.set(cart.filter(item => item.id !== productId));
  }

  /**
   * Update cart item quantity
   */
  updateQuantity(productId: number, quantity: number): void {
    const cart = this.cartItems();
    const item = cart.find(item => item.id === productId);

    if (item) {
      if (quantity <= 0) {
        this.removeFromCart(productId);
      } else {
        item.quantity = quantity;
        this.cartItems.set([...cart]);
      }
    }
  }

  /**
   * Clear entire cart
   */
  clearCart(): void {
    this.cartItems.set([]);
  }

  /**
   * Check if product is in cart
   */
  isInCart(productId: number): boolean {
    return this.cartItems().some(item => item.id === productId);
  }

  /**
   * Get cart item count
   */
  getItemCount(): number {
    return this.totalItems();
  }
}
