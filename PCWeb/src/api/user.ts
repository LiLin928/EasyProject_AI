import request from '@/utils/request'

/**
 * 用户登录
 */
export function login(data: { username: string; password: string }) {
  return request.post('/api/Login/login', data)
}

/**
 * 获取用户列表
 */
export function getUserList(params: any) {
  return request.get('/api/User/list', params)
}

/**
 * 获取用户详情
 */
export function getUserDetail(id: number) {
  return request.get('/api/User/detail', { id })
}

/**
 * 创建用户
 */
export function createUser(data: any) {
  return request.post('/api/User/create', data)
}

/**
 * 更新用户
 */
export function updateUser(data: any) {
  return request.post('/api/User/update', data)
}

/**
 * 删除用户
 */
export function deleteUser(id: number) {
  return request.post('/api/User/delete', { id })
}

/**
 * 获取角色列表
 */
export function getRoleList(params: any) {
  return request.get('/api/Role/list', params)
}

/**
 * 创建角色
 */
export function createRole(data: any) {
  return request.post('/api/Role/create', data)
}

/**
 * 更新角色
 */
export function updateRole(data: any) {
  return request.post('/api/Role/update', data)
}

/**
 * 删除角色
 */
export function deleteRole(id: number) {
  return request.post('/api/Role/delete', { id })
}
