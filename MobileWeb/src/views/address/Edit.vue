<template>
  <div class="address-edit-page">
    <van-address-edit
      area-list="/area-list.json"
      show-postal
      show-set-default
      show-search-result
      :search-result="searchResult"
      :area-columns-placeholder="['请选择', '请选择', '请选择']"
      @save="onSave"
      @delete="onDelete"
    />
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { showToast } from 'vant'

const router = useRouter()
const route = useRoute()
const searchResult = ref([])

const onSave = (content: any) => {
  showToast('保存成功')
  router.back()
}

const onDelete = () => {
  showToast('删除成功')
  router.back()
}
</script>
