<template>
  <div class="register-page">
    <div class="register-form">
      <h2>用户注册</h2>
      
      <van-field v-model="username" name="username" placeholder="请输入用户名" />
      <van-field v-model="password" type="password" name="password" placeholder="请输入密码" />
      <van-field v-model="confirmPassword" type="password" name="confirmPassword" placeholder="请确认密码" />
      <van-field v-model="phone" name="phone" placeholder="请输入手机号" />
      
      <div class="register-button">
        <van-button type="primary" block :loading="loading" @click="handleRegister">
          注册
        </van-button>
      </div>
      
      <div class="login-link">
        已有账号？<router-link to="/login">立即登录</router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { showToast } from 'vant'

const router = useRouter()
const username = ref('')
const password = ref('')
const confirmPassword = ref('')
const phone = ref('')
const loading = ref(false)

const handleRegister = async () => {
  if (password.value !== confirmPassword.value) {
    showToast('两次密码输入不一致')
    return
  }
  
  loading.value = true
  try {
    // TODO: 调用注册 API
    showToast('注册成功')
    router.push('/login')
  } catch (error) {
    console.error('注册失败:', error)
  } finally {
    loading.value = false
  }
}
</script>

<style scoped lang="less">
.register-page {
  padding: 40px 20px;
  
  .register-form {
    h2 {
      text-align: center;
      color: #1989fa;
      margin-bottom: 40px;
    }
    
    .register-button {
      margin-top: 40px;
    }
    
    .login-link {
      text-align: center;
      margin-top: 20px;
      color: #969799;
      
      a {
        color: #1989fa;
      }
    }
  }
}
</style>
