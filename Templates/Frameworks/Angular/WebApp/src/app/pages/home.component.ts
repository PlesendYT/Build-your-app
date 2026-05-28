import { Component, OnInit, inject } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { RouterLink } from '@angular/router'
import { CommonModule } from '@angular/common'

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink, CommonModule],
  template: `
    <div class="hero">
      <h2>Willkommen bei AngularApp</h2>
      <p>Moderne Angular-Anwendung mit Standalone Components</p>
      <a routerLink="/about" class="btn">Mehr erfahren</a>
    </div>
    <section class="content">
      <h3>Daten</h3>
      <div *ngIf="loading" class="loading">Laden...</div>
      <div *ngIf="!loading" class="data-grid">
        <div *ngFor="let item of items" class="data-card">
          <h4>{{ item.title }}</h4>
          <p>{{ item.description }}</p>
        </div>
      </div>
    </section>
  `,
  styles: [`
    .hero { text-align: center; padding: 4rem 0; }
    .hero h2 { font-size: 2.5rem; margin-bottom: 1rem; }
    .hero p { font-size: 1.2rem; color: #6c757d; margin-bottom: 2rem; }
    .btn { display: inline-block; background: #c3002f; color: #fff; padding: 0.6rem 1.5rem; border-radius: 8px; text-decoration: none; }
    .data-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(300px, 1fr)); gap: 1.5rem; margin-top: 1.5rem; }
    .data-card { background: #fff; border: 1px solid #dee2e6; border-radius: 8px; padding: 1.5rem; }
    .loading { text-align: center; padding: 3rem; color: #6c757d; }
  `]
})
export class HomeComponent implements OnInit {
  private http = inject(HttpClient)
  items: any[] = []
  loading = true

  ngOnInit() {
    this.http.get('/api/items').subscribe({
      next: (data: any) => this.items = data,
      error: () => this.loading = false,
      complete: () => this.loading = false
    })
  }
}
