import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/form/:id',
      name: 'form',
      component: () => import('../views/FormView.vue')
    },
    {
      path: '/form-spec',
      name: 'create-form-spec',
      component: () => import('../views/CreateFormView.vue')
    }
  ]
})

export default router
