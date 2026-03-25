import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'

const routes: RouteRecordRaw[] = [
  {
    path: '/login',
    name: 'Login',
    component: () => import('@/views/user/Login.vue'),
    meta: { title: '登录' }
  },
  {
    path: '/',
    name: 'Layout',
    component: () => import('@/views/home/Layout.vue'),
    redirect: '/home',
    children: [
      {
        path: 'home',
        name: 'Home',
        component: () => import('@/views/home/Home.vue'),
        meta: { title: '首页' }
      },
      {
        path: 'category',
        name: 'Category',
        component: () => import('@/views/home/Category.vue'),
        meta: { title: '分类' }
      },
      {
        path: 'cart',
        name: 'Cart',
        component: () => import('@/views/cart/Cart.vue'),
        meta: { title: '购物车' }
      },
      {
        path: 'user',
        name: 'User',
        component: () => import('@/views/user/User.vue'),
        meta: { title: '我的' }
      }
    ]
  },
  {
    path: '/goods/:id',
    name: 'GoodsDetail',
    component: () => import('@/views/goods/Detail.vue'),
    meta: { title: '商品详情' }
  },
  {
    path: '/order/list',
    name: 'OrderList',
    component: () => import('@/views/order/List.vue'),
    meta: { title: '订单列表' }
  },
  {
    path: '/order/:id',
    name: 'OrderDetail',
    component: () => import('@/views/order/Detail.vue'),
    meta: { title: '订单详情' }
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token')
  
  if (to.path !== '/login' && !token) {
    next('/login')
    return
  }
  
  next()
})

export default router
