<template>
  <div class="cart-page">
    <van-card
      v-for="item in cartStore.cartList"
      :key="item.id"
      :price="item.price"
      :title="item.name"
      :thumb="item.image"
      :num="item.count"
      @change="onChange"
      @click="goDetail(item.id)"
    />

    <div v-if="cartStore.cartList.length === 0" class="empty">
      <van-empty description="购物车空空如也" />
    </div>

    <van-submit-bar
      v-show="cartStore.cartList.length > 0"
      :price="cartStore.totalPrice * 100"
      button-text="结算"
      button-type="primary"
      @submit="onSubmit"
    />
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { useCartStore } from '@/stores/cart'
import { showToast } from 'vant'

const router = useRouter()
const cartStore = useCartStore()

const goDetail = (id: number) => {
  router.push(`/goods/${id}`)
}

const onChange = (item: any, index: number, value: boolean) => {
  console.log('选中状态:', value)
}

const onSubmit = () => {
  showToast('结算功能待实现')
}
</script>

<style scoped lang="less">
.cart-page {
  padding-bottom: 50px;
  
  .empty {
    padding: 100px 0;
  }
}
</style>
