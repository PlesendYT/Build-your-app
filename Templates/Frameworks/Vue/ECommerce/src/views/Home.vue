<script setup>
// Import cart store to enable adding products from the shop view
import { useCartStore } from '../stores/cart'

// Reactive cart instance for add-to-cart functionality
const cart = useCartStore()
</script>

<!-- Shop home view rendering product cards with add-to-cart buttons -->
<template>
  <div>
    <h2>Shop</h2>
    <div class="product-grid">
      <div v-for="p in products" :key="p.id" class="product-card">
        <h3>{{ p.name }}</h3>
        <p>€{{ p.price }}</p>
        <button @click="cart.add(p)">In den Warenkorb</button>
      </div>
    </div>
  </div>
</template>

<script>
// Product catalog data (non-setup script)
const products = [
  { id: 1, name: 'Laptop', price: 1299 },
  { id: 2, name: 'Maus', price: 49 },
  { id: 3, name: 'Monitor', price: 399 },
]
</script>
