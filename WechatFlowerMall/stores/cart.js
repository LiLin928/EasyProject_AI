import { observable, action } from 'mobx-miniprogram'

export const cartStore = observable({
  cartList: [],

  get totalCount() {
    return this.cartList.reduce((sum, item) => sum + item.count, 0)
  },

  get totalPrice() {
    return this.cartList.reduce((sum, item) => sum + item.price * item.count, 0)
  },

  addToCart: action(function (product) {
    const existItem = this.cartList.find(item => item.id === product.id)
    if (existItem) {
      existItem.count++
    } else {
      this.cartList.push({ ...product, count: 1 })
    }
  }),

  removeFromCart: action(function (id) {
    this.cartList = this.cartList.filter(item => item.id !== id)
  }),

  clearCart: action(function () {
    this.cartList = []
  })
})
