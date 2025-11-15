import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-color-selector',
  imports: [],
  templateUrl: './color-selector.component.html',
  styleUrl: './color-selector.component.css'
})
export class ColorSelectorComponent {
  @Input() selectedColor: string | null = null;
  @Input() colors: string[] = [];
  @Output() onColorSelected:any = new EventEmitter();

  selectColor(color: string) {
    this.selectedColor = color;
    this.onColorSelected.emit(color);
  }
}
