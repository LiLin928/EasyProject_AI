<template>
  <div class="login-page">
    <div class="login-form">
      <h2>鲜花商城</h2>
      
      <van-field
        v-model="username"
        name="username"
        placeholder="请输入用户名"
        :rules="[{ required: true, message: '请输入用户名' }]"
      />
      
      <van-field
        v-model="password"
        type="password"
        name="password"
        placeholder="请输入密码"
        :rules="[{ required: true, message: '请输入密码' }]"
      />
      
      <div class="login-button">
        <van-button type="primary" block :loading="loading" @click="handleLogin">
          登录
        </van-button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { showToast } from 'vant'
import { login } from '@/api/goods'

const router = useRouter()
const username = ref('admin')
const password = ref('123456')
const loading = ref(false)

const handleLogin = async () => {
  loading.value = true
  try {
    const res = await login({ username: username.value, password: password.value })
    
    if (res.data) {
      localStorage.setItem('token', res.data.token)
      showToast('登录成功')
      router.push('/')
    }
  } catch (error) {
    console.error('登录失败:', error)
  } finally {
    loading.value = false
  }
}
</script>

<style scoped lang="less">
.login-page {
  padding: 40px 20px;
  
  .login-form {
    h2 {
      text-align: center;
      color: #1989fa;
      margin-bottom: 40px;
    }
    
    .login-button {
      margin-top: 40px;
    }
  }
}
</style>
