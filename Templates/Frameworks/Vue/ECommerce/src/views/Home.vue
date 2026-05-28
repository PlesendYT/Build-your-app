<script setup>
import { useCartStore } from '../stores/cart'

const cart = useCartStore()
</script>

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
const products = [
  { id: 1, name: 'Laptop', price: 1299 },
  { id: 2, name: 'Maus', price: 49 },
  { id: 3, name: 'Monitor', price: 399 },
]
</script>
