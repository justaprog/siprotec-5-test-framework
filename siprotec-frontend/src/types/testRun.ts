export interface TestCaseSummary {
  id: string
  name: string
  deviceFamily: string
  protectionFunction: string
}

export interface TestRun {
  id: string
  testCaseId: string
  status: string
  startedAt: string | null
  finishedAt: string | null
  actualTrip: boolean | null
  actualTripTimeMs: number | null
  passed: boolean | null
  resultMessage: string | null
  testCaseSummary: TestCaseSummary | null
}