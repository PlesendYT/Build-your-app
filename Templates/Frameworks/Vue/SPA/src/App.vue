<script setup>
// Import router components for navigation and routing
import { RouterView, RouterLink, useRoute } from 'vue-router'

// Navigation pages configuration with paths, names, and display labels
const pages = [
  { path: '/', name: 'home', label: 'Home' },
  { path: '/page/services', name: 'services', label: 'Services' },
  { path: '/page/team', name: 'team', label: 'Team' },
  { path: '/page/contact', name: 'contact', label: 'Kontakt' },
]
</script>

<!-- SPA layout with header, dynamic navigation links, and router view -->
<template>
  <div class="app">
    <header class="header">
      <h1 class="logo">MyVueSPA</h1>
      <nav class="nav">
        <RouterLink v-for="p in pages" :key="p.name" :to="p.path">{{ p.label }}</RouterLink>
      </nav>
    </header>
    <main class="main">
      <RouterView />
    </main>
  </div>
</template>
