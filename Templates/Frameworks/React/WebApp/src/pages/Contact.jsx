import { useState } from 'react'
import api from '../services/api'

/** Contact page with a form that submits user messages via the API. */
function Contact() {
  const [form, setForm] = useState({ name: '', email: '', message: '' })
  const [sent, setSent] = useState(false)

  // Submit the contact form to the backend API
  const handleSubmit = async (e) => {
    e.preventDefault()
    try {
      await api.post('/contact', form)
      setSent(true)
    } catch (err) {
      console.error(err)
    }
  }

  if (sent) return <div className="success">Nachricht gesendet!</div>

  return (
    <div className="contact-page">
      <h2>Kontakt</h2>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="name">Name</label>
          <input id="name" value={form.name} onChange={e => setForm({...form, name: e.target.value})} required />
        </div>
        <div className="form-group">
          <label htmlFor="email">E-Mail</label>
          <input id="email" type="email" value={form.email} onChange={e => setForm({...form, email: e.target.value})} required />
        </div>
        <div className="form-group">
          <label htmlFor="message">Nachricht</label>
          <textarea id="message" value={form.message} onChange={e => setForm({...form, message: e.target.value})} required />
        </div>
        <button type="submit" className="btn">Senden</button>
      </form>
    </div>
  )
}

export default Contact
