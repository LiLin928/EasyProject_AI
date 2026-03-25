<template>
  <div class="invoice-page">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>发票管理</span>
          <el-button type="primary" @click="handleCreate">
            <el-icon><Plus /></el-icon>
            录入发票
          </el-button>
        </div>
      </template>

      <!-- 搜索栏 -->
      <el-form :model="queryParams" :inline="true">
        <el-form-item label="状态">
          <el-select v-model="queryParams.status" placeholder="请选择" clearable>
            <el-option label="待录入" :value="0" />
            <el-option label="待审核" :value="1" />
            <el-option label="已审核" :value="2" />
            <el-option label="已驳回" :value="3" />
          </el-select>
        </el-form-item>
        <el-form-item label="关键词">
          <el-input v-model="queryParams.keyword" placeholder="发票号/供应商" clearable />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleSearch">搜索</el-button>
          <el-button @click="handleReset">重置</el-button>
        </el-form-item>
      </el-form>

      <!-- 表格 -->
      <el-table v-loading="loading" :data="tableData" border>
        <el-table-column prop="invoiceNo" label="发票号" />
        <el-table-column prop="supplierName" label="供应商" />
        <el-table-column prop="invoiceTypeText" label="发票类型" />
        <el-table-column prop="totalAmount" label="总金额" />
        <el-table-column prop="status" label="状态">
          <template #default="{ row }">
            <el-tag :type="getStatusType(row.status)">
              {{ row.statusText }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="invoiceDate" label="开票日期" />
        <el-table-column label="操作" width="200">
          <template #default="{ row }">
            <el-button link type="primary" @click="handleView(row)">查看</el-button>
            <el-button v-if="row.status === 1" link type="success" @click="handleAudit(row, true)">审核</el-button>
            <el-button v-if="row.status === 1" link type="danger" @click="handleAudit(row, false)">驳回</el-button>
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
  status: null as number | null,
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
        invoiceNo: 'INV202603250001',
        supplierName: '某某供应商',
        invoiceTypeText: '增值税专票',
        totalAmount: 10000,
        status: 1,
        statusText: '待审核',
        invoiceDate: '2026-03-25'
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
  queryParams.status = null
  queryParams.keyword = ''
  handleSearch()
}

const handleCreate = () => {
  ElMessage.info('录入发票功能待实现')
}

const handleView = (row: any) => {
  ElMessage.info(`查看发票：${row.invoiceNo}`)
}

const handleAudit = async (row: any, approved: boolean) => {
  try {
    await ElMessageBox.confirm(`确定${approved ? '审核通过' : '驳回'}该发票吗？`, '提示', { type: 'warning' })
    ElMessage.success(approved ? '审核通过' : '已驳回')
    handleSearch()
  } catch (error: any) {
    if (error !== 'cancel') console.error('审核失败:', error)
  }
}

const getStatusType = (status: number) => {
  const types: Record<number, string> = { 0: 'info', 1: 'warning', 2: 'success', 3: 'danger' }
  return types[status] || 'info'
}
</script>

<style scoped lang="less">
.invoice-page {
  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
}
</style>
