# TestManagement.Api.Tests

This project contains automated tests for the TestManagement API.

## Scope

Currently, the tests focus on controller behavior for the `TestCasesController`, including:

- retrieving all test cases
- retrieving a test case by id
- creating valid and invalid test cases
- deleting test cases

## Test stack

- **xUnit** for test execution
- **EF Core InMemory** for a lightweight test database

## Running tests

Run all tests from the solution root:

```bash
dotnet test