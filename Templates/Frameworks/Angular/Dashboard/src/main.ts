import { bootstrapApplication } from '@angular/platform-browser'
import { provideRouter } from '@angular/router'
import { Component, signal } from '@angular/core'
import { RouterOutlet, RouterLink, Routes } from '@angular/router'

@Component({
  selector: 'app-dashboard',
  standalone: true,
  template: `
    <div class="dashboard-content">
      <h2>Dashboard</h2>
      <div class="stats-grid">
        <div class="stat-card" *ngFor="let s of stats">
          <h3>{{ s.label }}</h3>
          <p class="stat-value">{{ s.value }}</p>
          <span [class.positive]="s.positive" [class.negative]="!s.positive">{{ s.change }}</span>
        </div>
      </div>
      <div class="table-section">
        <h3>Letzte Bestellungen</h3>
        <table class="data-table">
          <thead><tr><th>Kunde</th><th>Produkt</th><th>Status</th><th>Datum</th></tr></thead>
          <tbody>
            <tr *ngFor="let o of orders">
              <td>{{ o.customer }}</td><td>{{ o.product }}</td>
              <td><span class="status">{{ o.status }}</span></td><td>{{ o.date }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  `
})
export class DashboardComponent {
  stats = [
    { label: 'Umsatz', value: '€12,430', change: '+12%', positive: true },
    { label: 'Bestellungen', value: '156', change: '+8%', positive: true },
    { label: 'Kunden', value: '1,234', change: '+23%', positive: true },
    { label: 'Besucher', value: '8,932', change: '-3%', positive: false },
  ]
  orders = [
    { customer: 'Max M.', product: 'Laptop', status: 'Versandt', date: '2024-01-15' },
    { customer: 'Anna S.', product: 'Monitor', status: 'Bearbeitung', date: '2024-01-14' },
  ]
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink],
  template: `
    <div class="app">
      <aside class="sidebar">
        <h2 class="sidebar-logo">Admin</h2>
        <nav><a routerLink="/">Dashboard</a><a routerLink="/users">Benutzer</a><a routerLink="/settings">Einstellungen</a></nav>
      </aside>
      <div class="main-area">
        <header class="topbar"><span>Administrator</span></header>
        <main class="main-content"><router-outlet /></main>
      </div>
    </div>
  `
})
export class AppComponent {}

@Component({ selector: 'app-users', standalone: true, template: `<h2>Benutzerverwaltung</h2>` })
export class UsersComponent {}

@Component({ selector: 'app-settings', standalone: true, template: `<h2>Einstellungen</h2>` })
export class SettingsComponent {}

const routes: Routes = [
  { path: '', component: DashboardComponent },
  { path: 'users', component: UsersComponent },
  { path: 'settings', component: SettingsComponent },
]

bootstrapApplication(AppComponent, { providers: [provideRouter(routes)] })
