<script setup lang="ts">
import type { TestCase } from '@/types/testCase'

defineProps<{
  testCases: TestCase[]
}>()

function formatDate(dateString: string): string {
  return new Date(dateString).toLocaleString()
}
</script>

<template>
  <table class="test-case-table">
    <thead>
      <tr>
        <th>Name</th>
        <th>Device Family</th>
        <th>Protection Function</th>
        <th>Fault Type</th>
        <th>Expected Trip</th>
        <th>Created At</th>
        <th>Test Runs</th>
        <th>Details</th>
        <th>Delete</th>
      </tr>
    </thead>

    <tbody>
      <tr v-for="testCase in testCases" :key="testCase.id">
        <td>{{ testCase.name }}</td>
        <td>{{ testCase.deviceFamily }}</td>
        <td>{{ testCase.protectionFunction }}</td>
        <td>{{ testCase.simulation.faultType }}</td>
        <td>{{ testCase.expectedOutcome.expectedTrip ? 'Yes' : 'No' }}</td>
        <td>{{ formatDate(testCase.createdAt) }}</td>
        <td>{{ testCase.testRuns.length }}</td>
        <td>
          <RouterLink
            :to="`/test-cases/${testCase.id}`"
            class="details-button"
          >
            Details
          </RouterLink>
        </td>
        <td>
          <button class="delete-button" @click="$emit('delete', testCase.id)">
            Delete
          </button>
        </td>
      </tr>
    </tbody>
  </table>
</template>

<style scoped>
.test-case-table {
  width: 100%;
  border-collapse: collapse;
}

.test-case-table th,
.test-case-table td {
  padding: 0.75rem;
  border: 1px solid #dcdcdc;
  text-align: left;
}

.test-case-table th {
  background-color: #f5f5f5;
}

.details-button {
  display: inline-block;
  padding: 0.4rem 0.8rem;
  background-color: #2c3e50;
  color: white;
  text-decoration: none;
  border-radius: 4px;
}
.delete-button {
  padding: 0.4rem 0.8rem;
  border: none;
  background-color: red;
  border-radius: 4px;
  cursor: pointer;
}
</style>