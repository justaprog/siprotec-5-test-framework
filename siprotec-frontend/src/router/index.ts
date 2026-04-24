import { createRouter, createWebHistory } from 'vue-router'
import TestCaseListView from '@/views/TestCaseListView.vue'
import TestCaseDetailView from '@/views/TestCaseDetailView.vue'
import CreateTestCaseView from '@/views/CreateTestCaseView.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'test-cases',
      component: TestCaseListView
    },
    {
      path: '/test-cases/:id',
      name: 'test-case-details',
      component: TestCaseDetailView
    },
    {
      path: '/test-cases/new',
      name: 'create-test-case',
      component: CreateTestCaseView,
    },
  ]
})

export default router