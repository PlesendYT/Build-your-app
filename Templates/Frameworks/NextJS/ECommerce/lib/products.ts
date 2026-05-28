export interface Product {
  id: number
  name: string
  price: number
  category: string
  image: string
}

export const products: Product[] = [
  { id: 1, name: 'Laptop Pro', price: 1299, category: 'Elektronik', image: '💻' },
  { id: 2, name: 'Wireless Maus', price: 49, category: 'Zubehör', image: '🖱️' },
  { id: 3, name: 'Monitor 27"', price: 399, category: 'Elektronik', image: '🖥️' },
  { id: 4, name: 'Kopfhörer ANC', price: 199, category: 'Audio', image: '🎧' },
]
