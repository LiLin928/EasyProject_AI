Page({
  data: {
    address: {}
  },

  onLoad(options) {
    if (options.id) {
      // TODO: 加载地址详情
      this.setData({ address: { id: options.id } })
    }
  },

  onSave(event) {
    console.log('保存地址:', event.detail)
    wx.showToast({ title: '保存成功', icon: 'success' })
    setTimeout(() => {
      wx.navigateBack()
    }, 1000)
  },

  onDelete() {
    wx.showModal({
      title: '提示',
      content: '确定删除该地址吗？',
      success: (res) => {
        if (res.confirm) {
          wx.showToast({ title: '删除成功', icon: 'success' })
          setTimeout(() => {
            wx.navigateBack()
          }, 1000)
        }
      }
    })
  }
})
