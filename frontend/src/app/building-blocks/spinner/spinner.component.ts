import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-spinner',
  templateUrl: './spinner.component.html',
  imports: [
    CommonModule
  ],
  styleUrls: ['./spinner.component.css'] 
})
export class SpinnerComponent  {
  @Input() text: string= "";
}