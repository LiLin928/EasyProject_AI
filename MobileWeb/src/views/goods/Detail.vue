<template>
  <div class="goods-detail">
    <van-image :src="goods.image" width="100%" height="300" />

    <div class="goods-info">
      <h1>{{ goods.name }}</h1>
      <div class="price">
        <span class="symbol">¥</span>
        <span class="num">{{ goods.price }}</span>
      </div>
      <div class="description">{{ goods.description }}</div>
    </div>

    <van-action-bar>
      <van-action-bar-icon icon="cart-o" :badge="cartStore.totalCount" />
      <van-action-bar-button type="primary" text="加入购物车" @click="handleAddCart" />
    </van-action-bar>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRoute } from 'vue-router'
import { showToast } from 'vant'
import { getGoodsDetail } from '@/api/goods'
import { useCartStore } from '@/stores/cart'

const route = useRoute()
const cartStore = useCartStore()

const goods = ref<any>({
  id: Number(route.params.id),
  name: '鲜花商品',
  price: 199,
  image: 'https://via.placeholder.com/750x750',
  description: '这是一束美丽的鲜花'
})

onMounted(async () => {
  try {
    const res = await getGoodsDetail(goods.value.id)
    goods.value = res.data
  } catch (error) {
    console.error('加载失败:', error)
  }
})

const handleAddCart = () => {
  cartStore.addToCart(goods.value)
  showToast('已加入购物车')
}
</script>

<style scoped lang="less">
.goods-detail {
  padding-bottom: 50px;
  
  .goods-info {
    padding: 16px;
    
    h1 {
      font-size: 18px;
      margin-bottom: 8px;
    }
    
    .price {
      color: #ee0a24;
      font-size: 24px;
      margin: 10px 0;
      
      .symbol {
        font-size: 14px;
      }
    }
    
    .description {
      font-size: 14px;
      color: #969799;
      line-height: 1.6;
    }
  }
}
</style>
