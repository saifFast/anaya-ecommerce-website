import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Category } from '../../../shared/models';

/**
 * Category Filter Component
 * Displays category list for filtering products
 */
@Component({
  selector: 'app-category-filter',
  standalone: false,
  template: `
    <div class="category-filter">
      <h3>Categories</h3>
      <div class="category-list">
        <button
          (click)="onSelectCategory(null)"
          [class.active]="selectedCategory === null"
          class="category-btn"
        >
          All Products
        </button>
        <button
          *ngFor="let category of categories"
          (click)="onSelectCategory(category.id)"
          [class.active]="selectedCategory === category.id"
          class="category-btn"
        >
          {{ category.name }}
        </button>
      </div>
    </div>
  `,
  styles: [`
    .category-filter {
      margin-bottom: 2rem;
    }

    .category-filter h3 {
      margin-top: 0;
      margin-bottom: 1rem;
      font-size: 1.1rem;
    }

    .category-list {
      display: flex;
      flex-direction: column;
      gap: 0.5rem;
    }

    .category-btn {
      padding: 0.75rem 1rem;
      background-color: #f5f5f5;
      border: 2px solid transparent;
      border-radius: 4px;
      cursor: pointer;
      text-align: left;
      transition: all 0.3s;
    }

    .category-btn:hover {
      background-color: #f0f0f0;
    }

    .category-btn.active {
      background-color: var(--primary-color, #007bff);
      color: white;
      border-color: var(--primary-color, #007bff);
    }
  `]
})
export class CategoryFilterComponent {
  @Input() categories: Category[] = [];
  @Input() selectedCategory: number | null = null;
  @Output() selectCategory = new EventEmitter<number | null>();

  onSelectCategory(categoryId: number | null): void {
    this.selectCategory.emit(categoryId);
  }
}
