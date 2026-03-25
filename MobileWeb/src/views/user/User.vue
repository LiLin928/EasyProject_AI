<template>
  <div class="user-page">
    <van-cell-group :inset="false">
      <van-cell center :is-link="false">
        <template #title>
          <div class="user-info">
            <van-image round width="60" height="60" src="https://via.placeholder.com/100x100" />
            <div class="info">
              <div class="nickname">{{ userStore.userInfo?.nickname || '用户' }}</div>
              <div class="phone">{{ userStore.userInfo?.phone || '暂无手机号' }}</div>
            </div>
          </div>
        </template>
      </van-cell>
    </van-cell-group>

    <van-cell-group :inset="false" style="margin-top: 20px;">
      <van-cell title="我的订单" is-link to="/order/list" />
      <van-cell title="收货地址" is-link />
      <van-cell title="优惠券" is-link />
    </van-cell-group>

    <div style="margin-top: 20px;">
      <van-button type="danger" block @click="handleLogout">
        退出登录
      </van-button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import { useUserStore } from '@/stores/user'
import { showToast } from 'vant'

const router = useRouter()
const userStore = useUserStore()

const handleLogout = () => {
  userStore.logout()
  router.push('/login')
  showToast('已退出登录')
}
</script>

<style scoped lang="less">
.user-page {
  padding: 20px 0;
  
  .user-info {
    display: flex;
    align-items: center;
    
    .info {
      margin-left: 15px;
      
      .nickname {
        font-size: 16px;
        font-weight: bold;
        margin-bottom: 5px;
      }
      
      .phone {
        font-size: 14px;
        color: #969799;
      }
    }
  }
}
</style>
