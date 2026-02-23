import { Component, Input, Output, EventEmitter } from '@angular/core';

/**
 * Search Bar Component
 * Shared component for searching products
 */
@Component({
  selector: 'app-search-bar',
  standalone: false,
  template: `
    <div class="search-container">
      <input
        type="text"
        placeholder="Search products..."
        (input)="onSearch($event)"
        class="search-input"
      />
      <button (click)="onClear()" class="search-clear">Clear</button>
    </div>
  `,
  styles: [`
    .search-container {
      display: flex;
      gap: 0.5rem;
      margin-bottom: 1.5rem;
    }

    .search-input {
      flex: 1;
      padding: 0.75rem;
      border: 2px solid #ddd;
      border-radius: 4px;
      font-size: 1rem;
    }

    .search-input:focus {
      outline: none;
      border-color: var(--primary-color, #007bff);
    }

    .search-clear {
      padding: 0.75rem 1.5rem;
      background-color: #f0f0f0;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-weight: 500;
    }

    .search-clear:hover {
      background-color: #e0e0e0;
    }
  `]
})
export class SearchBarComponent {
  @Output() search = new EventEmitter<string>();
  @Output() clear = new EventEmitter<void>();

  onSearch(event: any): void {
    const term = event.target.value;
    this.search.emit(term);
  }

  onClear(): void {
    this.clear.emit();
  }
}
