import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environments/environment';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private apiUrl = environment.apiUrl; // http://localhost:8080/WeatherForecast

  constructor(private http: HttpClient) {}

  getValues() {
    // Elimina '/values'
    return this.http.get(this.apiUrl);
  }
}


