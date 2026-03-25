<template>
  <div class="category-page">
    <van-sidebar v-model="activeKey">
      <van-sidebar-item title="鲜花分类" />
      <van-sidebar-item title="礼品分类" />
      <van-sidebar-item title="场景分类" />
    </van-sidebar>

    <div class="content">
      <van-grid :column-num="2" :border="false">
        <van-grid-item text="红玫瑰" icon="flower-o" />
        <van-grid-item text="粉玫瑰" icon="flower-o" />
        <van-grid-item text="白玫瑰" icon="flower-o" />
        <van-grid-item text="香槟玫瑰" icon="flower-o" />
      </van-grid>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
const activeKey = ref(0)
</script>

<style scoped lang="less">
.category-page {
  display: flex;
  height: calc(100vh - 50px);
  
  .content {
    flex: 1;
    padding: 10px;
  }
}
</style>
