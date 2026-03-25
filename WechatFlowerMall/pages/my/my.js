const app = getApp()

Page({
  data: {
    userInfo: null
  },

  onLoad() {
    this.loadUserInfo()
  },

  onShow() {
    this.loadUserInfo()
  },

  loadUserInfo() {
    const userInfo = app.globalData.userInfo
    this.setData({ userInfo })
  },

  onLogout() {
    wx.showModal({
      title: '提示',
      content: '确定退出登录吗？',
      success: (res) => {
        if (res.confirm) {
          wx.clearStorageSync()
          app.globalData.userInfo = null
          app.globalData.token = ''
          wx.switchTab({
            url: '/pages/my/my'
          })
        }
      }
    })
  }
})
