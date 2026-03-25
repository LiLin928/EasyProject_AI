<template>
  <div class="goods-page">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>商品管理</span>
          <el-button type="primary" @click="handleCreate">
            <el-icon><Plus /></el-icon>
            新建商品
          </el-button>
        </div>
      </template>

      <el-table v-loading="loading" :data="tableData" border>
        <el-table-column prop="name" label="商品名称" />
        <el-table-column prop="price" label="价格" />
        <el-table-column prop="stock" label="库存" />
        <el-table-column prop="status" label="状态">
          <template #default="{ row }">
            <el-tag :type="row.status === 1 ? 'success' : 'danger'">
              {{ row.status === 1 ? '上架' : '下架' }}
            </el-tag>
          </template>
        </el-table-column>
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
import { ref, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'

const loading = ref(false)
const tableData = ref<any[]>([])

onMounted(() => {
  tableData.value = [
    { id: 1, name: '玫瑰花束', price: 199, stock: 100, status: 1 },
    { id: 2, name: '百合花束', price: 299, stock: 50, status: 1 }
  ]
})

const handleCreate = () => ElMessage.info('新建商品功能待实现')
const handleEdit = (row: any) => ElMessage.info(`编辑商品：${row.name}`)
const handleDelete = async (row: any) => {
  try {
    await ElMessageBox.confirm(`确定删除商品 ${row.name} 吗？`, '提示', { type: 'warning' })
    ElMessage.success('删除成功')
  } catch (error: any) { if (error !== 'cancel') console.error('删除失败:', error) }
}
</script>

<style scoped lang="less">
.goods-page { .card-header { display: flex; justify-content: space-between; align-items: center; } }
</style>
