<script setup lang="ts">
useHead({ title: 'Home' })
</script>

<template>
  <div class="hero">
    <h2>Willkommen bei NuxtApp</h2>
    <p class="mt-4 mb-8">Nuxt 3 Anwendung mit Composition API</p>
    <NuxtLink to="/about" class="btn">Mehr erfahren</NuxtLink>
  </div>
</template>
