import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-rating',
  imports: [],
  templateUrl: './rating.component.html',
  styleUrl: './rating.component.css'
})
export class RatingComponent implements OnChanges {
  @Input() rating:number = 0;
  @Input() text = "";
  @Input() isReadOnly:boolean = false;
  @Output() ratingChange = new EventEmitter<number>();

  hoveredRating = 0;
  selectedStarIndex = -1;
  animateCount = 0; 

  ngOnChanges(changes: SimpleChanges) {
    if (changes['rating'] && !this.isReadOnly) {
      this.hoveredRating = this.rating;
    }
  }

  setRating(newRating: number) {
    if (!this.isReadOnly) {
      this.rating = newRating;
      this.ratingChange.emit(newRating);
      this.hoveredRating = newRating;
      this.selectedStarIndex = newRating - 1;
      this.animateCount = 0; 

      const intervalId = setInterval(() => {
        this.animateCount++;
        if (this.animateCount > 4) { 
          clearInterval(intervalId);
          this.selectedStarIndex = -1; 
        }
      }, 150); 
    }
  }

  hoverRating(starValue: number) {
    if (!this.isReadOnly) {
      this.hoveredRating = starValue;
    }
  }

  resetHover() {
    if (!this.isReadOnly) {
      this.hoveredRating = this.rating;
    }
  }

  getInputStarColor(starValue: number): string {
    return this.isReadOnly ? (starValue <= this.rating ? '#FFA500' : '#D3D3D3') : (starValue <= this.hoveredRating ? '#FFA500' : '#D3D3D3');
  }
}