// Root component — demonstrates Angular Signals with a counter and HTTP weather fetch
import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';

interface WeatherData {
  current_weather?: { temperature: number };
  error?: string;
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  template: `
    <header>
      <h1>Web-Angular + TS</h1>
      <p>Angular TypeScript Template</p>
    </header>

    <main>
      <section>
        <h2>Counter</h2>
        <p class="count">{{ count() }}</p>
        <button (click)="increment()">+1</button>
        <button (click)="decrement()">-1</button>
        <button (click)="reset()">Reset</button>
      </section>

      <section>
        <h2>Wetter / Weather</h2>
        <button (click)="fetchWeather()" [disabled]="loading()">
          {{ loading() ? 'Lade…' : 'Daten laden' }}
        </button>
        @if (weather(); as data) {
          <pre>{{ data | json }}</pre>
        }
      </section>
    </main>
  `,
  styles: [`
    :host { font-family: system-ui, sans-serif; display: block; background: #f5f5f5; padding: 1rem; }
    header { text-align: center; padding: 2rem 0; }
    main { max-width: 640px; margin: 0 auto; display: flex; flex-direction: column; gap: 2rem; }
    section { background: #fff; border-radius: 0.5rem; padding: 1rem; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
    .count { font-size: 3rem; font-weight: bold; }
    button { background: #4a90d9; color: #fff; border: none; border-radius: 0.25rem; padding: 0.5rem 1rem; cursor: pointer; margin-right: 0.5rem; }
    button:hover { background: #357abd; }
    pre { margin-top: 1rem; white-space: pre-wrap; }
  `],
})
export class AppComponent {
  readonly count = signal(0);
  readonly loading = signal(false);
  readonly weather = signal<WeatherData | null>(null);

  constructor(private http: HttpClient) {}

  // Counter actions
  increment(): void { this.count.update(c => c + 1); }
  decrement(): void { this.count.update(c => c - 1); }
  reset(): void     { this.count.set(0); }

  // Fetch current weather data from the Open-Meteo API
  fetchWeather(): void {
    this.loading.set(true);
    this.http.get<WeatherData>(
      'https://api.open-meteo.com/v1/forecast?latitude=52.52&longitude=13.41&current_weather=true'
    ).subscribe({
      next: (data) => { this.weather.set(data); this.loading.set(false); },
      error: (err)  => { this.weather.set({ error: err.message }); this.loading.set(false); },
    });
  }
}
