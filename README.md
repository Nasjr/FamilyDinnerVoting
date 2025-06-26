
# 🍽️ Family Dinner Voting API

A backend Web API that allows authenticated users to vote on what meal to eat for family dinner — built with ASP.NET Core, PostgreSQL, Entity Framework Core, and JWT authentication.

This project simulates a real-world environment by following SOLID principles, applying the Generic Repository pattern, and using CI/CD with GitHub Actions.

---

## 🛠️ Features

- 👤 **Authentication & Authorization** using ASP.NET Core Identity + JWT
- 🗳️ **Vote Sessions**:
  - Start/end a voting session
  - Assign meals to sessions
  - Cast votes and determine winners
- 🍛 **Meals**:
  - Create and list meals
- 📊 **Results**:
  - See results and winning meals per session
- 🧪 **Unit-tested business logic**
- 📦 **Generic Repository pattern** for reusable data access
- ⚙️ **SOLID principles** applied throughout services and design
- 🐳 **Docker** support for containerized builds
- 🔁 **CI/CD** via GitHub Actions: build, test, and optionally deploy on push/PR

---

## 🧩 Tech Stack

- **.NET 8 Web API**
- **Entity Framework Core**
- **PostgreSQL** (with or without Docker)
- **ASP.NET Core Identity**
- **JWT Authentication**
- **xUnit** (for unit testing)
- **Swagger** (auto API documentation)
- **GitHub Actions** (for CI/CD)
- **Docker** (optional containerized run)

---

## 🧪 CI/CD Workflow

Configured via `.github/workflows/dotnet-ci.yml`

- On every push or pull request to `main` or `dev`:
  - Restore dependencies
  - Build the app
  - Run tests
  - (Optional) Build & push Docker image to DockerHub

To enable Docker pushes:
- Add secrets `DOCKER_USERNAME` and `DOCKER_PASSWORD` (Docker Hub Access Token) in repo settings

---

## 🚀 Running the App

### 🔹 Option 1: Run Locally (with installed .NET SDK & PostgreSQL)

```bash
# 1. Restore & run
dotnet restore
dotnet ef database update
dotnet run
```

App will be available at: `https://localhost:5001/swagger`

### 🔹 Option 2: Run with Docker

```bash
# Build the container
docker build -t family-dinner-api .

# Run the container
docker run -p 8080:80 family-dinner-api
```

Access at: [http://localhost:8080/swagger](http://localhost:8080/swagger)

---

## 🔐 Authentication

- Register/Login to get a JWT
- Include the JWT token in Swagger or headers like:
  ```
  Authorization: Bearer <your_token>
  ```

---

## 📁 Project Structure

```
FamilyDinnerVotingAPI/
├── Controllers/
├── DTOs/
├── Models/
│   └── Entities/
├── Repositories/
│   ├── Interfaces/
│   └── Implementations/
├── Services/
│   ├── Interfaces/
│   └── Implementations/
├── Data/
│   └── AppDbContext.cs
├── Program.cs
├── appsettings.json
```

---

## 📜 License

MIT License – feel free to fork, modify, and build on this!

---

## 👨‍💻 Author

**Mahmoud Nasser** – [GitHub](https://github.com/mahmoudnasser2)
