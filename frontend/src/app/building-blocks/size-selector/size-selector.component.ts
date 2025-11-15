import { Component, EventEmitter, input, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-size-selector',
  imports: [],
  templateUrl: './size-selector.component.html',
  styleUrl: './size-selector.component.css'
})
export class SizeSelectorComponent implements OnInit {
  @Input() availableSizes: string[] = []; 
  @Input() selectedSize: string | null = null; 
  @Input() label: string = "Sizes";
  @Output() onSizeSelect: any = new EventEmitter<string>();
  ngOnInit(): void {
    this.availableSizes.length > 0 ? this.selectedSize = this.availableSizes[0]: null;
  }

  selectSize(size: string) {
    this.selectedSize = size;
  }
}
