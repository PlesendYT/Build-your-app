import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useCartStore = defineStore('cart', () => {
  const items = ref([])

  const count = computed(() => items.value.reduce((s, i) => s + i.quantity, 0))
  const total = computed(() => items.value.reduce((s, i) => s + i.price * i.quantity, 0))

  function add(product) {
    const existing = items.value.find(i => i.id === product.id)
    if (existing) {
      existing.quantity++
    } else {
      items.value.push({ ...product, quantity: 1 })
    }
  }

  function remove(id) {
    items.value = items.value.filter(i => i.id !== id)
  }

  function updateQty(id, qty) {
    const item = items.value.find(i => i.id === id)
    if (item) item.quantity = Math.max(1, qty)
  }

  return { items, count, total, add, remove, updateQty }
})
