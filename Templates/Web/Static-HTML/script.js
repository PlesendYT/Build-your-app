document.addEventListener('DOMContentLoaded', () => {
  // CTA Button
  document.getElementById('ctaButton')?.addEventListener('click', () => {
    document.getElementById('about')?.scrollIntoView({ behavior: 'smooth' });
  });

  // Contact Form
  document.getElementById('contactForm')?.addEventListener('submit', (e) => {
    e.preventDefault();
    const name = document.getElementById('name').value;
    const email = document.getElementById('email').value;
    const message = document.getElementById('message').value;
    const status = document.getElementById('statusMessage');

    if (name && email && message) {
      status.textContent = `Thank you, ${name}! Your message has been sent.`;
      status.style.color = 'green';
      e.target.reset();
    } else {
      status.textContent = 'Please fill in all fields.';
      status.style.color = 'red';
    }
  });

  // Smooth scroll for nav links
  document.querySelectorAll('.nav-links a').forEach(link => {
    link.addEventListener('click', (e) => {
      e.preventDefault();
      const target = document.querySelector(link.getAttribute('href'));
      if (target) target.scrollIntoView({ behavior: 'smooth' });
    });
  });
});
