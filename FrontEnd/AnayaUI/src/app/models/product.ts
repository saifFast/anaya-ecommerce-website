export interface Product {
  id: number;
  name: string;
  description: string;
  categoryId: number | null;
  price: number;
  imageUrl: string;
  quantity?: number;
}
