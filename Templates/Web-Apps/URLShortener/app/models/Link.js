// Link model - in-memory store for shortened URLs with click tracking
const links = [];
let nextId = 1;

const Link = {
  // Find a link by its short code
  findByCode(code) {
    return links.find(l => l.code === code) || null;
  },

  // Create a new shortened link
  create({ url, code }) {
    const link = { id: nextId++, url, code, clicks: 0, createdAt: new Date() };
    links.push(link);
    return link;
  },

  // Increment the click counter for analytics
  incrementClicks(code) {
    const link = links.find(l => l.code === code);
    if (link) link.clicks++;
    return link;
  }
};

module.exports = Link;
