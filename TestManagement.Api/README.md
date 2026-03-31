# Test Management Service
The Test Management Service is the coordinator of the whole framework. It manages:
- test case definitions
- test run creation
- execution state
- links to results from other services

## Getting Started
### Prerequisites
- .NET 10 SDK
- Docker

### Database Setup 
- EF Core CLI tools: Install the EF Core CLI tools globally if you haven't already:
```bash
dotnet tool install --global dotnet-ef
```
- Start the PostgreSQL database:
```bash
docker compose up -d
```
- Apply database migrations to create the necessary tables:
```bash
# Create the initial migration and update the database
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Running the API 
- Start the API:
```bash
dotnet run dev
```

## Key Responsibilities
- create a test case
- list test cases
- get one test case
- start a test run for a given test case
- get test run status
- store test run results

## Data Models
- TestCase: represents the definition of a test case
- TestRun: represents an execution of a test case

## API Endpoints
Test cases:
- POST /api/testcases: create a new test case
- GET /api/testcases: list all test cases
- GET /api/testcases/{id}: get details of a specific test case
- Put /api/testcases/{id}: start a test run for the given test case
- DELETE /api/testcases/{id}: delete a test case

Test runs:
- POST /api/testruns: create a new test run
- GET /api/testruns: list all test runs
- GET /api/testruns/{id}: get details of a specific test run
- PATCH /api/testruns/{id}/status: update test run status
- PATCH /api/testruns/{id}/result: update test run results

## Techstack
- ASP.NET Core Web API
- PostgreSQL
- EF Core
- Swagger