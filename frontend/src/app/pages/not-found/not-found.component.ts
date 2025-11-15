import { Component } from '@angular/core';
import { ButtonComponent } from '../../building-blocks/button/button.component';
import { BreadScrumComponent } from '../../building-blocks/bread-scrum/bread-scrum.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-not-found',
  imports: [ButtonComponent, BreadScrumComponent],
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.css'
})
export class NotFoundComponent {
  constructor(private router: Router)
  {}

  navigateToHome()
  {
    this.router.navigateByUrl("/")
  }
}
