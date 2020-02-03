import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ProductCatalogue';
  columnDefs = [
    {headerName: 'Title', field: 'title', editable: true},
    {headerName: 'Cost', field: 'cost', editable: true},
    {headerName: 'Quantity', field: 'quantity', editable: true},
    {headerName: 'TotalCost', field: 'totalCost', editable: true}
];

rowData = [];

  ngOnInit() {
    fetch('http://localhost:5000/api/ProductCatalogue/SearchProduct')
      .then(result => result.json())
      .then(rowData => this.rowData = rowData);
  }
}
