# siprotec-5-test-framework
A modular C#/.NET test platform with REST-based microservices to simulate power-system faults, evaluate relay behavior, and automatically validate test outcomes. 

The system simulates SIPROTEC 5, a modern digital protection relay and automation device developed by Siemens for medium- to high-voltage power grids, focusing on overcurrent protection. It enables automated testing of relay behavior using fault injection scenarios and validates trip responses against expected timing constraints

## Workflow
Create test case → run simulation → evaluate result → store/report outcome: 
1. User creates a test case
2. User starts a test run
3. Test Management Service calls Simulation Service
4. Simulation Service returns generated signal data
5. Relay Logic Service processes the signals
6. Validation Service compares actual behavior to expected behavior
7. Results are saved
8. Final report is returned

## Architecture
### Backend
#### Microservices
The system is designed as a set of loosely coupled microservices, each responsible for a specific domain of functionality. The services communicate via RESTful APIs, allowing for scalability and maintainability. The main services include: 
1. **Test Management Service**:
This service manages:
- test case creation, retrieval, and deletion
- test run creation, retrieval, and result updates
- execution state
- links to results from other services (to be implemented)

2. **Simulation Service** (to be implemented):
This service simulates the SIPROTEC 5 power-system behavior:
- generate 3-phase current/voltage signals
- inject faults
- return measurement streams or sampled datasets

3. **Relay Logic Service** (to be implemented):
This service implements the relay's protection logic (simplified SIPROTEC-like device behavior):
- consume simulated measurements
- apply relay logic
- emit events such as: pickup, trip, no trip

4. **Validation Service** (to be implemented):
This service validates the relay's response against expected outcomes:
- compare actual results to expected results (output: pass/fail, mismatch reasons, timing deviation)

5. **Reporting Service** (to be implemented):
This service stores and exposes comprehensive test reports:
- test run results
- event logs
- measurement summaries
- validation reports

For more details on the functionality and implementation of each service, please go to the respective service documentation and API specifications in each service's README file.

#### Database
A PostgreSQL database is used as the primary database for storing test cases, test runs, 
and results. Each service manages its own database schema to ensure separation 
of concerns and data integrity.

### Frontend
A Vue 3 + TypeScript frontend for the SIPROTEC 5 Test Framework.  
It provides a simple user interface for creating test cases, starting test runs, and viewing test execution results.

The frontend follows the backend workflow of the project:

1. User creates a test case
2. User starts a test run
3. Test Management Service triggers the backend workflow
4. Simulation, relay logic, and validation are performed by backend services
5. Results are stored and returned
6. User views the result in the frontend

## Development
### How to set up a a new .NET service
1. Create a new folder for the service:
```bash
mkdir MyNewService
cd MyNewService
```
1. Create a solution file
```bash
dotnet new sln --name MyNewService
```
1. Create src and test projects folders
```bash
mkdir src tests
```
1. Add src and test projects, e.g. for an API project and test projects:
```bash
dotnet new webapi -n MyNew.Api -o src/MyNew.Api --use-controllers
dotnet new xunit -n MyNew.UnitTests  -o tests/MyNew.UnitTests
dotnet new xunit -n MyNew.IntegrationTests  -o tests/MyNew.IntegrationTests
```
1. Add projects to the solution
```bash
dotnet sln add src/MyNew.Api/MyNew.Api.csproj
dotnet sln add tests/MyNew.UnitTests/MyNew.UnitTests.csproj
dotnet sln add tests/MyNew.IntegrationTests/MyNew.IntegrationTests.csproj
```
1. Add project references as needed, e.g. for unit tests referencing the API project:
```bash
dotnet add tests/MyNew.UnitTests/MyNew.UnitTests.csproj reference src/MyNew.Api/MyNew.Api.csproj
```

## Test
### Unit tests
Run unit tests for each service using the following command in the 
respective test directory or solution directory for all services:
```bash
dotnet test
```
For more details on the test strategy, scope, and stack for each service, 
please refer to the README file in each service's test project directory.

## Tech Stack
### Backend
- **C# / .NET 10**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **PostgreSQL**
- **Docker + Docker Compose**
- **Swagger/OpenAPI for API documentation**
- **xUnit for tests**

### Frontend
- **Vue 3**
- **TypeScript**
- **Vue Router**
- **Fetch API**
