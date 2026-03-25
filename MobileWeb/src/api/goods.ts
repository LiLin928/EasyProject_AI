import request from '@/utils/request'

export function login(data: { username: string; password: string }) {
  return request.post('/api/Login/login', data)
}

export function getGoodsList(params: any) {
  return request.get('/api/Mall/Goods/list', params)
}

export function getGoodsDetail(id: number) {
  return request.get('/api/Mall/Goods/detail', { id })
}

export function getCartList() {
  return request.get('/api/Mall/Cart/list')
}

export function addToCart(data: any) {
  return request.post('/api/Mall/Cart/add', data)
}

export function createOrder(data: any) {
  return request.post('/api/Mall/Order/create', data)
}

export function getOrderList(params: any) {
  return request.get('/api/Mall/Order/list', params)
}
