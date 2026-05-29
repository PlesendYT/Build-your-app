// Contact page component — form with name, email, and message fields that submits to an API endpoint
import { Component } from '@angular/core'
import { FormsModule } from '@angular/forms'
import { HttpClient } from '@angular/common/http'
import { inject } from '@angular/core'

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [FormsModule],
  template: `
    <div class="contact-page">
      <h2>Kontakt</h2>
      <div *ngIf="sent" class="success">Nachricht gesendet!</div>
      <form *ngIf="!sent" (ngSubmit)="handleSubmit()">
        <div class="form-group">
          <label for="name">Name</label>
          <input id="name" [(ngModel)]="form.name" name="name" required />
        </div>
        <div class="form-group">
          <label for="email">E-Mail</label>
          <input id="email" type="email" [(ngModel)]="form.email" name="email" required />
        </div>
        <div class="form-group">
          <label for="message">Nachricht</label>
          <textarea id="message" [(ngModel)]="form.message" name="message" required></textarea>
        </div>
        <button type="submit" class="btn">Senden</button>
      </form>
    </div>
  `,
  styles: [`
    .form-group { margin-bottom: 1.5rem; }
    .form-group label { display: block; margin-bottom: 0.5rem; font-weight: 500; }
    .form-group input, .form-group textarea { width: 100%; padding: 0.6rem; border: 1px solid #dee2e6; border-radius: 6px; font-size: 1rem; }
    .form-group textarea { min-height: 120px; }
    .btn { background: #c3002f; color: #fff; border: none; padding: 0.6rem 1.5rem; border-radius: 8px; cursor: pointer; }
    .success { text-align: center; padding: 3rem; color: #2ecc71; }
  `]
})
export class ContactComponent {
  private http = inject(HttpClient)
  form = { name: '', email: '', message: '' }
  sent = false

  // Submit the contact form data to the API
  handleSubmit() {
    this.http.post('/api/contact', this.form).subscribe(() => this.sent = true)
  }
}
