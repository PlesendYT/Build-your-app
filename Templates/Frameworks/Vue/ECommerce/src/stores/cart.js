/**
 * Cart store using Pinia (Composition API style).
 * Manages cart items, computed totals, add/remove/update operations.
 */
import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useCartStore = defineStore('cart', () => {
  // Reactive array of cart items with quantity
  const items = ref([])

  // Computed total item count across all entries
  const count = computed(() => items.value.reduce((s, i) => s + i.quantity, 0))
  // Computed total price of all items in cart
  const total = computed(() => items.value.reduce((s, i) => s + i.price * i.quantity, 0))

  /** Add a product to the cart, incrementing quantity if it already exists */
  function add(product) {
    const existing = items.value.find(i => i.id === product.id)
    if (existing) {
      existing.quantity++
    } else {
      items.value.push({ ...product, quantity: 1 })
    }
  }

  /** Remove an item from the cart by product ID */
  function remove(id) {
    items.value = items.value.filter(i => i.id !== id)
  }

  /** Update the quantity of a specific item, minimum of 1 */
  function updateQty(id, qty) {
    const item = items.value.find(i => i.id === id)
    if (item) item.quantity = Math.max(1, qty)
  }

  return { items, count, total, add, remove, updateQty }
})
