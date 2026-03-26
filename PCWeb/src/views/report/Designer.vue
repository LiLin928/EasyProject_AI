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
        <el-button @click="newTab">
          <el-icon><Plus /></el-icon>
          新建
        </el-button>
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
      <!-- 标签页导航栏 -->
      <div class="tab-navbar">
        <div class="tab-list">
          <div
            v-for="tab in openTabs"
            :key="tab.id"
            class="tab-item"
            :class="{ active: activeTabId === tab.id }"
            @click="switchTab(tab.id)"
          >
            <span class="tab-icon">📊</span>
            <span class="tab-title">{{ tab.name }}</span>
            <el-icon class="tab-close" @click.stop="closeTab(tab.id)"><Close /></el-icon>
          </div>
        </div>
        <div class="tab-actions">
          <el-button type="text" size="small" @click="closeAllTabs">
            <el-icon><Close /></el-icon>
            全部关闭
          </el-button>
        </div>
      </div>

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
      <div class="main-content">
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

// 标签页管理
const openTabs = ref<any[]>([
  { id: 'tab_1', name: '新建报表', reportData: null }
])
const activeTabId = ref('tab_1')
const tabCounter = ref(1)

// 获取当前激活的报表数据
const currentReport = computed(() => {
  const tab = openTabs.value.find(t => t.id === activeTabId.value)
  return tab ? tab.reportData : null
})

// 更新当前报表的字段
const updateCurrentReport = (field: string, value: any) => {
  const tab = openTabs.value.find(t => t.id === activeTabId.value)
  if (tab && tab.reportData) {
    tab.reportData[field] = value
  }
}

const reportName = computed({
  get: () => currentReport.value?.name || '',
  set: (val) => {
    updateCurrentReport('name', val)
    // 更新标签页名称
    const tab = openTabs.value.find(t => t.id === activeTabId.value)
    if (tab) {
      tab.name = val || '新建报表'
    }
  }
})
const reportCode = computed({
  get: () => currentReport.value?.code || '',
  set: (val) => updateCurrentReport('code', val)
})
const reportCategory = computed({
  get: () => currentReport.value?.category || '',
  set: (val) => updateCurrentReport('category', val)
})
const dataSourceType = computed({
  get: () => currentReport.value?.dataSourceType || 'sql',
  set: (val) => updateCurrentReport('dataSourceType', val)
})
const dataSource = computed({
  get: () => currentReport.value?.dataSource || '',
  set: (val) => updateCurrentReport('dataSource', val)
})
const permissionType = computed({
  get: () => currentReport.value?.permissionType || 'none',
  set: (val) => updateCurrentReport('permissionType', val)
})
const defaultSort = computed({
  get: () => currentReport.value?.defaultSort || '',
  set: (val) => updateCurrentReport('defaultSort', val)
})
const isEnabled = computed({
  get: () => currentReport.value?.isEnabled !== false,
  set: (val) => updateCurrentReport('isEnabled', val)
})
const remark = computed({
  get: () => currentReport.value?.remark || '',
  set: (val) => updateCurrentReport('remark', val)
})
const dataSourceSql = computed({
  get: () => currentReport.value?.sql || '',
  set: (val) => updateCurrentReport('sql', val)
})
const activeTab = ref('fields')

// 字段配置
const fieldConfigs = computed({
  get: () => currentReport.value?.fields || [{ fieldName: '', displayName: '', fieldType: 'string', align: 'left', width: 120, visible: true, frozen: false, summary: false }],
  set: (val) => updateCurrentReport('fields', val)
})

// 查询条件
const queryConditions = computed({
  get: () => currentReport.value?.conditions || [{ fieldName: '', displayName: '', controlType: 'text', queryType: 'eq', defaultValue: '', required: false }],
  set: (val) => updateCurrentReport('conditions', val)
})

// 钻取链接
const drillLinks = computed({
  get: () => currentReport.value?.drillLinks || [],
  set: (val) => updateCurrentReport('drillLinks', val)
})

// 解析出的字段
const parsedFields = ref<any[]>([])

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

// 标签页操作
const switchTab = (tabId: string) => {
  activeTabId.value = tabId
}

const closeTab = (tabId: string) => {
  if (openTabs.value.length === 1) {
    ElMessage.warning('至少保留一个标签页')
    return
  }
  
  const index = openTabs.value.findIndex(t => t.id === tabId)
  if (index === -1) return
  
  // 如果关闭的是当前标签，切换到相邻标签
  if (tabId === activeTabId.value) {
    const newIndex = index > 0 ? index - 1 : index + 1
    activeTabId.value = openTabs.value[newIndex].id
  }
  
  openTabs.value.splice(index, 1)
}

const closeAllTabs = () => {
  if (openTabs.value.length === 1) {
    ElMessage.warning('至少保留一个标签页')
    return
  }
  
  openTabs.value = [{ id: `tab_${++tabCounter.value}`, name: '新建报表', reportData: null }]
  activeTabId.value = openTabs.value[0].id
}

const newTab = () => {
  const newTab = {
    id: `tab_${++tabCounter.value}`,
    name: '新建报表',
    reportData: {
      code: 'RPT-' + Date.now(),
      name: '',
      category: '',
      dataSourceType: 'sql',
      dataSource: '',
      permissionType: 'none',
      defaultSort: '',
      isEnabled: true,
      remark: '',
      sql: '',
      fields: [{ fieldName: '', displayName: '', fieldType: 'string', align: 'left', width: 120, visible: true, frozen: false, summary: false }],
      conditions: [{ fieldName: '', displayName: '', controlType: 'text', queryType: 'eq', defaultValue: '', required: false }],
      drillLinks: []
    }
  }
  openTabs.value.push(newTab)
  activeTabId.value = newTab.id
}

// 初始化
onMounted(() => {
  // 初始化第一个标签页的数据
  openTabs.value[0].reportData = {
    code: 'RPT-' + Date.now(),
    name: '',
    category: '',
    dataSourceType: 'sql',
    dataSource: '',
    permissionType: 'none',
    defaultSort: '',
    isEnabled: true,
    remark: '',
    sql: '',
    fields: [{ fieldName: '', displayName: '', fieldType: 'string', align: 'left', width: 120, visible: true, frozen: false, summary: false }],
    conditions: [{ fieldName: '', displayName: '', controlType: 'text', queryType: 'eq', defaultValue: '', required: false }],
    drillLinks: []
  }
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
  flex-direction: column;
  overflow: hidden;
}

.tab-navbar {
  height: 40px;
  background: #f5f7fa;
  border-bottom: 1px solid #e4e7ed;
  display: flex;
  align-items: center;
  padding: 0 16px;
  gap: 16px;
  
  .tab-list {
    flex: 1;
    display: flex;
    align-items: center;
    gap: 4px;
    overflow-x: auto;
    
    .tab-item {
      display: flex;
      align-items: center;
      gap: 8px;
      padding: 6px 12px;
      background: #fff;
      border: 1px solid #e4e7ed;
      border-bottom: none;
      border-radius: 4px 4px 0 0;
      cursor: pointer;
      transition: all 0.3s;
      font-size: 13px;
      white-space: nowrap;
      
      &:hover {
        background: #ecf5ff;
        border-color: #409EFF;
        
        .tab-close {
          opacity: 1;
        }
      }
      
      &.active {
        background: #fff;
        border-color: #409EFF;
        color: #409EFF;
      }
      
      .tab-icon {
        font-size: 14px;
      }
      
      .tab-title {
        max-width: 150px;
        overflow: hidden;
        text-overflow: ellipsis;
      }
      
      .tab-close {
        width: 16px;
        height: 16px;
        border-radius: 50%;
        opacity: 0;
        transition: opacity 0.3s;
        
        &:hover {
          background: #F56C6C;
          color: #fff;
        }
      }
    }
  }
  
  .tab-actions {
    .el-button {
      color: #606266;
      
      &:hover {
        color: #F56C6C;
      }
    }
  }
}

.config-panel {
  width: 340px;
  border-right: 1px solid #e4e7ed;
  padding: 20px;
  background: #fafafa;
  overflow-y: auto;
  flex-shrink: 0;
}

.main-content {
  flex: 1;
  display: flex;
  overflow: hidden;
}

.main-panel {
  flex: 1;
  padding: 20px;
  background: #fff;
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
