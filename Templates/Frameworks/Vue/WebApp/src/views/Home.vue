<script setup>
// Import lifecycle hook, app store, and router link for hero section
import { onMounted } from 'vue'
import { useAppStore } from '../stores/app'
import { RouterLink } from 'vue-router'

// App store instance for data fetching and loading state
const store = useAppStore()

// Fetch data on component mount
onMounted(() => {
  store.fetchItems()
})
</script>

<!-- Home page with hero section, call-to-action, and dynamic data grid from store -->
<template>
  <div class="home-page">
    <section class="hero">
      <h2>Willkommen bei VueWebApp</h2>
      <p>Moderne Vue 3 Anwendung mit Composition API</p>
      <RouterLink to="/about" class="btn">Mehr erfahren</RouterLink>
    </section>
    <section class="content">
      <h3>Daten</h3>
      <div v-if="store.loading" class="loading">Laden...</div>
      <div v-else class="data-grid">
        <div v-for="item in store.items" :key="item.id" class="data-card">
          <h4>{{ item.title }}</h4>
          <p>{{ item.description }}</p>
        </div>
      </div>
    </section>
  </div>
</template>
