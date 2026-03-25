import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'

const routes: RouteRecordRaw[] = [
  {
    path: '/login',
    name: 'Login',
    component: () => import('@/views/index/LoginPage.vue'),
    meta: { title: '登录' }
  },
  {
    path: '/',
    name: 'Layout',
    component: () => import('@/views/index/Layout.vue'),
    redirect: '/desktop',
    children: [
      {
        path: 'desktop',
        name: 'Desktop',
        component: () => import('@/views/index/DeskTop.vue'),
        meta: { title: '工作台', icon: 'Home' }
      },
      {
        path: 'person',
        name: 'Person',
        component: () => import('@/views/index/Person.vue'),
        meta: { title: '个人中心', icon: 'User' }
      }
    ]
  },
  {
    path: '/basic',
    name: 'Basic',
    redirect: '/basic/user',
    meta: { title: '基础管理', icon: 'Setting' },
    children: [
      {
        path: 'user',
        name: 'User',
        component: () => import('@/views/basic/User.vue'),
        meta: { title: '用户管理', icon: 'User' }
      },
      {
        path: 'role',
        name: 'Role',
        component: () => import('@/views/basic/Role.vue'),
        meta: { title: '角色管理', icon: 'Lock' }
      }
    ]
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// 路由守卫
router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token')
  
  // 设置页面标题
  document.title = to.meta.title ? `${to.meta.title} - EasyProject` : 'EasyProject'
  
  // 访问登录页，直接放行
  if (to.path === '/login') {
    next()
    return
  }
  
  // 未登录，跳转到登录页
  if (!token) {
    next('/login')
    return
  }
  
  next()
})

export default router
