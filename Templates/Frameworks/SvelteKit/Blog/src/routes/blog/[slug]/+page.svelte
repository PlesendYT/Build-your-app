<script lang="ts">
  import { page } from '$app/stores'
  import { posts } from '$lib/posts'
  const post = posts.find(p => p.slug === $page.params.slug)
</script>

{#if post}
  <h2>{post.title}</h2>
  <p>{post.excerpt}</p>
{:else}
  <h2>Nicht gefunden</h2>
{/if}
