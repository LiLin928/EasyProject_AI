<template>
  <div class="report-preview-container">
    <!-- 顶部工具栏 -->
    <div class="preview-header">
      <div class="header-left">
        <el-button @click="backToDesigner">
          <el-icon><Back /></el-icon>
          返回设计
        </el-button>
        <span class="report-title">{{ report?.name || '报表预览' }}</span>
      </div>
      <div class="header-right">
        <el-button @click="refreshData">
          <el-icon><Refresh /></el-icon>
          刷新数据
        </el-button>
        <el-button @click="exportData">
          <el-icon><Download /></el-icon>
          导出
        </el-button>
      </div>
    </div>

    <!-- 查询条件区 -->
    <div class="query-section" v-if="report?.conditions && report.conditions.length > 0">
      <el-form :inline="true" :model="queryForm" size="small">
        <el-form-item
          v-for="condition in report.conditions"
          :key="condition.fieldName"
          :label="condition.displayName"
        >
          <!-- 文本输入 -->
          <el-input
            v-if="condition.controlType === 'text'"
            v-model="queryForm[condition.fieldName]"
            :placeholder="`请输入${condition.displayName}`"
            clearable
          />
          
          <!-- 下拉选择 -->
          <el-select
            v-else-if="condition.controlType === 'select'"
            v-model="queryForm[condition.fieldName]"
            :placeholder="`请选择${condition.displayName}`"
            clearable
          >
            <el-option label="选项 1" value="1" />
            <el-option label="选项 2" value="2" />
          </el-select>
          
          <!-- 日期选择 -->
          <el-date-picker
            v-else-if="condition.controlType === 'date'"
            v-model="queryForm[condition.fieldName]"
            type="date"
            :placeholder="`选择${condition.displayName}`"
            style="width: 180px"
          />
          
          <!-- 日期范围 -->
          <el-date-picker
            v-else-if="condition.controlType === 'daterange'"
            v-model="queryForm[condition.fieldName]"
            type="daterange"
            range-separator="至"
            start-placeholder="开始日期"
            end-placeholder="结束日期"
            style="width: 240px"
          />
        </el-form-item>
        
        <el-form-item>
          <el-button type="primary" @click="handleQuery">查询</el-button>
          <el-button @click="resetQuery">重置</el-button>
        </el-form-item>
      </el-form>
    </div>

    <!-- 数据表格区 -->
    <div class="table-section">
      <el-table
        :data="tableData"
        v-loading="loading"
        border
        stripe
        style="width: 100%"
        :height="tableHeight"
      >
        <el-table-column
          v-for="field in visibleFields"
          :key="field.fieldName"
          :prop="field.fieldName"
          :label="field.displayName"
          :width="field.width"
          :align="field.align"
          :fixed="field.frozen ? 'left' : false"
        >
          <template #default="{ row }">
            <span v-if="field.fieldType === 'money'">
              ¥{{ formatMoney(row[field.fieldName]) }}
            </span>
            <span v-else-if="field.fieldType === 'date'">
              {{ formatDate(row[field.fieldName]) }}
            </span>
            <span v-else-if="field.fieldType === 'percent'">
              {{ row[field.fieldName] }}%
            </span>
            <span v-else>
              {{ row[field.fieldName] }}
            </span>
          </template>
        </el-table-column>
      </el-table>
      
      <!-- 分页 -->
      <el-pagination
        v-model:current-page="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        :page-sizes="[10, 20, 50, 100]"
        layout="total, sizes, prev, pager, next, jumper"
        @current-change="handlePageChange"
        @size-change="handleSizeChange"
        style="margin-top: 16px; justify-content: flex-end"
      />
    </div>

    <!-- 汇总区 -->
    <div class="summary-section" v-if="showSummary">
      <el-table :data="summaryData" border style="width: 100%">
        <el-table-column label="汇总项" width="120" />
        <el-table-column
          v-for="field in summaryFields"
          :key="field.fieldName"
          :label="field.displayName"
          :width="field.width"
          :align="field.align"
        >
          <template #default>
            {{ calculateSummary(field) }}
          </template>
        </el-table-column>
      </el-table>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { ElMessage } from 'element-plus'
import { Back, Refresh, Download } from '@element-plus/icons-vue'

const router = useRouter()
const route = useRoute()

const report = ref<any>(null)
const loading = ref(false)
const tableData = ref<any[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const tableHeight = ref(500)

// 查询表单
const queryForm = reactive<any>({})

// 获取报表配置
const fetchReportConfig = async () => {
  const reportId = route.query.id as string
  if (!reportId) {
    ElMessage.error('报表 ID 不能为空')
    return
  }
  
  loading.value = true
  try {
    // TODO: 调用 API 获取报表配置
    // 模拟数据
    report.value = {
      id: reportId,
      name: '销售报表',
      fields: [
        { fieldName: 'id', displayName: 'ID', fieldType: 'number', width: 80, align: 'center', visible: true, frozen: false, summary: false },
        { fieldName: 'orderNo', displayName: '订单号', fieldType: 'string', width: 150, align: 'left', visible: true, frozen: true, summary: false },
        { fieldName: 'customerName', displayName: '客户名称', fieldType: 'string', width: 150, align: 'left', visible: true, frozen: false, summary: false },
        { fieldName: 'amount', displayName: '金额', fieldType: 'money', width: 120, align: 'right', visible: true, frozen: false, summary: true },
        { fieldName: 'status', displayName: '状态', fieldType: 'string', width: 100, align: 'center', visible: true, frozen: false, summary: false },
        { fieldName: 'createDate', displayName: '创建日期', fieldType: 'date', width: 120, align: 'center', visible: true, frozen: false, summary: false }
      ],
      conditions: [
        { fieldName: 'orderNo', displayName: '订单号', controlType: 'text', queryType: 'like', defaultValue: '', required: false },
        { fieldName: 'customerName', displayName: '客户名称', controlType: 'text', queryType: 'like', defaultValue: '', required: false },
        { fieldName: 'status', displayName: '状态', controlType: 'select', queryType: 'eq', defaultValue: '', required: false },
        { fieldName: 'createDate', displayName: '创建日期', controlType: 'daterange', queryType: 'between', defaultValue: '', required: false }
      ]
    }
    
    // 初始化查询表单
    report.value.conditions.forEach((cond: any) => {
      queryForm[cond.fieldName] = cond.defaultValue
    })
    
    // 加载数据
    await loadData()
  } catch (error) {
    ElMessage.error('加载报表配置失败')
  } finally {
    loading.value = false
  }
}

// 加载数据
const loadData = async () => {
  loading.value = true
  try {
    // TODO: 调用 API 加载数据
    // 模拟数据
    tableData.value = []
    for (let i = 0; i < 20; i++) {
      tableData.value.push({
        id: i + 1,
        orderNo: `ORD${Date.now()}${i}`,
        customerName: `客户${i + 1}`,
        amount: Math.random() * 10000,
        status: ['待支付', '已支付', '已完成', '已取消'][Math.floor(Math.random() * 4)],
        createDate: new Date(Date.now() - Math.random() * 30 * 24 * 60 * 60 * 1000).toISOString()
      })
    }
    total.value = 100
  } catch (error) {
    ElMessage.error('加载数据失败')
  } finally {
    loading.value = false
  }
}

// 可见字段
const visibleFields = computed(() => {
  return report.value?.fields?.filter((f: any) => f.visible) || []
})

// 汇总字段
const summaryFields = computed(() => {
  return report.value?.fields?.filter((f: any) => f.summary) || []
})

// 是否显示汇总
const showSummary = computed(() => {
  return summaryFields.value.length > 0
})

// 汇总数据
const summaryData = computed(() => {
  return [{ name: '汇总' }]
})

// 计算汇总
const calculateSummary = (field: any) => {
  if (!tableData.value.length) return 0
  
  const sum = tableData.value.reduce((acc, row) => {
    return acc + (Number(row[field.fieldName]) || 0)
  }, 0)
  
  if (field.fieldType === 'money') {
    return `¥${formatMoney(sum)}`
  }
  if (field.fieldType === 'percent') {
    return `${sum}%`
  }
  return sum.toFixed(2)
}

// 格式化金额
const formatMoney = (value: number) => {
  return value?.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,')
}

// 格式化日期
const formatDate = (value: string) => {
  if (!value) return ''
  return new Date(value).toLocaleDateString('zh-CN')
}

// 查询
const handleQuery = () => {
  currentPage.value = 1
  loadData()
}

// 重置查询
const resetQuery = () => {
  report.value?.conditions.forEach((cond: any) => {
    queryForm[cond.fieldName] = cond.defaultValue
  })
  handleQuery()
}

// 分页变化
const handlePageChange = (page: number) => {
  currentPage.value = page
  loadData()
}

const handleSizeChange = (size: number) => {
  pageSize.value = size
  currentPage.value = 1
  loadData()
}

// 刷新数据
const refreshData = () => {
  loadData()
  ElMessage.success('数据已刷新')
}

// 导出数据
const exportData = () => {
  ElMessage.info('导出功能开发中')
}

// 返回设计器
const backToDesigner = () => {
  router.push('/report/designer')
}

onMounted(() => {
  fetchReportConfig()
})
</script>

<style scoped lang="less">
.report-preview-container {
  height: 100vh;
  display: flex;
  flex-direction: column;
  background: #f5f7fa;
}

.preview-header {
  height: 60px;
  background: #fff;
  border-bottom: 1px solid #e4e7ed;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0 20px;
  
  .header-left {
    display: flex;
    align-items: center;
    gap: 12px;
    
    .report-title {
      font-size: 18px;
      font-weight: 600;
      color: #303133;
    }
  }
}

.query-section {
  background: #fff;
  padding: 16px 20px;
  border-bottom: 1px solid #e4e7ed;
}

.table-section {
  flex: 1;
  background: #fff;
  margin: 16px;
  padding: 20px;
  overflow: auto;
}

.summary-section {
  background: #fff;
  margin: 0 16px 16px;
  padding: 20px;
}
</style>
