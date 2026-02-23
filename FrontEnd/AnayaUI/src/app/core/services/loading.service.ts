import { Injectable, signal } from '@angular/core';

/**
 * Loading Service
 * Manages loading state across the application
 */
@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  isLoading = signal(false);
  private loadingRequests = 0;

  /**
   * Start loading
   */
  start(): void {
    this.loadingRequests++;
    this.isLoading.set(true);
  }

  /**
   * Stop loading
   */
  stop(): void {
    this.loadingRequests = Math.max(0, this.loadingRequests - 1);
    if (this.loadingRequests === 0) {
      this.isLoading.set(false);
    }
  }

  /**
   * Reset loading state
   */
  reset(): void {
    this.loadingRequests = 0;
    this.isLoading.set(false);
  }
}
