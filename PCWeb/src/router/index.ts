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
        component: () => import('@/views/index/Dashboard.vue'),
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
      },
      {
        path: 'menu',
        name: 'Menu',
        component: () => import('@/views/basic/Menu.vue'),
        meta: { title: '菜单管理', icon: 'Menu' }
      },
      {
        path: 'config',
        name: 'Config',
        component: () => import('@/views/system/Config.vue'),
        meta: { title: '配置管理', icon: 'Setting' }
      }
    ]
  },
  {
    path: '/srm',
    name: 'Srm',
    redirect: '/srm/purchase-request',
    meta: { title: '采购管理', icon: 'ShoppingCart' },
    children: [
      {
        path: 'purchase-request',
        name: 'PurchaseRequest',
        component: () => import('@/views/srm/PurchaseRequest.vue'),
        meta: { title: '采购申请', icon: 'Document' }
      },
      {
        path: 'purchase-order',
        name: 'PurchaseOrder',
        component: () => import('@/views/srm/PurchaseOrder.vue'),
        meta: { title: '采购订单', icon: 'ShoppingCart' }
      },
      {
        path: 'supplier',
        name: 'Supplier',
        component: () => import('@/views/srm/Supplier.vue'),
        meta: { title: '供应商管理', icon: 'User' }
      }
    ]
  },
  {
    path: '/workflow',
    name: 'Workflow',
    redirect: '/workflow/list',
    meta: { title: '工作流', icon: 'Connection' },
    children: [
      {
        path: 'list',
        name: 'WorkflowList',
        component: () => import('@/views/workflow/List.vue'),
        meta: { title: '工作流管理', icon: 'List' }
      },
      {
        path: 'designer',
        name: 'WorkflowDesigner',
        component: () => import('@/views/workflow/Designer.vue'),
        meta: { title: '工作流设计器', icon: 'Connection' }
      },
      {
        path: 'designer/:id',
        name: 'WorkflowDesignerEdit',
        component: () => import('@/views/workflow/Designer.vue'),
        meta: { title: '编辑工作流', icon: 'Connection' }
      }
    ]
  },
  {
    path: '/etl',
    name: 'Etl',
    redirect: '/etl/task',
    meta: { title: 'ETL 管理', icon: 'DataLine' },
    children: [
      {
        path: 'task',
        name: 'EtlTask',
        component: () => import('@/views/etl/Task.vue'),
        meta: { title: 'ETL 任务', icon: 'Task' }
      }
    ]
  },
  {
    path: '/report',
    name: 'Report',
    redirect: '/report/list',
    meta: { title: '报表管理', icon: 'DataAnalysis' },
    children: [
      {
        path: 'list',
        name: 'ReportList',
        component: () => import('@/views/report/List.vue'),
        meta: { title: '报表列表', icon: 'Document' }
      }
    ]
  },
  {
    path: '/screen',
    name: 'Screen',
    redirect: '/screen/project',
    meta: { title: '大屏管理', icon: 'Monitor' },
    children: [
      {
        path: 'project',
        name: 'ScreenProject',
        component: () => import('@/views/screen/Project.vue'),
        meta: { title: '大屏项目', icon: 'Grid' }
      },
      {
        path: 'designer',
        name: 'ScreenDesigner',
        component: () => import('@/views/screen/Designer.vue'),
        meta: { title: '大屏设计器', icon: 'Monitor' }
      },
      {
        path: 'designer/:id',
        name: 'ScreenDesignerEdit',
        component: () => import('@/views/screen/Designer.vue'),
        meta: { title: '编辑大屏', icon: 'Monitor' }
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
