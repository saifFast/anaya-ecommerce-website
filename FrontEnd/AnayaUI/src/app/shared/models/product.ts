/**
 * Product Model
 * Represents a product in the e-commerce system
 */
export interface Product {
  id: number;
  name: string;
  description: string;
  categoryId: number | null;
  price: number;
  imageUrl: string;
  quantity?: number;
}

/**
 * Product Filter Options
 */
export interface ProductFilter {
  searchTerm?: string;
  categoryId?: number | null;
  minPrice?: number;
  maxPrice?: number;
}
