<script setup>
import { ref } from 'vue'
import api from '../services/api'

const form = ref({ name: '', email: '', message: '' })
const sent = ref(false)

async function handleSubmit() {
  try {
    await api.post('/contact', form.value)
    sent.value = true
  } catch (err) {
    console.error(err)
  }
}
</script>

<template>
  <div class="contact-page">
    <h2>Kontakt</h2>
    <div v-if="sent" class="success">Nachricht gesendet!</div>
    <form v-else @submit.prevent="handleSubmit">
      <div class="form-group">
        <label for="name">Name</label>
        <input id="name" v-model="form.name" required />
      </div>
      <div class="form-group">
        <label for="email">E-Mail</label>
        <input id="email" type="email" v-model="form.email" required />
      </div>
      <div class="form-group">
        <label for="message">Nachricht</label>
        <textarea id="message" v-model="form.message" required></textarea>
      </div>
      <button type="submit" class="btn">Senden</button>
    </form>
  </div>
</template>
