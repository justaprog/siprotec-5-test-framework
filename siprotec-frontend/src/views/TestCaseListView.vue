<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { deleteTestCaseById, getTestCases } from '@/services/testCaseService'
import type { TestCase } from '@/types/testCase'
import TestCaseTable from '@/components/TestCaseTable.vue'

const router = useRouter()

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

function goToCreatePage() {
  router.push('/test-cases/new')
}

async function handleDelete(testCaseId: string) {
  const confirmed = window.confirm('Do you really want to delete this test case?')

  if (!confirmed) {
    return
  }

  try {
    await deleteTestCaseById(testCaseId)
    await loadTestCases()
  } catch (error) {
    errorMessage.value =
      error instanceof Error
        ? error.message
        : 'Failed to delete test case.'
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
      <button class="create-button" @click="goToCreatePage">
        Create New Test Case
      </button>
    </div>

    <p v-if="isLoading">Loading test cases...</p>

    <p v-else-if="errorMessage" class="error-message">
      {{ errorMessage }}
    </p>

    <p v-else-if="testCases.length === 0">
      No test cases found.
    </p>

    <TestCaseTable
      v-else
      :test-cases="testCases"
      @delete="handleDelete"
    />
  </section>
</template>

<style scoped>
.test-case-list-view {
  padding: 2rem;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}

.create-button {
  padding: 0.6rem 1rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  background-color: blue;
  color: white;
}

.error-message {
  color: #b00020;
}
</style>