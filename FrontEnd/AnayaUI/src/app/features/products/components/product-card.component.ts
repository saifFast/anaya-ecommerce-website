import { Component, Input, Output, EventEmitter, signal } from '@angular/core';
import { Product } from '../../../shared/models';

/**
 * Product Card Component
 * Displays a product in card format
 */
@Component({
  selector: 'app-product-card',
  standalone: false,
  template: `
    <div class="product-card">
      <div class="product-image">
        <img [src]="product.imageUrl" [alt]="product.name" />
      </div>
      <div class="product-body">
        <h3 class="product-name">{{ product.name }}</h3>
        <p class="product-description">{{ product.description }}</p>
        <div class="product-footer">
          <span class="product-price">\${{ product.price }}</span>
          <div class="product-actions">
            <button (click)="onAddToCart()" class="btn-add-cart">
              🛒 Add to Cart
            </button>
            <button (click)="onToggleWishlist()" 
                    [class.in-wishlist]="isInWishlist()">
              {{ isInWishlist() ? '❤️' : '🤍' }}
            </button>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .product-card {
      border: 1px solid #ddd;
      border-radius: 8px;
      overflow: hidden;
      transition: transform 0.3s, box-shadow 0.3s;
    }

    .product-card:hover {
      transform: translateY(-4px);
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
    }

    .product-image {
      width: 100%;
      height: 200px;
      overflow: hidden;
    }

    .product-image img {
      width: 100%;
      height: 100%;
      object-fit: cover;
    }

    .product-body {
      padding: 1rem;
    }

    .product-name {
      margin: 0 0 0.5rem;
      font-size: 1rem;
      font-weight: 600;
    }

    .product-description {
      margin: 0 0 1rem;
      font-size: 0.875rem;
      color: #666;
      overflow: hidden;
      text-overflow: ellipsis;
      display: -webkit-box;
      -webkit-line-clamp: 2;
      -webkit-box-orient: vertical;
    }

    .product-footer {
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .product-price {
      font-size: 1.25rem;
      font-weight: 700;
      color: var(--primary-color, #007bff);
    }

    .product-actions {
      display: flex;
      gap: 0.5rem;
    }

    .btn-add-cart {
      padding: 0.5rem 1rem;
      background-color: var(--primary-color, #007bff);
      color: white;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 0.875rem;
    }

    .btn-add-cart:hover {
      background-color: var(--primary-dark, #0056b3);
    }

    button:last-child {
      background: none;
      border: none;
      font-size: 1.25rem;
      cursor: pointer;
    }

    button.in-wishlist {
      color: red;
    }
  `]
})
export class ProductCardComponent {
  @Input() product!: Product;
  @Input() isInWishlist = signal(false);
  @Output() addToCart = new EventEmitter<Product>();
  @Output() toggleWishlist = new EventEmitter<Product>();

  onAddToCart(): void {
    this.addToCart.emit(this.product);
  }

  onToggleWishlist(): void {
    this.toggleWishlist.emit(this.product);
  }
}
