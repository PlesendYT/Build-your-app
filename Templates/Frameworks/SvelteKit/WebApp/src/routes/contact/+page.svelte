<script lang="ts">
  // Track whether the contact form has been submitted
  let sent = false
  // Form field bindings
  let name = '', email = '', message = ''

  // Handle form submission – marks the message as sent
  function handleSubmit() {
    sent = true
  }
</script>

<div class="max-w-lg mx-auto">
  <h2 class="text-2xl font-bold mb-4">Kontakt</h2>
  {#if sent}
    <div class="success">Nachricht gesendet!</div>
  {:else}
    <form on:submit|preventDefault={handleSubmit} class="space-y-4">
      <div><label class="block mb-1">Name</label><input bind:value={name} class="input" required /></div>
      <div><label class="block mb-1">E-Mail</label><input type="email" bind:value={email} class="input" required /></div>
      <div><label class="block mb-1">Nachricht</label><textarea bind:value={message} class="input" required></textarea></div>
      <button type="submit" class="btn">Senden</button>
    </form>
  {/if}
</div>

<style>
  .input { width: 100%; padding: 0.6rem; border: 1px solid #dee2e6; border-radius: 6px; }
  .success { text-align: center; padding: 3rem; color: #2ecc71; }
</style>
