import React, { useState } from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter, Routes, Route, Link, NavLink } from 'react-router-dom'
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts'
import './index.css'

// Chart data for the monthly overview bar chart
const data = [
  { name: 'Jan', value: 400 }, { name: 'Feb', value: 300 }, { name: 'Mär', value: 600 },
  { name: 'Apr', value: 800 }, { name: 'Mai', value: 500 }, { name: 'Jun', value: 900 }
]

// Static sample data for the recent orders table
const recentOrders = [
  { id: 1, customer: 'Max Mustermann', product: 'Laptop', status: 'Versandt', date: '2024-01-15' },
  { id: 2, customer: 'Anna Schmidt', product: 'Monitor', status: 'Bearbeitung', date: '2024-01-14' },
  { id: 3, customer: 'Tom Weber', product: 'Tastatur', status: 'Ausgeliefert', date: '2024-01-13' },
]

/** Main dashboard overview page with stats cards, chart, and recent orders table. */
function DashboardHome() {
  // KPI stat cards displayed at the top of the dashboard
  const stats = [
    { label: 'Umsatz', value: '€12,430', change: '+12%' },
    { label: 'Bestellungen', value: '156', change: '+8%' },
    { label: 'Kunden', value: '1,234', change: '+23%' },
    { label: 'Besucher', value: '8,932', change: '-3%' },
  ]

  return (
    <div className="dashboard-content">
      <h2>Dashboard</h2>
      {/* Stat cards grid */}
      <div className="stats-grid">
        {stats.map(s => (
          <div key={s.label} className="stat-card">
            <h3>{s.label}</h3>
            <p className="stat-value">{s.value}</p>
            <span className={`stat-change ${s.change.startsWith('+') ? 'positive' : 'negative'}`}>{s.change}</span>
          </div>
        ))}
      </div>
      {/* Monthly revenue bar chart */}
      <div className="chart-section">
        <h3>Monatsübersicht</h3>
        <ResponsiveContainer width="100%" height={300}>
          <BarChart data={data}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="name" />
            <YAxis />
            <Tooltip />
            <Bar dataKey="value" fill="#646cff" radius={[4, 4, 0, 0]} />
          </BarChart>
        </ResponsiveContainer>
      </div>
      {/* Recent orders table */}
      <div className="table-section">
        <h3>Letzte Bestellungen</h3>
        <table className="data-table">
          <thead>
            <tr><th>Kunde</th><th>Produkt</th><th>Status</th><th>Datum</th></tr>
          </thead>
          <tbody>
            {recentOrders.map(o => (
              <tr key={o.id}>
                <td>{o.customer}</td>
                <td>{o.product}</td>
                <td><span className={`status status-${o.status.toLowerCase()}`}>{o.status}</span></td>
                <td>{o.date}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}

/** Settings page placeholder. */
function Settings() {
  return <div className="dashboard-content"><h2>Einstellungen</h2><p>Konfigurieren Sie Ihre Dashboard-Einstellungen.</p></div>
}

/** User management page placeholder. */
function Users() {
  return <div className="dashboard-content"><h2>Benutzerverwaltung</h2><p>Verwalten Sie Ihre Benutzer und Berechtigungen.</p></div>
}

/** Root application component wrapping sidebar, topbar, and routes. */
function App() {
  // Tracks whether the sidebar is open or collapsed
  const [sidebarOpen, setSidebarOpen] = useState(true)

  return (
    <div className={`app ${sidebarOpen ? 'sidebar-open' : ''}`}>
      <aside className="sidebar">
        <h2 className="sidebar-logo">Admin</h2>
        <nav className="sidebar-nav">
          <NavLink to="/" end className="nav-item">Dashboard</NavLink>
          <NavLink to="/users" className="nav-item">Benutzer</NavLink>
          <NavLink to="/settings" className="nav-item">Einstellungen</NavLink>
        </nav>
      </aside>
      <div className="main-area">
        <header className="topbar">
          <button className="toggle-btn" onClick={() => setSidebarOpen(!sidebarOpen)}>☰</button>
          <div className="topbar-right">
            <span>Admin</span>
          </div>
        </header>
        <main className="main-content">
          <Routes>
            <Route index element={<DashboardHome />} />
            <Route path="users" element={<Users />} />
            <Route path="settings" element={<Settings />} />
          </Routes>
        </main>
      </div>
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
