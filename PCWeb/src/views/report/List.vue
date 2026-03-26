<template>
  <div class="report-page">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>报表管理</span>
          <el-button type="primary" @click="handleCreate">
            <el-icon><Plus /></el-icon>
            新建报表
          </el-button>
        </div>
      </template>

      <el-table v-loading="loading" :data="tableData" border>
        <el-table-column prop="reportCode" label="报表编码" />
        <el-table-column prop="reportName" label="报表名称" />
        <el-table-column prop="reportType" label="类型" />
        <el-table-column prop="status" label="状态">
          <template #default="{ row }">
            <el-tag :type="row.status === 1 ? 'success' : 'info'">{{ row.status === 1 ? '启用' : '停用' }}</el-tag>
          </template>
        </el-table-column>
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
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'

const router = useRouter()
const loading = ref(false)
const tableData = ref<any[]>([])

onMounted(() => {
  tableData.value = [
    { id: 1, reportCode: 'RPT_PURCHASE', reportName: '采购报表', reportType: 'SQL', status: 1 }
  ]
})

const handleCreate = () => router.push('/report/designer')
const handleView = (row: any) => ElMessage.info(`查看报表：${row.reportName}`)
const handleDesign = (row: any) => router.push(`/report/designer/${row.id}`)
const handleDelete = (row: any) => ElMessage.info(`删除报表：${row.reportName}`)
</script>

<style scoped lang="less">
.report-page { .card-header { display: flex; justify-content: space-between; align-items: center; } }
</style>
