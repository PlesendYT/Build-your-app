<script setup>
import { useCartStore } from '../stores/cart'
const cart = useCartStore()
</script>

<template>
  <div>
    <h2>Warenkorb</h2>
    <div v-if="cart.count === 0" class="empty">Ihr Warenkorb ist leer.</div>
    <div v-else>
      <div v-for="item in cart.items" :key="item.id" class="cart-item">
        <span>{{ item.name }} - {{ item.quantity }}x - €{{ (item.price * item.quantity).toFixed(2) }}</span>
        <button @click="cart.remove(item.id)">Entfernen</button>
      </div>
      <h3>Gesamt: €{{ cart.total.toFixed(2) }}</h3>
    </div>
  </div>
</template>
