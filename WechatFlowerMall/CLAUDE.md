# CLAUDE.md - 微信小程序鲜花商城开发指南

> 本文件帮助 AI 助手快速理解小程序项目结构、开发规范和常见任务

---

## 🎯 项目概述

**guigu-flower-mall** 是 EasyProject 项目的微信小程序商城：
- **框架**: 微信小程序原生
- **语言**: JavaScript ES6+
- **UI**: Vant Weapp
- **状态**: MobX-miniprogram

---

## 📁 核心目录说明

```
guigu-flower-mall/
├── pages/                   # 【页面目录】所有页面文件
│   ├── index/              # 首页
│   ├── classify/           # 分类页
│   ├── shopping/           # 购物车
│   ├── my/                 # 个人中心
│   ├── details/            # 商品详情
│   ├── pay/                # 支付页
│   └── order/              # 订单相关
├── components/              # 【通用组件】可复用组件
├── static/                  # 【静态资源】图片/图标等
├── stores/                   # 【状态管理】MobX stores
├── utils/                   # 【工具函数】通用工具
├── app.js                   # 【小程序入口】全局逻辑
├── app.json                 # 【小程序配置】页面路由/底部栏
└── project.config.json      # 【项目配置】项目设置
```

---

## 🔧 常见开发任务

### 1. 添加新页面

**步骤**:
1. 在 `pages/` 创建页面目录
2. 创建 page 的 4 个文件 (js/json/wxml/wxss)
3. 在 `app.json` 中注册页面

**示例**:
```javascript
// pages/product/list.js
Page({
  data: {
    productList: [],
    loading: false,
    pageIndex: 1,
    pageSize: 10,
    total: 0,
  },

  onLoad() {
    this.loadProductList();
  },

  onReachBottom() {
    // 加载更多
    this.data.pageIndex++;
    this.loadProductList();
  },

  async loadProductList() {
    this.setData({ loading: true });
    try {
      const res = await wx.request({
        url: 'https://api.yourdomain.com/api/Mall/Product/list',
        data: {
          pageIndex: this.data.pageIndex,
          pageSize: this.data.pageSize,
        },
      });
      this.setData({
        productList: [...this.data.productList, ...res.data.data.list],
        total: res.data.data.total,
        loading: false,
      });
    } catch (error) {
      wx.showToast({ title: '加载失败', icon: 'none' });
      this.setData({ loading: false });
    }
  },

  onProductTap(e) {
    const { id } = e.currentTarget.dataset;
    wx.navigateTo({ url: `/pages/details/details?id=${id}` });
  },
});
```

```json
// pages/product/list.json
{
  "usingComponents": {
    "van-button": "@vant/weapp/button",
    "van-image": "@vant/weapp/image",
    "van-loading": "@vant/weapp/loading"
  }
}
```

```xml
<!-- pages/product/list.wxml -->
<view class="product-list">
  <van-loading v-if="{{loading}}" />
  
  <view 
    wx:for="{{productList}}" 
    wx:key="id"
    class="product-item"
    data-id="{{item.id}}"
    bindtap="onProductTap"
  >
    <van-image :src="item.image" width="100" height="100" />
    <view class="product-info">
      <text class="product-name">{{item.name}}</text>
      <text class="product-price">¥{{item.price}}</text>
    </view>
  </view>
</view>
```

```wxss
/* pages/product/list.wxss */
.product-list {
  padding: 20rpx;
}

.product-item {
  display: flex;
  margin-bottom: 20rpx;
  padding: 20rpx;
  background: #fff;
  border-radius: 8rpx;
}

.product-info {
  flex: 1;
  margin-left: 20rpx;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.product-name {
  font-size: 28rpx;
  color: #323233;
}

.product-price {
  font-size: 32rpx;
  color: #f56c6c;
}
```

### 2. 添加 API 封装

**文件**: `utils/api.js`
```javascript
const BASE_URL = 'https://api.yourdomain.com';

function getToken() {
  return wx.getStorageSync('token') || '';
}

function request(url, data = {}, method = 'GET') {
  return new Promise((resolve, reject) => {
    wx.request({
      url: BASE_URL + url,
      data,
      method,
      header: {
        'Content-Type': 'application/json',
        'Authorization': getToken() ? `Bearer ${getToken()}` : '',
      },
      success: (res) => {
        if (res.statusCode === 200) {
          resolve(res.data);
        } else if (res.statusCode === 401) {
          wx.navigateTo({ url: '/pages/login/login' });
          reject(new Error('未登录'));
        } else {
          wx.showToast({ title: res.data.message || '请求失败', icon: 'none' });
          reject(new Error(res.data.message));
        }
      },
      fail: (err) => {
        wx.showToast({ title: '网络错误', icon: 'none' });
        reject(err);
      },
    });
  });
}

module.exports = {
  // 商品相关
  getProductList: (params) => request('/api/Mall/Product/list', params),
  getProductDetail: (id) => request(`/api/Mall/Product/detail?id=${id}`),
  
  // 购物车相关
  getCartList: () => request('/api/Mall/Cart/list'),
  addToCart: (data) => request('/api/Mall/Cart/add', data, 'POST'),
  removeFromCart: (id) => request(`/api/Mall/Cart/delete?id=${id}`, {}, 'POST'),
  
  // 订单相关
  createOrder: (data) => request('/api/Mall/Order/create', data, 'POST'),
  getOrderList: (params) => request('/api/Mall/Order/list', params),
  getOrderDetail: (id) => request(`/api/Mall/Order/detail?id=${id}`),
  
  // 地址相关
  getAddressList: () => request('/api/Mall/Address/list'),
  saveAddress: (data) => request('/api/Mall/Address/save', data, 'POST'),
  deleteAddress: (id) => request(`/api/Mall/Address/delete?id=${id}`, {}, 'POST'),
};
```

### 3. 添加状态管理

**文件**: `store/cart.js`
```javascript
import { observable, action } from 'mobx-miniprogram';

export const cartStore = observable({
  cartList: [],
  
  // 计算属性
  get totalCount() {
    return this.cartList.reduce((sum, item) => sum + item.count, 0);
  },
  
  get totalPrice() {
    return this.cartList.reduce((sum, item) => sum + item.price * item.count, 0);
  },
  
  // 动作
  setCartList: action(function (list) {
    this.cartList = list;
  }),
  
  addToCart: action(function (product) {
    const existItem = this.cartList.find(item => item.id === product.id);
    if (existItem) {
      existItem.count++;
    } else {
      this.cartList.push({ ...product, count: 1 });
    }
  }),
  
  removeFromCart: action(function (id) {
    this.cartList = this.cartList.filter(item => item.id !== id);
  }),
  
  clearCart: action(function () {
    this.cartList = [];
  }),
});
```

**使用**:
```javascript
// pages/shopping/shopping.js
import { cartStore } from '../../store/cart';

Page({
  data: {},
  
  onLoad() {
    this.setData({ cartStore });
  },
  
  onRemove(e) {
    const { id } = e.currentTarget.dataset;
    cartStore.removeFromCart(id);
  },
});
```

---

## 📊 代码规范

### 命名规范
- **页面**: 小写 + 下划线 (如 `product_list`)
- **组件**: 小写 + 中划线 (如 `product-item`)
- **函数**: camelCase (如 `handleClick`)
- **变量**: camelCase (如 `productList`)
- **常量**: UPPER_SNAKE_CASE (如 `BASE_URL`)

### 注释规范
```javascript
/**
 * 获取商品列表
 * @param {Object} params - 查询参数
 * @param {number} params.pageIndex - 页码
 * @param {number} params.pageSize - 每页数量
 * @returns {Promise} 商品列表
 */
function getProductList(params) {
  return request('/api/Mall/Product/list', params);
}
```

---

## 🚀 运行和调试

### 安装依赖
```bash
cd guigu-flower-mall
npm install
```

### 导入项目
1. 打开微信开发者工具
2. 导入项目
3. 选择项目目录
4. 填入 AppID

### 构建 npm
```
工具 -> 构建 npm
```

---

## 📝 常用工具

### 本地存储
```javascript
// 设置
wx.setStorageSync('key', 'value');

// 获取
const value = wx.getStorageSync('key');

// 删除
wx.removeStorageSync('key');

// 清空
wx.clearStorageSync();
```

### 页面跳转
```javascript
// 跳转到新页面
wx.navigateTo({ url: '/pages/detail/detail?id=1' });

// 返回上一页
wx.navigateBack();

// 跳转到 tabBar 页面
wx.switchTab({ url: '/pages/index/index' });

// 重定向
wx.redirectTo({ url: '/pages/login/login' });
```

### 提示框
```javascript
// Toast
wx.showToast({ title: '成功', icon: 'success' });

// 警告
wx.showLoading({ title: '加载中' });
wx.hideLoading();

// 确认框
wx.showModal({
  title: '提示',
  content: '确定删除吗？',
  success: (res) => {
    if (res.confirm) {
      // 确认操作
    }
  }
});
```

---

## 🐛 常见问题

### 1. 真机调试空白
**原因**: 服务器域名未配置  
**解决**: 在微信公众平台配置 request 合法域名

### 2. npm 包无法使用
**原因**: 未构建 npm  
**解决**: 工具 -> 构建 npm

### 3. 样式不生效
**原因**: 样式作用域问题  
**解决**: 检查选择器是否正确，使用全局样式

---

## 📚 相关文档

- [需求方案.md](需求方案.md) - 项目需求说明
- [Vant Weapp 文档](https://vant-contrib.gitee.io/vant-weapp/) - Vant 组件文档
- [微信小程序文档](https://developers.weixin.qq.com/miniprogram/dev/framework/) - 官方文档

---

**最后更新**: 2026-03-24  
**维护人**: 开发团队
