import { Component } from '@angular/core';
import { JsonPipe } from '@angular/common';
import { ApiService } from './api.service';

@Component({
  selector: 'app-test-api',
  standalone: true,
  imports: [JsonPipe],
  template: `
    <h2>Probar conexión al backend</h2>
    <button (click)="loadValues()">Cargar datos</button>
    <button (click)="loadUsuarios()">Cargar usuarios</button>
    <pre>{{ data | json }}</pre>
  `
})
export class TestApiComponent {
  data: any;

  constructor(private api: ApiService) {}

  loadValues() {
    this.api.checkConnection().subscribe({
      next: (res) => this.data = res,
      error: (err) => this.data = err
    });
  }
 
  loadUsuarios() {
  this.api.getUsuarios().subscribe({
    next: (res) => this.data = res,
    error: (err) => this.data = err
  });
}
}
