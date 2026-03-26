<template>
  <div class="workflow-page">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>工作流管理</span>
          <el-button type="primary" @click="handleCreate">
            <el-icon><Plus /></el-icon>
            新建工作流
          </el-button>
        </div>
      </template>

      <!-- 搜索栏 -->
      <el-form :model="queryParams" :inline="true">
        <el-form-item label="关键词">
          <el-input v-model="queryParams.keyword" placeholder="工作流名称/编码" clearable />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleSearch">搜索</el-button>
          <el-button @click="handleReset">重置</el-button>
        </el-form-item>
      </el-form>

      <!-- 表格 -->
      <el-table v-loading="loading" :data="tableData" border>
        <el-table-column prop="code" label="工作流编码" />
        <el-table-column prop="name" label="工作流名称" />
        <el-table-column prop="version" label="版本" />
        <el-table-column prop="status" label="状态">
          <template #default="{ row }">
            <el-tag :type="row.status === 1 ? 'success' : 'info'">
              {{ row.statusText }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="createTime" label="创建时间" />
        <el-table-column label="操作" width="250">
          <template #default="{ row }">
            <el-button link type="primary" @click="handleView(row)">查看</el-button>
            <el-button link type="primary" @click="handleDesign(row)">设计</el-button>
            <el-button v-if="row.status === 0" link type="success" @click="handlePublish(row)">发布</el-button>
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
import { useRouter } from 'vue-router'
import { ElMessage, ElMessageBox } from 'element-plus'

const router = useRouter()
const loading = ref(false)
const tableData = ref<any[]>([])
const total = ref(0)

const queryParams = reactive({
  pageIndex: 1,
  pageSize: 10,
  keyword: ''
})

onMounted(() => {
  handleSearch()
})

const handleSearch = async () => {
  loading.value = true
  try {
    tableData.value = [
      {
        id: 1,
        code: 'WF_PURCHASE_REQUEST',
        name: '采购申请流程',
        version: '1.0',
        status: 1,
        statusText: '已发布',
        createTime: '2026-03-25 10:00:00'
      }
    ]
    total.value = 1
  } catch (error) {
    console.error('查询失败:', error)
  } finally {
    loading.value = false
  }
}

const handleReset = () => {
  queryParams.keyword = ''
  handleSearch()
}

const handleCreate = () => {
  router.push('/workflow/designer')
}

const handleView = (row: any) => {
  router.push(`/workflow/designer/${row.id}`)
}

const handleDesign = (row: any) => {
  router.push(`/workflow/designer/${row.id}`)
}

const handlePublish = async (row: any) => {
  try {
    await ElMessageBox.confirm(`确定发布工作流 ${row.name} 吗？`, '提示', { type: 'warning' })
    ElMessage.success('发布成功')
    handleSearch()
  } catch (error: any) {
    if (error !== 'cancel') console.error('发布失败:', error)
  }
}

const handleDelete = async (row: any) => {
  try {
    await ElMessageBox.confirm(`确定删除工作流 ${row.name} 吗？`, '提示', { type: 'warning' })
    ElMessage.success('删除成功')
    handleSearch()
  } catch (error: any) {
    if (error !== 'cancel') console.error('删除失败:', error)
  }
}
</script>

<style scoped lang="less">
.workflow-page {
  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
}
</style>
