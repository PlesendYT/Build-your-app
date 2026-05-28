import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import api from '../services/api'

function Home() {
  const [data, setData] = useState([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    api.get('/items')
      .then(res => setData(res.data))
      .catch(err => console.error(err))
      .finally(() => setLoading(false))
  }, [])

  if (loading) return <div className="loading">Laden...</div>

  return (
    <div className="home-page">
      <section className="hero">
        <h2>Willkommen bei WebApp</h2>
        <p>Ihre moderne React-Anwendung</p>
        <Link to="/about" className="btn">Mehr erfahren</Link>
      </section>
      <section className="content">
        <h3>Daten</h3>
        <div className="data-grid">
          {data.map(item => (
            <div key={item.id} className="data-card">
              <h4>{item.title}</h4>
              <p>{item.description}</p>
            </div>
          ))}
        </div>
      </section>
    </div>
  )
}

export default Home
