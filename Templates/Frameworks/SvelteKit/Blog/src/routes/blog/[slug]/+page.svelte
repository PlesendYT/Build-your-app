<script lang="ts">
  // Import SvelteKit page store and blog posts data
  import { page } from '$app/stores'
  import { posts } from '$lib/posts'
  // Find the current post by matching the slug route parameter
  const post = posts.find(p => p.slug === $page.params.slug)
</script>

{#if post}
  <h2>{post.title}</h2>
  <p>{post.excerpt}</p>
{:else}
  <h2>Nicht gefunden</h2>
{/if}
