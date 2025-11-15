import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';


export type SnackbarType = 'info' | 'success' | 'warning' | 'error';
export type SnackbarPosition = 'bottom-center' | 'top-center' | 'top-right' | 'bottom-right';

export interface SnackbarMessage {
  message: string; 
  type?: SnackbarType; 
  duration?: number; 
  position?: SnackbarPosition; 
}

@Injectable({
  providedIn: 'root'
})
export class SnackbarService {
  
  private messageSubject = new Subject<SnackbarMessage | null>();

  
  message$: Observable<SnackbarMessage | null> = this.messageSubject.asObservable();

  constructor() { }

  /**
   * Shows a snackbar message.
   * @param message The message text to display.
   * @param type The type of message (info, success, warning, error). Defaults to 'info'.
   * @param duration How long the snackbar is visible in milliseconds. Defaults to 3000ms. Use 0 for sticky.
   * @param position The position of the snackbar. Defaults to 'bottom-center'.
   */
  show(message: string, type: SnackbarType = 'info', duration: number = 3000, position: SnackbarPosition = 'bottom-right'): void {
    this.messageSubject.next({ message, type, duration, position });
  }

  /**
   * Shows an info snackbar message.
   * @param message The message text.
   * @param duration Duration in milliseconds.
   * @param position The position.
   */
  info(message: string, duration?: number, position?: SnackbarPosition): void {
    this.show(message, 'info', duration, position);
  }

  /**
   * Shows a success snackbar message.
   * @param message The message text.
   * @param duration Duration in milliseconds.
   * @param position The position.
   */
  success(message: string, duration?: number, position?: SnackbarPosition): void {
    this.show(message, 'success', duration, position);
  }

  /**
   * Shows a warning snackbar message.
   * @param message The message text.
   * @param duration Duration in milliseconds.
   * @param position The position.
   */
  warning(message: string, duration?: number, position?: SnackbarPosition): void {
    this.show(message, 'warning', duration, position);
  }

  /**
   * Shows an error snackbar message.
   * @param message The message text.
   * @param duration Duration in milliseconds.
   * @param position The position.
   */
  error(message: string, duration?: number, position?: SnackbarPosition): void {
    this.show(message, 'error', duration, position);
  }


  /**
   * Hides the currently visible snackbar.
   */
  hide(): void {
    this.messageSubject.next(null);
  }
}