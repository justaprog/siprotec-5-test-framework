import { createRouter, createWebHistory } from 'vue-router'
import TestCaseListView from '@/views/TestCaseListView.vue'
import TestCaseDetailView from '@/views/TestCaseDetailView.vue'

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
    }
  ]
})

export default router