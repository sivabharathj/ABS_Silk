# ABS Silk Saris

Premium Kanchipuram silk storefront with an **ADO.NET** catalog and an **admin login** that drives categories, subcategories, product images, site options, and the top menu.

## Stack

- ASP.NET Core 8 MVC
- Bootstrap 5, HTML5, CSS3, JavaScript / jQuery
- **ADO.NET** (`Microsoft.Data.Sqlite` locally, `Microsoft.Data.SqlClient` for SQL Server)
- Cookie authentication for staff (not Entity Framework / Identity)

## Run

```bash
cd AbsSilkSaris
dotnet restore
dotnet run --launch-profile http
```

Open http://localhost:5058

Admin: http://localhost:5058/Admin/Account/Login  
Default credentials: **admin** / **Admin@123**

Schema is created on first launch and seed data is inserted if the catalog is empty.

## SQL Server

In `appsettings.json`:

```json
"Database": { "Provider": "SqlServer" },
"ConnectionStrings": {
  "SqlServer": "Server=.;Database=AbsSilkSaris;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

## Admin can manage

- **Categories** — become top-level menu items when “Show in menu” is on
- **Subcategories** — appear in that category’s dropdown
- **Products & images** — primary + gallery uploads
- **Site options** — hero copy/images, announcement, WhatsApp, address

Changes show on the public storefront immediately.
