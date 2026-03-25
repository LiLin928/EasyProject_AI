const app = getApp()

Page({
  data: {
    cartList: [],
    totalCount: 0,
    totalPrice: 0
  },

  onLoad() {
    this.loadCart()
  },

  loadCart() {
    const cartStore = require('../../stores/cart.js')
    const cartList = cartStore.cartStore.cartList
    const totalCount = cartList.reduce((sum, item) => sum + item.count, 0)
    const totalPrice = cartList.reduce((sum, item) => sum + item.price * item.count, 0)
    
    this.setData({
      cartList,
      totalCount,
      totalPrice
    })
  },

  onCartEmpty() {
    wx.showToast({
      title: '购物车为空',
      icon: 'none'
    })
  },

  onSubmit() {
    wx.showToast({
      title: '结算功能待实现',
      icon: 'none'
    })
  }
})
