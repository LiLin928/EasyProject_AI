import axios, { AxiosInstance, AxiosResponse } from 'axios'
import { showToast } from 'vant'
import router from '@/router'

const request: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:7018',
  timeout: 30000,
})

request.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => {
    console.error('请求错误:', error)
    return Promise.reject(error)
  }
)

request.interceptors.response.use(
  (response: AxiosResponse) => {
    const res = response.data
    
    if (res.code !== 200) {
      showToast(res.message || '请求失败')
      
      if (res.code === 401) {
        localStorage.removeItem('token')
        router.push('/login')
      }
      
      return Promise.reject(new Error(res.message || '请求失败'))
    }
    
    return res
  },
  (error) => {
    console.error('响应错误:', error)
    showToast('网络错误，请检查网络连接')
    return Promise.reject(error)
  }
)

export const get = <T>(url: string, params?: any) => request.get<T>(url, { params })
export const post = <T>(url: string, data?: any) => request.post<T>(url, data)

export default request
