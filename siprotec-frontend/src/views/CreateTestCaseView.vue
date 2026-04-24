<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { createTestCase } from '@/services/testCaseService'
import type { CreateTestCaseRequest } from '@/types/testCaseRequest'

const router = useRouter()

const isSubmitting = ref(false)
const errorMessage = ref('')

const form = ref<CreateTestCaseRequest>({
  name: '',
  description: '',
  deviceFamily: 0,
  protectionFunction: 0,
  simulation: {
    faultType: 0,
    nominalCurrent: 1,
    pickupCurrent: 1,
    faultCurrent: 5,
    faultStartMs: 100,
    durationMs: 500,
    samplingRateHz: 1000
  },
  expectedOutcome: {
    expectedTrip: true,
    tripDelayMs: 100,
    expectedTripMinMs: 90,
    expectedTripMaxMs: 110
  }
})

async function handleSubmit() {
  isSubmitting.value = true
  errorMessage.value = ''

  try {
    await createTestCase(form.value)
    router.push('/')
  } catch (error) {
    errorMessage.value =
      error instanceof Error
        ? error.message
        : 'Failed to create test case.'
  } finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <section class="create-test-case-view">
    <h1>Create Test Case</h1>

    <p v-if="errorMessage" class="error-message">
      {{ errorMessage }}
    </p>

    <form class="test-case-form" @submit.prevent="handleSubmit">
      <h2>General</h2>

      <label>
        Name
        <input v-model="form.name" type="text" required />
      </label>

      <label>
        Description
        <textarea v-model="form.description" rows="3" />
      </label>

      <label>
        Device Family
        <input v-model.number="form.deviceFamily" type="number" required />
      </label>

      <label>
        Protection Function
        <input v-model.number="form.protectionFunction" type="number" required />
      </label>

      <h2>Simulation</h2>

      <label>
        Fault Type
        <input v-model.number="form.simulation.faultType" type="number" required />
      </label>

      <label>
        Nominal Current
        <input v-model.number="form.simulation.nominalCurrent" type="number" step="0.1" required />
      </label>

      <label>
        Pickup Current
        <input v-model.number="form.simulation.pickupCurrent" type="number" step="0.1" required />
      </label>

      <label>
        Fault Current
        <input v-model.number="form.simulation.faultCurrent" type="number" step="0.1" required />
      </label>

      <label>
        Fault Start (ms)
        <input v-model.number="form.simulation.faultStartMs" type="number" required />
      </label>

      <label>
        Duration (ms)
        <input v-model.number="form.simulation.durationMs" type="number" required />
      </label>

      <label>
        Sampling Rate (Hz)
        <input v-model.number="form.simulation.samplingRateHz" type="number" required />
      </label>

      <h2>Expected Outcome</h2>

      <label>
        Expected Trip
        <input v-model="form.expectedOutcome.expectedTrip" type="checkbox" />
      </label>

      <label>
        Trip Delay (ms)
        <input v-model.number="form.expectedOutcome.tripDelayMs" type="number" />
      </label>

      <label>
        Expected Trip Min (ms)
        <input v-model.number="form.expectedOutcome.expectedTripMinMs" type="number" />
      </label>

      <label>
        Expected Trip Max (ms)
        <input v-model.number="form.expectedOutcome.expectedTripMaxMs" type="number" />
      </label>

      <div class="actions">
        <button type="button" @click="router.push('/')">Cancel</button>
        <button type="submit" :disabled="isSubmitting">
          {{ isSubmitting ? 'Creating...' : 'Create Test Case' }}
        </button>
      </div>
    </form>
  </section>
</template>

<style scoped>
.create-test-case-view {
  padding: 2rem;
  max-width: 700px;
}

.test-case-form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

label {
  display: flex;
  flex-direction: column;
  gap: 0.35rem;
}

input,
textarea {
  padding: 0.6rem;
}

.actions {
  display: flex;
  gap: 1rem;
}

.error-message {
  color: #b00020;
}
</style>