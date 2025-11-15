import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, input, Output } from '@angular/core';

@Component({
  selector: 'app-button',
  imports: [CommonModule],
  templateUrl: './button.component.html',
  styleUrl: './button.component.css'
})
export class ButtonComponent {
  @Input() label: string = "label"
  @Input() size:"normal"|"small" = "normal"; 
  @Input() type:"normal" | "outlined" = "normal";
  @Input() buttonType: "submit"|"menu" | "reset" | "button" = "submit";
  @Input() disabled: boolean = false;
  @Output() onClick:any = new EventEmitter();

  clickPerformed()
  {
    this.onClick.emit();
  }
}
