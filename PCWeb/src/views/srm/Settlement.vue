<template>
  <div class="settlement-page">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>结算管理</span>
          <el-button type="primary" @click="handleCreate">
            <el-icon><Plus /></el-icon>
            新建结算
          </el-button>
        </div>
      </template>

      <!-- 搜索栏 -->
      <el-form :model="queryParams" :inline="true">
        <el-form-item label="状态">
          <el-select v-model="queryParams.status" placeholder="请选择" clearable>
            <el-option label="待结算" :value="0" />
            <el-option label="结算中" :value="1" />
            <el-option label="已完成" :value="2" />
          </el-select>
        </el-form-item>
        <el-form-item label="关键词">
          <el-input v-model="queryParams.keyword" placeholder="结算单号/供应商" clearable />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleSearch">搜索</el-button>
          <el-button @click="handleReset">重置</el-button>
        </el-form-item>
      </el-form>

      <!-- 表格 -->
      <el-table v-loading="loading" :data="tableData" border>
        <el-table-column prop="settlementNo" label="结算单号" />
        <el-table-column prop="supplierName" label="供应商" />
        <el-table-column prop="totalAmount" label="结算金额" />
        <el-table-column prop="paidAmount" label="已付金额" />
        <el-table-column prop="unpaidAmount" label="未付金额" />
        <el-table-column prop="status" label="状态">
          <template #default="{ row }">
            <el-tag :type="getStatusType(row.status)">
              {{ row.statusText }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="150">
          <template #default="{ row }">
            <el-button link type="primary" @click="handleView(row)">查看</el-button>
            <el-button v-if="row.status !== 2" link type="success" @click="handleComplete(row)">完成</el-button>
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
        settlementNo: 'SET202603250001',
        supplierName: '某某供应商',
        totalAmount: 100000,
        paidAmount: 80000,
        unpaidAmount: 20000,
        status: 1,
        statusText: '结算中'
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
  ElMessage.info('新建结算功能待实现')
}

const handleView = (row: any) => {
  ElMessage.info(`查看结算单：${row.settlementNo}`)
}

const handleComplete = async (row: any) => {
  try {
    await ElMessageBox.confirm(`确定完成该结算吗？`, '提示', { type: 'warning' })
    ElMessage.success('结算完成')
    handleSearch()
  } catch (error: any) {
    if (error !== 'cancel') console.error('结算失败:', error)
  }
}

const getStatusType = (status: number) => {
  const types: Record<number, string> = { 0: 'info', 1: 'warning', 2: 'success' }
  return types[status] || 'info'
}
</script>

<style scoped lang="less">
.settlement-page {
  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
}
</style>
