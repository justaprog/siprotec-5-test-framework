import { API_BASE_URL } from './api'
import type { TestCase } from '@/types/testCase'

// Fetch all test cases from the test management service
export async function getTestCases(): Promise<TestCase[]> {
  const response = await fetch(`${API_BASE_URL}/testcases`, {
    headers: {
      'Content-Type': 'application/json'
    }
  })

  if (!response.ok) {
    throw new Error(`Failed to fetch test cases (${response.status})`)
  }

  const data: TestCase[] = await response.json()
  return data
}