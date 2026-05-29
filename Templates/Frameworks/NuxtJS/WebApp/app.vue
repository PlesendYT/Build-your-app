<!-- Root layout with header navigation, NuxtPage outlet, and footer -->
<template>
  <div class="app-layout">
    <header class="header">
      <h1 class="logo">NuxtApp</h1>
      <nav class="nav">
        <NuxtLink to="/">Home</NuxtLink>
        <NuxtLink to="/about">About</NuxtLink>
        <NuxtLink to="/contact">Contact</NuxtLink>
      </nav>
    </header>
    <main class="main">
      <NuxtPage />
    </main>
    <footer class="footer">
      <p>&copy; {{ new Date().getFullYear() }} NuxtApp</p>
    </footer>
  </div>
</template>

<style>
/* Global reset and base styles for the Nuxt application */
* { margin: 0; padding: 0; box-sizing: border-box; }
body { font-family: system-ui, sans-serif; background: #f8f9fa; color: #333; }
.app-layout { display: flex; flex-direction: column; min-height: 100vh; }
.header { background: #fff; border-bottom: 1px solid #dee2e6; padding: 1rem 2rem; display: flex; align-items: center; justify-content: space-between; }
.logo { font-size: 1.5rem; color: #00dc82; }
/* Horizontal navigation bar */
.nav { display: flex; gap: 1.5rem; }
.nav a { text-decoration: none; color: #333; }
.nav a:hover { color: #00dc82; }
/* Main content area with centered max-width container */
.main { flex: 1; padding: 2rem; max-width: 1200px; margin: 0 auto; width: 100%; }
/* Sticky footer with top border */
.footer { background: #fff; border-top: 1px solid #dee2e6; padding: 1rem; text-align: center; color: #6c757d; }
/* Hero section centered with vertical padding */
.hero { text-align: center; padding: 4rem 0; }
.hero h2 { font-size: 2.5rem; }
/* Primary call-to-action button */
.btn { display: inline-block; background: #00dc82; color: #fff; padding: 0.6rem 1.5rem; border-radius: 8px; text-decoration: none; }
</style>
