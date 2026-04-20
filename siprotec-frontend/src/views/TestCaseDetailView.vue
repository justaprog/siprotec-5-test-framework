<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRoute } from 'vue-router'
import { getTestCaseById } from '@/services/testCaseService'
import type { TestCase } from '@/types/testCase'

const route = useRoute()

const testCase = ref<TestCase | null>(null)
const isLoading = ref(false)
const errorMessage = ref('')

async function loadTestCase() {
  isLoading.value = true
  errorMessage.value = ''

  try {
    const id = route.params.id as string
    console.log('Loading test case with ID:', id)
    const loadedTestCase = await getTestCaseById(id)
    console.log('Loaded test case:', loadedTestCase)
    testCase.value = loadedTestCase
  } catch (error) {
    console.error('Error loading test case details:', error)
    errorMessage.value =
      error instanceof Error
        ? error.message
        : 'An unexpected error occurred while loading the test case.'
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  loadTestCase()
})

</script>

<template>
  <section class="test-case-detail-view">
    <h1>Test Case Details</h1>

    <p v-if="isLoading">Loading test case...</p>

    <p v-else-if="errorMessage">{{ errorMessage }}</p>

    <div v-else-if="testCase">
      <h2>{{ testCase.name }}</h2>
      <p><strong>Description:</strong> {{ testCase.description }}</p>
      <p><strong>Device Family:</strong> {{ testCase.deviceFamily }}</p>
      <p><strong>Protection Function:</strong> {{ testCase.protectionFunction }}</p>
      <p><strong>Created At:</strong> {{ new Date(testCase.createdAt).toLocaleString() }}</p>

      <h3>Simulation</h3>
      <p><strong>Fault Type:</strong> {{ testCase.simulation.faultType }}</p>
      <p><strong>Nominal Current:</strong> {{ testCase.simulation.nominalCurrent }}</p>
      <p><strong>Pickup Current:</strong> {{ testCase.simulation.pickupCurrent }}</p>
      <p><strong>Fault Current:</strong> {{ testCase.simulation.faultCurrent }}</p>
      <p><strong>Fault Start (ms):</strong> {{ testCase.simulation.faultStartMs }}</p>
      <p><strong>Duration (ms):</strong> {{ testCase.simulation.durationMs }}</p>
      <p><strong>Sampling Rate (Hz):</strong> {{ testCase.simulation.samplingRateHz }}</p>

      <h3>Expected Outcome</h3>
      <p><strong>Expected Trip:</strong> {{ testCase.expectedOutcome.expectedTrip ? 'Yes' : 'No' }}</p>
      <p><strong>Trip Delay (ms):</strong> {{ testCase.expectedOutcome.tripDelayMs ?? '-' }}</p>
      <p><strong>Expected Trip Min (ms):</strong> {{ testCase.expectedOutcome.expectedTripMinMs ?? '-' }}</p>
      <p><strong>Expected Trip Max (ms):</strong> {{ testCase.expectedOutcome.expectedTripMaxMs ?? '-' }}</p>
      <p v-for="testRun in testCase.testRuns" :key="testRun.id">
        <strong>Test Run {{ testRun.id }}:</strong> {{ testRun.status }}
      </p>
    </div>
  </section>
</template>

<style scoped>
.test-case-detail-view {
  padding: 2rem;
}
</style>