import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-legend',
  imports: [],
  templateUrl: './legend.component.html',
  styleUrl: './legend.component.css'
})
export class LegendComponent {
 @Input() label:string = "Today's"
}
