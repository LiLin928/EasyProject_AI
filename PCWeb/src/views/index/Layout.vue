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
            
            <el-menu-item v-else :index="route.path">
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

        <!-- 内容区 -->
        <el-main>
          <router-view />
        </el-main>
      </el-container>
    </el-container>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
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
  ArrowDown
} from '@element-plus/icons-vue'

const route = useRoute()
const router = useRouter()
const userStore = useUserStore()

const activeMenu = computed(() => route.path)

// 获取所有菜单路由（排除 Layout 和 Login）
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

.el-main {
  background-color: #f0f2f5;
  padding: 20px;
}
</style>
