<script setup>
// Import route for dynamic page resolution based on URL parameter
import { useRoute } from 'vue-router'

// Current route instance to read the :id param
const route = useRoute()

// Page content map keyed by route parameter values
const pages = {
  services: { title: 'Dienstleistungen', content: 'Webentwicklung, Design und Beratung aus einer Hand.' },
  team: { title: 'Team', content: 'Unser erfahrenes Team freut sich auf Ihre Herausforderungen.' },
  contact: { title: 'Kontakt', content: 'Kontaktieren Sie uns per E-Mail oder Telefon.' },
}

// Resolve current page object from route param
const page = pages[route.params.id]
</script>

<!-- Dynamic page view — renders content from route param or shows a not-found fallback -->
<template>
  <div class="page" v-if="page">
    <h2>{{ page.title }}</h2>
    <p>{{ page.content }}</p>
  </div>
  <div class="page" v-else>
    <h2>Seite nicht gefunden</h2>
  </div>
</template>
