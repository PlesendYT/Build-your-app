import { Link } from 'react-router-dom'

/** 404 fallback page displayed when no route matches. */
function NotFound() {
  return (
    <div className="not-found">
      <h2>404 - Seite nicht gefunden</h2>
      <p>Die angeforderte Seite existiert nicht.</p>
      <Link to="/" className="btn">Zurück zur Startseite</Link>
    </div>
  )
}

export default NotFound
