// Root layout — provides the global HTML structure, navigation header, and footer
import type { Metadata } from 'next'
import './globals.css'

export const metadata: Metadata = {
  title: 'NextWebApp',
  description: 'Next.js 14 Web Application',
}

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="de">
      <body className="min-h-screen flex flex-col">
        <header className="bg-white border-b sticky top-0 z-50">
          <div className="max-w-6xl mx-auto px-4 py-3 flex items-center justify-between">
            <h1 className="text-xl font-bold text-blue-600">NextApp</h1>
            <nav className="flex gap-6">
              <a href="/" className="hover:text-blue-600 transition">Home</a>
              <a href="/about" className="hover:text-blue-600 transition">About</a>
              <a href="/contact" className="hover:text-blue-600 transition">Contact</a>
            </nav>
          </div>
        </header>
        <main className="flex-1 max-w-6xl mx-auto px-4 py-8 w-full">
          {children}
        </main>
        <footer className="bg-white border-t py-4 text-center text-gray-500 text-sm">
          &copy; {new Date().getFullYear()} NextApp
        </footer>
      </body>
    </html>
  )
}
