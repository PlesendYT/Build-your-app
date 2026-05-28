const projects = [
  { title: 'Projekt 1', description: 'Eine Webanwendung mit React', tech: 'React, Node.js' },
  { title: 'Projekt 2', description: 'Mobile App mit Flutter', tech: 'Flutter, Dart' },
  { title: 'Projekt 3', description: 'API mit Django', tech: 'Python, Django' },
]

export default function Portfolio() {
  return (
    <div>
      <div className="text-center py-12">
        <h2 className="text-4xl font-bold mb-2">Max Mustermann</h2>
        <p className="text-gray-600">Full-Stack Entwickler</p>
      </div>
      <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
        {projects.map(p => (
          <div key={p.title} className="bg-white p-6 rounded-lg shadow-sm">
            <h3 className="font-semibold text-lg">{p.title}</h3>
            <p className="text-gray-600 mt-2">{p.description}</p>
            <p className="text-sm text-blue-600 mt-2">{p.tech}</p>
          </div>
        ))}
      </div>
    </div>
  )
}
