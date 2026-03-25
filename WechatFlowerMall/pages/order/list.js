const app = getApp()

Page({
  data: {
    active: 0,
    orderList: [],
    loading: false,
    finished: false
  },

  onLoad() {
    this.loadOrderList()
  },

  async loadOrderList() {
    this.setData({ loading: true })
    try {
      const api = require('../../utils/api.js')
      const res = await api.getOrderList({ status: this.data.active })
      this.setData({
        orderList: [...this.data.orderList, ...res.data.list],
        finished: res.data.list.length < 10,
        loading: false
      })
    } catch (error) {
      console.error('加载订单失败:', error)
      wx.showToast({ title: '加载失败', icon: 'none' })
      this.setData({ loading: false })
    }
  },

  onChange(event) {
    this.setData({
      active: event.detail,
      orderList: [],
      finished: false
    })
    this.loadOrderList()
  },

  onDetail(e) {
    const { id } = e.currentTarget.dataset
    wx.navigateTo({ url: `/pages/order/detail?id=${id}` })
  }
})
