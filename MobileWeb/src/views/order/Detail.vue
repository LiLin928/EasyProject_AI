<template>
  <div class="order-detail">
    <van-cell-group :inset="false">
      <van-cell title="订单编号" :value="order.orderNo" />
      <van-cell title="订单状态" :value="order.statusText" />
      <van-cell title="下单时间" :value="order.createTime" />
      <van-cell title="支付时间" :value="order.payTime || '未支付'" />
    </van-cell-group>

    <van-cell-group :inset="false" style="margin-top: 20px;">
      <van-card
        v-for="item in order.items"
        :key="item.id"
        :price="item.price"
        :title="item.goodsName"
        :num="item.quantity"
        thumb="https://via.placeholder.com/100x100"
      />
    </van-cell-group>

    <van-submit-bar
      :price="order.totalAmount * 100"
      button-text="立即支付"
      button-type="primary"
      @submit="onPay"
    />
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { showToast } from 'vant'

const order = ref<any>({
  orderNo: 'ORD202603250001',
  statusText: '待付款',
  createTime: '2026-03-25 18:00:00',
  payTime: '',
  totalAmount: 199,
  items: [
    { id: 1, goodsName: '鲜花商品', price: 199, quantity: 1 }
  ]
})

const onPay = () => {
  showToast('支付功能待实现')
}
</script>

<style scoped lang="less">
.order-detail {
  padding-bottom: 50px;
}
</style>
