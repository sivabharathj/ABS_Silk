(function () {
  function token() {
    var anti = document.querySelector('input[name="__RequestVerificationToken"]');
    if (anti && anti.value) {
      return anti.value;
    }
    var hidden = document.querySelector('#anti-forgery input');
    if (hidden && hidden.value) {
      return hidden.value;
    }
    return '';
  }

  var heroSlides = document.querySelectorAll('.hero-slide');
  if (heroSlides.length > 1) {
    var i = 0;
    setInterval(function () {
      heroSlides[i].classList.remove('active');
      i = (i + 1) % heroSlides.length;
      heroSlides[i].classList.add('active');
    }, 6500);
  }

  var track = document.getElementById('best-track');
  var carouselButtons = document.querySelectorAll('[data-carousel]');
  for (var c = 0; c < carouselButtons.length; c++) {
    carouselButtons[c].addEventListener('click', function () {
      if (!track) return;
      var dir = this.dataset.carousel === 'next' ? 1 : -1;
      track.scrollBy({ left: dir * 280, behavior: 'smooth' });
    });
  }

  var quickButtons = document.querySelectorAll('[data-quickview]');
  for (var q = 0; q < quickButtons.length; q++) {
    quickButtons[q].addEventListener('click', function () {
      var id = this.getAttribute('data-quickview');
      fetch('/Shop/QuickView/' + id).then(function (res) {
        if (!res.ok) return;
        return res.text();
      }).then(function (html) {
        if (!html) return;
        document.getElementById('quickViewBody').innerHTML = html;
        bootstrap.Modal.getOrCreateInstance('#quickViewModal').show();
      });
    });
  }

  document.addEventListener('click', function (e) {
    var add = e.target.closest('[data-add-cart]');
    if (add) {
      e.preventDefault();
      var id = add.getAttribute('data-add-cart');
      var fd = new FormData();
      fd.append('id', id);
      fd.append('qty', add.getAttribute('data-qty') || '1');
      fd.append('__RequestVerificationToken', token());
      fetch('/Cart/Add', {
        method: 'POST',
        headers: { 'X-Requested-With': 'XMLHttpRequest' },
        body: fd
      }).then(function (res) {
        return res.json();
      }).then(function (data) {
        if (data.ok) {
          var el = document.getElementById('cart-count');
          if (el) el.textContent = data.count;
          toast(data.message || 'Added to cart');
        }
      });
    }
    var wish = e.target.closest('[data-wish]');
    if (wish) {
      e.preventDefault();
      var wid = wish.getAttribute('data-wish');
      var wfd = new FormData();
      wfd.append('id', wid);
      wfd.append('__RequestVerificationToken', token());
      fetch('/Wishlist/Toggle', {
        method: 'POST',
        headers: { 'X-Requested-With': 'XMLHttpRequest' },
        body: wfd
      }).then(function (res) {
        return res.json();
      }).then(function (data) {
        if (data.ok) {
          var el = document.getElementById('wish-count');
          if (el) el.textContent = data.count;
          toast('Wishlist updated');
        }
      });
    }
  });

  function toast(msg) {
    var n = document.createElement('div');
    n.className = 'toast-notice';
    n.textContent = msg;
    var main = document.querySelector('main');
    if (main) {
      main.prepend(n);
    }
    setTimeout(function () {
      n.remove();
    }, 2800);
  }
})();
