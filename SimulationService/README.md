# Simulation Service
The Simulation Service generates simulated measurement data for protection relay test scenarios.
It is part of a modular SIPROTEC 5 test framework and provides sampled current signals for downstream services such as Relay Logic and Validation.

## Responsibilities
- generate simulated 3-phase current signals
- inject overcurrent fault scenarios
- return sampled datasets for a requested test scenario

## Scope
- supports only overcurrent fault scenarios
- generates only current signals
- uses deterministic step-based fault injection
- returns sampled phase currents Ia, Ib, Ic

## API Endpoints
- POST /api/simulations/run: Runs a simulation for a given overcurrent scenario and returns sampled current data.

## Example Use Case
A test run requests a simulation where fault current exceeds pickup current after 100 ms.
The service returns sampled current values that can later be evaluated by the Relay Logic Service.

## Future Improvements
- support voltage signals
- support additional fault types
- simulate measurement noise
- support more realistic phase behavior