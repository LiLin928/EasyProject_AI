const app = getApp()

Page({
  data: {
    goods: {
      id: 0,
      name: '鲜花商品',
      price: 199,
      image: 'https://via.placeholder.com/750x750',
      description: '这是一束美丽的鲜花'
    },
    totalCount: 0
  },

  onLoad(options) {
    this.setData({ 'goods.id': options.id })
    this.loadGoodsDetail()
    this.loadCartCount()
  },

  async loadGoodsDetail() {
    try {
      const api = require('../../utils/api.js')
      const res = await api.getGoodsDetail(this.data.goods.id)
      this.setData({ goods: res.data })
    } catch (error) {
      console.error('加载失败:', error)
    }
  },

  loadCartCount() {
    const cartStore = require('../../stores/cart.js')
    const cartList = cartStore.cartStore.cartList
    const totalCount = cartList.reduce((sum, item) => sum + item.count, 0)
    this.setData({ totalCount })
  },

  onAddCart() {
    const cartStore = require('../../stores/cart.js')
    cartStore.cartStore.addToCart(this.data.goods)
    
    wx.showToast({
      title: '已加入购物车',
      icon: 'success'
    })
    
    this.loadCartCount()
  }
})
