<script setup>
// Import router components for layout navigation and view rendering
import { RouterView, RouterLink } from 'vue-router'
</script>

<!-- Root layout with header navigation, main router view, and footer -->
<template>
  <div class="app-layout">
    <header class="app-header">
      <h1 class="app-logo">VueWebApp</h1>
      <nav class="app-nav">
        <RouterLink to="/">Home</RouterLink>
        <RouterLink to="/about">About</RouterLink>
        <RouterLink to="/contact">Contact</RouterLink>
      </nav>
    </header>
    <main class="app-main">
      <RouterView />
    </main>
    <footer class="app-footer">
      <p>&copy; {{ new Date().getFullYear() }} VueWebApp</p>
    </footer>
  </div>
</template>
