# siprotec-5-test-framework
A modular C#/.NET test platform with REST-based microservices to simulate power-system faults, evaluate relay behavior, and automatically validate test outcomes. 

The system simulates a SIPROTEC 5 protection relay focusing on overcurrent protection. It enables automated testing of relay behavior using fault injection scenarios and validates trip responses against expected timing constraints

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
The system is designed as a set of loosely coupled microservices, each responsible for a specific domain of functionality. The services communicate via RESTful APIs, allowing for scalability and maintainability. The main services include: 
1. **Test Management Service**:
This service manages:
- test case definitions
- test suites
- execution requests
- metadata

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
- C# / .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Docker
- Swagger/OpenAPI for API documentation
- xUnit for unit testing

