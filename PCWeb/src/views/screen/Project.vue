<template>
  <div class="screen-page">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>大屏项目管理</span>
          <el-button type="primary" @click="handleCreate">
            <el-icon><Plus /></el-icon>
            新建大屏
          </el-button>
        </div>
      </template>

      <el-table v-loading="loading" :data="tableData" border>
        <el-table-column prop="projectCode" label="项目编码" />
        <el-table-column prop="projectName" label="项目名称" />
        <el-table-column prop="status" label="状态">
          <template #default="{ row }">
            <el-tag :type="row.status === 1 ? 'success' : 'info'">{{ row.status === 1 ? '启用' : '停用' }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="createTime" label="创建时间" />
        <el-table-column label="操作" width="200">
          <template #default="{ row }">
            <el-button link type="primary" @click="handleView(row)">查看</el-button>
            <el-button link type="primary" @click="handleDesign(row)">设计</el-button>
            <el-button link type="danger" @click="handleDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { ElMessage } from 'element-plus'

const loading = ref(false)
const tableData = ref<any[]>([])

onMounted(() => {
  tableData.value = [
    { id: 1, projectCode: 'SCR_PURCHASE', projectName: '采购数据大屏', status: 1, createTime: '2026-03-25' }
  ]
})

const handleCreate = () => ElMessage.info('新建大屏功能待实现')
const handleView = (row: any) => ElMessage.info(`查看大屏：${row.projectName}`)
const handleDesign = (row: any) => ElMessage.info('大屏设计器待实现')
const handleDelete = (row: any) => ElMessage.info(`删除大屏：${row.projectName}`)
</script>

<style scoped lang="less">
.screen-page { .card-header { display: flex; justify-content: space-between; align-items: center; } }
</style>
