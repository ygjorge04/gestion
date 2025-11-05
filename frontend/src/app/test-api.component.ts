import { Component } from '@angular/core';
import { JsonPipe } from '@angular/common';
import { ApiService } from './api.service';

@Component({
  selector: 'app-test-api',
  standalone: true, // 👈 importante si estás en Angular moderno
  imports: [JsonPipe], // 👈 aquí se importa el pipe JSON
  template: `
    <h2>Probar conexión al backend</h2>
    <button (click)="loadValues()">Cargar datos</button>
    <pre>{{ data | json }}</pre>
  `
})
export class TestApiComponent {
  data: any;

  constructor(private api: ApiService) {}

  loadValues() {
    this.api.getValues().subscribe({
      next: (res) => this.data = res,
      error: (err) => this.data = err
    });
  }
}
