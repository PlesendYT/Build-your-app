/**
 * Application store using Pinia (Composition API style).
 * Manages data items fetched from the API and loading state.
 */
import { defineStore } from 'pinia'
import { ref } from 'vue'
import api from '../services/api'

export const useAppStore = defineStore('app', () => {
  // Fetched data items array
  const items = ref([])
  // Loading indicator for async operations
  const loading = ref(false)

  /** Fetch items from the API and update the store */
  async function fetchItems() {
    loading.value = true
    try {
      const res = await api.get('/items')
      items.value = res.data
    } finally {
      loading.value = false
    }
  }

  return { items, loading, fetchItems }
})
