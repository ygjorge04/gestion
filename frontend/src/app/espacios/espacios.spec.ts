// src/app/espacios/espacios.service.ts

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EspaciosService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  // Método para obtener los espacios
  getEspacios(): Observable<any> {
    return this.http.get(`${this.apiUrl}/espacios`);
  }

  // Método para crear un nuevo espacio
  createEspacio(espacio: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/espacios`, espacio);
  }

  // Método para eliminar un espacio
  deleteEspacio(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/espacios/${id}`);
  }

  // Método para actualizar un espacio
  updateEspacio(id: number, espacio: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/espacios/${id}`, espacio);
  }
}
