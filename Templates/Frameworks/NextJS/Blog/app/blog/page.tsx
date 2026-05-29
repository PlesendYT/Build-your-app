// Blog listing page — renders a list of all posts with links to individual articles
import Link from 'next/link'
import { posts } from '@/lib/posts'

export default function Blog() {
  return (
    <div>
      <h2 className="text-3xl font-bold mb-8">Blog</h2>
      <div className="grid gap-6">
        {posts.map(post => (
          <article key={post.slug} className="bg-white p-6 rounded-lg shadow-sm">
            <Link href={`/blog/${post.slug}`} className="text-xl font-semibold text-blue-600 hover:underline">
              {post.title}
            </Link>
            <p className="text-gray-500 text-sm mt-1">{post.date}</p>
            <p className="mt-2 text-gray-700">{post.excerpt}</p>
          </article>
        ))}
      </div>
    </div>
  )
}
