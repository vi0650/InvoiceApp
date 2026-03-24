# 📦 Bill Manager System

A full-stack web application for managing bills with a **Super Admin / Admin architecture**. Built using **Angular (Nebular UI)** for the frontend and **.NET Core Web API** with **SQL Server** for the backend.

---

## 🚀 Features

### 🔐 Authentication

* Secure login with JWT authentication
* Role-based access (Super Admin / Admin)

### 👑 Super Admin

* Create and manage Admin users
* Automatically generate Admin dashboards
* Centralized control over the system

### 🧑‍💼 Admin

* Login using provided credentials
* Manage bills (CRUD operations)
* Dashboard with bill-related data

### 📊 Bills Management

* Create, update, delete, and view bills
* Organized data handling with SQL Server

---

## 🛠️ Tech Stack

### Frontend

* Angular
* Nebular UI
* RxJS
* Angular Material (optional modules)

### Backend

* .NET Core Web API
* Entity Framework Core
* SQL Server

---

## ⚙️ Project Structure

```
BillManager/
│
├── frontend/         # Angular App
│   ├── src/
│   └── ...
│
├── backend/          # .NET Core API
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   └── ...
│
└── README.md
```

---

## 🧑‍💻 Setup Instructions

### 🔹 1. Clone Repository

```bash
git clone https://github.com/your-username/bill-manager.git
cd bill-manager
```

---

### 🔹 2. Backend Setup (.NET)

```bash
cd backend
dotnet restore
dotnet build
dotnet run
```

* Update `appsettings.json` with your SQL Server connection string
* Apply migrations (if needed):

```bash
dotnet ef database update
```

---

### 🔹 3. Frontend Setup (Angular)

```bash
cd frontend
npm install
ng serve
```

* App will run on: `http://localhost:4200`

---

## 🔗 API Configuration

Update environment file in Angular:

```ts
export const environment = {
  apiUrl: 'http://localhost:5065/api'
};
```

---

## 🔒 Environment Variables

Do NOT commit sensitive data. Use:

* `appsettings.Development.json` for local config
* Secret managers for production

---

## 📁 .gitignore Notes

Make sure to ignore:

```
**/bin/
**/obj/
**/node_modules/
**/dist/
**/Properties/launchSettings.json
```

---

## 🧪 Testing

* Backend: Use Swagger (`/swagger`)
* Frontend: Angular testing tools or manual UI testing

---

## 🚧 Future Improvements

* Role-based permissions (fine-grained)
* Reports & analytics dashboard
* PDF invoice generation
* Email notifications
* Multi-tenant architecture

---

## 🤝 Contributing

1. Fork the repo
2. Create a new branch
3. Make your changes
4. Submit a pull request

---

## 📄 License

This project is open-source and available under the MIT License.

---

## 💡 Author

Developed with focus on scalable architecture and clean code practices.

---

🔥 If you want, I can customize this README specifically for your deployment (IIS + network IP + production setup).
