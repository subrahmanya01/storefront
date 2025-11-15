import { animate, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
export type AlertType = 'success' | 'error' | 'warning' | 'info';
@Component({
  selector: 'app-alerts',
  imports: [CommonModule],
  templateUrl: './alerts.component.html',
  styleUrl: './alerts.component.css',
  animations: [
    trigger('fadeInOut', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-10px)' }),
        animate('300ms ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ]),
      transition(':leave', [
        animate('300ms ease-in', style({ opacity: 0, transform: 'translateY(-10px)' }))
      ])
    ])
  ]
})
export class AlertsComponent implements OnInit, OnDestroy {
  @Input() type: AlertType = 'info'; 
  @Input() message: string = '';
  @Input() dismissible: boolean = true; 
  @Input() autoCloseDelay: number | null = null; 

  @Output() closed = new EventEmitter<void>();

  isVisible: boolean = true;
  private timerId: any = null; 

  ngOnInit(): void {
    if (this.autoCloseDelay && this.autoCloseDelay > 0) {
      this.timerId = setTimeout(() => {
        this.close();
      }, this.autoCloseDelay);
    }
  }

  ngOnDestroy(): void {
    
    if (this.timerId) {
      clearTimeout(this.timerId);
    }
  }

  get alertClasses(): { [key: string]: boolean } {
    return {
      'alert': true,
      [`alert-${this.type}`]: true,
      'alert-dismissible': this.dismissible
    };
  }

  close(): void {
    if (this.timerId) { 
      clearTimeout(this.timerId);
      this.timerId = null;
    }
    this.isVisible = false;
    
    setTimeout(() => this.closed.emit(), 150); 
  }
}
