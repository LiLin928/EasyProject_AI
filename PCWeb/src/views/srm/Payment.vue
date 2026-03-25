<template>
  <div class="payment-page">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>付款管理</span>
          <el-button type="primary" @click="handleCreate">
            <el-icon><Plus /></el-icon>
            新建付款
          </el-button>
        </div>
      </template>

      <!-- 搜索栏 -->
      <el-form :model="queryParams" :inline="true">
        <el-form-item label="状态">
          <el-select v-model="queryParams.status" placeholder="请选择" clearable>
            <el-option label="草稿" :value="0" />
            <el-option label="待审批" :value="1" />
            <el-option label="已批准" :value="2" />
            <el-option label="已付款" :value="3" />
            <el-option label="已拒绝" :value="4" />
          </el-select>
        </el-form-item>
        <el-form-item label="关键词">
          <el-input v-model="queryParams.keyword" placeholder="申请单号/供应商" clearable />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleSearch">搜索</el-button>
          <el-button @click="handleReset">重置</el-button>
        </el-form-item>
      </el-form>

      <!-- 表格 -->
      <el-table v-loading="loading" :data="tableData" border>
        <el-table-column prop="requestNo" label="申请单号" />
        <el-table-column prop="supplierName" label="供应商" />
        <el-table-column prop="amount" label="付款金额" />
        <el-table-column prop="paymentTypeText" label="付款方式" />
        <el-table-column prop="status" label="状态">
          <template #default="{ row }">
            <el-tag :type="getStatusType(row.status)">
              {{ row.statusText }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="requestDate" label="申请日期" />
        <el-table-column label="操作" width="250">
          <template #default="{ row }">
            <el-button link type="primary" @click="handleView(row)">查看</el-button>
            <el-button v-if="row.status === 1" link type="success" @click="handleApprove(row, true)">批准</el-button>
            <el-button v-if="row.status === 1" link type="danger" @click="handleApprove(row, false)">拒绝</el-button>
            <el-button v-if="row.status === 2" link type="warning" @click="handlePay(row)">付款</el-button>
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
        requestNo: 'PAY202603250001',
        supplierName: '某某供应商',
        amount: 10000,
        paymentTypeText: '银行转账',
        status: 1,
        statusText: '待审批',
        requestDate: '2026-03-25'
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
  ElMessage.info('新建付款功能待实现')
}

const handleView = (row: any) => {
  ElMessage.info(`查看付款申请：${row.requestNo}`)
}

const handleApprove = async (row: any, approved: boolean) => {
  try {
    await ElMessageBox.confirm(`确定${approved ? '批准' : '拒绝'}该付款吗？`, '提示', { type: 'warning' })
    ElMessage.success(approved ? '批准成功' : '已拒绝')
    handleSearch()
  } catch (error: any) {
    if (error !== 'cancel') console.error('审批失败:', error)
  }
}

const handlePay = async (row: any) => {
  try {
    await ElMessageBox.confirm(`确定确认付款吗？`, '提示', { type: 'warning' })
    ElMessage.success('付款成功')
    handleSearch()
  } catch (error: any) {
    if (error !== 'cancel') console.error('付款失败:', error)
  }
}

const getStatusType = (status: number) => {
  const types: Record<number, string> = { 0: 'info', 1: 'warning', 2: 'success', 3: 'success', 4: 'danger' }
  return types[status] || 'info'
}
</script>

<style scoped lang="less">
.payment-page {
  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
}
</style>
