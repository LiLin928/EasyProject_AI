<template>
  <div class="config-page">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>系统配置管理</span>
          <el-button type="primary" @click="handleCreate">
            <el-icon><Plus /></el-icon>
            新建配置
          </el-button>
        </div>
      </template>

      <el-table v-loading="loading" :data="tableData" border>
        <el-table-column prop="configKey" label="配置键" />
        <el-table-column prop="configValue" label="配置值" />
        <el-table-column prop="configType" label="类型" />
        <el-table-column prop="description" label="描述" />
        <el-table-column label="操作" width="150">
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
import { ElMessage } from 'element-plus'

const loading = ref(false)
const tableData = ref<any[]>([])

onMounted(() => {
  tableData.value = [
    { id: 1, configKey: 'SITE_NAME', configValue: 'EasyProject', configType: 'SYSTEM', description: '系统名称' }
  ]
})

const handleCreate = () => ElMessage.info('新建配置功能待实现')
const handleEdit = (row: any) => ElMessage.info(`编辑配置：${row.configKey}`)
const handleDelete = (row: any) => ElMessage.info(`删除配置：${row.configKey}`)
</script>

<style scoped lang="less">
.config-page { .card-header { display: flex; justify-content: space-between; align-items: center; } }
</style>
