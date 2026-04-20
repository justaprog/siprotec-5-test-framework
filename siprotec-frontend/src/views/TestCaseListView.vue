<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { getTestCases } from '@/services/testCaseService'
import type { TestCase } from '@/types/testCase'
import TestCaseTable from '@/components/TestCaseTable.vue'

const testCases = ref<TestCase[]>([])
const isLoading = ref(false)
const errorMessage = ref('')

async function loadTestCases() {
  isLoading.value = true
  errorMessage.value = ''

  try {
    testCases.value = await getTestCases()
  } catch (error) {
    errorMessage.value =
      error instanceof Error
        ? error.message
        : 'An unexpected error occurred while loading test cases.'
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  loadTestCases()
})
</script>

<template>
  <section class="test-case-list-view">
    <div class="page-header">
      <h1>Test Cases</h1>
    </div>

    <p v-if="isLoading">Loading test cases...</p>

    <p v-else-if="errorMessage" class="error-message">
      {{ errorMessage }}
    </p>

    <p v-else-if="testCases.length === 0">
      No test cases found.
    </p>

    <TestCaseTable v-else :test-cases="testCases" />
  </section>
</template>

<style scoped>
.test-case-list-view {
  padding: 2rem;
}

.page-header {
  margin-bottom: 1.5rem;
}

.error-message {
  color: #b00020;
}
</style>