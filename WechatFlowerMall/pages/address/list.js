Page({
  data: {
    addressList: []
  },

  onLoad() {
    this.loadAddressList()
  },

  onShow() {
    this.loadAddressList()
  },

  async loadAddressList() {
    try {
      // TODO: 调用 API 加载地址列表
      this.setData({
        addressList: [
          {
            id: 1,
            name: '张三',
            tel: '13800138000',
            address: '北京市朝阳区 xxx 街道',
            isDefault: true
          }
        ]
      })
    } catch (error) {
      console.error('加载地址失败:', error)
    }
  },

  onAdd() {
    wx.navigateTo({ url: '/pages/address/edit' })
  },

  onEdit(e) {
    const { id } = e.currentTarget.dataset
    wx.navigateTo({ url: `/pages/address/edit?id=${id}` })
  },

  onSelect(e) {
    const { item } = e.detail
    wx.showToast({ title: `选择：${item.address}`, icon: 'none' })
  }
})
