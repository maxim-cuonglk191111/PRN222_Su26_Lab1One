# Product Requirements Document: Product Management System (PostgreSQL Edition)

## Product Overview

**Product Vision:** An enterprise-grade, lightweight Product Management System engineered to offer store operators a clean web interface for tracking inventory and categorizing items securely. It replaces legacy desktop frameworks with a modern, cloud-optimized multi-tier architectural pattern.

**Target Users:** - **Store Managers (Administrators):** Need comprehensive control over full catalog structures, inventory volumes, and price adjustments.

* **Store Staff (Regular Members):** Need quick read access to check stock status and update basic details without system-wide administrative access.

**Business Objectives:** - Migrate legacy desktop systems into a responsive cloud-native web format.

* Maximize deployment cost efficiency by leveraging high-performance, open-source PostgreSQL infrastructure over restrictive SQL Server free-tier limits.
* Guarantee strict system protection via decoupled tier boundaries and robust session isolation.

**Success Metrics:** - **Operational Cost Reduction:** Near-zero database maintenance costs on cloud computing tiers.

* **Data Integrity:** Zero orphaned products through strictly enforced relational foreign-key integrity constraints.
* **System Responsiveness:** Core page interactions and catalog updates completing within 200ms.

---

## User Personas

### Persona 1: Admin Alex

* **Demographics:** 34 years old, Store Inventory Manager, high technical proficiency in data administration.
* **Goals:** Maintain category hierarchies, modify stock units, delete obsolete entries, and control access permissions.
* **Pain Points:** Hard locked into a desktop workspace, unable to access operations from home, and limited by data thresholds on legacy configurations.
* **User Journey:** Logs in securely, explores the system dashboard, reviews critical low-stock alerts, adjusts unit configurations, and adds new store categories.

### Persona 2: Staff Sam

* **Demographics:** 22 years old, Front-counter Associate, baseline technical proficiency.
* **Goals:** Swiftly query product details, check units in stock for customers, and modify prices during store promotions.
* **Pain Points:** Overwhelmed by cluttered database structures and accidentally changing structural data.
* **User Journey:** Authenticates through a web window, queries a targeted keyword phrase, inspects stock depths, updates individual field entries, and logs out safely.

---

## Feature Requirements

| Feature | Description | User Stories | Priority | Acceptance Criteria | Dependencies |
| --- | --- | --- | --- | --- | --- |
| **Authentication System** | Secure email and password entry with active state tracking. | As a registered member, I want to log in using my email and password so that I can access inventory screens. | **Must** | - Redirect anonymous paths to `/Account/Login`. <br>

<br>- Session automatically times out after 20 hours of inactivity. <br>

<br>- Password entries masked in the UI. | `AccountRepository`, `Session Middleware` |
| **Category Segmentation** | View and manage distinct structural departments. | As a store manager, I want to build clean classifications so products stay grouped logically. | **Must** | - Enforce unique names. <br>

<br>- Category IDs must link perfectly to the related inventory items. | `CategoryRepository` |
| **Product Lifecycle (CRUD)** | Complete creation, display, editing, and deletion mechanics. | As an inventory specialist, I want full management control of stock rows to maintain accurate catalog entries. | **Must** | - Block blank strings on names. <br>

<br>- Restrict numeric entries to positive values. <br>

<br>- Show confirmation modal prior to removal. | `ProductRepository`, `CategoryService` |
| **Access Control (RBAC)** | Role restrictions bound to specific feature access paths. | As a floor worker, I want restricted viewing boundaries so I do not accidentally clear entire categories. | **Should** | - Hide structural controls from standard roles. <br>

<br>- Block dangerous REST routing actions unless an executive account key matches. | `Authentication System` |

---

## User Flows

### Flow 1: Secure Authentication & Dashboard Access

1. The user navigates to the core portal landing route.
2. The middleware detects an unauthenticated session state and triggers an internal route correction to `/Account/Login`.
3. The member enters an email address and a string password, then clicks **Login**.
* *Alternative Path:* If database values do not match the inputs, the application displays an immediate, user-friendly error string: `"Invalid username or password."`
* *Error State:* If the connection to PostgreSQL drops, the engine returns an explicit database unavailable warning block.


4. The system updates the session cache with parameters for `UserId` and `Username`, then transfers routing control to `/Products/Index/`.

### Flow 2: Complete New Product Catalog Insertion

1. An authorized user navigates to `/Products/Create`.
2. The interface uses a clean view lookup loop to present an explicit selection dropdown containing every active catalog category option.
3. The administrator fills out fields for product name, units in stock, and unit pricing, then presses **Save**.
* *Alternative Path:* If formatting constraints fail (e.g., negative prices or empty strings), validation alerts fire inline without breaking user form states.


4. The application inserts the entry into PostgreSQL and changes the view back to the index grid view layout.

---

## Non-Functional Requirements

### Performance

* **Load Time:** Core product list calculations and UI draws must process within 300ms under standard loads.
* **Concurrent Users:** Architecture safely accommodates up to 250 active open connection pools via optimized infrastructure configurations.
* **Response Time:** API state changes must respond in less than 150ms.

### Security

* **Authentication:** Enforced throughout all secure areas via `HttpContext.Session` checks.
* **Authorization:** Int fields handle specific system permissions, separating regular views from administrative configurations.
* **Data Protection:** Connection strings must be isolated outside source control configurations using secure production container env keys.

### Compatibility

* **Devices:** Optimized across all target workstation form factors, desktop viewboxes, and warehouse tablet layouts.
* **Browsers:** Confirmed structural consistency across Chrome, Edge, Safari, and Firefox.
* **Screen Sizes:** Fluid layout support covering responsive transitions from 768px up to 1920px width brackets.

### Accessibility

* **Compliance Level:** Form controls and core layouts adhere closely to general WCAG 2.1 AA accessibility principles.
* **Specific Requirements:** All form text boxes match target context tags, structural tables utilize clear table header declarations, and color changes use strong contrast margins.

---

## Technical Specifications

### File Structure

```text
ProductManagementASPNETCoreMVC/
│
├── BusinessObjects/
│   ├── Models/
│   │   ├── AccountMember.cs
│   │   ├── Category.cs
│   │   └── Product.cs
│   ├── MyStoreContext.cs
│   └── BusinessObjects.csproj
│
├── DataAccessObjects/
│   ├── AccountDAO.cs
│   ├── CategoryDAO.cs
│   ├── ProductDAO.cs
│   └── DataAccessObjects.csproj
│
├── Repositories/
│   ├── IAccountRepository.cs
│   ├── ICategoryRepository.cs
│   ├── IProductRepository.cs
│   ├── AccountRepository.cs
│   ├── CategoryRepository.cs
│   ├── ProductRepository.cs
│   └── Repositories.csproj
│
├── Services/
│   ├── IAccountService.cs
│   ├── ICategoryService.cs
│   ├── IProductService.cs
│   ├── AccountService.cs
│   ├── CategoryService.cs
│   ├── ProductService.cs
│   └── Services.csproj
│
└── ProductManagementMVC/
    ├── Controllers/
    │   ├── AccountController.cs
    │   └── ProductsController.cs
    ├── Views/
    │   ├── Account/
    │   │   └── Login.cshtml
    │   └── Products/
    │       ├── Create.cshtml
    │       ├── Delete.cshtml
    │       ├── Details.cshtml
    │       ├── Edit.cshtml
    │       └── Index.cshtml
    ├── appsettings.json
    ├── Program.csproj
    └── Program.cs

```

### Frontend

* **Technology Stack:** ASP.NET Core MVC utilizing Razor Page View engines.
* **Design System:** Standard Bootstrap 5 setup styled for modern responsive application layouts.
* **Responsive Design:** Dynamic tables that shift naturally into scannable cards on narrow mobile breakpoints.

### Backend

* **Technology Stack:** .NET 8 Framework runtimes.
* **API Requirements:** Enforces a clean server-side repository architecture handling standard transactional web states.
* **Database:** PostgreSQL. Tables are structured to map seamlessly to entity definitions via modern ORM scaffolding mechanics:

```sql
-- PostgreSQL Scaffolding Commands & Compatible DDL Scripts

CREATE TABLE "AccountMember" (
    "MemberID" VARCHAR(20) PRIMARY KEY,
    "MemberPassword" VARCHAR(80) NOT NULL,
    "FullName" VARCHAR(80) NOT NULL,
    "EmailAddress" VARCHAR(100) NOT NULL,
    "MemberRole" INT NOT NULL
);

CREATE TABLE "Categories" (
    "CategoryID" SERIAL PRIMARY KEY,
    "CategoryName" VARCHAR(15) NOT NULL
);

CREATE TABLE "Products" (
    "ProductID" SERIAL PRIMARY KEY,
    "ProductName" VARCHAR(40) NOT NULL,
    "CategoryID" INT NOT NULL,
    "UnitsInStock" SMALLINT NULL,
    "UnitPrice" NUMERIC(19, 4) NULL,
    CONSTRAINT "FK_Products_Categories" FOREIGN KEY ("CategoryID") 
        REFERENCES "Categories" ("CategoryID") ON DELETE CASCADE
);

```

### Infrastructure

* **Hosting:** App Service runtime engine paired with a managed cloud PostgreSQL database cluster tier.
* **Scaling:** Vertical resource scaling configurations adapted to match ongoing store usage increments.
* **CI/CD:** Automated builds compile code assets, execute tests, and push deployment layers via version control delivery flows.

---

## Analytics & Monitoring

* **Key Metrics:** Monitors authentication error rates, catalog modification speeds, and active concurrent storage connections.
* **Events:** Tracks security anomalies, standard inventory record removals, and system configuration exceptions.
* **Dashboards:** Operational summaries displaying application processing metrics and database cluster throughput patterns.
* **Alerting:** Real-time logging notices capture persistent query execution drops or systemic data connection issues.

---

## Release Planning

### MVP (v1.0)

* **Features:** Complete cross-tier architecture supporting session authentication and core transactional CRUD workflows.
* **Timeline:** 3-week engineering window.
* **Success Criteria:** Zero deployment integration issues using PostgreSQL database layers on cloud tiers.

### Future Releases

* **v1.1:** Adds internal export tools to process stock datasets into structured files.
* **v1.2:** Supports configurable low-stock triggers that send automated email notices to supervisors.
* **v2.0:** Integrates real-time barcode scanning capabilities for field-level mobile inventory management.

---

## Open Questions & Assumptions

* **Question 1:** Are password values hashed prior to comparison, or does legacy migration require plaintext comparisons for initial validation cycles? 
Answer: Passwords are hashed prior to comparison
* **Question 2:** Will store managers require deep structural category adjustment capabilities from mobile views, or are operations restricted to desktop environments? 
Answer: Store managers require deep structural category adjustment capabilities from mobile views, so operations are not restricted to desktop environments.
* **Assumption 1:** Session values are cached securely within single instance memory pools without requiring isolated distributed cache networks.
* **Assumption 2:** The underlying cloud server infrastructure supports native PostgreSQL drivers without additional network proxy configurations.

---

## Appendix

### Glossary

* **Multi-Tier Architecture:** An architectural pattern that enforces separate logic layers for data structures, business logic, processing components, and rendering interfaces.
* **ORM (Object-Relational Mapping):** Code components that translate underlying database tables into native development code classes.
* **RBAC (Role-Based Access Control):** Security enforcement schemas that restrict feature capabilities using profile definitions.