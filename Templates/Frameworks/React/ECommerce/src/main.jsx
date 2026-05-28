import React, { createContext, useContext, useReducer } from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom'
import './index.css'

const CartContext = createContext()

function cartReducer(state, action) {
  switch (action.type) {
    case 'ADD':
      const existing = state.find(item => item.id === action.product.id)
      if (existing) {
        return state.map(item =>
          item.id === action.product.id
            ? { ...item, quantity: item.quantity + 1 }
            : item
        )
      }
      return [...state, { ...action.product, quantity: 1 }]
    case 'REMOVE':
      return state.filter(item => item.id !== action.id)
    case 'UPDATE_QTY':
      return state.map(item =>
        item.id === action.id ? { ...item, quantity: action.quantity } : item
      )
    default:
      return state
  }
}

const products = [
  { id: 1, name: 'Laptop Pro', price: 1299, category: 'Elektronik', image: '💻' },
  { id: 2, name: 'Wireless Maus', price: 49, category: 'Zubehör', image: '🖱️' },
  { id: 3, name: 'Monitor 27"', price: 399, category: 'Elektronik', image: '🖥️' },
  { id: 4, name: 'Tastatur Mechanisch', price: 89, category: 'Zubehör', image: '⌨️' },
  { id: 5, name: 'Kopfhörer ANC', price: 199, category: 'Audio', image: '🎧' },
  { id: 6, name: 'Webcam HD', price: 79, category: 'Zubehör', image: '📷' },
]

function Home() {
  const { dispatch } = useContext(CartContext)

  return (
    <div className="home-page">
      <section className="hero">
        <h2>Willkommen im Shop</h2>
        <p>Entdecken Sie unsere Produkte</p>
      </section>
      <section className="product-grid">
        {products.map(p => (
          <div key={p.id} className="product-card">
            <div className="product-image">{p.image}</div>
            <div className="product-info">
              <h3>{p.name}</h3>
              <p className="product-category">{p.category}</p>
              <p className="product-price">€{p.price.toFixed(2)}</p>
              <button className="btn" onClick={() => dispatch({ type: 'ADD', product: p })}>
                In den Warenkorb
              </button>
            </div>
          </div>
        ))}
      </section>
    </div>
  )
}

function Cart() {
  const { state, dispatch } = useContext(CartContext)
  const total = state.reduce((sum, item) => sum + item.price * item.quantity, 0)

  if (state.length === 0) {
    return <div className="cart-empty"><h2>Warenkorb ist leer</h2><Link to="/" className="btn">Weiter einkaufen</Link></div>
  }

  return (
    <div className="cart-page">
      <h2>Warenkorb</h2>
      <div className="cart-items">
        {state.map(item => (
          <div key={item.id} className="cart-item">
            <span className="cart-item-image">{item.image}</span>
            <div className="cart-item-info">
              <h4>{item.name}</h4>
              <p>€{item.price.toFixed(2)}</p>
            </div>
            <div className="cart-item-qty">
              <button onClick={() => dispatch({ type: 'UPDATE_QTY', id: item.id, quantity: Math.max(1, item.quantity - 1) })}>-</button>
              <span>{item.quantity}</span>
              <button onClick={() => dispatch({ type: 'UPDATE_QTY', id: item.id, quantity: item.quantity + 1 })}>+</button>
            </div>
            <p className="cart-item-total">€{(item.price * item.quantity).toFixed(2)}</p>
            <button className="btn-remove" onClick={() => dispatch({ type: 'REMOVE', id: item.id })}>✕</button>
          </div>
        ))}
      </div>
      <div className="cart-total">
        <h3>Gesamt: €{total.toFixed(2)}</h3>
        <button className="btn btn-checkout">Zur Kasse</button>
      </div>
    </div>
  )
}

function App() {
  const [state, dispatch] = useReducer(cartReducer, [])
  const itemCount = state.reduce((sum, item) => sum + item.quantity, 0)

  return (
    <CartContext.Provider value={{ state, dispatch }}>
      <div className="app">
        <header className="header">
          <Link to="/" className="logo">MyShop</Link>
          <nav className="nav">
            <Link to="/">Shop</Link>
            <Link to="/cart" className="cart-link">
              🛒 Warenkorb {itemCount > 0 && <span className="cart-badge">{itemCount}</span>}
            </Link>
          </nav>
        </header>
        <main className="main">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/cart" element={<Cart />} />
          </Routes>
        </main>
      </div>
    </CartContext.Provider>
  )
}

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </React.StrictMode>
)
