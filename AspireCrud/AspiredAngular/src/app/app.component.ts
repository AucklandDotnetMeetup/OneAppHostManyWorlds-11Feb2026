import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WeatherService } from './services/weather.service';
import { WeatherForecast } from './models/weather-forecast.model';

@Component({
  selector: 'app-root',
  imports: [CommonModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'Weather Forecast Manager';
  forecasts: WeatherForecast[] = [];
  loading = false;
  error: string | null = null;
  
  // Form model
  formModel: WeatherForecast = {
    date: new Date().toISOString().split('T')[0],
    temperatureC: 20,
    summary: '',
    description: ''
  };
  
  isEditing = false;
  showForm = false;

  constructor(private weatherService: WeatherService) {}

  ngOnInit(): void {
    this.loadForecasts();
  }

  loadForecasts(): void {
    this.loading = true;
    this.error = null;
    this.weatherService.getAll().subscribe({
      next: (data) => {
        this.forecasts = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load forecasts: ' + err.message;
        this.loading = false;
      }
    });
  }

  createForecast(): void {
    this.loading = true;
    this.error = null;
    this.weatherService.create(this.formModel).subscribe({
      next: (forecast) => {
        this.forecasts.push(forecast);
        this.resetForm();
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to create forecast: ' + err.message;
        this.loading = false;
      }
    });
  }

  editForecast(forecast: WeatherForecast): void {
    this.formModel = { ...forecast };
    this.isEditing = true;
    this.showForm = true;
  }

  updateForecast(): void {
    if (!this.formModel?.id) return;
    
    this.loading = true;
    this.error = null;
    const updatedData = { ...this.formModel };
    this.weatherService.update(this.formModel.id, this.formModel).subscribe({
      next: () => {
        // Update successful (204 No Content), update local data
        const index = this.forecasts.findIndex(f => f.id === updatedData.id);
        if (index !== -1) {
          this.forecasts[index] = updatedData;
        }
        this.cancelEdit();
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to update forecast: ' + err.message;
        this.loading = false;
      }
    });
  }

  deleteForecast(id: number | undefined): void {
    if (!id || !confirm('Are you sure you want to delete this forecast?')) return;
    
    this.loading = true;
    this.error = null;
    this.weatherService.delete(id).subscribe({
      next: () => {
        this.forecasts = this.forecasts.filter(f => f.id !== id);
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to delete forecast: ' + err.message;
        this.loading = false;
      }
    });
  }

  createBatch(): void {
    this.loading = true;
    this.error = null;
    this.weatherService.createBatch().subscribe({
      next: (newForecasts) => {
        this.forecasts.push(...newForecasts);
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to create batch: ' + err.message;
        this.loading = false;
      }
    });
  }

  resetForm(): void {
    this.formModel = {
      date: new Date().toISOString().split('T')[0],
      temperatureC: 20,
      summary: '',
      description: ''
    };
    this.isEditing = false;
    this.showForm = false;
  }

  cancelEdit(): void {
    this.resetForm();
  }

  getTemperatureF(temperatureC: number): number {
    return Math.round(32 + temperatureC * 9 / 5);
  }

  getTemperatureColor(tempC: number): string {
    if (tempC < 0) return '#3b82f6'; // blue
    if (tempC < 15) return '#06b6d4'; // cyan
    if (tempC < 25) return '#10b981'; // green
    if (tempC < 35) return '#f59e0b'; // orange
    return '#ef4444'; // red
  }
}
