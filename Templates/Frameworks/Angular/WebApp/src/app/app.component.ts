import { Component } from '@angular/core'
import { RouterOutlet, RouterLink } from '@angular/router'

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink],
  template: `
    <div class="app-layout">
      <header class="app-header">
        <h1 class="app-logo">AngularApp</h1>
        <nav class="app-nav">
          <a routerLink="/">Home</a>
          <a routerLink="/about">About</a>
          <a routerLink="/contact">Contact</a>
        </nav>
      </header>
      <main class="app-main">
        <router-outlet />
      </main>
      <footer class="app-footer">
        <p>&copy; {{ year }} AngularApp</p>
      </footer>
    </div>
  `,
  styles: [`
    .app-layout { display: flex; flex-direction: column; min-height: 100vh; }
    .app-header { background: #fff; border-bottom: 1px solid #dee2e6; padding: 1rem 2rem; display: flex; align-items: center; justify-content: space-between; position: sticky; top: 0; z-index: 100; }
    .app-logo { font-size: 1.5rem; color: #c3002f; margin: 0; }
    .app-nav { display: flex; gap: 1.5rem; }
    .app-nav a { text-decoration: none; color: #213547; font-weight: 500; padding: 0.5rem 0; border-bottom: 2px solid transparent; transition: border-color 0.2s; }
    .app-nav a:hover { border-bottom-color: #c3002f; color: #c3002f; }
    .app-main { flex: 1; padding: 2rem; max-width: 1200px; margin: 0 auto; width: 100%; }
    .app-footer { background: #fff; border-top: 1px solid #dee2e6; padding: 1rem; text-align: center; color: #6c757d; }
  `]
})
export class AppComponent {
  year = new Date().getFullYear()
}
