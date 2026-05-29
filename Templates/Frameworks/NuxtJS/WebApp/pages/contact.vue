<script setup lang="ts">
// Set page metadata and define form submission state
useHead({ title: 'Kontakt' })

// Track whether the contact form has been successfully submitted
const sent = ref(false)
</script>

<!-- Contact form with name, email, and message fields, plus success state -->
<template>
  <div class="max-w-lg mx-auto">
    <h2 class="text-2xl font-bold mb-4">Kontakt</h2>
    <div v-if="sent">Nachricht gesendet!</div>
    <form v-else @submit.prevent="sent = true" class="space-y-4">
      <div><label class="block mb-1">Name</label><input class="w-full border rounded px-3 py-2" required /></div>
      <div><label class="block mb-1">E-Mail</label><input type="email" class="w-full border rounded px-3 py-2" required /></div>
      <div><label class="block mb-1">Nachricht</label><textarea class="w-full border rounded px-3 py-2" required></textarea></div>
      <button type="submit" class="btn">Senden</button>
    </form>
  </div>
</template>
