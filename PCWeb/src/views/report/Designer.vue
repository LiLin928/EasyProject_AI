<template>
  <div class="report-designer-container">
    <!-- 顶部导航 -->
    <div class="designer-header">
      <div class="header-left">
        <el-button @click="backToList">
          <el-icon><Back /></el-icon>
          返回
        </el-button>
        <span class="report-title">{{ reportName || '新建报表' }}</span>
      </div>
      <div class="header-center">
        <el-input
          v-model="reportName"
          placeholder="输入报表名称"
          style="width: 300px"
          clearable
        >
          <template #prefix>
            <el-icon><Edit /></el-icon>
          </template>
        </el-input>
      </div>
      <div class="header-right">
        <el-button @click="saveReport">
          <el-icon><Save /></el-icon>
          保存
        </el-button>
        <el-button type="primary" @click="previewReport">
          <el-icon><View /></el-icon>
          预览
        </el-button>
      </div>
    </div>

    <div class="designer-body">
      <!-- 左侧配置面板 -->
      <div class="config-panel">
        <div class="panel-section">
          <div class="section-title">基本信息</div>
          <el-form label-position="top" size="small">
            <el-form-item label="报表编码">
              <el-input v-model="reportCode" placeholder="自动生成" disabled />
            </el-form-item>
            
            <el-form-item label="报表名称" required>
              <el-input v-model="reportName" placeholder="请输入报表名称" />
            </el-form-item>
            
            <el-form-item label="报表分类">
              <el-select v-model="reportCategory" placeholder="请选择分类" style="width: 100%">
                <el-option label="销售报表" value="sales" />
                <el-option label="财务报表" value="finance" />
                <el-option label="运营报表" value="operation" />
                <el-option label="用户报表" value="user" />
              </el-select>
            </el-form-item>
            
            <el-form-item label="数据源类型">
              <el-select v-model="dataSourceType" placeholder="选择数据源类型" style="width: 100%">
                <el-option label="SQL 语句" value="sql" />
                <el-option label="存储过程" value="procedure" />
                <el-option label="API 接口" value="api" />
              </el-select>
            </el-form-item>
            
            <el-form-item label="数据源选择">
              <el-select v-model="dataSource" placeholder="选择数据源" style="width: 100%">
                <el-option label="MySQL-生产库" value="mysql_prod" />
                <el-option label="MySQL-测试库" value="mysql_test" />
                <el-option label="Oracle-数仓" value="oracle_dw" />
              </el-select>
            </el-form-item>
            
            <el-form-item label="权限类型">
              <el-select v-model="permissionType" placeholder="选择权限类型" style="width: 100%">
                <el-option label="无限制" value="none" />
                <el-option label="按部门" value="department" />
                <el-option label="按角色" value="role" />
                <el-option label="按用户" value="user" />
              </el-select>
            </el-form-item>
            
            <el-form-item label="默认排序">
              <el-input v-model="defaultSort" placeholder="如：CreateDate DESC" />
            </el-form-item>
            
            <el-form-item label="是否启用">
              <el-switch v-model="isEnabled" />
            </el-form-item>
            
            <el-form-item label="备注">
              <el-input
                v-model="remark"
                type="textarea"
                :rows="3"
                placeholder="输入备注信息"
              />
            </el-form-item>
          </el-form>
        </div>
        
        <el-divider />
        
        <div class="panel-section">
          <div class="section-title">数据源 SQL</div>
          <el-input
            v-model="dataSourceSql"
            type="textarea"
            :rows="6"
            placeholder="请输入 SQL 语句，支持参数占位符：{{参数名}}"
            class="sql-editor"
          />
          <el-button type="primary" @click="parseFields" style="width: 100%; margin-top: 12px">
            <el-icon><MagicStick /></el-icon>
            解析字段
          </el-button>
        </div>
      </div>

      <!-- 右侧配置区域 -->
      <div class="main-panel">
        <el-tabs v-model="activeTab" type="border-card">
          <!-- 字段配置 -->
          <el-tab-pane label="字段配置" name="fields">
            <div class="tab-header">
              <span class="tab-tip">拖动行可调整顺序</span>
            </div>
            
            <el-table :data="fieldConfigs" border style="width: 100%">
              <el-table-column width="50" align="center">
                <template #header>
                  <el-icon><Rank /></el-icon>
                </template>
                <template #default>
                  <el-icon class="drag-handle"><Rank /></el-icon>
                </template>
              </el-table-column>
              
              <el-table-column label="字段名" width="150">
                <template #default="{ row }">
                  <el-select v-model="row.fieldName" placeholder="选择字段" style="width: 100%">
                    <el-option
                      v-for="field in parsedFields"
                      :key="field.name"
                      :label="field.name"
                      :value="field.name"
                    />
                  </el-select>
                </template>
              </el-table-column>
              
              <el-table-column label="显示名称" width="150">
                <template #default="{ row }">
                  <el-input v-model="row.displayName" placeholder="输入显示名称" />
                </template>
              </el-table-column>
              
              <el-table-column label="字段类型" width="120">
                <template #default="{ row }">
                  <el-select v-model="row.fieldType" placeholder="选择类型" style="width: 100%">
                    <el-option label="字符串" value="string" />
                    <el-option label="数字" value="number" />
                    <el-option label="日期" value="date" />
                    <el-option label="金额" value="money" />
                    <el-option label="百分比" value="percent" />
                  </el-select>
                </template>
              </el-table-column>
              
              <el-table-column label="对齐" width="100">
                <template #default="{ row }">
                  <el-select v-model="row.align" placeholder="对齐" style="width: 100%">
                    <el-option label="左" value="left" />
                    <el-option label="中" value="center" />
                    <el-option label="右" value="right" />
                  </el-select>
                </template>
              </el-table-column>
              
              <el-table-column label="宽度" width="100">
                <template #default="{ row }">
                  <el-input-number v-model="row.width" :min="50" :max="500" :step="10" style="width: 100%" />
                </template>
              </el-table-column>
              
              <el-table-column label="显示" width="60" align="center">
                <template #default="{ row }">
                  <el-checkbox v-model="row.visible" />
                </template>
              </el-table-column>
              
              <el-table-column label="冻结" width="60" align="center">
                <template #default="{ row }">
                  <el-checkbox v-model="row.frozen" />
                </template>
              </el-table-column>
              
              <el-table-column label="汇总" width="60" align="center">
                <template #default="{ row }">
                  <el-checkbox v-model="row.summary" />
                </template>
              </el-table-column>
              
              <el-table-column label="操作" width="80" align="center">
                <template #default="{ row, $index }">
                  <el-button type="text" style="color: #F56C6C" @click="removeField($index)">
                    删除
                  </el-button>
                </template>
              </el-table-column>
            </el-table>
            
            <el-button type="primary" @click="addField" style="margin-top: 12px">
              <el-icon><Plus /></el-icon>
              添加字段
            </el-button>
          </el-tab-pane>
          
          <!-- 查询条件 -->
          <el-tab-pane label="查询条件" name="conditions">
            <div class="tab-header">
              <span class="tab-tip">拖动行可调整顺序</span>
            </div>
            
            <el-table :data="queryConditions" border style="width: 100%">
              <el-table-column width="50" align="center">
                <template #header>
                  <el-icon><Rank /></el-icon>
                </template>
                <template #default>
                  <el-icon class="drag-handle"><Rank /></el-icon>
                </template>
              </el-table-column>
              
              <el-table-column label="字段名" width="150">
                <template #default="{ row }">
                  <el-select v-model="row.fieldName" placeholder="选择字段" style="width: 100%">
                    <el-option
                      v-for="field in parsedFields"
                      :key="field.name"
                      :label="field.name"
                      :value="field.name"
                    />
                  </el-select>
                </template>
              </el-table-column>
              
              <el-table-column label="显示名称" width="150">
                <template #default="{ row }">
                  <el-input v-model="row.displayName" placeholder="输入显示名称" />
                </template>
              </el-table-column>
              
              <el-table-column label="控件类型" width="120">
                <template #default="{ row }">
                  <el-select v-model="row.controlType" placeholder="选择控件" style="width: 100%">
                    <el-option label="文本" value="text" />
                    <el-option label="下拉框" value="select" />
                    <el-option label="日期" value="date" />
                    <el-option label="日期范围" value="daterange" />
                    <el-option label="多选框" value="checkbox" />
                  </el-select>
                </template>
              </el-table-column>
              
              <el-table-column label="查询方式" width="120">
                <template #default="{ row }">
                  <el-select v-model="row.queryType" placeholder="查询方式" style="width: 100%">
                    <el-option label="等于" value="eq" />
                    <el-option label="包含" value="like" />
                    <el-option label="大于" value="gt" />
                    <el-option label="小于" value="lt" />
                    <el-option label="介于" value="between" />
                  </el-select>
                </template>
              </el-table-column>
              
              <el-table-column label="默认值" width="150">
                <template #default="{ row }">
                  <el-input v-model="row.defaultValue" placeholder="默认值" />
                </template>
              </el-table-column>
              
              <el-table-column label="必填" width="60" align="center">
                <template #default="{ row }">
                  <el-checkbox v-model="row.required" />
                </template>
              </el-table-column>
              
              <el-table-column label="操作" width="80" align="center">
                <template #default="{ row, $index }">
                  <el-button type="text" style="color: #F56C6C" @click="removeCondition($index)">
                    删除
                  </el-button>
                </template>
              </el-table-column>
            </el-table>
            
            <el-button type="primary" @click="addCondition" style="margin-top: 12px">
              <el-icon><Plus /></el-icon>
              添加条件
            </el-button>
          </el-tab-pane>
          
          <!-- 钻取链接 -->
          <el-tab-pane label="钻取链接" name="drill">
            <div class="tab-header">
              <div class="tab-tip">
                <el-icon><InfoFilled /></el-icon>
                参数映射说明：使用 {} 格式从主报表查询条件传递参数到钻取报表，例如：{id} 表示从主报表的 id 条件获取值
              </div>
            </div>
            
            <el-table :data="drillLinks" border style="width: 100%">
              <el-table-column width="50" align="center">
                <template #header>
                  <el-icon><Rank /></el-icon>
                </template>
                <template #default>
                  <el-icon class="drag-handle"><Rank /></el-icon>
                </template>
              </el-table-column>
              
              <el-table-column label="触发字段" width="150">
                <template #default="{ row }">
                  <el-input v-model="row.triggerField" placeholder="点击字段名" />
                </template>
              </el-table-column>
              
              <el-table-column label="链接类型" width="150">
                <template #default="{ row }">
                  <el-select v-model="row.linkType" placeholder="选择类型" style="width: 100%">
                    <el-option label="钻取报表" value="report" />
                    <el-option label="外部 URL" value="url" />
                    <el-option label="详情页" value="detail" />
                  </el-select>
                </template>
              </el-table-column>
              
              <el-table-column label="目标 URL/报表" width="200">
                <template #default="{ row }">
                  <el-input v-model="row.targetUrl" placeholder="URL 或报表 ID" />
                </template>
              </el-table-column>
              
              <el-table-column label="参数映射" width="250">
                <template #default="{ row }">
                  <el-input v-model="row.paramMapping" placeholder="如：id={id}&name={Name}" />
                </template>
              </el-table-column>
              
              <el-table-column label="操作" width="80" align="center">
                <template #default="{ row, $index }">
                  <el-button type="text" style="color: #F56C6C" @click="removeDrill($index)">
                    删除
                  </el-button>
                </template>
              </el-table-column>
            </el-table>
            
            <el-button type="primary" @click="addDrill" style="margin-top: 12px">
              <el-icon><Plus /></el-icon>
              添加链接
            </el-button>
          </el-tab-pane>
        </el-tabs>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage, ElMessageBox } from 'element-plus'
import {
  Back,
  FolderChecked as Save,
  View,
  Edit,
  Plus,
  Rank,
  MagicStick,
  InfoFilled
} from '@element-plus/icons-vue'

const router = useRouter()
const reportName = ref('')
const reportCode = ref('RPT-' + Date.now())
const reportCategory = ref('')
const dataSourceType = ref('sql')
const dataSource = ref('')
const permissionType = ref('none')
const defaultSort = ref('')
const isEnabled = ref(true)
const remark = ref('')
const dataSourceSql = ref('')
const activeTab = ref('fields')

// 解析出的字段
const parsedFields = ref<any[]>([])

// 字段配置
const fieldConfigs = ref<any[]>([
  { fieldName: '', displayName: '', fieldType: 'string', align: 'left', width: 120, visible: true, frozen: false, summary: false }
])

// 查询条件
const queryConditions = ref<any[]>([
  { fieldName: '', displayName: '', controlType: 'text', queryType: 'eq', defaultValue: '', required: false }
])

// 钻取链接
const drillLinks = ref<any[]>([])

// 解析字段
const parseFields = () => {
  if (!dataSourceSql.value.trim()) {
    ElMessage.warning('请先输入 SQL 语句')
    return
  }
  
  // 模拟解析 SQL 获取字段
  const mockFields = [
    { name: 'id', type: 'number' },
    { name: 'name', type: 'string' },
    { name: 'amount', type: 'money' },
    { name: 'createDate', type: 'date' },
    { name: 'status', type: 'string' }
  ]
  
  parsedFields.value = mockFields
  ElMessage.success(`解析成功，共 ${mockFields.length} 个字段`)
}

// 添加字段
const addField = () => {
  fieldConfigs.value.push({
    fieldName: '',
    displayName: '',
    fieldType: 'string',
    align: 'left',
    width: 120,
    visible: true,
    frozen: false,
    summary: false
  })
}

// 删除字段
const removeField = (index: number) => {
  if (fieldConfigs.value.length === 1) {
    ElMessage.warning('至少保留一个字段')
    return
  }
  fieldConfigs.value.splice(index, 1)
}

// 添加条件
const addCondition = () => {
  queryConditions.value.push({
    fieldName: '',
    displayName: '',
    controlType: 'text',
    queryType: 'eq',
    defaultValue: '',
    required: false
  })
}

// 删除条件
const removeCondition = (index: number) => {
  if (queryConditions.value.length === 1) {
    ElMessage.warning('至少保留一个条件')
    return
  }
  queryConditions.value.splice(index, 1)
}

// 添加钻取链接
const addDrill = () => {
  drillLinks.value.push({
    triggerField: '',
    linkType: 'report',
    targetUrl: '',
    paramMapping: ''
  })
}

// 删除钻取链接
const removeDrill = (index: number) => {
  drillLinks.value.splice(index, 1)
}

// 保存报表
const saveReport = () => {
  if (!reportName.value.trim()) {
    ElMessage.warning('请输入报表名称')
    return
  }
  
  const reportData = {
    code: reportCode.value,
    name: reportName.value,
    category: reportCategory.value,
    dataSourceType: dataSourceType.value,
    dataSource: dataSource.value,
    permissionType: permissionType.value,
    defaultSort: defaultSort.value,
    isEnabled: isEnabled.value,
    remark: remark.value,
    sql: dataSourceSql.value,
    fields: fieldConfigs.value,
    conditions: queryConditions.value,
    drillLinks: drillLinks.value
  }
  
  console.log('保存报表:', reportData)
  ElMessage.success('报表已保存')
}

// 预览报表
const previewReport = () => {
  if (!reportName.value.trim()) {
    ElMessage.warning('请输入报表名称')
    return
  }
  
  ElMessage.info('预览功能开发中')
}

// 返回列表
const backToList = () => {
  router.push('/report/list')
}

// 初始化
onMounted(() => {
  // 可以从路由参数加载已有报表配置
})
</script>

<style scoped lang="less">
.report-designer-container {
  height: 100vh;
  display: flex;
  flex-direction: column;
  background: #fff;
}

.designer-header {
  height: 60px;
  border-bottom: 1px solid #e4e7ed;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0 20px;
  
  .header-left, .header-right {
    display: flex;
    align-items: center;
    gap: 12px;
  }
  
  .report-title {
    font-size: 18px;
    font-weight: 600;
    color: #303133;
  }
}

.designer-body {
  flex: 1;
  display: flex;
  overflow: hidden;
}

.config-panel {
  width: 340px;
  border-right: 1px solid #e4e7ed;
  padding: 20px;
  background: #fafafa;
  overflow-y: auto;
  
  .panel-section {
    margin-bottom: 20px;
    
    .section-title {
      font-size: 14px;
      font-weight: 600;
      color: #303133;
      margin-bottom: 16px;
    }
  }
  
  .sql-editor {
    font-family: 'Courier New', monospace;
    font-size: 13px;
  }
}

.main-panel {
  flex: 1;
  padding: 20px;
  background: #fff;
  overflow-y: auto;
  
  .tab-header {
    margin-bottom: 16px;
    
    .tab-tip {
      font-size: 13px;
      color: #909399;
      
      .el-icon {
        margin-right: 4px;
      }
    }
  }
  
  .drag-handle {
    cursor: move;
    color: #C0C4CC;
    
    &:hover {
      color: #409EFF;
    }
  }
}

:deep(.el-tabs__content) {
  padding: 0;
}

:deep(.el-table) {
  font-size: 13px;
  
  .el-input, .el-select {
    width: 100%;
  }
}
</style>
