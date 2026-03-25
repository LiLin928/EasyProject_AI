# CLAUDE.md - MobileWeb 开发指南

> 本文件帮助 AI 助手快速理解 MobileWeb 项目结构、开发规范和常见任务

---

## 🎯 项目概述

**MobileWeb** 是 EasyProject 项目的移动端 H5 应用：
- **框架**: Vue 3.3 + TypeScript
- **UI**: Vant 4.0
- **构建**: Vite 3.0
- **状态**: Pinia 2.1
- **路由**: Vue Router 4.1

---

## 📁 核心目录说明

```
MobileWeb/src/
├── api/                   # 【API 请求】按模块封装 API
│   ├── user/             # 用户 API
│   ├── goods/            # 商品 API
│   ├── cart/             # 购物车 API
│   ├── order/            # 订单 API
│   └── address/          # 地址 API
├── components/            # 【通用组件】可复用组件
├── router/                # 【路由配置】路由定义和守卫
├── store/                 # 【状态管理】Pinia stores
├── types/                 # 【TypeScript 类型】类型定义
├── utils/                 # 【工具函数】通用工具
├── views/                 # 【页面组件】业务页面
│   ├── user/             # 用户相关 (登录/注册/个人)
│   ├── home/             # 首页
│   ├── goods/            # 商品相关
│   ├── cart/             # 购物车
│   ├── order/            # 订单相关
│   └── address/          # 地址相关
└── style/                 # 【全局样式】全局样式文件
```

---

## 🔧 常见开发任务

### 1. 添加新页面

**步骤**:
1. 在 `views/模块名/` 创建页面组件
2. 在 `router/index.ts` 添加路由
3. 在 `api/模块名/` 添加 API 封装

**示例**:
```vue
<!-- views/goods/Detail.vue -->
<template>
  <div class="goods-detail">
    <!-- 商品图片 -->
    <van-image
      :src="goods.image"
      width="100%"
      height="300"
    />

    <!-- 商品信息 -->
    <div class="goods-info">
      <h1>{{ goods.name }}</h1>
      <div class="price">
        <span class="symbol">¥</span>
        <span class="num">{{ goods.price }}</span>
      </div>
    </div>

    <!-- 操作栏 -->
    <van-action-bar>
      <van-action-bar-icon icon="cart-o" :badge="cartCount" />
      <van-action-bar-button
        type="primary"
        text="加入购物车"
        @click="handleAddCart"
      />
    </van-action-bar>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { useRoute } from 'vue-router';
import { showSuccessToast } from 'vant';
import { getGoodsDetail } from '@/api/goods/goodsApi';
import { useCartStore } from '@/store/cart';

const route = useRoute();
const cartStore = useCartStore();

const goods = ref<any>({});
const loading = ref(false);

const cartCount = computed(() => cartStore.totalCount);

onMounted(async () => {
  loading.value = true;
  try {
    const res = await getGoodsDetail(Number(route.params.id));
    goods.value = res.data;
  } catch (error) {
    showFailToast('加载失败');
  } finally {
    loading.value = false;
  }
});

const handleAddCart = () => {
  cartStore.addToCart(goods.value);
  showSuccessToast('已加入购物车');
};
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
      color: #f56c6c;
      font-size: 24px;
      
      .symbol {
        font-size: 14px;
      }
    }
  }
}
</style>
```

### 2. 添加 API 封装

**文件**: `api/goods/goodsApi.ts`
```typescript
import { get, post } from '@/utils/request';
import type { Goods } from '@/types/goods';

/**
 * 获取商品列表
 */
export function getGoodsList(params: any) {
  return get('/api/Mall/Goods/list', params);
}

/**
 * 获取商品详情
 */
export function getGoodsDetail(id: number) {
  return get<Goods>(`/api/Mall/Goods/detail?id=${id}`);
}

/**
 * 搜索商品
 */
export function searchGoods(keyword: string) {
  return get('/api/Mall/Goods/search', { keyword });
}
```

### 3. 添加状态管理

**文件**: `store/cart.ts`
```typescript
import { defineStore } from 'pinia';
import { ref, computed } from 'vue';

export const useCartStore = defineStore('cart', () => {
  const cartList = ref<any[]>([]);

  const totalCount = computed(() => {
    return cartList.value.reduce((sum, item) => sum + item.count, 0);
  });

  const totalPrice = computed(() => {
    return cartList.value.reduce((sum, item) => sum + item.price * item.count, 0);
  });

  function addToCart(product: any) {
    const existItem = cartList.value.find(item => item.id === product.id);
    if (existItem) {
      existItem.count++;
    } else {
      cartList.value.push({ ...product, count: 1 });
    }
  }

  function removeFromCart(id: number) {
    cartList.value = cartList.value.filter(item => item.id !== id);
  }

  function clearCart() {
    cartList.value = [];
  }

  return {
    cartList,
    totalCount,
    totalPrice,
    addToCart,
    removeFromCart,
    clearCart,
  };
});
```

---

## 📊 代码规范

### 命名规范
- **组件**: PascalCase (如 `GoodsDetail.vue`)
- **函数**: camelCase (如 `handleAddCart`)
- **变量**: camelCase (如 `cartList`)
- **类型**: PascalCase (如 `Goods`, `Cart`)

### 组件结构
```vue
<template>
  <!-- 使用 Vant 组件 -->
  <van-button type="primary" @click="handleClick">按钮</van-button>
</template>

<script setup lang="ts">
// 导入
import { ref } from 'vue';
import { showSuccessToast } from 'vant';

// 响应式数据
const loading = ref(false);

// 函数
const handleClick = () => {
  showSuccessToast('操作成功');
};
</script>

<style scoped lang="less">
// 样式
</style>
```

---

## 🚀 运行和调试

### 安装依赖
```bash
cd MobileWeb
pnpm install
```

### 启动开发服务器
```bash
pnpm run dev
```

### 构建生产版本
```bash
pnpm run build
```

### 预览生产构建
```bash
pnpm run preview
```

---

## 📝 常用工具

### HTTP 请求封装
```typescript
// utils/request.ts
import axios from 'axios';
import { showToast } from 'vant';

const request = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  timeout: 30000,
});

// 请求拦截器
request.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// 响应拦截器
request.interceptors.response.use(
  response => response.data,
  error => {
    showToast(error.message || '请求失败');
    return Promise.reject(error);
  }
);

export default request;

export const get = <T>(url: string, params?: any) => 
  request.get<T>(url, { params });

export const post = <T>(url: string, data?: any) => 
  request.post<T>(url, data);
```

---

## 🐛 常见问题

### 1. 移动端适配
**问题**: 在不同设备上显示不一致  
**解决**: 使用 viewport 单位，配置 postcss-pxtorem

### 2. Vant 组件不显示
**问题**: 组件未正确引入  
**解决**: 检查是否按需引入或全量引入

### 3. 样式穿透
**问题**: scoped 样式无法修改 Vant 组件样式  
**解决**: 使用 `:deep()` 选择器

---

## 📚 相关文档

- [需求方案.md](需求方案.md) - 项目需求说明
- [README.md](../README.md) - 项目总览
- [Vant 文档](https://vant-contrib.gitee.io/vant/) - Vant 组件文档

---

**最后更新**: 2026-03-24  
**维护人**: 开发团队
