import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Breadcrumb, BreadscrumService } from '../../services/breadscrum.service';

@Component({
  selector: 'app-bread-scrum',
  imports: [
    CommonModule,
    RouterModule
  ],
  templateUrl: './bread-scrum.component.html',
  styleUrl: './bread-scrum.component.css'
})
export class BreadScrumComponent implements OnInit {
  breadcrumbs: Breadcrumb[] = [];

  constructor(private breadcrumbService: BreadscrumService) {}

  ngOnInit(): void {
    this.breadcrumbService.breadcrumbs$.subscribe(bc => this.breadcrumbs = bc);
  }
}
