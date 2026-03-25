import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useCartStore = defineStore('cart', () => {
  const cartList = ref<any[]>([])

  const totalCount = computed(() => {
    return cartList.value.reduce((sum, item) => sum + item.count, 0)
  })

  const totalPrice = computed(() => {
    return cartList.value.reduce((sum, item) => sum + item.price * item.count, 0)
  })

  function addToCart(product: any) {
    const existItem = cartList.value.find(item => item.id === product.id)
    if (existItem) {
      existItem.count++
    } else {
      cartList.value.push({ ...product, count: 1 })
    }
  }

  function removeFromCart(id: number) {
    cartList.value = cartList.value.filter(item => item.id !== id)
  }

  function clearCart() {
    cartList.value = []
  }

  return {
    cartList,
    totalCount,
    totalPrice,
    addToCart,
    removeFromCart,
    clearCart
  }
})
