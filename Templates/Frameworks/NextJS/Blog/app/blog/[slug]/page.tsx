// Individual blog post page — looks up the post by slug and renders it
import { notFound } from 'next/navigation'
import Link from 'next/link'
import { posts } from '@/lib/posts'

export default function PostPage({ params }: { params: { slug: string } }) {
  const post = posts.find(p => p.slug === params.slug)
  if (!post) notFound()

  return (
    <article className="max-w-2xl mx-auto">
      <Link href="/blog" className="text-blue-600 hover:underline mb-4 block">&larr; Zurück</Link>
      <h2 className="text-3xl font-bold mb-2">{post.title}</h2>
      <p className="text-gray-500 mb-6">{post.date}</p>
      <div className="prose max-w-none">{post.content}</div>
    </article>
  )
}
