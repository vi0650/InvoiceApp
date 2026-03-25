# 📦 Invoice Management System

A full-stack invoicing solution with user management, inventory, GST, and billing workflows.

- Frontend: `Frontend/` (Angular 19 + Angular Material)
- Backend: `Invoice/` (.NET 8 Web API)
- Database: SQL Server (default), cloud database Neon, PostgreSQL support via EF Core provider
- Auth: JWT-based authentication with role-based access (Admin/User)

---

## 🚀 Key Features

- User authentication (JWT) and role-based access (Admin / User)
- Customer management (CRUD)
- Product management (CRUD)
- GST settings management
- Invoice generation, update, and delete
- Dashboard metrics (sales totals, invoices by status) / static
- Invoice items with quantity, price, and GST computation
- API documentation via Swagger (`/swagger`)

---

## 🛠️ Tech Stack

### Frontend

- Angular 19
- Angular Material 19
- RxJS
- Bootstrap icons / google icons
- @ngneat/overview, @ngxpert/hot-toast
- TypeScript 5.7

### Backend

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- Microsoft.EntityFrameworkCore.SqlServer
- Npgsql.EntityFrameworkCore.PostgreSQL (optional)
- AutoMapper
- JWT Bearer Authentication
- Swashbuckle / OpenAPI

---

## 📁 Project Structure

### Root

- `README.md`
- `LICENSE`

### Frontend (Angular app)

- `Frontend/angular.json`
- `Frontend/package.json`
- `Frontend/src/app/`  (main Angular modules and components)
- `Frontend/src/environment/` (environment variables)

### Backend (ASP.NET Core API)

- `Invoice/Invoice.csproj`
- `Invoice/Program.cs`
- `Invoice/appsettings.json`
- `Invoice/Controllers/` (Auth, Customer, Gst, Invoice, Product, User)
- `Invoice/Data/InvoiceDbContext.cs`
- `Invoice/DTOs/` (request/response models)
- `Invoice/Entities/` (database entities not used in project)
- `Invoice/Repository/`, `Invoice/Services/`
- `Invoice/Config/` (dependency injection)

---

## 🧰 Setup Instructions

### 1. Clone repository

```bash
git clone <repository-url>
cd invoiceProject
```

### 2. Backend setup

```bash
cd Invoice
dotnet restore
dotnet build
```

- Configure database connection in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=<your_database>;Trusted_Connection=True;"
}
```

- Run migrations (if not included):

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

- Run API:

```bash
dotnet run
```

Default API URL: `https://localhost:7068` or `http://localhost:5065` (as configured on my backend)

### 3. Frontend setup

```bash
cd ..\Frontend
npm install
npm start
```

Default Angular URL: `http://localhost:4200`

Update frontend API endpoint in `Frontend/src/environments/environment.ts`:

```ts
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7068/api'
};
```

---

## 🧪 Running & Testing

- Backend: `https://localhost:7068/swagger` for interactive API testing.
- Frontend: `http://localhost:4200` UI tests via Angular CLI.
- Add unit/e2e tests in related directories (`Frontend/src/app/...` and `Invoice/` service tests).

---

## 🔐 Security Notes

- Do not commit connection strings or secrets.
- Use `appsettings.Development.json` for local settings.
- Use `Secret Manager` or environment variables in production.

---

## 🚀 Deployment Notes

- Backend: host as ASP.NET Core app (IIS / Azure App Service / Docker)
- Frontend: build (`ng build --prod`) and serve static files or deploy on static hosting
- Ensure CORS is configured in `Program.cs` for frontend origin.

---

## 🤝 Contributing

1. Fork repo
2. Create branch `feature/<name>`
3. Build, test, and commit
4. Open PR against `main`

---

## 📄 License

MIT License

---

## 🙋‍♂️ Contact

Ask for help or additional integration steps (reporting, emailing, invoices PDF).

## give your suggestion

give your suggestions to improve this project features , design etc.
