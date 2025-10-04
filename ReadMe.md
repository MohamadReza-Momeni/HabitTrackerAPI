# Task Management API

A simple but complete Task Management REST API built with .NET 9, Entity Framework Core, and PostgreSQL, packaged in Docker for easy local development.  
Includes Scalar API documentation for frontend integration and developer-friendly CORS configuration.

---

## Features

- CRUD operations for tasks (create, read, update, delete)  
- Task priority with enums (Low, Medium, High)  
- Persistent storage using PostgreSQL & Docker volumes  
- Auto migrations on startup  
- CORS configured for localhost (any port) — ideal for frontend development  
- Scalar API documentation at `/scalar`  
- Ready-to-run with one command via Docker Compose

---

## Tech Stack

- .NET 9 (ASP.NET Core Web API)
- Entity Framework Core 9
- PostgreSQL 16
- Scalar (OpenAPI documentation UI)
- Docker & Docker Compose

---

## Getting Started (Local Development)

### 1. Install Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)  
- Optional: [Postman](https://www.postman.com/) or `curl` for API testing

No .NET SDK required — everything runs in Docker.

---

### 2. Run the API

```bash
docker-compose up --build
````

This will:

* Build the .NET 9 API Docker image
* Spin up a PostgreSQL database
* Apply EF Core migrations automatically
* Start the API on port 5250

Once it’s up, you should see logs like:

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://0.0.0.0:8080
```

---

### 3. Access the API

* Scalar Docs UI:
  [http://localhost:5250/scalar](http://localhost:5250/scalar)

* Base URL:

  ```
  http://localhost:5250
  ```

---

## Example Endpoints

### Create a Task

`POST /api/tasks`

```json
{
  "title": "Write API README",
  "description": "Complete documentation for Task Management API",
  "priority": "High",
  "dueDate": "2025-12-31T00:00:00Z"
}
```

### Get All Tasks

`GET /api/tasks`

### Update a Task

`PUT /api/tasks/{id}`

### Delete a Task

`DELETE /api/tasks/{id}`

---

## CORS Policy

CORS is configured to allow any localhost or 127.0.0.1 origin, so frontend developers can work without CORS issues from React, Vue, Angular, etc.

No HTTPS is required during local development.

---

## Database Details

| Setting  | Value     |
| -------- | --------- |
| Host     | localhost |
| Port     | 5432      |
| User     | postgres  |
| Password | postgres  |
| Database | taskdb    |

You can connect using TablePlus, DBeaver, pgAdmin, or psql if needed.

Data is persistent thanks to Docker volumes — stopping and restarting containers will not wipe your database.

---

## Rebuilding & Resetting

If you make model changes or want a clean DB:

```bash
# Rebuild containers
docker-compose down
docker-compose up --build
```

To wipe all data (including the database):

```bash
docker-compose down -v
docker-compose up --build
```

---

## Frontend Integration

Frontend developers can:

* Run the backend locally via Docker
* Use `http://localhost:5250` as the API base URL
* Use the `/scalar` page to explore endpoints, schemas, and example requests

CORS is already configured — no extra proxy setup is needed.

---

## Project Structure (Backend)

```
TaskManagementAPI/
├── Controllers/
│   └── TasksController.cs
├── Data/
│   └── TaskDbContext.cs
├── DTOs/
│   └── CreateTaskRequest.cs, UpdateTaskRequest.cs, TaskResponse.cs
├── Models/
│   └── TaskItem.cs
├── Migrations/
├── Program.cs
├── Dockerfile
├── docker-compose.yml
└── README.md
```

---

## Future Improvements

* Authentication & Authorization (e.g., JWT)
* Filtering, searching, and pagination
* Background jobs / scheduled reminders
* Tests (unit & integration)
* CI/CD pipeline

---

## Author

Mohamad Reza Momeni Yazdi
.NET Developer

---
