// src/app/reservas/reservas.service.ts

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReservasService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  // Método para obtener las reservas
  getReservas(): Observable<any> {
    return this.http.get(`${this.apiUrl}/reservas`);
  }

  // Método para crear una nueva reserva
  createReserva(reserva: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/reservas`, reserva);
  }

  // Método para eliminar una reserva
  deleteReserva(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/reservas/${id}`);
  }

  // Método para actualizar una reserva
  updateReserva(id: number, reserva: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/reservas/${id}`, reserva);
  }
}
