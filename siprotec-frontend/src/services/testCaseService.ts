//import { API_BASE_URL } from './api'
import type { TestCase } from '@/types/testCase'

// Fetch all test cases from the test management service
export async function getTestCases(): Promise<TestCase[]> {
  const response = await fetch(`/api/testcases`, {
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

// Fetch a single test case by ID from the test management service
export async function getTestCaseById(id: string): Promise<TestCase> {
  const response = await fetch(`/api/testcases/${id}`, {
    headers: {
      'Content-Type': 'application/json'
    }
  })

  if (!response.ok) {
    console.error(`Failed to fetch test case with ID ${id}: ${response.statusText}`)
    throw new Error(`Failed to fetch test case (${response.status})`)
  }

  const data: TestCase = await response.json()

  console.log("Response data:", data) // Log the response data for debugging
  return data
}