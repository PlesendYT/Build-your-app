// SaaS landing page — hero section with CTAs and feature highlights
import Link from 'next/link'

export default function Home() {
  return (
    <div className="text-center py-20">
      <h2 className="text-5xl font-bold mb-4">SaaS Platform</h2>
      <p className="text-xl text-gray-600 mb-8 max-w-2xl mx-auto">
        Die leistungsstarke Plattform für Ihr Unternehmen
      </p>
      <div className="flex gap-4 justify-center">
        <Link href="/dashboard" className="bg-blue-600 text-white px-8 py-3 rounded-lg hover:bg-blue-700">
          Dashboard
        </Link>
        <Link href="/api/auth/signin" className="border border-gray-300 px-8 py-3 rounded-lg hover:bg-gray-50">
          Anmelden
        </Link>
      </div>
      <div className="grid grid-cols-3 gap-8 mt-20 max-w-4xl mx-auto">
        {['Feature 1', 'Feature 2', 'Feature 3'].map(f => (
          <div key={f} className="bg-white p-6 rounded-lg shadow-sm">
            <h3 className="font-semibold text-lg">{f}</h3>
            <p className="text-gray-500 mt-2">Beschreibung dieses Features</p>
          </div>
        ))}
      </div>
    </div>
  )
}
