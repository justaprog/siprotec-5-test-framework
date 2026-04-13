# Examples
This document contains concrete examples for the Simulation Service.

Its purpose is to help developers understand:
- what kind of input the service receives
- what kind of output it should generate
- how to think about the generated data in simple software terms

These examples are intentionally simplified. They are meant to support implementation and testing, not to model the full complexity of a real power system.

---

## Example 1: Basic overcurrent scenario

This is the main example for version 1.

### Scenario

We simulate an overcurrent fault where:
- the system starts in a normal state
- the fault begins at `100 ms`
- current jumps from a normal value to a fault value
- the service returns sampled 3-phase current data

### Request
Request body for POST /api/simulations/run:
```json
{
  "faultType": "Overcurrent",
  "nominalCurrent": 100, # to be added in test cases
  "pickupCurrent": 300,
  "faultCurrent": 600,
  "faultStartMs": 100,
  "durationMs": 300, # to be added in test cases
  "samplingRateHz": 1000 # to be added in test cases
}
```

Explanation of parameters:
- nominalCurrent = 100: Before the fault starts, each phase current is 100.
- pickupCurrent = 300: This is the threshold that later relay logic may use to determine whether the signal is abnormal.
- faultCurrent = 600: After the fault starts, each phase current becomes 600.
- faultStartMs = 100: The signal changes from normal current to fault current at 100 ms.
- durationMs = 300: The full simulation runs from 0 ms to 300 ms.
- samplingRateHz = 1000: The service generates 1000 samples per second, which is about 1 sample every 1 ms.

Expected behavior:
- from 0 ms to 99 ms: current is normal
- from 100 ms onward: current is at fault level

### Response
Example response body:
```json
{
  "samplingRateHz": 1000,
  "durationMs": 300,
  "faultStartMs": 100,
  "samples": [
    { "timeMs": 0, "ia": 100, "ib": 100, "ic": 100 },
    { "timeMs": 1, "ia": 100, "ib": 100, "ic": 100 },
    { "timeMs": 2, "ia": 100, "ib": 100, "ic": 100 },
    { "timeMs": 99, "ia": 100, "ib": 100, "ic": 100 },
    { "timeMs": 100, "ia": 600, "ib": 600, "ic": 600 },
    { "timeMs": 101, "ia": 600, "ib": 600, "ic": 600 },
    { "timeMs": 102, "ia": 600, "ib": 600, "ic": 600 }
  ]
}
```
