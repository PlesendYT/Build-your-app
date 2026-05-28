export interface Post {
  slug: string
  title: string
  date: string
  excerpt: string
  content: string
}

export const posts: Post[] = [
  {
    slug: 'hello-world',
    title: 'Hello World',
    date: '2024-01-15',
    excerpt: 'Willkommen zum Blog!',
    content: 'Dies ist unser erster Blog-Post mit Next.js.',
  },
  {
    slug: 'nextjs-guide',
    title: 'Next.js Guide',
    date: '2024-01-20',
    excerpt: 'Ein Leitfaden für Next.js 14',
    content: 'Next.js 14 bringt viele neue Features wie den App Router.',
  },
]
