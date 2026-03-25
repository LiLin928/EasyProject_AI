<template>
  <div class="search-page">
    <van-search v-model="keyword" placeholder="搜索商品" @search="handleSearch" />
    
    <van-list v-model:loading="loading" :finished="finished" finished-text="没有更多了">
      <van-card
        v-for="item in goodsList"
        :key="item.id"
        :price="item.price"
        :title="item.name"
        :thumb="item.image"
        @click="goDetail(item.id)"
      />
    </van-list>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { getGoodsList } from '@/api/goods'

const router = useRouter()
const keyword = ref('')
const loading = ref(false)
const finished = ref(false)
const goodsList = ref<any[]>([])

const handleSearch = async () => {
  loading.value = true
  try {
    const res = await getGoodsList({ keyword: keyword.value, pageIndex: 1, pageSize: 10 })
    goodsList.value = res.data.list
    finished.value = res.data.list.length < 10
  } catch (error) {
    console.error('搜索失败:', error)
  } finally {
    loading.value = false
  }
}

const goDetail = (id: number) => {
  router.push(`/goods/${id}`)
}
</script>

<style scoped lang="less">
.search-page {
  padding-bottom: 20px;
}
</style>
