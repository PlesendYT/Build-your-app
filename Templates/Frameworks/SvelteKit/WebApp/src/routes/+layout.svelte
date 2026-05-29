<script lang="ts">
  // Import global application styles
  import '../app.css'
</script>

<div class="app-layout">
  <header class="header">
    <h1 class="logo">SvelteApp</h1>
    <nav class="nav">
      <a href="/">Home</a>
      <a href="/about">About</a>
      <a href="/contact">Contact</a>
    </nav>
  </header>
  <main class="main">
    <slot />
  </main>
  <footer class="footer">
    <p>&copy; {new Date().getFullYear()} SvelteApp</p>
  </footer>
</div>

<style>
  * { margin: 0; padding: 0; box-sizing: border-box; }
  body { font-family: system-ui, sans-serif; background: #f8f9fa; color: #333; }
  .app-layout { display: flex; flex-direction: column; min-height: 100vh; }
  .header { background: #fff; border-bottom: 1px solid #dee2e6; padding: 1rem 2rem; display: flex; align-items: center; justify-content: space-between; }
  .logo { font-size: 1.5rem; color: #ff3e00; }
  .nav { display: flex; gap: 1.5rem; }
  .nav a { text-decoration: none; color: #333; }
  .nav a:hover { color: #ff3e00; }
  .main { flex: 1; padding: 2rem; max-width: 1200px; margin: 0 auto; width: 100%; }
  .footer { background: #fff; border-top: 1px solid #dee2e6; padding: 1rem; text-align: center; }
  .hero { text-align: center; padding: 4rem 0; }
  .hero h2 { font-size: 2.5rem; }
  .btn { display: inline-block; background: #ff3e00; color: #fff; padding: 0.6rem 1.5rem; border-radius: 8px; text-decoration: none; }
</style>
