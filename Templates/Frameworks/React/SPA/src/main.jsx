import React from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter, Routes, Route, Link, useParams } from 'react-router-dom'
import './index.css'

const pages = [
  { id: 'home', title: 'Startseite', content: 'Willkommen bei unserer SPA.' },
  { id: 'services', title: 'Dienstleistungen', content: 'Wir bieten Webentwicklung, Design und Beratung.' },
  { id: 'team', title: 'Team', content: 'Unser erfahrenes Team freut sich auf Sie.' },
  { id: 'contact', title: 'Kontakt', content: 'Erreichen Sie uns per E-Mail oder Telefon.' }
]

function Home() {
  return (
    <div className="page home">
      <h2>Single Page Application</h2>
      <p>Diese SPA demonstriert nahtlose Navigation und dynamisches Routing.</p>
    </div>
  )
}

function Page() {
  const { id } = useParams()
  const page = pages.find(p => p.id === id)
  if (!page) return <div className="page"><h2>Seite nicht gefunden</h2></div>
  return (
    <div className="page">
      <h2>{page.title}</h2>
      <p>{page.content}</p>
    </div>
  )
}

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

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </React.StrictMode>
)
