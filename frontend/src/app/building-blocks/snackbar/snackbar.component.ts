import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { SnackbarPosition, SnackbarService, SnackbarType } from '../../services/snackbar.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-snackbar',
  templateUrl: './snackbar.component.html',
  imports: [
    CommonModule
  ],
  styleUrls: ['./snackbar.component.css'] 
})
export class SnackbarComponent implements OnInit, OnDestroy {
  isVisible = false; 
  message: string = ''; 
  messageType: SnackbarType = 'info'; 
  position: SnackbarPosition = 'bottom-right'; 

  private destroy$ = new Subject<void>(); 

  constructor(private snackbarService: SnackbarService) { }

  ngOnInit(): void {
    
    this.snackbarService.message$
      .pipe(takeUntil(this.destroy$)) 
      .subscribe(messageConfig => {
        if (messageConfig) {
          
          this.message = messageConfig.message;
          this.messageType = messageConfig.type || 'info'; 
          this.position = messageConfig.position || 'bottom-right'; 
          this.isVisible = true;

          
          if (messageConfig.duration !== 0) {
             setTimeout(() => {
               this.hide();
             }, messageConfig.duration || 3000); 
          }

        } else {
          
          this.hide();
        }
      });
  }

  ngOnDestroy(): void {
    
    this.destroy$.next();
    this.destroy$.complete();
  }

  
  hide(): void {
    this.isVisible = false;
    this.message = ''; 
    this.messageType = 'info'; 
    this.position = 'bottom-right'; 
  }

  
  get messageTypeClass(): string {
    return `${this.messageType}`;
  }

   
  get positionClass(): string {
     return this.position;
  }
}