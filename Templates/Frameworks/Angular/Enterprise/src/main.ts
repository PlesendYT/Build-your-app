import { bootstrapApplication } from '@angular/platform-browser'
import { provideRouter, CanActivateFn } from '@angular/router'
import { inject } from '@angular/core'
import { Component } from '@angular/core'
import { RouterOutlet, RouterLink, Routes } from '@angular/router'

const authGuard: CanActivateFn = () => {
  return !!localStorage.getItem('token')
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink],
  template: `
    <div class="app-layout">
      <header class="app-header">
        <h1>EnterpriseApp</h1>
        <nav><a routerLink="/">Home</a><a routerLink="/dashboard">Dashboard</a><a routerLink="/admin">Admin</a></nav>
      </header>
      <main><router-outlet /></main>
    </div>
  `
})
export class AppComponent {}

@Component({
  selector: 'app-home',
  standalone: true,
  template: `<div class="page"><h2>Enterprise Application</h2><p>Willkommen bei der Enterprise-App mit Angular.</p></div>`
})
export class HomeComponent {}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  template: `<div class="page"><h2>Dashboard</h2><p>Geschützter Dashboard-Bereich.</p></div>`
})
export class DashboardComponent {}

@Component({
  selector: 'app-admin',
  standalone: true,
  template: `<div class="page"><h2>Admin-Bereich</h2><p>Nur für authentifizierte Benutzer.</p></div>`
})
export class AdminComponent {}

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },
  { path: 'admin', component: AdminComponent, canActivate: [authGuard] },
]

bootstrapApplication(AppComponent, { providers: [provideRouter(routes)] })
