// Contact page — simple form that shows a success message after submission
'use client'
import { useState } from 'react'

export default function Contact() {
  const [sent, setSent] = useState(false)

  // Handle form submit: prevent reload and show the success message
  async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault()
    setSent(true)
  }

  if (sent) return <div className="text-center py-12 text-green-600 text-xl">Nachricht gesendet!</div>

  return (
    <div className="max-w-lg mx-auto">
      <h2 className="text-3xl font-bold mb-6">Kontakt</h2>
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label className="block mb-1 font-medium">Name</label>
          <input className="w-full border rounded-lg px-3 py-2" required />
        </div>
        <div>
          <label className="block mb-1 font-medium">E-Mail</label>
          <input type="email" className="w-full border rounded-lg px-3 py-2" required />
        </div>
        <div>
          <label className="block mb-1 font-medium">Nachricht</label>
          <textarea className="w-full border rounded-lg px-3 py-2 min-h-[120px]" required />
        </div>
        <button type="submit" className="bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700">
          Senden
        </button>
      </form>
    </div>
  )
}
