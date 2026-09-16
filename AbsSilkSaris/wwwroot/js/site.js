(() => {
  const token = () => document.querySelector('input[name="__RequestVerificationToken"]')?.value
    || document.querySelector('#anti-forgery input')?.value
    || '';

  const heroSlides = document.querySelectorAll('.hero-slide');
  if (heroSlides.length > 1) {
    let i = 0;
    setInterval(() => {
      heroSlides[i].classList.remove('active');
      i = (i + 1) % heroSlides.length;
      heroSlides[i].classList.add('active');
    }, 6500);
  }

  const track = document.getElementById('best-track');
  document.querySelectorAll('[data-carousel]').forEach((btn) => {
    btn.addEventListener('click', () => {
      if (!track) return;
      const dir = btn.dataset.carousel === 'next' ? 1 : -1;
      track.scrollBy({ left: dir * 280, behavior: 'smooth' });
    });
  });

  document.querySelectorAll('[data-quickview]').forEach((btn) => {
    btn.addEventListener('click', async () => {
      const id = btn.getAttribute('data-quickview');
      const res = await fetch(`/Shop/QuickView/${id}`);
      if (!res.ok) return;
      document.getElementById('quickViewBody').innerHTML = await res.text();
      bootstrap.Modal.getOrCreateInstance('#quickViewModal').show();
    });
  });

  document.addEventListener('click', async (e) => {
    const add = e.target.closest('[data-add-cart]');
    if (add) {
      e.preventDefault();
      const id = add.getAttribute('data-add-cart');
      const fd = new FormData();
      fd.append('id', id);
      fd.append('qty', add.getAttribute('data-qty') || '1');
      fd.append('__RequestVerificationToken', token());
      const res = await fetch('/Cart/Add', {
        method: 'POST',
        headers: { 'X-Requested-With': 'XMLHttpRequest' },
        body: fd
      });
      const data = await res.json();
      if (data.ok) {
        const el = document.getElementById('cart-count');
        if (el) el.textContent = data.count;
        toast(data.message || 'Added to cart');
      }
    }
    const wish = e.target.closest('[data-wish]');
    if (wish) {
      e.preventDefault();
      const id = wish.getAttribute('data-wish');
      const fd = new FormData();
      fd.append('id', id);
      fd.append('__RequestVerificationToken', token());
      const res = await fetch('/Wishlist/Toggle', {
        method: 'POST',
        headers: { 'X-Requested-With': 'XMLHttpRequest' },
        body: fd
      });
      const data = await res.json();
      if (data.ok) {
        const el = document.getElementById('wish-count');
        if (el) el.textContent = data.count;
        toast('Wishlist updated');
      }
    }
  });

  function toast(msg) {
    const n = document.createElement('div');
    n.className = 'toast-notice';
    n.textContent = msg;
    document.querySelector('main')?.prepend(n);
    setTimeout(() => n.remove(), 2800);
  }
})();
