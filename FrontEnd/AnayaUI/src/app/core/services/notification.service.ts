import { Injectable, signal } from '@angular/core';

/**
 * Notification Type
 */
export type NotificationType = 'success' | 'error' | 'warning' | 'info';

/**
 * Notification Interface
 */
export interface Notification {
  id: string;
  message: string;
  type: NotificationType;
  duration?: number;
}

/**
 * Notification Service
 * Centralized service for managing notifications
 */
@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  notifications = signal<Notification[]>([]);
  private notificationId = 0;

  /**
   * Show success notification
   */
  success(message: string, duration: number = 3000): void {
    this.show(message, 'success', duration);
  }

  /**
   * Show error notification
   */
  error(message: string, duration: number = 5000): void {
    this.show(message, 'error', duration);
  }

  /**
   * Show warning notification
   */
  warning(message: string, duration: number = 4000): void {
    this.show(message, 'warning', duration);
  }

  /**
   * Show info notification
   */
  info(message: string, duration: number = 3000): void {
    this.show(message, 'info', duration);
  }

  /**
   * Show custom notification
   */
  private show(message: string, type: NotificationType, duration: number): void {
    const id = `notification-${this.notificationId++}`;
    const notification: Notification = { id, message, type, duration };
    
    const currentNotifications = this.notifications();
    this.notifications.set([...currentNotifications, notification]);

    if (duration > 0) {
      setTimeout(() => this.remove(id), duration);
    }
  }

  /**
   * Remove notification by ID
   */
  remove(id: string): void {
    const notifications = this.notifications();
    this.notifications.set(notifications.filter(n => n.id !== id));
  }

  /**
   * Clear all notifications
   */
  clearAll(): void {
    this.notifications.set([]);
  }
}
