<template>
  <div class="layout-container">
    <el-container>
      <!-- 侧边栏 -->
      <el-aside width="200px">
        <div class="logo">EasyProject</div>
        <el-menu
          :default-active="activeMenu"
          background-color="#304156"
          text-color="#bfcbd9"
          active-text-color="#409EFF"
          router
        >
          <el-menu-item index="/desktop">
            <el-icon><Home /></el-icon>
            <span>工作台</span>
          </el-menu-item>
          
          <el-sub-menu index="basic">
            <template #title>
              <el-icon><Setting /></el-icon>
              <span>基础管理</span>
            </template>
            <el-menu-item index="/basic/user">用户管理</el-menu-item>
            <el-menu-item index="/basic/role">角色管理</el-menu-item>
          </el-sub-menu>
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
import { useRoute, useRouter } from 'vue-router'
import { useUserStore } from '@/stores/user'

const route = useRoute()
const router = useRouter()
const userStore = useUserStore()

const activeMenu = computed(() => route.path)

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
