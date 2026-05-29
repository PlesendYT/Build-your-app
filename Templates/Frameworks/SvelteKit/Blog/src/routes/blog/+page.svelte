<script lang="ts">
  // Import the posts data source
  import { posts } from '$lib/posts'
</script>

<h2>Blog</h2>
{#each posts as post}
  <article>
    <h3><a href="/blog/{post.slug}">{post.title}</a></h3>
    <p>{post.excerpt}</p>
  </article>
{/each}
