<template>
  <div class="address-page">
    <van-address-list
      v-model="chosenAddressId"
      :list="addressList"
      default-tag-text="默认"
      @add="onAdd"
      @edit="onEdit"
      @select="onSelect"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { showToast } from 'vant'

const router = useRouter()
const chosenAddressId = ref('')
const addressList = ref<any[]>([])

onMounted(() => {
  // TODO: 加载地址列表
  addressList.value = [
    {
      id: '1',
      name: '张三',
      tel: '13800138000',
      address: '北京市朝阳区 xxx 街道',
      isDefault: true
    }
  ]
})

const onAdd = () => {
  router.push('/address/edit')
}

const onEdit = (item: any) => {
  router.push(`/address/edit?id=${item.id}`)
}

const onSelect = (item: any) => {
  showToast(`选择地址：${item.address}`)
}
</script>

<style scoped lang="less">
.address-page {
  padding-bottom: 50px;
}
</style>
