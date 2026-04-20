export interface TestRunSummary {
  id: string
  status: string
  passed: boolean | null
  startedAt: string | null
  finishedAt: string | null
}

export interface SimulationSettings {
  faultType: string
  nominalCurrent: number
  pickupCurrent: number
  faultCurrent: number
  faultStartMs: number
  durationMs: number
  samplingRateHz: number
}

export interface ExpectedOutcome {
  expectedTrip: boolean
  tripDelayMs: number | null
  expectedTripMinMs: number | null
  expectedTripMaxMs: number | null
}

export interface TestCase {
  id: string
  name: string
  description: string
  deviceFamily: string
  protectionFunction: string
  simulation: SimulationSettings
  expectedOutcome: ExpectedOutcome
  createdAt: string
  testRuns: TestRunSummary[]
}