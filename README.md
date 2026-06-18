# PRN222 Lab 1: Product Management MVC

This is a robust, cloud-ready N-Tier ASP.NET Core MVC application designed to manage product inventory and categories. It serves as Lab 1 of the PRN222 course.

## Architecture

The application is structured into the following independent layers:
- **BusinessObjects:** Contains Entity Framework Core Models and the `DbContext`.
- **DataAccessObjects (DAO):** Handles all asynchronous direct database operations.
- **Repositories:** Interfaces and implementations mapping to DAOs.
- **Services:** Business logic layer that orchestrates Repositories.
- **ProductManagementMVC:** The presentation layer using ASP.NET Core MVC (Controllers & Views).

## Database Configuration

This project expects an SQL Server instance. 
To accommodate shared hosting environments (such as a single MonsterASP.NET database), all tables are generated under the `[lab1]` schema.

### Setup Instructions
1. Run the database setup script located at `doc/db_lab01.sql` on your SQL Server instance.
2. Update the `ConnectionStrings:DefaultConnection` in `ProductManagementMVC/appsettings.json` (or via environment variables) to point to your database.
3. Build and run the project:
   ```bash
   dotnet restore
   dotnet build
   dotnet run --project ProductManagementMVC
   ```

## Key Features
- **Session-Based Authentication:** Employs `HttpContext.Session` to handle access control.
- **Async/Await Backend:** Fully optimized backend layers utilizing `Microsoft.EntityFrameworkCore` asynchronous operations to prevent thread blocking.
- **Role-Based Access Control:** Differentiates UI capabilities based on Administrator (Role 1) and Staff (Role 2) privileges.

## Cloud Deployment (Render & MonsterASP)

This project is fully containerized via Docker and optimized for free-tier cloud deployment.

### 1. Database Setup (MonsterASP.NET)
1. Go to **MonsterASP.NET** and create a free MS SQL Server database.
2. Connect to your remote database using SSMS (SQL Server Management Studio).
3. Execute the `doc/db_lab01.sql` script provided in the `doc` folder.
   - *This automatically isolates your tables inside the `[lab1]` schema, allowing multiple projects to share the single 1GB free tier without data collisions.*
4. Copy your Database Connection String provided by MonsterASP.

### 2. Web Application Setup (Render.com)
1. Push this Lab 1 MVC repository to GitHub. 
   - *(Note: Ensure the `Dockerfile` is present at the root of the repository being deployed).*
2. Log into **Render.com** and click **New+ -> Web Service**.
3. Connect your GitHub repository.
4. Render will automatically detect the `Dockerfile` and select the **Docker** runtime.
5. Scroll down to the **Environment Variables** section and add:
   - **Key:** `ConnectionStrings__DefaultConnection`
   - **Value:** `Server=...;Database=...;User Id=...;Password=...;TrustServerCertificate=True;`
   - *Note: You MUST append `TrustServerCertificate=True;` to bypass SSL certificate validation errors between Render's Linux containers and MonsterASP.*

6. Click **Create Web Service**. Render will build the Docker container and deploy your Lab 1 MVC application.
