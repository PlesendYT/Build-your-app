import { Outlet, Link, NavLink } from 'react-router-dom'

/** Shared layout component rendering header navigation, page content via Outlet, and footer. */
function Layout() {
  return (
    <div className="app-layout">
      <header className="app-header">
        <h1 className="app-logo">WebApp</h1>
        <nav className="app-nav">
          <NavLink to="/" end>Home</NavLink>
          <NavLink to="/about">About</NavLink>
          <NavLink to="/contact">Contact</NavLink>
        </nav>
      </header>
      <main className="app-main">
        <Outlet />
      </main>
      <footer className="app-footer">
        <p>&copy; {new Date().getFullYear()} WebApp. All rights reserved.</p>
      </footer>
    </div>
  )
}

export default Layout
