import React, { useState } from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom'
import './index.css'

const initialPosts = [
  { id: 1, author: 'Max M.', avatar: '👤', content: 'Gerade die neue React-Version ausprobiert! 🚀', likes: 12, comments: 3, time: '2h' },
  { id: 2, author: 'Anna S.', avatar: '👩', content: 'Schöner Sonnenuntergang heute 🌅', likes: 24, comments: 5, time: '4h' },
  { id: 3, author: 'Tom W.', avatar: '👨', content: 'Toller Artikel über Webentwicklung 📖', likes: 8, comments: 1, time: '6h' },
]

function Feed() {
  const [posts, setPosts] = useState(initialPosts)
  const [newPost, setNewPost] = useState('')

  const handlePost = () => {
    if (!newPost.trim()) return
    const post = {
      id: Date.now(),
      author: 'Du',
      avatar: '🙋',
      content: newPost,
      likes: 0,
      comments: 0,
      time: 'Jetzt'
    }
    setPosts([post, ...posts])
    setNewPost('')
  }

  const handleLike = (id) => {
    setPosts(posts.map(p => p.id === id ? { ...p, likes: p.likes + 1 } : p))
  }

  return (
    <div className="feed">
      <div className="create-post">
        <textarea
          placeholder="Was gibt's Neues?"
          value={newPost}
          onChange={e => setNewPost(e.target.value)}
        />
        <button onClick={handlePost} className="btn">Posten</button>
      </div>
      <div className="posts">
        {posts.map(post => (
          <div key={post.id} className="post-card">
            <div className="post-header">
              <span className="avatar">{post.avatar}</span>
              <div>
                <strong>{post.author}</strong>
                <span className="post-time">{post.time}</span>
              </div>
            </div>
            <p className="post-content">{post.content}</p>
            <div className="post-actions">
              <button onClick={() => handleLike(post.id)} className="action-btn">
                👍 {post.likes}
              </button>
              <button className="action-btn">
                💬 {post.comments}
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}

function Profile() {
  return (
    <div className="profile">
      <div className="profile-header">
        <span className="profile-avatar">👤</span>
        <h2>Dein Profil</h2>
        <p>Mitglied seit Januar 2024</p>
      </div>
      <div className="profile-stats">
        <div className="stat"><strong>42</strong> Beiträge</div>
        <div className="stat"><strong>128</strong> Follower</div>
        <div className="stat"><strong>56</strong> Folgend</div>
      </div>
    </div>
  )
}

function App() {
  return (
    <div className="app">
      <header className="header">
        <Link to="/" className="logo">SocialApp</Link>
        <nav className="nav">
          <Link to="/">Feed</Link>
          <Link to="/profile">Profil</Link>
        </nav>
      </header>
      <main className="main">
        <Routes>
          <Route path="/" element={<Feed />} />
          <Route path="/profile" element={<Profile />} />
        </Routes>
      </main>
    </div>
  )
}

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </React.StrictMode>
)
