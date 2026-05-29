<script setup>
// Import router components and cart store for header navigation
import { RouterView, RouterLink } from 'vue-router'
import { useCartStore } from './stores/cart'

// Reactive cart state for displaying item count in nav
const cart = useCartStore()
</script>

<!-- ECommerce app layout with header logo, navigation links, and router view -->
<template>
  <div class="app">
    <header class="header">
      <RouterLink to="/" class="logo">VueShop</RouterLink>
      <nav class="nav">
        <RouterLink to="/">Shop</RouterLink>
        <RouterLink to="/cart" class="cart-link">
          🛒 {{ cart.count > 0 ? `(${cart.count})` : '' }}
        </RouterLink>
      </nav>
    </header>
    <main class="main">
      <RouterView />
    </main>
  </div>
</template>
