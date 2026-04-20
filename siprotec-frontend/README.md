# SIPROTEC 5 Test Framework Frontend

A Vue 3 + TypeScript frontend for the SIPROTEC 5 Test Framework.  
It provides a simple user interface for creating test cases, starting test runs, and viewing test execution results.

The frontend is designed to work with the backend microservices of the SIPROTEC 
5 Test Framework, especially the **Test Management Service** in the current version.

## Purpose

The goal of this frontend is to make the test workflow easier to use and demonstrate.  
Instead of interacting with the backend only through Swagger or manual API calls, users can manage test cases and runs through a web interface.

## Current Scope

This frontend currently focuses on the following features:

- display all test cases
- create a new test case
- (to be implemented) delete a test case
- start a test run
- view test run details and execution status

At the moment, the main integration target is the **Test Management Service**.  
Other services such as Simulation, Relay Logic, Validation, and Reporting can be 
integrated later as they are implemented.

## Planned Workflow

The frontend follows the backend workflow of the project:

1. User creates a test case
2. User starts a test run
3. Test Management Service triggers the backend workflow
4. Simulation, relay logic, and validation are performed by backend services
5. Results are stored and returned
6. User views the result in the frontend

## Getting Started
### Prerequisites
Make sure you have the following installed:
- Node.js v24
- npm v11
### Install dependencies
```bash
npm install
```
### Run the development server
```bash
npm run dev
```

## Tech Stack

- **Vue 3**
- **TypeScript**
- **Fetch API**