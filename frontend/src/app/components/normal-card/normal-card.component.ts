import { Component, input, Input } from '@angular/core';
import { NormalItems } from '../../interfaces/component-models';
import { Router } from '@angular/router';

@Component({
  selector: 'app-normal-card',
  imports: [],
  templateUrl: './normal-card.component.html',
  styleUrl: './normal-card.component.css'
})
export class NormalCardComponent {
  @Input() data!: NormalItems;

  constructor(private router: Router)
  {

  }
  searchKeyword(data: string)
  {
    this.router.navigateByUrl(`search?key=${data}`);
    window.scrollTo(0, 0); 
  }
}
