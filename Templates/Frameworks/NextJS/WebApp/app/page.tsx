import Link from 'next/link'

export default function Home() {
  return (
    <div className="text-center py-16">
      <h2 className="text-4xl font-bold mb-4">Willkommen bei NextApp</h2>
      <p className="text-xl text-gray-600 mb-8">Next.js 14 mit App Router und Server Components</p>
      <Link href="/about" className="inline-block bg-blue-600 text-white px-6 py-3 rounded-lg hover:bg-blue-700 transition">
        Mehr erfahren
      </Link>
    </div>
  )
}
