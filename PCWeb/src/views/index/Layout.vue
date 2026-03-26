<template>
  <div class="layout-container">
    <el-container>
      <!-- 侧边栏 -->
      <el-aside width="220px">
        <div class="logo">EasyProject</div>
        <el-menu
          :default-active="activeMenu"
          background-color="#304156"
          text-color="#bfcbd9"
          active-text-color="#409EFF"
          router
          :unique-opened="true"
        >
          <!-- 工作台 -->
          <el-menu-item index="/desktop">
            <el-icon><Home /></el-icon>
            <span>工作台</span>
          </el-menu-item>
          
          <!-- 动态渲染所有菜单 -->
          <template v-for="route in menuRoutes" :key="route.path">
            <el-sub-menu v-if="route.children && route.children.length > 0" :index="route.path">
              <template #title>
                <el-icon :component="getIcon(route.meta?.icon)" />
                <span>{{ route.meta?.title }}</span>
              </template>
              <el-menu-item
                v-for="child in route.children"
                :key="child.path"
                :index="`${route.path}/${child.path}`"
              >
                <el-icon v-if="child.meta?.icon" :component="getIcon(child.meta.icon)" />
                <span>{{ child.meta?.title }}</span>
              </el-menu-item>
            </el-sub-menu>
            
            <el-menu-item v-else :index="`${route.path}`">
              <el-icon :component="getIcon(route.meta?.icon)" />
              <span>{{ route.meta?.title }}</span>
            </el-menu-item>
          </template>
        </el-menu>
      </el-aside>

      <!-- 主内容区 -->
      <el-container>
        <!-- 顶部导航 -->
        <el-header>
          <div class="header-content">
            <span class="breadcrumb">EasyProject 管理后台</span>
            <div class="user-info">
              <el-dropdown>
                <span class="user-name">
                  {{ userStore.userInfo?.username || '管理员' }}
                  <el-icon><ArrowDown /></el-icon>
                </span>
                <template #dropdown>
                  <el-dropdown-menu>
                    <el-dropdown-item @click="handleLogout">
                      <el-icon><SwitchButton /></el-icon>
                      退出登录
                    </el-dropdown-item>
                  </el-dropdown-menu>
                </template>
              </el-dropdown>
            </div>
          </div>
        </el-header>

        <!-- 标签页导航栏 -->
        <div class="tab-navbar">
          <div class="tab-list">
            <div
              v-for="tab in openTabs"
              :key="tab.path"
              class="tab-item"
              :class="{ active: activeTabPath === tab.path }"
              @click="switchTab(tab.path)"
            >
              <span class="tab-icon">{{ getTabIcon(tab.path) }}</span>
              <span class="tab-title">{{ tab.title }}</span>
              <el-icon class="tab-close" @click.stop="closeTab(tab.path)"><Close /></el-icon>
            </div>
          </div>
          <div class="tab-actions">
            <el-button type="text" size="small" @click="closeAllTabs">
              <el-icon><Close /></el-icon>
              全部关闭
            </el-button>
            <el-dropdown :disabled="historyIndex <= 0">
              <el-button type="text" size="small">
                <el-icon><RefreshLeft /></el-icon>
              </el-button>
              <template #dropdown>
                <el-dropdown-menu>
                  <el-dropdown-item @click="goBack">
                    <el-icon><RefreshLeft /></el-icon>
                    后退
                  </el-dropdown-item>
                  <el-dropdown-item @click="goForward" :disabled="historyIndex >= history.length - 1">
                    <el-icon><RefreshRight /></el-icon>
                    前进
                  </el-dropdown-item>
                  <el-dropdown-item divided @click="reloadTab">
                    <el-icon><Refresh /></el-icon>
                    刷新
                  </el-dropdown-item>
                </el-dropdown-menu>
              </template>
            </el-dropdown>
          </div>
        </div>

        <!-- 内容区 -->
        <div class="main-content">
          <keep-alive :include="cachedViews">
            <router-view v-if="isRouterAlive" :key="route.fullPath" />
          </keep-alive>
        </div>
      </el-container>
    </el-container>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch, provide, nextTick } from 'vue'
import { useRoute, useRouter, type RouteRecordRaw } from 'vue-router'
import { useUserStore } from '@/stores/user'
import {
  House as Home,
  Setting,
  User,
  Lock,
  Menu as MenuIcon,
  ShoppingCart,
  Document,
  Connection,
  List,
  DataLine,
  Tickets as Task,
  DataAnalysis,
  Monitor,
  Grid,
  SwitchButton,
  ArrowDown,
  Close,
  RefreshLeft,
  RefreshRight,
  Refresh
} from '@element-plus/icons-vue'

const route = useRoute()
const router = useRouter()
const userStore = useUserStore()

const activeMenu = computed(() => route.path)
const isRouterAlive = ref(true)

// 标签页管理
interface Tab {
  path: string
  title: string
  icon?: string
}

const openTabs = ref<Tab[]>([
  { path: '/desktop', title: '工作台', icon: 'Home' }
])
const activeTabPath = ref('/desktop')
const cachedViews = ref<string[]>([])

// 路由历史
const history = ref<string[]>(['/desktop'])
const historyIndex = ref(0)

// 获取所有菜单路由
const menuRoutes = computed(() => {
  return router.options.routes.filter((r: RouteRecordRaw) => {
    return r.name !== 'Layout' && r.name !== 'Login' && r.path !== '/login'
  })
})

// 图标映射
const iconMap: Record<string, any> = {
  Home,
  Setting,
  User,
  Lock,
  Menu: MenuIcon,
  ShoppingCart,
  Document,
  Connection,
  List,
  DataLine,
  Task,
  DataAnalysis,
  Monitor,
  Grid,
  SwitchButton,
  ArrowDown
}

const getIcon = (iconName?: string) => {
  return iconName ? (iconMap[iconName] || Setting) : Setting
}

// 获取标签页图标
const getTabIcon = (path: string): string => {
  const iconMap: Record<string, string> = {
    '/desktop': '🏠',
    '/basic/user': '👤',
    '/basic/role': '🔐',
    '/basic/menu': '📋',
    '/system/config': '⚙️',
    '/srm/purchase-request': '📝',
    '/srm/purchase-order': '🛒',
    '/srm/supplier': '🏢',
    '/workflow/list': '🔗',
    '/workflow/designer': '🎨',
    '/etl/task': '📊',
    '/etl/designer': '⚙️',
    '/report/list': '📈',
    '/report/designer': '📉',
    '/screen/project': '🖥️',
    '/screen/designer': '🎬'
  }
  return iconMap[path] || '📄'
}

// 获取菜单标题
const getMenuTitle = (path: string): string => {
  const titleMap: Record<string, string> = {
    '/desktop': '工作台',
    '/basic/user': '用户管理',
    '/basic/role': '角色管理',
    '/basic/menu': '菜单管理',
    '/system/config': '配置管理',
    '/srm/purchase-request': '采购申请',
    '/srm/purchase-order': '采购订单',
    '/srm/supplier': '供应商管理',
    '/workflow/list': '工作流管理',
    '/workflow/designer': '工作流设计器',
    '/etl/task': 'ETL 任务',
    '/etl/designer': 'ETL 设计器',
    '/report/list': '报表列表',
    '/report/designer': '报表设计器',
    '/screen/project': '大屏项目',
    '/screen/designer': '大屏设计器'
  }
  return titleMap[path] || '页面'
}

// 处理菜单选择
const handleMenuSelect = (indexPath: string) => {
  const title = getMenuTitle(indexPath)
  
  // 检查标签页是否已存在
  const existingTab = openTabs.value.find(tab => tab.path === indexPath)
  
  if (!existingTab) {
    openTabs.value.push({
      path: indexPath,
      title,
      icon: getTabIcon(indexPath)
    })
  }
  
  activeTabPath.value = indexPath
  
  // 添加到历史
  if (history.value[historyIndex.value] !== indexPath) {
    history.value = history.value.slice(0, historyIndex.value + 1)
    history.value.push(indexPath)
    historyIndex.value = history.value.length - 1
  }
  
  // 添加到缓存
  const routeName = route.name as string
  if (!cachedViews.value.includes(routeName)) {
    cachedViews.value.push(routeName)
  }
}

// 切换标签页
const switchTab = (path: string) => {
  activeTabPath.value = path
  router.push(path)
}

// 关闭标签页
const closeTab = (path: string) => {
  if (openTabs.value.length === 1) {
    return
  }
  
  const index = openTabs.value.findIndex(tab => tab.path === path)
  if (index === -1) return
  
  // 如果关闭的是当前标签，切换到相邻标签
  if (path === activeTabPath.value) {
    const newIndex = index > 0 ? index - 1 : index + 1
    activeTabPath.value = openTabs.value[newIndex].path
    router.push(activeTabPath.value)
  }
  
  // 从缓存中移除
  const tab = openTabs.value[index]
  const routeName = getMenuTitle(tab.path)
  cachedViews.value = cachedViews.value.filter(name => name !== routeName)
  
  openTabs.value.splice(index, 1)
}

// 全部关闭
const closeAllTabs = () => {
  if (openTabs.value.length === 1) {
    return
  }
  
  openTabs.value = [{ path: '/desktop', title: '工作台', icon: '🏠' }]
  activeTabPath.value = '/desktop'
  router.push('/desktop')
  cachedViews.value = []
}

// 后退
const goBack = () => {
  if (historyIndex.value > 0) {
    historyIndex.value--
    const path = history.value[historyIndex.value]
    router.push(path)
  }
}

// 前进
const goForward = () => {
  if (historyIndex.value < history.value.length - 1) {
    historyIndex.value++
    const path = history.value[historyIndex.value]
    router.push(path)
  }
}

// 刷新
const reloadTab = () => {
  isRouterAlive.value = false
  nextTick(() => {
    isRouterAlive.value = true
  })
}

// 监听路由变化
watch(() => route.path, (newPath) => {
  activeTabPath.value = newPath
  
  // 如果标签页不存在，自动添加
  const existingTab = openTabs.value.find(tab => tab.path === newPath)
  if (!existingTab) {
    const title = getMenuTitle(newPath)
    openTabs.value.push({
      path: newPath,
      title,
      icon: getTabIcon(newPath)
    })
  }
}, { immediate: true })

const handleLogout = () => {
  userStore.logout()
  router.push('/login')
}
</script>

<style scoped lang="less">
.layout-container {
  height: 100vh;
}

.el-container {
  height: 100%;
}

.el-aside {
  background-color: #304156;
  color: #fff;
  
  .logo {
    height: 60px;
    line-height: 60px;
    text-align: center;
    font-size: 20px;
    font-weight: bold;
    color: #fff;
    background-color: #2b3a4b;
  }
  
  .el-menu {
    border-right: none;
  }
}

.el-header {
  background-color: #fff;
  box-shadow: 0 1px 4px rgba(0,21,41,.08);
  
  .header-content {
    display: flex;
    justify-content: space-between;
    align-items: center;
    height: 100%;
    
    .breadcrumb {
      font-size: 14px;
      color: #97a8be;
    }
    
    .user-info {
      .user-name {
        cursor: pointer;
        color: #606266;
        
        &:hover {
          color: #409EFF;
        }
      }
    }
  }
}

.tab-navbar {
  height: 40px;
  background: #f5f7fa;
  border-bottom: 1px solid #e4e7ed;
  display: flex;
  align-items: center;
  padding: 0 16px;
  gap: 16px;
  
  .tab-list {
    flex: 1;
    display: flex;
    align-items: center;
    gap: 4px;
    overflow-x: auto;
    
    .tab-item {
      display: flex;
      align-items: center;
      gap: 8px;
      padding: 6px 12px;
      background: #fff;
      border: 1px solid #e4e7ed;
      border-bottom: none;
      border-radius: 4px 4px 0 0;
      cursor: pointer;
      transition: all 0.3s;
      font-size: 13px;
      white-space: nowrap;
      
      &:hover {
        background: #ecf5ff;
        border-color: #409EFF;
        
        .tab-close {
          opacity: 1;
        }
      }
      
      &.active {
        background: #fff;
        border-color: #409EFF;
        color: #409EFF;
      }
      
      .tab-icon {
        font-size: 14px;
      }
      
      .tab-title {
        max-width: 150px;
        overflow: hidden;
        text-overflow: ellipsis;
      }
      
      .tab-close {
        width: 16px;
        height: 16px;
        border-radius: 50%;
        opacity: 0;
        transition: opacity 0.3s;
        
        &:hover {
          background: #F56C6C;
          color: #fff;
        }
      }
    }
  }
  
  .tab-actions {
    display: flex;
    align-items: center;
    gap: 8px;
    
    .el-button {
      color: #606266;
      
      &:hover {
        color: #F56C6C;
      }
    }
  }
}

.main-content {
  flex: 1;
  background-color: #f0f2f5;
  overflow: auto;
}
</style>
