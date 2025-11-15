import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-icon-button',
  imports: [],
  templateUrl: './icon-button.component.html',
  styleUrl: './icon-button.component.css'
})
export class IconButtonComponent {
  @Input() label: string = "label";
  @Input() icon: string = "";
  @Output() onClick:any = new EventEmitter();

  clickPerformed()
  {
    this.onClick.emit();
  }
}
