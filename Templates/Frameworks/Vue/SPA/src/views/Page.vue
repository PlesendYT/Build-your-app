<script setup>
import { useRoute } from 'vue-router'

const route = useRoute()
const pages = {
  services: { title: 'Dienstleistungen', content: 'Webentwicklung, Design und Beratung aus einer Hand.' },
  team: { title: 'Team', content: 'Unser erfahrenes Team freut sich auf Ihre Herausforderungen.' },
  contact: { title: 'Kontakt', content: 'Kontaktieren Sie uns per E-Mail oder Telefon.' },
}
const page = pages[route.params.id]
</script>

<template>
  <div class="page" v-if="page">
    <h2>{{ page.title }}</h2>
    <p>{{ page.content }}</p>
  </div>
  <div class="page" v-else>
    <h2>Seite nicht gefunden</h2>
  </div>
</template>
