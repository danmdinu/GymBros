/* ─────────────────────────────────────────────
   GymBros — script.js
───────────────────────────────────────────── */

// ── Navbar scroll effect ─────────────────────
const navbar = document.getElementById('navbar');

window.addEventListener('scroll', () => {
  if (window.scrollY > 40) {
    navbar.classList.add('scrolled');
  } else {
    navbar.classList.remove('scrolled');
  }
}, { passive: true });


// ── Mobile menu toggle ───────────────────────
const menuToggle = document.getElementById('menu-toggle');
const mobileMenu = document.getElementById('mobile-menu');
const iconOpen   = document.getElementById('icon-open');
const iconClose  = document.getElementById('icon-close');

menuToggle.addEventListener('click', () => {
  const isHidden = mobileMenu.classList.contains('hidden');
  mobileMenu.classList.toggle('hidden', !isHidden);
  iconOpen.classList.toggle('hidden', isHidden);
  iconClose.classList.toggle('hidden', !isHidden);
});

// Close mobile menu when a link is clicked
mobileMenu.querySelectorAll('a').forEach(link => {
  link.addEventListener('click', () => {
    mobileMenu.classList.add('hidden');
    iconOpen.classList.remove('hidden');
    iconClose.classList.add('hidden');
  });
});


// ── Scroll animations (Intersection Observer) ─
const fadeElements = document.querySelectorAll('.fade-up');

const observer = new IntersectionObserver(
  (entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        entry.target.classList.add('visible');
        observer.unobserve(entry.target);
      }
    });
  },
  {
    threshold: 0.12,
    rootMargin: '0px 0px -40px 0px'
  }
);

fadeElements.forEach(el => observer.observe(el));


// ── Smooth scroll for anchor links ──────────
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
  anchor.addEventListener('click', (e) => {
    const target = document.querySelector(anchor.getAttribute('href'));
    if (!target) return;
    e.preventDefault();
    const navHeight = navbar.offsetHeight;
    const top = target.getBoundingClientRect().top + window.scrollY - navHeight - 8;
    window.scrollTo({ top, behavior: 'smooth' });
  });
});


// ── Waitlist form (Formspree AJAX) ───────────
const form       = document.getElementById('waitlist-form');
const submitBtn  = document.getElementById('submit-btn');
const btnText    = document.getElementById('btn-text');
const btnLoading = document.getElementById('btn-loading');
const success    = document.getElementById('form-success');
const error      = document.getElementById('form-error');

if (form) {
  form.addEventListener('submit', async (e) => {
    e.preventDefault();

    // Guard: if form endpoint is still the placeholder, show a gentle warning in dev
    if (form.action.includes('YOUR_FORM_ID')) {
      success.classList.remove('hidden');
      success.textContent = '(Dev mode) Form submitted! Replace YOUR_FORM_ID in index.html with your Formspree endpoint to go live.';
      form.reset();
      return;
    }

    // Loading state
    submitBtn.disabled = true;
    btnText.classList.add('hidden');
    btnLoading.classList.remove('hidden');
    success.classList.add('hidden');
    error.classList.add('hidden');

    try {
      const data = new FormData(form);
      const response = await fetch(form.action, {
        method: 'POST',
        body: data,
        headers: { Accept: 'application/json' }
      });

      if (response.ok) {
        form.reset();
        form.classList.add('hidden');
        success.classList.remove('hidden');
      } else {
        throw new Error('Server error');
      }
    } catch (err) {
      error.classList.remove('hidden');
    } finally {
      submitBtn.disabled = false;
      btnText.classList.remove('hidden');
      btnLoading.classList.add('hidden');
    }
  });
}


// ── Counter animation for stat cards ─────────
function animateCounter(el, target, duration = 1500) {
  const isInfinity = el.textContent.trim() === '∞';
  if (isInfinity) return;

  const start = 0;
  const startTime = performance.now();
  const suffix = el.dataset.suffix || '';

  const step = (currentTime) => {
    const elapsed = currentTime - startTime;
    const progress = Math.min(elapsed / duration, 1);
    const eased = 1 - Math.pow(1 - progress, 3); // ease-out cubic
    const current = Math.floor(eased * target);
    el.textContent = current + suffix;
    if (progress < 1) requestAnimationFrame(step);
  };

  requestAnimationFrame(step);
}

const statObserver = new IntersectionObserver(
  (entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        const numEl = entry.target.querySelector('.font-display');
        if (!numEl) return;
        const raw = numEl.textContent.trim();
        if (raw === '∞' || raw === '0') return;
        const num = parseInt(raw, 10);
        if (!isNaN(num)) animateCounter(numEl, num);
        statObserver.unobserve(entry.target);
      }
    });
  },
  { threshold: 0.5 }
);

document.querySelectorAll('.stat-card').forEach(card => statObserver.observe(card));


// ── Highlight active nav link on scroll ──────
const sections = document.querySelectorAll('section[id]');
const navLinks = document.querySelectorAll('nav a[href^="#"]');

const sectionObserver = new IntersectionObserver(
  (entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        navLinks.forEach(link => {
          link.classList.remove('text-lime');
          link.classList.add('text-gray-400');
          if (link.getAttribute('href') === `#${entry.target.id}`) {
            link.classList.remove('text-gray-400');
            link.classList.add('text-lime');
          }
        });
      }
    });
  },
  { threshold: 0.4 }
);

sections.forEach(section => sectionObserver.observe(section));
