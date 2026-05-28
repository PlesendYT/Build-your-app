import { defineStore } from 'pinia'
import { ref } from 'vue'
import api from '../services/api'

export const useAppStore = defineStore('app', () => {
  const items = ref([])
  const loading = ref(false)

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
