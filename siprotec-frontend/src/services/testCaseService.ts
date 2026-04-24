//import { API_BASE_URL } from './api'
import type { TestCase } from '@/types/testCase'
import type { CreateTestCaseRequest } from '@/types/testCaseRequest'

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

// Create a new test case
export async function createTestCase(payload: CreateTestCaseRequest): Promise<TestCase> {
  const response = await fetch(`/api/testcases`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(payload)
  })

  if (!response.ok) {
    throw new Error(`Failed to create test case (${response.status})`)
  }

  return response.json()
}


// Delete test case
export async function deleteTestCaseById(id: string): Promise<void> {
  const response = await fetch(`/api/testcases/${id}`, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json'
    }
  })

  if (!response.ok) {
    console.error(`Failed to delete test case with ID ${id}: ${response.statusText}`)
    throw new Error(`Failed to delete test case (${response.status})`)
  }

  console.log(`Test case with ID ${id} deleted successfully`)
}