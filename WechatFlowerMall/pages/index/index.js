Page({
  data: {
    bannerList: [
      'https://via.placeholder.com/750x300/1989fa/ffffff?text=Banner+1',
      'https://via.placeholder.com/750x300/07c160/ffffff?text=Banner+2'
    ],
    categoryList: [
      { name: '玫瑰', icon: 'flower-o' },
      { name: '百合', icon: 'flower-o' },
      { name: '康乃馨', icon: 'flower-o' },
      { name: '郁金香', icon: 'flower-o' }
    ],
    goodsList: [],
    loading: false
  },

  onLoad() {
    this.loadGoodsList()
  },

  async loadGoodsList() {
    this.setData({ loading: true })
    try {
      const api = require('../../utils/api.js')
      const res = await api.getGoodsList({ pageIndex: 1, pageSize: 10 })
      this.setData({
        goodsList: res.data.list,
        loading: false
      })
    } catch (error) {
      console.error('加载失败:', error)
      wx.showToast({ title: '加载失败', icon: 'none' })
      this.setData({ loading: false })
    }
  },

  onGoodsTap(e) {
    const { id } = e.currentTarget.dataset
    wx.navigateTo({
      url: `/pages/details/details?id=${id}`
    })
  }
})
