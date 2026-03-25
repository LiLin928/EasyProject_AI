<template>
  <div class="person">
    <el-card>
      <template #header>
        <span>个人中心</span>
      </template>
      <el-descriptions title="个人信息" :column="1" border>
        <el-descriptions-item label="用户名">{{ userStore.userInfo?.username }}</el-descriptions-item>
        <el-descriptions-item label="昵称">{{ userStore.userInfo?.nickname }}</el-descriptions-item>
      </el-descriptions>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { useUserStore } from '@/stores/user'
const userStore = useUserStore()
</script>
