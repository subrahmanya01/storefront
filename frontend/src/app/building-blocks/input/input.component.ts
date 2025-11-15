import { Component, forwardRef, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-input',
  imports: [],
  templateUrl: './input.component.html',
  styleUrl: './input.component.css',
  providers:[
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputComponent),
      multi: true,
    },
  ]
})
export class InputComponent {
  @Input() placeholder: string = '';
  @Input() type = "text"

  value: string = '';
  onChange: any = () => {};
  onTouched: any = () => {};

  writeValue(value: string): void {
    this.value = value || '';
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  onInputChange(target: EventTarget | null): void {
    if (target instanceof HTMLInputElement) {
      this.onChange(target.value);
    }
  }
}
