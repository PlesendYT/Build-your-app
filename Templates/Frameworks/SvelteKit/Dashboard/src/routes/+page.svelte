<script lang="ts">
  // Dashboard statistics data: label, current value, change indicator
  const stats = [
    { label: 'Umsatz', value: '€12,430', change: '+12%', positive: true },
    { label: 'Bestellungen', value: '156', change: '+8%', positive: true },
    { label: 'Kunden', value: '1,234', change: '+23%', positive: true },
    { label: 'Besucher', value: '8,932', change: '-3%', positive: false },
  ]
</script>

<div class="dashboard">
  <h2>Dashboard</h2>
  <div class="stats-grid">
    {#each stats as s}
      <div class="stat-card">
        <h3>{s.label}</h3>
        <p class="value">{s.value}</p>
        <span class:positive={s.positive} class:negative={!s.positive}>{s.change}</span>
      </div>
    {/each}
  </div>
</div>

<style>
  .stats-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 1rem; }
  .stat-card { background: #fff; padding: 1.5rem; border-radius: 8px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); }
  .value { font-size: 1.8rem; font-weight: bold; }
  .positive { color: #27ae60; }
  .negative { color: #e74c3c; }
</style>
