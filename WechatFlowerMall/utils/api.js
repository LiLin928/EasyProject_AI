/**
 * 网络请求封装
 */
const BASE_URL = 'http://localhost:7018'

function getToken() {
  return wx.getStorageSync('token') || ''
}

function request(url, data = {}, method = 'GET') {
  return new Promise((resolve, reject) => {
    wx.request({
      url: BASE_URL + url,
      data,
      method,
      header: {
        'Content-Type': 'application/json',
        'Authorization': getToken() ? `Bearer ${getToken()}` : ''
      },
      success: (res) => {
        if (res.statusCode === 200) {
          resolve(res.data)
        } else if (res.statusCode === 401) {
          wx.navigateTo({ url: '/pages/login/login' })
          reject(new Error('未登录'))
        } else {
          wx.showToast({ title: res.data.message || '请求失败', icon: 'none' })
          reject(new Error(res.data.message))
        }
      },
      fail: (err) => {
        wx.showToast({ title: '网络错误', icon: 'none' })
        reject(err)
      }
    })
  })
}

module.exports = {
  // 登录
  login: (data) => request('/api/Login/login', data, 'POST'),
  
  // 商品
  getGoodsList: (params) => request('/api/Mall/Goods/list', params),
  getGoodsDetail: (id) => request(`/api/Mall/Goods/detail?id=${id}`),
  
  // 购物车
  getCartList: () => request('/api/Mall/Cart/list'),
  addToCart: (data) => request('/api/Mall/Cart/add', data, 'POST'),
  
  // 订单
  createOrder: (data) => request('/api/Mall/Order/create', data, 'POST'),
  getOrderList: (params) => request('/api/Mall/Order/list', params)
}
