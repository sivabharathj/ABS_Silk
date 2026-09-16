# ABS Silk Saris

Premium luxury e-commerce storefront for **ABS Silk Saris** — authentic Kanchipuram / Kanjivaram silk sarees. The experience is modelled on leading silk houses (including the editorial, collection-led feel of [Hayagrivas Silk House](https://hayagrivassilkhouse.com/)): heritage storytelling, large photography, gold-and-maroon atelier styling, and a complete shop flow.

## Stack

- ASP.NET Core 8 MVC
- Bootstrap 5, HTML5, CSS3, JavaScript / jQuery
- Entity Framework Core
- ASP.NET Identity (login / register)
- **SQLite** out of the box for local and Cloud Agent runs
- **SQL Server** ready (switch the provider)

## Run locally

```bash
cd AbsSilkSaris
dotnet restore
dotnet run --launch-profile http
```

Open http://localhost:5058

The first launch applies migrations and seeds categories, products, and reviews.

## SQL Server

In `appsettings.json`:

```json
"Database": { "Provider": "SqlServer" },
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=AbsSilkSaris;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

Then:

```bash
dotnet ef database update --project AbsSilkSaris
```

## Features

- Homepage: hero, featured categories, new arrivals, best-seller carousel, heritage parallax, why-us, weaving video, reviews, Instagram grid, newsletter, footer
- Mega menu, search, product filters, quick view, wishlist, cart, checkout, order tracking
- WhatsApp concierge, schema.org markup, lazy-loaded images, mobile-first layout
- Policies: privacy, terms, shipping, returns

Sample login is self-service via **Register** (email confirmation is disabled in development).
