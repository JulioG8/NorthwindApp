# Northwind Application - Documentacion Tecnica 

## Prueba Tecnica - Desarrollador .NET

---

## Tabla de Contenidos

1. [Descripcion General](#descripcion-general)
2. [Arquitectura del Sistema](#arquitectura-del-sistema)
3. [Principios SOLID Aplicados](#principios-solid-aplicados)
4. [Estructura del Proyecto](#estructura-del-proyecto)
5. [Tecnologias Utilizadas](#tecnologias-utilizadas)
6. [Backend - API REST](#backend---api-rest)
7. [Frontend - Aplicacion Web](#frontend---aplicacion-web)
8. [Base de Datos](#base-de-datos)
9. [Instrucciones de Configuracion](#instrucciones-de-configuracion)
10. [Endpoints de la API](#endpoints-de-la-api)
11. [Flujo de Datos](#flujo-de-datos)

---

## Descripcion General

Esta aplicacion implementa los requerimientos de la prueba tecnica utilizando la base de datos Northwind. El sistema consta de:

### Backend (ASP.NET Core Web API)
- API REST con arquitectura limpia
- Entity Framework Core para acceso a datos
- Middleware para tracking de requests

### Frontend (AngularJS + Kendo UI + Bootstrap)
- Vista de busqueda de clientes por pais
- Vista de ordenes de un cliente
- Grids interactivos con Kendo UI

### Funcionalidades Implementadas

| # | Requerimiento | Implementacion |
|---|---------------|----------------|
| 1 | CustomersByCountry | Vista con busqueda por pais, grid con Customer ID (link), Company Name, Contact Name, Phone, Fax |
| 2 | CustomerOrdersInformation | Vista con info del cliente y grid de ordenes (Order ID, Customer ID, Order Date, Shipped Date) |
| 3 | Tabla WebTracker | Tabla con Id, URLRequest, SourceIp, TimeOfAction |
| 4 | Logging automatico | Middleware que registra todas las acciones en WebTracker |

**Nota:** Se utilizo Web API en lugar de WCF para compatibilidad con .NET 8+ y macOS.

---

## Arquitectura del Sistema

Se implemento **Clean Architecture** (Arquitectura Limpia) con separacion clara entre frontend y backend.

```
┌─────────────────────────────────────────────────────────────────┐
│                         FRONTEND                                 │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │  AngularJS 1.8.2 + Kendo UI + Bootstrap 4.6.2             │  │
│  │  - index.html (CustomersByCountry)                        │  │
│  │  - orders.html (CustomerOrdersInformation)                │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                              │ HTTP/REST
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    PRESENTACION (API Layer)                      │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │  ASP.NET Core Web API (Puerto 8082)                       │  │
│  │  - CustomersController                                    │  │
│  │  - OrdersController                                       │  │
│  │  - WebTrackerMiddleware                                   │  │
│  │  - Static Files (wwwroot)                                 │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    CAPA APPLICATION                              │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │  Services: CustomerService, OrderService                  │  │
│  │  DTOs: CustomerDto, OrderDto                              │  │
│  │  Interfaces: ICustomerService, IOrderService              │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    CAPA DOMAIN                                   │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │  Entities: Customer, Order, WebTracker                    │  │
│  │  Interfaces: ICustomerRepository, IOrderRepository        │  │
│  │              IWebTrackerRepository                        │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    CAPA INFRASTRUCTURE                           │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │  Data: NorthwindDbContext (Entity Framework Core)         │  │
│  │  Repositories: CustomerRepository, OrderRepository        │  │
│  │                WebTrackerRepository                       │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    BASE DE DATOS                                 │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │  SQL Server / Northwind Database                          │  │
│  │  - Customers, Orders (tablas existentes)                  │  │
│  │  - WebTracker (tabla nueva para logging)                  │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
```

---

## Principios SOLID Aplicados

### 1. Single Responsibility Principle (SRP)
Cada clase tiene una unica responsabilidad:
- `CustomerService` - Solo maneja logica de negocio de clientes
- `OrderService` - Solo maneja logica de negocio de ordenes
- `CustomerRepository` - Solo acceso a datos de clientes
- `WebTrackerMiddleware` - Solo logging de requests

### 2. Open/Closed Principle (OCP)
Las clases estan abiertas para extension pero cerradas para modificacion:
- Los servicios pueden extenderse mediante herencia
- Nuevos repositorios pueden agregarse sin modificar los existentes

### 3. Liskov Substitution Principle (LSP)
Las implementaciones concretas pueden sustituir a sus interfaces:
- `CustomerRepository` implementa `ICustomerRepository`
- `CustomerService` implementa `ICustomerService`

### 4. Interface Segregation Principle (ISP)
Interfaces pequenas y especificas:
- `ICustomerRepository` - Solo operaciones de Customer
- `IOrderRepository` - Solo operaciones de Order
- `IWebTrackerRepository` - Solo operaciones de logging

### 5. Dependency Inversion Principle (DIP)
Dependencia de abstracciones, no de implementaciones concretas:
- Los servicios dependen de interfaces de repositorios
- Los controladores dependen de interfaces de servicios
- Dependency Injection configurado en Program.cs

---

## Estructura del Proyecto

```
NorthwindApp/
├── NorthwindApp.sln                    # Solucion de Visual Studio
├── DOCUMENTACION.md                    # Este archivo
├── docker-compose.yml                  # Configuracion de Docker
├── Database/
│   ├── 01-CreateNorthwindDatabase.sql  # Crear BD y tablas
│   ├── 02-InsertSampleData.sql         # Datos de ejemplo
│   └── CreateWebTrackerTable.sql       # Script WebTracker
│
└── src/
    ├── NorthwindApp.Domain/            # Capa de Dominio
    │   ├── Entities/
    │   │   ├── Customer.cs
    │   │   ├── Order.cs
    │   │   └── WebTracker.cs
    │   └── Interfaces/
    │       ├── ICustomerRepository.cs
    │       ├── IOrderRepository.cs
    │       └── IWebTrackerRepository.cs
    │
    ├── NorthwindApp.Application/       # Capa de Aplicacion
    │   ├── DTOs/
    │   │   ├── CustomerDto.cs
    │   │   └── OrderDto.cs
    │   ├── Interfaces/
    │   │   ├── ICustomerService.cs
    │   │   └── IOrderService.cs
    │   └── Services/
    │       ├── CustomerService.cs
    │       └── OrderService.cs
    │
    ├── NorthwindApp.Infrastructure/    # Capa de Infraestructura
    │   ├── Data/
    │   │   └── NorthwindDbContext.cs
    │   └── Repositories/
    │       ├── CustomerRepository.cs
    │       ├── OrderRepository.cs
    │       └── WebTrackerRepository.cs
    │
    └── NorthwindApp.Api/               # Capa de Presentacion
        ├── Controllers/
        │   ├── CustomersController.cs
        │   └── OrdersController.cs
        ├── Middleware/
        │   └── WebTrackerMiddleware.cs
        ├── Properties/
        │   └── launchSettings.json     # Config puerto 8082
        ├── wwwroot/                    # Frontend (archivos estaticos)
        │   ├── index.html              # CustomersByCountry view
        │   └── orders.html             # CustomerOrdersInformation view
        ├── Program.cs
        ├── appsettings.json
        └── appsettings.Development.json
```

---

## Tecnologias Utilizadas

### Backend

| Tecnologia | Version | Uso |
|------------|---------|-----|
| .NET | 10.0 | Framework base (compatible 8.0, 9.0) |
| ASP.NET Core | 10.0 | Web API |
| Entity Framework Core | 9.0.0 | ORM |
| SQL Server | 2019+ | Base de datos |
| Swashbuckle | 6.6.2 | Swagger/OpenAPI |

### Frontend

| Tecnologia | Version | Uso |
|------------|---------|-----|
| AngularJS | 1.8.2 | Framework JavaScript (eventos, binding) |
| Kendo UI | 2023.1.117 | DataSource y Grid |
| Bootstrap | 4.6.2 | Estilos CSS |
| jQuery | 3.6.0 | Requerido por Kendo UI |

---

## Backend - API REST

### Controllers

#### CustomersController
```
Route: api/customers
```

| Endpoint | Metodo | Descripcion |
|----------|--------|-------------|
| `/api/customers/country/{country}` | GET | Clientes por pais (ordenados por ContactName) |
| `/api/customers/{customerId}` | GET | Cliente por ID |

#### OrdersController
```
Route: api/orders
```

| Endpoint | Metodo | Descripcion |
|----------|--------|-------------|
| `/api/orders/customer/{customerId}` | GET | Ordenes de un cliente (ordenadas por ShippedDate) |

### Middleware

#### WebTrackerMiddleware
- Intercepta todas las peticiones HTTP
- Registra: URL, IP del cliente, Timestamp (UTC)
- Excluye: Swagger UI, archivos estaticos (.css, .js, .png, etc.)
- Maneja proxies con header X-Forwarded-For

### DTOs

#### CustomerDto
```csharp
public record CustomerDto
{
    public string CustomerId { get; init; }
    public string CompanyName { get; init; }
    public string? ContactName { get; init; }
    public string? Phone { get; init; }
    public string? Fax { get; init; }
    public string? Country { get; init; }
}
```

#### OrderDto
```csharp
public record OrderDto
{
    public int OrderId { get; init; }
    public string? CustomerId { get; init; }
    public DateTime? OrderDate { get; init; }
    public DateTime? ShippedDate { get; init; }
}
```

---

## Frontend - Aplicacion Web

### Vista 1: CustomersByCountry (index.html)

**URL:** `http://localhost:8082/` o `http://localhost:8082/index.html`

**Funcionalidades:**
- Campo de texto para ingresar el pais
- Boton "Search" para buscar (Angular ng-submit)
- Spinner de carga mientras busca
- Mensaje "No customers found" si no hay resultados
- Kendo Grid con los resultados

**Columnas del Grid:**
| Columna | Descripcion |
|---------|-------------|
| Customer ID | Link clickeable a la pagina de ordenes |
| Company Name | Nombre de la empresa |
| Contact Name | Nombre del contacto (ordenado ASC) |
| Phone Number | Telefono |
| Fax Number | Fax |

**Caracteristicas del Grid:**
- Sortable (ordenable por columnas)
- Filterable (filtros por columna)
- Pageable (5, 10, 20, 50 items por pagina)

### Vista 2: CustomerOrdersInformation (orders.html)

**URL:** `http://localhost:8082/orders.html?customerId={id}&country={country}`

**Funcionalidades:**
- Breadcrumb de navegacion
- Card con informacion del cliente (ID, Company, Contact, Country)
- Kendo Grid con las ordenes
- Boton "Back to Customers" que mantiene el pais buscado

**Columnas del Grid:**
| Columna | Descripcion |
|---------|-------------|
| Order ID | Identificador de la orden |
| Customer ID | ID del cliente |
| Order Date | Fecha de orden (yyyy-MM-dd) |
| Shipped Date | Fecha de envio o "Not Shipped" |

**Caracteristicas del Grid:**
- Ordenado por Shipped Date (ASC)
- Sortable, Filterable, Pageable

### Comunicacion Frontend-Backend

```javascript
// Configuracion de API
var API_BASE_URL = 'http://localhost:8082/api';

// Kendo DataSource para llamadas AJAX
var dataSource = new kendo.data.DataSource({
    transport: {
        read: {
            url: API_BASE_URL + '/customers/country/' + country,
            dataType: 'json'
        }
    },
    schema: {
        model: {
            id: 'customerId',
            fields: {
                customerId: { type: 'string' },
                companyName: { type: 'string' },
                contactName: { type: 'string' },
                phone: { type: 'string' },
                fax: { type: 'string' }
            }
        }
    }
});
```

---

## Base de Datos

### Tablas Utilizadas

#### Customers (existente)
| Columna | Tipo | Descripcion |
|---------|------|-------------|
| CustomerID | NCHAR(5) | PK |
| CompanyName | NVARCHAR(40) | Nombre empresa |
| ContactName | NVARCHAR(30) | Nombre contacto |
| ContactTitle | NVARCHAR(30) | Titulo |
| Address | NVARCHAR(60) | Direccion |
| City | NVARCHAR(15) | Ciudad |
| Region | NVARCHAR(15) | Region |
| PostalCode | NVARCHAR(10) | Codigo postal |
| Country | NVARCHAR(15) | Pais |
| Phone | NVARCHAR(24) | Telefono |
| Fax | NVARCHAR(24) | Fax |

#### Orders (existente)
| Columna | Tipo | Descripcion |
|---------|------|-------------|
| OrderID | INT | PK (Identity) |
| CustomerID | NCHAR(5) | FK a Customers |
| EmployeeID | INT | ID empleado |
| OrderDate | DATETIME | Fecha orden |
| RequiredDate | DATETIME | Fecha requerida |
| ShippedDate | DATETIME | Fecha envio |
| ShipVia | INT | Metodo envio |
| Freight | MONEY | Costo envio |
| ShipName | NVARCHAR(40) | Nombre envio |
| ShipAddress | NVARCHAR(60) | Direccion envio |
| ShipCity | NVARCHAR(15) | Ciudad envio |
| ShipRegion | NVARCHAR(15) | Region envio |
| ShipPostalCode | NVARCHAR(10) | CP envio |
| ShipCountry | NVARCHAR(15) | Pais envio |

#### WebTracker (nueva)
| Columna | Tipo | Descripcion |
|---------|------|-------------|
| Id | INT | PK (Identity) |
| URLRequest | NVARCHAR(500) | Metodo + Path + QueryString |
| SourceIp | NVARCHAR(50) | IP del cliente |
| TimeOfAction | DATETIME | Timestamp UTC |

**Indices en WebTracker:**
- `IX_WebTracker_TimeOfAction` (DESC) - Consultas por fecha
- `IX_WebTracker_SourceIp` - Consultas por IP

---

## Instrucciones de Configuracion

### Pre-requisitos

- .NET SDK 8.0 o superior
- SQL Server (Express, Developer, o Docker)
- Navegador web moderno

### Opcion 1: SQL Server Local

1. **Crear la base de datos:**
```bash
# Ejecutar scripts en SQL Server Management Studio o Azure Data Studio
Database/01-CreateNorthwindDatabase.sql
Database/02-InsertSampleData.sql
```

2. **Configurar connection string** en `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "NorthwindConnection": "Server=localhost;Database=Northwind;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### Opcion 2: Docker (SQL Server)

1. **Iniciar SQL Server con Docker:**
```bash
cd NorthwindApp
docker-compose up -d
```

2. **Configurar connection string** en `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "NorthwindConnection": "Server=localhost,1433;Database=Northwind;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"
  }
}
```

3. **Ejecutar scripts SQL** conectandose al contenedor.

### Ejecutar la Aplicacion

```bash
# Desde la raiz del proyecto
cd NorthwindApp

# Restaurar paquetes
dotnet restore

# Compilar
dotnet build

# Ejecutar
dotnet run --project src/NorthwindApp.Api
```

### URLs de la Aplicacion

| URL | Descripcion |
|-----|-------------|
| `http://localhost:8082/` | Frontend - Pagina principal |
| `http://localhost:8082/swagger` | Swagger UI - Documentacion API |
| `http://localhost:8082/api/customers/country/USA` | API - Clientes de USA |

---

## Endpoints de la API

### Ejemplos con curl

```bash
# Obtener clientes de UK
curl http://localhost:8082/api/customers/country/UK

# Obtener cliente AROUT
curl http://localhost:8082/api/customers/AROUT

# Obtener ordenes del cliente AROUT
curl http://localhost:8082/api/orders/customer/AROUT
```

### Respuesta de ejemplo - Customers

```json
[
  {
    "customerId": "AROUT",
    "companyName": "Around the Horn",
    "contactName": "Thomas Hardy",
    "phone": "(171) 555-7788",
    "fax": "(171) 555-6750",
    "country": "UK"
  },
  {
    "customerId": "BSBEV",
    "companyName": "B's Beverages",
    "contactName": "Victoria Ashworth",
    "phone": "(171) 555-1212",
    "fax": null,
    "country": "UK"
  }
]
```

### Respuesta de ejemplo - Orders

```json
[
  {
    "orderId": 10355,
    "customerId": "AROUT",
    "orderDate": "1996-11-15T00:00:00",
    "shippedDate": "1996-11-20T00:00:00"
  },
  {
    "orderId": 10383,
    "customerId": "AROUT",
    "orderDate": "1996-12-16T00:00:00",
    "shippedDate": "1996-12-18T00:00:00"
  }
]
```

---

## Flujo de Datos

### Flujo Completo: Busqueda de Clientes

```
1. Usuario abre http://localhost:8082/
2. Usuario ingresa "UK" y presiona "Search"
3. AngularJS captura el evento ng-submit
4. Kendo DataSource hace GET /api/customers/country/UK
5. WebTrackerMiddleware registra la peticion en BD
6. CustomersController recibe la peticion
7. CustomerService consulta via CustomerRepository
8. EF Core ejecuta query: SELECT * FROM Customers WHERE Country = 'UK'
9. Resultados se ordenan por ContactName
10. CustomerService mapea a CustomerDto
11. Controller retorna JSON 200 OK
12. Kendo Grid renderiza los datos
13. Usuario hace clic en "AROUT"
14. Navegador va a orders.html?customerId=AROUT&country=UK
```

### Flujo Completo: Ver Ordenes

```
1. orders.html carga con parametros customerId=AROUT, country=UK
2. AngularJS lee parametros de URL
3. $http.get /api/customers/AROUT (info del cliente)
4. Kendo DataSource hace GET /api/orders/customer/AROUT
5. WebTrackerMiddleware registra ambas peticiones
6. OrderService consulta via OrderRepository
7. Resultados se ordenan por ShippedDate
8. Kendo Grid renderiza las ordenes
9. Usuario presiona "Back to Customers"
10. Navegador va a index.html?country=UK
11. AngularJS detecta parametro country y ejecuta busqueda automatica
```

---

## Notas Adicionales

### Compatibilidad
- Backend compatible con .NET 8, 9 y 10
- Funciona en Windows, macOS y Linux
- Frontend funciona en cualquier navegador moderno

### CORS
- Configurado para permitir cualquier origen (desarrollo)
- Ajustar en produccion para mayor seguridad

### WebTracker - Datos Registrados
Ejemplo de registro:
```
Id: 1
URLRequest: "GET /api/customers/country/UK"
SourceIp: "::1"
TimeOfAction: "2026-01-30 16:45:32.123"
```

### Mantenimiento del Estado
- El pais buscado se pasa como parametro en la URL
- Al regresar de orders.html, la busqueda se ejecuta automaticamente

---

**Autor:** Prueba tecnica para posicion .NET Developer
**Fecha:** Enero 2026
**Framework:** .NET 10.0 (compatible con .NET 8.0+)
**Puerto:** 8082
