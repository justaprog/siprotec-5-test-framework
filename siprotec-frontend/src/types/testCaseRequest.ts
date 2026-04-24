export interface CreateSimulationSettingsRequest {
  faultType: number
  nominalCurrent: number
  pickupCurrent: number
  faultCurrent: number
  faultStartMs: number
  durationMs: number
  samplingRateHz: number
}

export interface CreateExpectedOutcomeRequest {
  expectedTrip: boolean
  tripDelayMs: number | null
  expectedTripMinMs: number | null
  expectedTripMaxMs: number | null
}

export interface CreateTestCaseRequest {
  name: string
  description: string
  deviceFamily: number
  protectionFunction: number
  simulation: CreateSimulationSettingsRequest
  expectedOutcome: CreateExpectedOutcomeRequest
}