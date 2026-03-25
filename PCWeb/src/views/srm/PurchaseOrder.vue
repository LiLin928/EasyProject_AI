<template>
  <div class="purchase-order-page">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>采购订单管理</span>
          <el-button type="primary" @click="handleCreate">
            <el-icon><Plus /></el-icon>
            新建订单
          </el-button>
        </div>
      </template>

      <!-- 搜索栏 -->
      <el-form :model="queryParams" :inline="true">
        <el-form-item label="状态">
          <el-select v-model="queryParams.status" placeholder="请选择" clearable>
            <el-option label="草稿" :value="0" />
            <el-option label="待确认" :value="1" />
            <el-option label="已确认" :value="2" />
            <el-option label="发货中" :value="3" />
            <el-option label="已完成" :value="4" />
            <el-option label="已取消" :value="5" />
          </el-select>
        </el-form-item>
        <el-form-item label="关键词">
          <el-input v-model="queryParams.keyword" placeholder="订单号/供应商" clearable />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleSearch">搜索</el-button>
          <el-button @click="handleReset">重置</el-button>
        </el-form-item>
      </el-form>

      <!-- 表格 -->
      <el-table v-loading="loading" :data="tableData" border>
        <el-table-column prop="orderNo" label="订单号" />
        <el-table-column prop="supplierName" label="供应商" />
        <el-table-column prop="totalAmount" label="总金额" />
        <el-table-column prop="paidAmount" label="已付款" />
        <el-table-column prop="status" label="状态">
          <template #default="{ row }">
            <el-tag :type="getStatusType(row.status)">
              {{ row.statusText }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="orderDate" label="订单日期" />
        <el-table-column label="操作" width="250">
          <template #default="{ row }">
            <el-button link type="primary" @click="handleView(row)">查看</el-button>
            <el-button v-if="row.status === 1" link type="success" @click="handleConfirm(row)">确认</el-button>
            <el-button v-if="row.status === 3" link type="warning" @click="handleReceive(row)">收货</el-button>
            <el-button v-if="row.status === 1" link type="danger" @click="handleDelete(row)">删除</el-button>
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
    // TODO: 调用 API
    tableData.value = [
      {
        id: 1,
        orderNo: 'PO202603250001',
        supplierName: '某某供应商',
        totalAmount: 10000,
        paidAmount: 0,
        status: 1,
        statusText: '待确认',
        orderDate: '2026-03-25'
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
  ElMessage.info('新建采购订单功能待实现')
}

const handleView = (row: any) => {
  ElMessage.info(`查看采购订单：${row.orderNo}`)
}

const handleConfirm = async (row: any) => {
  try {
    await ElMessageBox.confirm(`确定确认该订单吗？`, '提示', { type: 'warning' })
    ElMessage.success('确认成功')
    handleSearch()
  } catch (error: any) {
    if (error !== 'cancel') console.error('确认失败:', error)
  }
}

const handleReceive = async (row: any) => {
  try {
    await ElMessageBox.confirm(`确定确认收货吗？`, '提示', { type: 'warning' })
    ElMessage.success('收货成功')
    handleSearch()
  } catch (error: any) {
    if (error !== 'cancel') console.error('收货失败:', error)
  }
}

const handleDelete = async (row: any) => {
  try {
    await ElMessageBox.confirm(`确定删除采购订单 ${row.orderNo} 吗？`, '提示', { type: 'warning' })
    ElMessage.success('删除成功')
    handleSearch()
  } catch (error: any) {
    if (error !== 'cancel') console.error('删除失败:', error)
  }
}

const getStatusType = (status: number) => {
  const types: Record<number, string> = {
    0: 'info',
    1: 'warning',
    2: 'success',
    3: 'primary',
    4: 'success',
    5: 'info'
  }
  return types[status] || 'info'
}
</script>

<style scoped lang="less">
.purchase-order-page {
  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
}
</style>
