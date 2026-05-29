// Entry point for the Angular SPA template — bootsraps a standalone app with dynamic page routing
import { bootstrapApplication } from '@angular/platform-browser'
import { provideRouter } from '@angular/router'
import { Component } from '@angular/core'
import { RouterOutlet, RouterLink, Routes } from '@angular/router'

// Home page component
@Component({
  selector: 'app-home',
  standalone: true,
  template: `<div class="page home-page"><h2>Single Page Application</h2><p>Diese SPA demonstriert nahtlose Navigation mit Angular.</p></div>`
})
export class HomeComponent {}

// Dynamic page component that loads content from a lookup table based on the route parameter
@Component({
  selector: 'app-page',
  standalone: true,
  template: `
    <div class="page" *ngIf="page; else notFound">
      <h2>{{ page.title }}</h2><p>{{ page.content }}</p>
    </div>
    <ng-template #notFound><div class="page"><h2>Seite nicht gefunden</h2></div></ng-template>
  `
})
export class PageComponent {
  // Static page content lookup
  pages: Record<string, {title: string, content: string}> = {
    services: { title: 'Dienstleistungen', content: 'Webentwicklung, Design und Beratung.' },
    team: { title: 'Team', content: 'Unser erfahrenes Team freut sich auf Sie.' },
    contact: { title: 'Kontakt', content: 'Kontaktieren Sie uns per E-Mail.' }
  }
  // Computed property to get the current page from the URL
  get page() { return this.pages[window.location.pathname.split('/').pop() || ''] }
}

// Root layout with navigation header
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink],
  template: `
    <div class="app">
      <header class="header">
        <h1 class="logo">AngularSPA</h1>
        <nav class="nav">
          <a routerLink="/">Home</a>
          <a routerLink="/page/services">Services</a>
          <a routerLink="/page/team">Team</a>
          <a routerLink="/page/contact">Kontakt</a>
        </nav>
      </header>
      <main class="main"><router-outlet /></main>
    </div>
  `
})
export class AppComponent {}

// Route definitions for home and dynamic pages
const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'page/:id', component: PageComponent },
]

// Bootstrap the standalone app
bootstrapApplication(AppComponent, {
  providers: [provideRouter(routes)]
})
