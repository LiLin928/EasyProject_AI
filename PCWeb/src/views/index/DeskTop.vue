<template>
  <div class="desktop">
    <el-card>
      <template #header>
        <span>工作台</span>
      </template>
      <div class="welcome">
        <h2>欢迎使用 EasyProject 管理系统</h2>
        <p>当前用户：{{ userStore.userInfo?.username || '管理员' }}</p>
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { useUserStore } from '@/stores/user'
const userStore = useUserStore()
</script>

<style scoped lang="less">
.desktop {
  .welcome {
    text-align: center;
    padding: 40px 0;
    
    h2 {
      color: #409EFF;
      margin-bottom: 20px;
    }
    
    p {
      color: #606266;
      font-size: 16px;
    }
  }
}
</style>
