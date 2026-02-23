import { Product } from './product';

/**
 * Cart Item Model
 * Represents an item in the shopping cart
 */
export interface CartItem extends Product {
  quantity: number;
}

/**
 * Cart State Model
 */
export interface CartState {
  items: CartItem[];
  totalItems: number;
  totalPrice: number;
}
