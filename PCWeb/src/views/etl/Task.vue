<template>
  <div class="etl-page">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>ETL 任务管理</span>
          <el-button type="primary" @click="handleCreate">
            <el-icon><Plus /></el-icon>
            新建任务
          </el-button>
        </div>
      </template>

      <el-table v-loading="loading" :data="tableData" border>
        <el-table-column prop="taskCode" label="任务编码" />
        <el-table-column prop="taskName" label="任务名称" />
        <el-table-column prop="sourceType" label="数据源" />
        <el-table-column prop="targetType" label="目标" />
        <el-table-column prop="schedule" label="调度周期" />
        <el-table-column prop="status" label="状态">
          <template #default="{ row }">
            <el-tag :type="row.status === 1 ? 'success' : 'info'">
              {{ row.status === 1 ? '启用' : '停用' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="200">
          <template #default="{ row }">
            <el-button link type="primary" @click="handleRun(row)">执行</el-button>
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
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'

const router = useRouter()
const loading = ref(false)
const tableData = ref<any[]>([])

onMounted(() => {
  tableData.value = [
    { id: 1, taskCode: 'ETL_SYNC_USER', taskName: '用户数据同步', sourceType: 'MySQL', targetType: 'MySQL', schedule: '每天 00:00', status: 1 }
  ]
})

const handleCreate = () => router.push('/etl/designer')
const handleRun = (row: any) => ElMessage.info(`执行任务：${row.taskName}`)
const handleEdit = (row: any) => router.push(`/etl/designer/${row.id}`)
const handleDelete = (row: any) => ElMessage.info(`删除任务：${row.taskName}`)
</script>

<style scoped lang="less">
.etl-page { .card-header { display: flex; justify-content: space-between; align-items: center; } }
</style>
