'use client'
import { useState } from 'react'
import { products } from '@/lib/products'

export default function Home() {
  const [cart, setCart] = useState<{id: number, qty: number}[]>([])

  const addToCart = (id: number) => {
    setCart(prev => {
      const existing = prev.find(item => item.id === id)
      if (existing) return prev.map(item => item.id === id ? {...item, qty: item.qty + 1} : item)
      return [...prev, { id, qty: 1 }]
    })
  }

  const total = cart.reduce((sum, item) => {
    const product = products.find(p => p.id === item.id)
    return sum + (product?.price || 0) * item.qty
  }, 0)

  return (
    <div>
      <div className="flex justify-between items-center mb-8">
        <h2 className="text-3xl font-bold">Shop</h2>
        <span className="text-lg">🛒 {cart.reduce((s, i) => s + i.qty, 0)} Artikel | €{total.toFixed(2)}</span>
      </div>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {products.map(p => (
          <div key={p.id} className="bg-white rounded-lg shadow-sm overflow-hidden">
            <div className="text-4xl text-center py-8 bg-gray-50">{p.image}</div>
            <div className="p-4">
              <h3 className="font-semibold">{p.name}</h3>
              <p className="text-gray-500 text-sm">{p.category}</p>
              <p className="text-xl font-bold text-blue-600 mt-2">€{p.price}</p>
              <button onClick={() => addToCart(p.id)} className="mt-3 w-full bg-blue-600 text-white py-2 rounded-lg hover:bg-blue-700">
                In den Warenkorb
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}
