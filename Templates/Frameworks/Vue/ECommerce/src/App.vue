<script setup>
import { RouterView, RouterLink } from 'vue-router'
import { useCartStore } from './stores/cart'

const cart = useCartStore()
</script>

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
