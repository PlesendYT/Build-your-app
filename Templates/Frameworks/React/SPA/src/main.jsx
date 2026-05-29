import React from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter, Routes, Route, Link, useParams } from 'react-router-dom'
import './index.css'

// Static page data used for dynamic routing via /page/:id
const pages = [
  { id: 'home', title: 'Startseite', content: 'Willkommen bei unserer SPA.' },
  { id: 'services', title: 'Dienstleistungen', content: 'Wir bieten Webentwicklung, Design und Beratung.' },
  { id: 'team', title: 'Team', content: 'Unser erfahrenes Team freut sich auf Sie.' },
  { id: 'contact', title: 'Kontakt', content: 'Erreichen Sie uns per E-Mail oder Telefon.' }
]

/** Landing page displayed at the root route. */
function Home() {
  return (
    <div className="page home">
      <h2>Single Page Application</h2>
      <p>Diese SPA demonstriert nahtlose Navigation und dynamisches Routing.</p>
    </div>
  )
}

/** Dynamic page rendered based on the :id route parameter. Falls back to a 404 message. */
function Page() {
  const { id } = useParams()
  // Look up the page data matching the current URL parameter
  const page = pages.find(p => p.id === id)
  if (!page) return <div className="page"><h2>Seite nicht gefunden</h2></div>
  return (
    <div className="page">
      <h2>{page.title}</h2>
      <p>{page.content}</p>
    </div>
  )
}

/** Root application component with header navigation and route definitions. */
function App() {
  return (
    <div className="app">
      <header className="header">
        <h1 className="logo">MySPA</h1>
        <nav className="nav">
          <Link to="/">Home</Link>
          {pages.filter(p => p.id !== 'home').map(p => (
            <Link key={p.id} to={`/page/${p.id}`}>{p.title}</Link>
          ))}
        </nav>
      </header>
      <main className="main">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/page/:id" element={<Page />} />
        </Routes>
      </main>
    </div>
  )
}

// Mount the React app into the DOM with routing enabled
ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </React.StrictMode>
)
