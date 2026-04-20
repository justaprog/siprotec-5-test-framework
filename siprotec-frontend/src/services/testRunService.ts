import { API_BASE_URL } from './api'
import type { TestRun } from '@/types/testRun'

// Fetch all test runs from the test management service
export async function getTestRuns(): Promise<TestRun[]> {
  const response = await fetch(`${API_BASE_URL}/testruns`, {
    headers: {
      'Content-Type': 'application/json'
    }
  })

  if (!response.ok) {
    throw new Error(`Failed to fetch test runs (${response.status})`)
  }

  const data: TestRun[] = await response.json()
  return data
}