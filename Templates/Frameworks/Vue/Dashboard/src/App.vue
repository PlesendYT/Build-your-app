<script setup>
// Import Vue composition API and router utilities
import { ref, onMounted } from 'vue'
import { RouterView, RouterLink, useRoute } from 'vue-router'
// Import and register Chart.js components for dashboard charts
import { Chart, registerables } from 'chart.js'

Chart.register(...registerables)

// Sidebar toggle state
const sidebarOpen = ref(true)
const toggleSidebar = () => sidebarOpen.value = !sidebarOpen.value

// Dashboard stat cards data (revenue, orders, customers, visitors)
const stats = [
  { label: 'Umsatz', value: '€12,430', change: '+12%', positive: true },
  { label: 'Bestellungen', value: '156', change: '+8%', positive: true },
  { label: 'Kunden', value: '1,234', change: '+23%', positive: true },
  { label: 'Besucher', value: '8,932', change: '-3%', positive: false },
]

// Recent orders table data
const recentOrders = [
  { customer: 'Max M.', product: 'Laptop', status: 'Versandt', date: '2024-01-15' },
  { customer: 'Anna S.', product: 'Monitor', status: 'Bearbeitung', date: '2024-01-14' },
  { customer: 'Tom W.', product: 'Tastatur', status: 'Ausgeliefert', date: '2024-01-13' },
]
</script>

<!-- Root layout with sidebar navigation, topbar, and main dashboard content -->
<template>
  <div class="app" :class="{ 'sidebar-closed': !sidebarOpen }">
    <aside class="sidebar">
      <h2 class="sidebar-logo">Admin</h2>
      <nav class="sidebar-nav">
        <RouterLink to="/" class="nav-item">Dashboard</RouterLink>
        <RouterLink to="/users" class="nav-item">Benutzer</RouterLink>
        <RouterLink to="/settings" class="nav-item">Einstellungen</RouterLink>
      </nav>
    </aside>
    <div class="main-area">
      <header class="topbar">
        <button class="toggle-btn" @click="toggleSidebar">☰</button>
        <span>Administrator</span>
      </header>
      <main class="main-content">
        <div class="dashboard-content">
          <h2>Dashboard</h2>
          <div class="stats-grid">
            <div v-for="s in stats" :key="s.label" class="stat-card">
              <h3>{{ s.label }}</h3>
              <p class="stat-value">{{ s.value }}</p>
              <span :class="['stat-change', s.positive ? 'positive' : 'negative']">{{ s.change }}</span>
            </div>
          </div>
          <div class="table-section">
            <h3>Letzte Bestellungen</h3>
            <table class="data-table">
              <thead>
                <tr><th>Kunde</th><th>Produkt</th><th>Status</th><th>Datum</th></tr>
              </thead>
              <tbody>
                <tr v-for="o in recentOrders" :key="o.customer">
                  <td>{{ o.customer }}</td><td>{{ o.product }}</td>
                  <td><span :class="['status', `status-${o.status.toLowerCase()}`]">{{ o.status }}</span></td>
                  <td>{{ o.date }}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </main>
    </div>
  </div>
</template>
