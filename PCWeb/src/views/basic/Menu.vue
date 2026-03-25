<template>
  <div class="menu-page">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>菜单管理</span>
          <el-button type="primary" @click="handleCreate">
            <el-icon><Plus /></el-icon>
            新建菜单
          </el-button>
        </div>
      </template>

      <el-table :data="tableData" row-key="id" border default-expand-all>
        <el-table-column prop="menuName" label="菜单名称" />
        <el-table-column prop="menuType" label="类型">
          <template #default="{ row }">
            <el-tag :type="row.menuType === 1 ? '' : row.menuType === 2 ? 'success' : 'warning'">
              {{ row.menuTypeText }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="path" label="路径" />
        <el-table-column prop="icon" label="图标" />
        <el-table-column prop="sort" label="排序" width="80" />
        <el-table-column label="操作" width="200">
          <template #default="{ row }">
            <el-button link type="primary" @click="handleEdit(row)">编辑</el-button>
            <el-button link type="danger" @click="handleDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ElMessage } from 'element-plus'

const tableData = [
  {
    id: 1,
    menuName: '系统管理',
    menuType: 1,
    menuTypeText: '目录',
    path: '/system',
    icon: 'Setting',
    sort: 1,
    children: [
      { id: 11, menuName: '用户管理', menuType: 2, menuTypeText: '菜单', path: '/system/user', icon: 'User', sort: 1 },
      { id: 12, menuName: '角色管理', menuType: 2, menuTypeText: '菜单', path: '/system/role', icon: 'Lock', sort: 2 }
    ]
  },
  {
    id: 2,
    menuName: '采购管理',
    menuType: 1,
    menuTypeText: '目录',
    path: '/srm',
    icon: 'ShoppingCart',
    sort: 2,
    children: [
      { id: 21, menuName: '采购申请', menuType: 2, menuTypeText: '菜单', path: '/srm/purchase-request', icon: 'Document', sort: 1 },
      { id: 22, menuName: '采购订单', menuType: 2, menuTypeText: '菜单', path: '/srm/purchase-order', icon: 'ShoppingCart', sort: 2 }
    ]
  }
]

const handleCreate = () => ElMessage.info('新建菜单功能待实现')
const handleEdit = (row: any) => ElMessage.info(`编辑菜单：${row.menuName}`)
const handleDelete = (row: any) => ElMessage.info(`删除菜单：${row.menuName}`)
</script>

<style scoped lang="less">
.menu-page {
  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
}
</style>
