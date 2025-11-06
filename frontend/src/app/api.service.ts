import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  
  checkConnection() {
  return this.http.get(`${this.baseUrl}/Test/db-connection`, { responseType: 'text' });
}

  // 🔹 Para cargar usuarios (o datos reales)
  getUsuarios() {
    return this.http.get(`${this.baseUrl}/Usuario`);
  }
  
}
