// About page — describes the app and lists the technologies used
export default function About() {
  return (
    <div>
      <h2 className="text-3xl font-bold mb-4">Über uns</h2>
      <p className="mb-6">Diese App wurde mit Next.js 14 und dem App Router erstellt.</p>
      <h3 className="text-xl font-semibold mb-2">Technologien</h3>
      <ul className="list-disc pl-5 space-y-1">
        <li>Next.js 14 App Router</li>
        <li>React Server Components</li>
        <li>Tailwind CSS</li>
        <li>TypeScript</li>
      </ul>
    </div>
  )
}
