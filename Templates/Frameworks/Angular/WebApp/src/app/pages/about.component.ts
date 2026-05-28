import { Component } from '@angular/core'

@Component({
  selector: 'app-about',
  standalone: true,
  template: `
    <div class="about-page">
      <h2>Über uns</h2>
      <p>Diese App wurde mit Angular 17 und Standalone Components erstellt.</p>
      <h3>Technologien</h3>
      <ul>
        <li>Angular 17+ Standalone Components</li>
        <li>Signals API</li>
        <li>Angular Router</li>
        <li>RxJS</li>
      </ul>
    </div>
  `,
  styles: [`h2 { color: #c3002f; margin-bottom: 1rem; } ul { margin-left: 1.5rem; }`]
})
export class AboutComponent {}
