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

      <!-- 搜索栏 -->
      <el-form :model="queryParams" :inline="true">
        <el-form-item label="类型">
          <el-select v-model="queryParams.type" placeholder="请选择" clearable>
            <el-option label="系统配置" value="SYSTEM" />
            <el-option label="业务配置" value="BUSINESS" />
            <el-option label="其他配置" value="OTHER" />
          </el-select>
        </el-form-item>
        <el-form-item label="关键词">
          <el-input v-model="queryParams.keyword" placeholder="配置键/描述" clearable />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleSearch">搜索</el-button>
          <el-button @click="handleReset">重置</el-button>
        </el-form-item>
      </el-form>

      <el-table v-loading="loading" :data="tableData" border>
        <el-table-column prop="configKey" label="配置键" width="200" />
        <el-table-column prop="configValue" label="配置值" />
        <el-table-column prop="configType" label="类型" width="120">
          <template #default="{ row }">
            <el-tag>{{ row.configType }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="description" label="描述" />
        <el-table-column prop="status" label="状态" width="100">
          <template #default="{ row }">
            <el-tag :type="row.status === 1 ? 'success' : 'danger'">
              {{ row.status === 1 ? '启用' : '禁用' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="150" fixed="right">
          <template #default="{ row }">
            <el-button link type="primary" @click="handleEdit(row)">编辑</el-button>
            <el-button link type="danger" @click="handleDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>

      <!-- 分页 -->
      <el-pagination
        v-model:current-page="queryParams.pageIndex"
        v-model:page-size="queryParams.pageSize"
        :total="total"
        :page-sizes="[10, 20, 50, 100]"
        layout="total, sizes, prev, pager, next, jumper"
        @current-change="handleSearch"
      />
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'

const loading = ref(false)
const tableData = ref<any[]>([])
const total = ref(0)

const queryParams = reactive({
  pageIndex: 1,
  pageSize: 10,
  type: '',
  keyword: ''
})

onMounted(() => {
  handleSearch()
})

const handleSearch = async () => {
  loading.value = true
  try {
    // TODO: 调用 API
    tableData.value = [
      { id: 1, configKey: 'SITE_NAME', configValue: 'EasyProject', configType: 'SYSTEM', description: '系统名称', status: 1 },
      { id: 2, configKey: 'SITE_TITLE', configValue: 'EasyProject 管理系统', configType: 'SYSTEM', description: '网站标题', status: 1 },
      { id: 3, configKey: 'MAX_UPLOAD_SIZE', configValue: '10485760', configType: 'BUSINESS', description: '最大上传文件大小 (字节)', status: 1 }
    ]
    total.value = 3
  } catch (error) {
    console.error('查询失败:', error)
  } finally {
    loading.value = false
  }
}

const handleReset = () => {
  queryParams.type = ''
  queryParams.keyword = ''
  handleSearch()
}

const handleCreate = () => {
  ElMessage.info('新建配置功能待实现')
}

const handleEdit = (row: any) => {
  ElMessage.info(`编辑配置：${row.configKey}`)
}

const handleDelete = async (row: any) => {
  try {
    await ElMessageBox.confirm(`确定删除配置 ${row.configKey} 吗？`, '提示', { type: 'warning' })
    ElMessage.success('删除成功')
    handleSearch()
  } catch (error: any) {
    if (error !== 'cancel') console.error('删除失败:', error)
  }
}
</script>

<style scoped lang="less">
.config-page {
  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
}
</style>
