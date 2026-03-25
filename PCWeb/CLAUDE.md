# CLAUDE.md - PCWeb 开发指南

> 本文件帮助 AI 助手快速理解 PCWeb 项目结构、开发规范和常见任务

---

## 🎯 项目概述

**PCWeb** 是 EasyProject 项目的 PC 端管理后台：
- **框架**: Vue 3.3 + TypeScript
- **UI**: Element Plus
- **构建**: Vite 3.0
- **状态**: Pinia 2.1
- **路由**: Vue Router 4.1

---

## 📁 核心目录说明

```
PCWeb/src/
├── api/                   # 【API 请求】按模块封装 API
│   ├── workflow/         # 工作流 API
│   ├── etl/              # ETL API
│   ├── report/           # 报表 API
│   └── screen/           # 大屏 API
├── components/            # 【通用组件】可复用组件
│   └── workflow/         # 工作流组件
├── composables/           # 【组合式函数】可复用逻辑
├── router/                # 【路由配置】路由定义和守卫
├── stores/                # 【状态管理】Pinia stores 
├── types/                 # 【TypeScript 类型】类型定义
├── utils/                 # 【工具函数】通用工具
├── views/                 # 【页面组件】业务页面
│   ├── index/            # 首页相关 (登录/工作台)
│   ├── basic/            # 基础管理
│   ├── workflow/         # 工作流管理
│   ├── etl/              # ETL 管理
│   ├── report/           # 报表管理
│   ├── screen/           # 大屏管理
│   └── mall/             # 鲜花商城
└── global.ts              # 全局工具函数
```

---

## 🔧 常见开发任务

### 1. 添加新页面

**步骤**:
1. 在 `views/模块名/` 创建页面组件
2. 在 `router/模块 Routes.ts` 添加路由
3. 在 `api/模块名/` 添加 API 封装

**示例**:
```vue
<!-- views/product/ProductList.vue -->
<template>
  <div class="product-list">
    <el-card shadow="never">
      <template #header>
        <div class="card-header">
          <span>产品管理</span>
          <el-button type="primary" @click="handleCreate">
            <el-icon><Plus /></el-icon> 新建
          </el-button>
        </div>
      </template>

      <!-- 搜索栏 -->
      <el-form :model="queryParams" :inline="true">
        <el-form-item label="产品名称">
          <el-input v-model="queryParams.name" placeholder="请输入" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleSearch">搜索</el-button>
        </el-form-item>
      </el-form>

      <!-- 表格 -->
      <el-table v-loading="loading" :data="tableData" border>
        <el-table-column prop="name" label="产品名称" />
        <el-table-column prop="price" label="价格" />
        <el-table-column label="操作">
          <template #default="{ row }">
            <el-button link type="primary" @click="handleEdit(row)">编辑</el-button>
            <el-button link type="danger" @click="handleDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>

      <!-- 分页 -->
      <el-pagination
        v-model:current-page="queryParams.pageIndex"
        v-model:page-size="queryParams.pageSize"
        :total="total"
        @current-change="handleSearch"
      />
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { ElMessage } from 'element-plus';
import { getProductList, deleteProduct } from '@/api/product/productApi';

const loading = ref(false);
const tableData = ref<any[]>([]);
const total = ref(0);

const queryParams = ref({
  pageIndex: 1,
  pageSize: 10,
  name: '',
});

onMounted(() => {
  handleSearch();
});

const handleSearch = async () => {
  loading.value = true;
  try {
    const res = await getProductList(queryParams.value);
    tableData.value = res.data.list;
    total.value = res.data.total;
  } catch (error) {
    ElMessage.error('加载失败');
  } finally {
    loading.value = false;
  }
};

const handleCreate = () => {
  // 新建逻辑
};

const handleEdit = (row: any) => {
  // 编辑逻辑
};

const handleDelete = async (row: any) => {
  await deleteProduct(row.id);
  ElMessage.success('删除成功');
  handleSearch();
};
</script>

<style scoped lang="less">
.product-list {
  padding: 20px;
  
  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
}
</style>
```

### 2. 添加 API 封装

**文件**: `api/product/productApi.ts`
```typescript
import { get, post, put, del } from '@/utils/request';
import type { PageResponse } from '@/types/response';
import type { Product } from '@/types/product';

/**
 * 获取产品列表
 */
export function getProductList(params: any) {
  return get<PageResponse<Product>>('/api/Product/list', params);
}

/**
 * 获取产品详情
 */
export function getProductDetail(id: number) {
  return get<Product>(`/api/Product/detail?id=${id}`);
}

/**
 * 创建产品
 */
export function createProduct(data: any) {
  return post('/api/Product/create', data);
}

/**
 * 更新产品
 */
export function updateProduct(data: any) {
  return post('/api/Product/update', data);
}

/**
 * 删除产品
 */
export function deleteProduct(id: number) {
  return del(`/api/Product/delete?id=${id}`);
}
```

### 3. 添加路由配置

**文件**: `router/productRoutes.ts`
```typescript
import type { RouteRecordRaw } from 'vue-router';

const productRoutes: RouteRecordRaw[] = [
  {
    path: '/product',
    name: 'Product',
    redirect: '/product/list',
    meta: {
      title: '产品管理',
      icon: 'Goods',
    },
    children: [
      {
        path: 'list',
        name: 'ProductList',
        component: () => import('@/views/product/ProductList.vue'),
        meta: {
          title: '产品列表',
          icon: 'List',
        },
      },
    ],
  },
];

export default productRoutes;
```

### 4. 添加状态管理

**文件**: `stores/product.ts`
```typescript
import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useProductStore = defineStore('product', () => {
  const productList = ref<any[]>([]);
  const currentProduct = ref<any>(null);

  function setProductList(list: any[]) {
    productList.value = list;
  }

  function setCurrentProduct(product: any) {
    currentProduct.value = product;
  }

  return {
    productList,
    currentProduct,
    setProductList,
    setCurrentProduct,
  };
});
```

---

## 📊 代码规范

### 命名规范
- **组件**: PascalCase (如 `ProductList.vue`)
- **函数**: camelCase (如 `handleSearch`)
- **变量**: camelCase (如 `tableData`)
- **常量**: UPPER_SNAKE_CASE (如 `API_BASE_URL`)
- **类型**: PascalCase (如 `Product`, `ProductListResponse`)

### 注释规范
```typescript
/**
 * 获取产品列表
 * @param params - 查询参数
 * @param params.pageIndex - 页码
 * @param params.pageSize - 每页数量
 * @param params.name - 产品名称
 * @returns 产品列表
 */
export function getProductList(params: any) {
  return get<PageResponse<Product>>('/api/Product/list', params);
}
```

### 组件结构
```vue
<template>
  <!-- 模板 -->
</template>

<script setup lang="ts">
// 导入
import { ref, onMounted } from 'vue';

// Props
const props = defineProps<{
  title: string;
}>();

// Emits
const emit = defineEmits<{
  (e: 'update', value: any): void;
}>();

// 响应式数据
const loading = ref(false);

// 生命周期
onMounted(() => {
  // 初始化逻辑
});

// 函数
const handleClick = () => {
  // 点击逻辑
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
cd PCWeb
npm install
```

### 启动开发服务器
```bash
npm run dev
```

### 构建生产版本
```bash
npm run build
```

### 预览生产构建
```bash
npm run preview
```

### 代码检查
```bash
npm run lint
```

---

## 📝 常用工具

### HTTP 请求封装
```typescript
// utils/request.ts
import axios from 'axios';

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
    if (error.response.status === 401) {
      // 跳转登录
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default request;

// 快捷方法
export const get = <T>(url: string, params?: any) => 
  request.get<T>(url, { params });

export const post = <T>(url: string, data?: any) => 
  request.post<T>(url, data);

export const put = <T>(url: string, data?: any) => 
  request.put<T>(url, data);

export const del = <T>(url: string) => 
  request.delete<T>(url);
```

### 全局工具函数
```typescript
// global.ts
export function formatDate(date: Date, format = 'YYYY-MM-DD HH:mm:ss') {
  // 日期格式化
}

export function formatNumber(num: number, decimals = 2) {
  // 数字格式化
}

export function debounce(fn: Function, delay = 300) {
  // 防抖
}

export function throttle(fn: Function, delay = 300) {
  // 节流
}
```

---

## 🐛 常见问题

### 1. API 请求 401
**原因**: Token 过期或无效  
**解决**: 清除 localStorage 中的 token，重新登录

### 2. 路由跳转空白
**原因**: 路由配置错误或组件路径错误  
**解决**: 检查路由配置和组件路径

### 3. 样式不生效
**原因**: scoped 样式作用域问题  
**解决**: 使用 `:deep()` 选择器或移除 scoped

---

## 📚 相关文档

- [需求方案.md](需求方案.md) - 项目需求说明

---

**最后更新**: 2026-03-24  
**维护人**: 开发团队
