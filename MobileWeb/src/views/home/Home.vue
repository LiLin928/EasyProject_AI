<template>
  <div class="home-page">
    <van-swipe :autoplay="3000" indicator-color="white">
      <van-swipe-item>
        <img src="https://via.placeholder.com/750x300/1989fa/ffffff?text=Banner+1" style="width: 100%;" />
      </van-swipe-item>
      <van-swipe-item>
        <img src="https://via.placeholder.com/750x300/07c160/ffffff?text=Banner+2" style="width: 100%;" />
      </van-swipe-item>
    </van-swipe>

    <van-grid :column-num="4" :border="false">
      <van-grid-item text="玫瑰" icon="flower-o" />
      <van-grid-item text="百合" icon="flower-o" />
      <van-grid-item text="康乃馨" icon="flower-o" />
      <van-grid-item text="郁金香" icon="flower-o" />
    </van-grid>

    <div class="goods-list">
      <h3>推荐鲜花</h3>
      <van-loading v-if="loading" type="spinner" size="24px" />
      <van-card
        v-for="item in goodsList"
        :key="item.id"
        :price="item.price"
        :title="item.name"
        :thumb="item.image"
        @click="goDetail(item.id)"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { getGoodsList } from '@/api/goods'

const router = useRouter()
const loading = ref(false)
const goodsList = ref<any[]>([])

onMounted(async () => {
  loading.value = true
  try {
    const res = await getGoodsList({ pageIndex: 1, pageSize: 10 })
    goodsList.value = res.data.list
  } catch (error) {
    console.error('加载失败:', error)
  } finally {
    loading.value = false
  }
})

const goDetail = (id: number) => {
  router.push(`/goods/${id}`)
}
</script>

<style scoped lang="less">
.home-page {
  padding-bottom: 50px;
  
  .goods-list {
    padding: 10px;
    
    h3 {
      margin: 10px 0;
      font-size: 16px;
      color: #323233;
    }
  }
}
</style>
