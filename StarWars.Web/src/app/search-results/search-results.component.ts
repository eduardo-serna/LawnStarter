import { Component } from '@angular/core';
import { NgForOf } from '@angular/common'; 

@Component({
  selector: 'app-search-results',
  imports: [NgForOf],
  templateUrl: './search-results.component.html',
  styleUrl: './search-results.component.scss'
})
export class SearchResultsComponent {
  items = [
    { name: 'Apple' },
    { name: 'Banana' },
    { name: 'Orange' }
  ];
}
