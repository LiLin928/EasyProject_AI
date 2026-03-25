<template>
  <div class="order-list">
    <van-tabs v-model:active="active">
      <van-tab title="全部" />
      <van-tab title="待付款" />
      <van-tab title="待发货" />
      <van-tab title="待收货" />
      <van-tab title="已完成" />
    </van-tabs>

    <van-list v-model:loading="loading" :finished="finished" finished-text="没有更多了">
      <van-card
        v-for="item in orderList"
        :key="item.id"
        :price="item.totalAmount"
        :title="item.orderNo"
        thumb="https://via.placeholder.com/100x100"
      >
        <template #tags>
          <van-tag plain type="primary">{{ item.statusText }}</van-tag>
        </template>
        <template #footer>
          <van-button size="mini" @click="goDetail(item.id)">查看详情</van-button>
        </template>
      </van-card>
    </van-list>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const active = ref(0)
const loading = ref(false)
const finished = ref(false)
const orderList = ref<any[]>([])

const goDetail = (id: number) => {
  router.push(`/order/${id}`)
}
</script>
