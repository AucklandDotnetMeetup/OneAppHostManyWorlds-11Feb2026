import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WeatherForecast } from '../models/weather-forecast.model';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {
  private apiUrl = '/api/weatherforecast';

  constructor(private http: HttpClient) {}

  getAll(): Observable<WeatherForecast[]> {
    return this.http.get<WeatherForecast[]>(this.apiUrl);
  }

  create(forecast: WeatherForecast): Observable<WeatherForecast> {
    return this.http.post<WeatherForecast>(this.apiUrl, forecast);
  }

  update(id: number, forecast: WeatherForecast): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, forecast);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  createBatch(): Observable<WeatherForecast[]> {
    return this.http.post<WeatherForecast[]>(`${this.apiUrl}/batch`, null);
  }

  getById(id: number): Observable<WeatherForecast> {
    return this.http.get<WeatherForecast>(`${this.apiUrl}/${id}`);
  }
}
