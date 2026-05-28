'use client'
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts'

const data = [
  { name: 'Jan', value: 400 }, { name: 'Feb', value: 300 }, { name: 'Mär', value: 600 },
  { name: 'Apr', value: 800 }, { name: 'Mai', value: 500 }, { name: 'Jun', value: 900 },
]

const stats = [
  { label: 'Umsatz', value: '€12,430', change: '+12%', positive: true },
  { label: 'Bestellungen', value: '156', change: '+8%', positive: true },
  { label: 'Kunden', value: '1,234', change: '+23%', positive: true },
  { label: 'Besucher', value: '8,932', change: '-3%', positive: false },
]

export default function Dashboard() {
  return (
    <div>
      <h2 className="text-3xl font-bold mb-6">Dashboard</h2>
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-8">
        {stats.map(s => (
          <div key={s.label} className="bg-white p-4 rounded-lg shadow-sm">
            <h3 className="text-gray-500 text-sm">{s.label}</h3>
            <p className="text-2xl font-bold">{s.value}</p>
            <span className={s.positive ? 'text-green-600' : 'text-red-600'}>{s.change}</span>
          </div>
        ))}
      </div>
      <div className="bg-white p-6 rounded-lg shadow-sm">
        <h3 className="font-semibold mb-4">Monatsübersicht</h3>
        <ResponsiveContainer width="100%" height={300}>
          <BarChart data={data}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="name" />
            <YAxis />
            <Tooltip />
            <Bar dataKey="value" fill="#3b82f6" radius={[4,4,0,0]} />
          </BarChart>
        </ResponsiveContainer>
      </div>
    </div>
  )
}
