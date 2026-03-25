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
        <el-table-column prop="menuName" label="菜单名称" width="200" />
        <el-table-column prop="menuType" label="类型" width="100">
          <template #default="{ row }">
            <el-tag :type="row.menuType === 1 ? '' : row.menuType === 2 ? 'success' : 'warning'">
              {{ row.menuTypeText }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="path" label="路径" />
        <el-table-column prop="component" label="组件" />
        <el-table-column prop="icon" label="图标" width="100" />
        <el-table-column prop="sort" label="排序" width="80" />
        <el-table-column prop="status" label="状态" width="100">
          <template #default="{ row }">
            <el-tag :type="row.status === 1 ? 'success' : 'danger'">
              {{ row.status === 1 ? '显示' : '隐藏' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="200" fixed="right">
          <template #default="{ row }">
            <el-button link type="primary" @click="handleEdit(row)">编辑</el-button>
            <el-button link type="primary" @click="handleAddChild(row)">子菜单</el-button>
            <el-button link type="danger" @click="handleDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'

const tableData = ref<any[]>([
  {
    id: 1,
    menuName: '系统管理',
    menuType: 1,
    menuTypeText: '目录',
    path: '/system',
    component: 'Layout',
    icon: 'Setting',
    sort: 1,
    status: 1,
    children: [
      { id: 11, menuName: '用户管理', menuType: 2, menuTypeText: '菜单', path: '/system/user', component: 'system/User', icon: 'User', sort: 1, status: 1 },
      { id: 12, menuName: '角色管理', menuType: 2, menuTypeText: '菜单', path: '/system/role', component: 'system/Role', icon: 'Lock', sort: 2, status: 1 },
      { id: 13, menuName: '菜单管理', menuType: 2, menuTypeText: '菜单', path: '/system/menu', component: 'system/Menu', icon: 'Menu', sort: 3, status: 1 },
      { id: 14, menuName: '配置管理', menuType: 2, menuTypeText: '菜单', path: '/system/config', component: 'system/Config', icon: 'Setting', sort: 4, status: 1 }
    ]
  },
  {
    id: 2,
    menuName: '采购管理',
    menuType: 1,
    menuTypeText: '目录',
    path: '/srm',
    component: 'Layout',
    icon: 'ShoppingCart',
    sort: 2,
    status: 1,
    children: [
      { id: 21, menuName: '采购申请', menuType: 2, menuTypeText: '菜单', path: '/srm/purchase-request', component: 'srm/PurchaseRequest', icon: 'Document', sort: 1, status: 1 },
      { id: 22, menuName: '采购订单', menuType: 2, menuTypeText: '菜单', path: '/srm/purchase-order', component: 'srm/PurchaseOrder', icon: 'ShoppingCart', sort: 2, status: 1 },
      { id: 23, menuName: '发票管理', menuType: 2, menuTypeText: '菜单', path: '/srm/invoice', component: 'srm/Invoice', icon: 'Tickets', sort: 3, status: 1 },
      { id: 24, menuName: '付款管理', menuType: 2, menuTypeText: '菜单', path: '/srm/payment', component: 'srm/Payment', icon: 'Money', sort: 4, status: 1 },
      { id: 25, menuName: '结算管理', menuType: 2, menuTypeText: '菜单', path: '/srm/settlement', component: 'srm/Settlement', icon: 'Wallet', sort: 5, status: 1 },
      { id: 26, menuName: '供应商管理', menuType: 2, menuTypeText: '菜单', path: '/srm/supplier', component: 'srm/Supplier', icon: 'User', sort: 6, status: 1 }
    ]
  },
  {
    id: 3,
    menuName: '工作流管理',
    menuType: 1,
    menuTypeText: '目录',
    path: '/workflow',
    component: 'Layout',
    icon: 'Connection',
    sort: 3,
    status: 1,
    children: [
      { id: 31, menuName: '工作流列表', menuType: 2, menuTypeText: '菜单', path: '/workflow/list', component: 'workflow/List', icon: 'List', sort: 1, status: 1 }
    ]
  },
  {
    id: 4,
    menuName: 'ETL 管理',
    menuType: 1,
    menuTypeText: '目录',
    path: '/etl',
    component: 'Layout',
    icon: 'DataLine',
    sort: 4,
    status: 1,
    children: [
      { id: 41, menuName: 'ETL 任务', menuType: 2, menuTypeText: '菜单', path: '/etl/task', component: 'etl/Task', icon: 'Task', sort: 1, status: 1 }
    ]
  },
  {
    id: 5,
    menuName: '报表管理',
    menuType: 1,
    menuTypeText: '目录',
    path: '/report',
    component: 'Layout',
    icon: 'DataAnalysis',
    sort: 5,
    status: 1,
    children: [
      { id: 51, menuName: '报表列表', menuType: 2, menuTypeText: '菜单', path: '/report/list', component: 'report/List', icon: 'Document', sort: 1, status: 1 }
    ]
  },
  {
    id: 6,
    menuName: '大屏管理',
    menuType: 1,
    menuTypeText: '目录',
    path: '/screen',
    component: 'Layout',
    icon: 'Monitor',
    sort: 6,
    status: 1,
    children: [
      { id: 61, menuName: '大屏项目', menuType: 2, menuTypeText: '菜单', path: '/screen/project', component: 'screen/Project', icon: 'Grid', sort: 1, status: 1 }
    ]
  }
])

const handleCreate = () => {
  ElMessage.info('新建菜单功能待实现')
}

const handleEdit = (row: any) => {
  ElMessage.info(`编辑菜单：${row.menuName}`)
}

const handleAddChild = (row: any) => {
  ElMessage.info(`添加子菜单：${row.menuName}`)
}

const handleDelete = async (row: any) => {
  try {
    await ElMessageBox.confirm(`确定删除菜单 ${row.menuName} 吗？`, '提示', { type: 'warning' })
    ElMessage.success('删除成功')
  } catch (error: any) {
    if (error !== 'cancel') console.error('删除失败:', error)
  }
}
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
