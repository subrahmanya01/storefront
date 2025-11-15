import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { SearchFilters } from '../../interfaces/search';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-search-filter',
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './search-filter.component.html',
  styleUrl: './search-filter.component.css'
})
export class SearchFilterComponent implements OnInit, OnChanges {
  @Input() availableCategories: string[] = [];
  @Input() availableBrands: string[] = [];
  @Output() filtersChanged = new EventEmitter<SearchFilters>();
  currentFilters: SearchFilters = {
    category: null,
    brand: null,
    priceStart: null,
    priceEnd: null
  };
  constructor() { }
  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
      if (changes['availableCategories'] || changes['availableBrands']) {
        console.log('Available filter options updated.');
      }
  }

  onFilterChange(): void {
    this.currentFilters.priceStart = this.currentFilters.priceStart !== null && this.currentFilters.priceStart !== undefined ? Number(this.currentFilters.priceStart) : null;
    this.currentFilters.priceEnd = this.currentFilters.priceEnd !== null && this.currentFilters.priceEnd !== undefined ? Number(this.currentFilters.priceEnd) : null;
    if (this.currentFilters.priceStart !== null && this.currentFilters.priceEnd !== null && this.currentFilters.priceStart > this.currentFilters.priceEnd) {
        console.warn('Price start is greater than price end. Swapping values.');
        const temp = this.currentFilters.priceStart;
        this.currentFilters.priceStart = this.currentFilters.priceEnd;
        this.currentFilters.priceEnd = temp;
    }
    console.log('Filter change detected:', this.currentFilters);
    this.emitFilters(); 
  }

  emitFilters(): void {
    this.filtersChanged.emit({ ...this.currentFilters });
  }
  
  resetFilters(): void {
    this.currentFilters = {
        category: null,
        brand: null,
        priceStart: null,
        priceEnd: null
    };
    console.log('Filters reset.');
    this.emitFilters();
  }
}
