# Habit Tracker API

A REST API habit and productivity tracker, using .Net 9, Entity Framwork Core, PostgresSQL and user authentication and validation 

This project is a continuation of [Task Management API](https://github.com/MohamadReza-Momeni/Task-Management-API)

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


### 3. Access the API

* Scalar Docs UI:
  [http://localhost:5250/scalar](http://localhost:5250/scalar)

* Base URL:

  ```
  http://localhost:5250
  ```

---
