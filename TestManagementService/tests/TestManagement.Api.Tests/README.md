# TestManagement.Api.Tests

This project contains automated tests for the TestManagement API.

## Scope

Currently, the tests focus on controller behavior for the `TestCasesController` covering:

- retrieving all test cases
- retrieving a test case by id
- creating valid and invalid test cases
- deleting test cases

The tests for `TestRunsController` covering:
- retrieving all test runs
- retrieving a test run by id
- creating test runs
- updating test run results

## Test stack

- **xUnit** for test execution
- **EF Core InMemory** for a lightweight test database

## Running tests

Run all tests from the solution root:

```bash
dotnet test
```
![alt text](../data/imgs/testmanagement_unittests.png)