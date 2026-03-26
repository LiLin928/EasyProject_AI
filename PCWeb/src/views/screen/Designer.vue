<template>
  <div class="screen-designer-container">
    <!-- 顶部导航 -->
    <div class="designer-header">
      <div class="header-left">
        <el-button @click="backToList">
          <el-icon><Back /></el-icon>
          返回
        </el-button>
        <span class="screen-title">{{ screenName || '新建大屏' }}</span>
      </div>
      <div class="header-center">
        <el-input
          v-model="screenName"
          placeholder="输入大屏名称"
          style="width: 300px"
          clearable
        />
      </div>
      <div class="header-right">
        <el-button @click="preview">
          <el-icon><VideoPlay /></el-icon>
          预览
        </el-button>
        <el-button @click="saveScreen">
          <el-icon><Save /></el-icon>
          保存
        </el-button>
        <el-button type="primary" @click="publishScreen">
          <el-icon><Upload /></el-icon>
          发布
        </el-button>
      </div>
    </div>

    <div class="designer-body">
      <!-- 左侧组件面板 -->
      <div class="component-panel">
        <div class="panel-title">组件列表</div>
        <el-input
          v-model="componentSearch"
          placeholder="搜索组件..."
          size="small"
          clearable
          style="margin-bottom: 12px"
        />
        
        <el-collapse v-model="activeCategories" accordion>
          <el-collapse-item title="📊 基础图表" name="chart">
            <div
              class="component-item"
              v-for="comp in filteredComponents.chart"
              :key="comp.type"
              draggable="true"
              @dragstart="onDragStart($event, comp)"
            >
              <el-icon :component="comp.icon" />
              <span>{{ comp.name }}</span>
            </div>
          </el-collapse-item>
          
          <el-collapse-item title="📈 统计卡片" name="card">
            <div
              class="component-item"
              v-for="comp in filteredComponents.card"
              :key="comp.type"
              draggable="true"
              @dragstart="onDragStart($event, comp)"
            >
              <el-icon :component="comp.icon" />
              <span>{{ comp.name }}</span>
            </div>
          </el-collapse-item>
          
          <el-collapse-item title="📋 列表表格" name="table">
            <div
              class="component-item"
              v-for="comp in filteredComponents.table"
              :key="comp.type"
              draggable="true"
              @dragstart="onDragStart($event, comp)"
            >
              <el-icon :component="comp.icon" />
              <span>{{ comp.name }}</span>
            </div>
          </el-collapse-item>
          
          <el-collapse-item title="🎨 其他组件" name="other">
            <div
              class="component-item"
              v-for="comp in filteredComponents.other"
              :key="comp.type"
              draggable="true"
              @dragstart="onDragStart($event, comp)"
            >
              <el-icon :component="comp.icon" />
              <span>{{ comp.name }}</span>
            </div>
          </el-collapse-item>
        </el-collapse>
      </div>

      <!-- 中间画布 -->
      <div class="canvas-container">
        <div class="canvas-toolbar">
          <el-button-group>
            <el-button @click="zoomOut">
              <el-icon><ZoomOut /></el-icon>
            </el-button>
            <el-button @click="resetZoom">{{ Math.round(canvasZoom * 100) }}%</el-button>
            <el-button @click="zoomIn">
              <el-icon><ZoomIn /></el-icon>
            </el-button>
          </el-button-group>
          <el-divider direction="vertical" />
          <el-button @click="toggleGrid">
            <el-icon><Grid /></el-icon>
            网格：{{ showGrid ? '开' : '关' }}
          </el-button>
          <el-button @click="clearCanvas">
            <el-icon><Delete /></el-icon>
            清空
          </el-button>
        </div>
        
        <div
          class="canvas"
          ref="canvasRef"
          @dragover="onDragOver"
          @drop="onDrop"
          @click="onCanvasClick"
        >
          <!-- 网格背景 -->
          <div class="canvas-grid" v-if="showGrid" :style="{ transform: `scale(${canvasZoom})` }"></div>
          
          <div class="canvas-content" :style="{ transform: `scale(${canvasZoom})` }">
            <div
              v-for="widget in widgets"
              :key="widget.id"
              class="widget"
              :class="{ selected: selectedWidget?.id === widget.id }"
              :style="getWidgetStyle(widget)"
              @click.stop="selectWidget(widget)"
              @mousedown="startDragWidget($event, widget)"
            >
              <div class="widget-header" v-if="selectedWidget?.id === widget.id">
                <span class="widget-title">{{ widget.name }}</span>
                <div class="widget-actions">
                  <el-button type="text" size="small" @click.stop="duplicateWidget(widget)">
                    <el-icon><CopyDocument /></el-icon>
                  </el-button>
                  <el-button type="text" size="small" @click.stop="deleteWidget(widget.id)">
                    <el-icon><Delete /></el-icon>
                  </el-button>
                </div>
              </div>
              
              <div class="widget-content">
                <!-- 图表组件 -->
                <div v-if="widget.type === 'line'" class="chart-placeholder">
                  <div class="chart-title">{{ widget.name }}</div>
                  <v-chart :option="getChartOption(widget)" autoresize />
                </div>
                
                <div v-else-if="widget.type === 'bar'" class="chart-placeholder">
                  <div class="chart-title">{{ widget.name }}</div>
                  <v-chart :option="getChartOption(widget)" autoresize />
                </div>
                
                <div v-else-if="widget.type === 'pie'" class="chart-placeholder">
                  <div class="chart-title">{{ widget.name }}</div>
                  <v-chart :option="getChartOption(widget)" autoresize />
                </div>
                
                <div v-else-if="widget.type === 'numberCard'" class="card-placeholder">
                  <div class="card-value">{{ widget.config?.value || '0' }}</div>
                  <div class="card-label">{{ widget.config?.label || '指标' }}</div>
                </div>
                
                <div v-else-if="widget.type === 'table'" class="table-placeholder">
                  <div class="table-title">{{ widget.name }}</div>
                  <el-table :data="widget.config?.data || []" size="small" style="width: 100%">
                    <el-table-column prop="name" label="名称" />
                    <el-table-column prop="value" label="数值" />
                  </el-table>
                </div>
                
                <div v-else class="unknown-component">
                  <el-icon><Warning /></el-icon>
                  <span>未知组件类型：{{ widget.type }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- 右侧属性面板 -->
      <div class="property-panel">
        <el-tabs v-model="activeTab" type="border-card">
          <el-tab-pane label="基础设置" name="basic">
            <el-form label-position="top" size="small">
              <el-form-item label="组件名称">
                <el-input v-model="selectedWidget.name" v-if="selectedWidget" />
              </el-form-item>
              
              <el-form-item label="位置 X" v-if="selectedWidget">
                <el-input-number :model-value="selectedWidget.x" @update:model-value="(v) => selectedWidget.x = v" :min="0" :step="10" />
              </el-form-item>
              
              <el-form-item label="位置 Y" v-if="selectedWidget">
                <el-input-number :model-value="selectedWidget.y" @update:model-value="(v) => selectedWidget.y = v" :min="0" :step="10" />
              </el-form-item>
              
              <el-form-item label="宽度" v-if="selectedWidget">
                <el-input-number :model-value="selectedWidget.width" @update:model-value="(v) => selectedWidget.width = v" :min="100" :step="10" />
              </el-form-item>
              
              <el-form-item label="高度" v-if="selectedWidget">
                <el-input-number :model-value="selectedWidget.height" @update:model-value="(v) => selectedWidget.height = v" :min="100" :step="10" />
              </el-form-item>
            </el-form>
          </el-tab-pane>
          
          <el-tab-pane label="数据配置" name="data">
            <el-form label-position="top" size="small" v-if="selectedWidget">
              <el-form-item label="数据源类型">
                <el-select :model-value="selectedWidget.config?.dataType" @update:model-value="(v) => selectedWidget.config.dataType = v" placeholder="请选择">
                  <el-option label="API 接口" value="api" />
                  <el-option label="SQL 查询" value="sql" />
                  <el-option label="静态数据" value="static" />
                </el-select>
              </el-form-item>
              
              <el-form-item label="API 地址" v-if="selectedWidget?.config?.dataType === 'api'">
                <el-input :model-value="selectedWidget.config?.apiUrl" @update:model-value="(v) => selectedWidget.config.apiUrl = v" placeholder="请输入 API 地址" />
              </el-form-item>
              
              <el-form-item label="SQL 语句" v-if="selectedWidget?.config?.dataType === 'sql'">
                <el-input
                  :model-value="selectedWidget.config?.sql"
                  @update:model-value="(v) => selectedWidget.config.sql = v"
                  type="textarea"
                  :rows="4"
                  placeholder="SELECT * FROM table WHERE ..."
                />
              </el-form-item>
              
              <el-form-item label="静态数据" v-if="selectedWidget?.config?.dataType === 'static'">
                <el-input
                  :model-value="selectedWidget.config?.staticData"
                  @update:model-value="(v) => selectedWidget.config.staticData = v"
                  type="textarea"
                  :rows="4"
                  placeholder='[{"name":"A","value":100}]'
                />
              </el-form-item>
              
              <el-form-item label="刷新间隔">
                <el-select :model-value="selectedWidget.config?.refreshInterval" @update:model-value="(v) => selectedWidget.config.refreshInterval = v" placeholder="请选择">
                  <el-option label="不刷新" :value="0" />
                  <el-option label="30 秒" :value="30000" />
                  <el-option label="1 分钟" :value="60000" />
                  <el-option label="5 分钟" :value="300000" />
                </el-select>
              </el-form-item>
            </el-form>
          </el-tab-pane>
          
          <el-tab-pane label="样式配置" name="style">
            <el-form label-position="top" size="small" v-if="selectedWidget">
              <el-form-item label="标题显示" v-if="selectedWidget">
                <el-switch :model-value="selectedWidget.config?.showTitle" @update:model-value="(v) => selectedWidget.config.showTitle = v" />
              </el-form-item>
              
              <el-form-item label="背景颜色" v-if="selectedWidget">
                <el-color-picker :model-value="selectedWidget.config?.backgroundColor" @update:model-value="(v) => selectedWidget.config.backgroundColor = v" />
              </el-form-item>
              
              <el-form-item label="文字颜色" v-if="selectedWidget">
                <el-color-picker :model-value="selectedWidget.config?.textColor" @update:model-value="(v) => selectedWidget.config.textColor = v" />
              </el-form-item>
              
              <el-form-item label="圆角" v-if="selectedWidget">
                <el-slider :model-value="selectedWidget.config?.borderRadius" @update:model-value="(v) => selectedWidget.config.borderRadius = v" :min="0" :max="20" />
              </el-form-item>
            </el-form>
          </el-tab-pane>
        </el-tabs>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed, onMounted, onUnmounted } from 'vue'
import { useRouter } from 'vue-router'
import { use } from 'echarts/core'
import { CanvasRenderer } from 'echarts/renderers'
import { LineChart, BarChart, PieChart } from 'echarts/charts'
import {
  TitleComponent,
  TooltipComponent,
  LegendComponent,
  GridComponent
} from 'echarts/components'
import VChart from 'vue-echarts'
import { ElMessage, ElMessageBox } from 'element-plus'
import {
  Back,
  FolderChecked as Save,
  UploadFilled as Upload,
  VideoPlay,
  ZoomOut,
  ZoomIn,
  Grid,
  Delete,
  CopyDocument,
  Warning,
  TrendCharts,
  Histogram,
  PieChart as PieChartIcon,
  Odometer,
  Ticket,
  Document,
  List,
  TrophyBase,
  Picture,
  Link
} from '@element-plus/icons-vue'

// 注册 ECharts
use([
  CanvasRenderer,
  LineChart,
  BarChart,
  PieChart,
  TitleComponent,
  TooltipComponent,
  LegendComponent,
  GridComponent
])

const router = useRouter()
const canvasRef = ref<HTMLDivElement | null>(null)
const screenName = ref('')
const componentSearch = ref('')
const activeCategories = ref('chart')
const canvasZoom = ref(1)
const showGrid = ref(true)
const activeTab = ref('basic')
const selectedWidget = ref<any>(null)
const widgets = ref<any[]>([])

// 组件定义
const components = reactive({
  chart: [
    { type: 'line', name: '折线图', icon: TrendCharts },
    { type: 'bar', name: '柱状图', icon: Histogram },
    { type: 'pie', name: '饼图', icon: PieChartIcon },
    { type: 'area', name: '面积图', icon: TrendCharts },
    { type: 'radar', name: '雷达图', icon: Odometer }
  ],
  card: [
    { type: 'numberCard', name: '数字卡片', icon: Ticket },
    { type: 'percentCard', name: '百分比卡片', icon: PieChartIcon },
    { type: 'iconCard', name: '图标卡片', icon: Grid }
  ],
  table: [
    { type: 'table', name: '表格', icon: Document },
    { type: 'list', name: '列表', icon: List },
    { type: 'ranking', name: '排行榜', icon: TrophyBase }
  ],
  other: [
    { type: 'text', name: '文本', icon: Document },
    { type: 'image', name: '图片', icon: Picture },
    { type: 'iframe', name: '网页', icon: Link }
  ]
})

// 过滤组件
const filteredComponents = computed(() => {
  const keyword = componentSearch.value.toLowerCase()
  const filter = (items: any[]) => items.filter(c => c.name.toLowerCase().includes(keyword))
  
  return {
    chart: filter(components.chart),
    card: filter(components.card),
    table: filter(components.table),
    other: filter(components.other)
  }
})

// 拖拽相关
let draggedComponent: any = null
let isDraggingWidget = false
let dragOffset = { x: 0, y: 0 }

// 获取图表配置
const getChartOption = (widget: any) => {
  const baseOption = {
    tooltip: { trigger: 'axis' },
    grid: { top: 30, right: 20, bottom: 30, left: 40 },
  }
  
  if (widget.type === 'line') {
    return {
      ...baseOption,
      xAxis: {
        type: 'category',
        data: ['2024-01', '2024-02', '2024-03', '2024-04', '2024-05', '2024-06']
      },
      yAxis: { type: 'value' },
      series: [{
        data: [820, 932, 901, 934, 1290, 1330],
        type: 'line',
        smooth: true
      }]
    }
  }
  
  if (widget.type === 'bar') {
    return {
      ...baseOption,
      xAxis: {
        type: 'category',
        data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
      },
      yAxis: { type: 'value' },
      series: [{
        data: [120, 200, 150, 80, 70, 110, 130],
        type: 'bar'
      }]
    }
  }
  
  if (widget.type === 'pie') {
    return {
      tooltip: { trigger: 'item' },
      series: [{
        type: 'pie',
        radius: '50%',
        data: [
          { value: 1048, name: '搜索' },
          { value: 735, name: '直接' },
          { value: 580, name: '邮件' }
        ]
      }]
    }
  }
  
  return {}
}

// 获取组件样式
const getWidgetStyle = (widget: any) => {
  return {
    left: `${widget.x}px`,
    top: `${widget.y}px`,
    width: `${widget.width}px`,
    height: `${widget.height}px`,
    backgroundColor: widget.config?.backgroundColor || '#fff',
    borderRadius: `${widget.config?.borderRadius || 4}px`
  }
}

// 拖拽开始
const onDragStart = (event: DragEvent, comp: any) => {
  draggedComponent = { ...comp }
  event.dataTransfer?.setData('application/json', JSON.stringify(comp))
}

const onDragOver = (event: DragEvent) => {
  event.preventDefault()
}

const onDrop = (event: DragEvent) => {
  event.preventDefault()
  if (!canvasRef.value || !draggedComponent) return
  
  const rect = canvasRef.value.getBoundingClientRect()
  const x = (event.clientX - rect.left) / canvasZoom.value - 100
  const y = (event.clientY - rect.top) / canvasZoom.value - 50
  
  const newWidget = {
    id: `widget_${Date.now()}`,
    type: draggedComponent.type,
    name: draggedComponent.name,
    x: Math.max(0, x),
    y: Math.max(0, y),
    width: 300,
    height: 200,
    config: {
      dataType: 'static',
      showTitle: true,
      backgroundColor: '#ffffff',
      textColor: '#333333',
      borderRadius: 4
    }
  }
  
  widgets.value.push(newWidget)
  selectWidget(newWidget)
  draggedComponent = null
}

const onCanvasClick = () => {
  selectedWidget.value = null
}

// 选择组件
const selectWidget = (widget: any) => {
  selectedWidget.value = widget
}

// 删除组件
const deleteWidget = (id: string) => {
  widgets.value = widgets.value.filter(w => w.id !== id)
  if (selectedWidget.value?.id === id) {
    selectedWidget.value = null
  }
  ElMessage.success('组件已删除')
}

// 复制组件
const duplicateWidget = (widget: any) => {
  const newWidget = {
    ...JSON.parse(JSON.stringify(widget)),
    id: `widget_${Date.now()}`,
    x: widget.x + 20,
    y: widget.y + 20
  }
  widgets.value.push(newWidget)
  ElMessage.success('组件已复制')
}

// 清空画布
const clearCanvas = async () => {
  try {
    await ElMessageBox.confirm('确定清空画布吗？', '警告', { type: 'warning' })
    widgets.value = []
    selectedWidget.value = null
    ElMessage.success('画布已清空')
  } catch (error: any) {
    if (error !== 'cancel') console.error('清空失败:', error)
  }
}

// 缩放控制
const zoomIn = () => {
  canvasZoom.value = Math.min(canvasZoom.value + 0.1, 2)
}

const zoomOut = () => {
  canvasZoom.value = Math.max(canvasZoom.value - 0.1, 0.5)
}

const resetZoom = () => {
  canvasZoom.value = 1
}

const toggleGrid = () => {
  showGrid.value = !showGrid.value
}

// 保存/发布
const saveScreen = () => {
  const screenData = {
    name: screenName.value,
    widgets: widgets.value,
    saveTime: new Date().toISOString()
  }
  console.log('保存大屏:', screenData)
  ElMessage.success('大屏已保存')
}

const publishScreen = () => {
  if (widgets.value.length === 0) {
    ElMessage.warning('请先添加组件')
    return
  }
  const screenData = {
    name: screenName.value,
    widgets: widgets.value,
    publishTime: new Date().toISOString()
  }
  console.log('发布大屏:', screenData)
  ElMessage.success('大屏已发布')
}

const preview = () => {
  ElMessage.info('预览功能开发中')
}

const backToList = () => {
  router.push('/screen/project')
}

// 组件卸载时清理
onUnmounted(() => {
  widgets.value = []
})
</script>

<style scoped lang="less">
.screen-designer-container {
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
  
  .screen-title {
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

.component-panel {
  width: 260px;
  border-right: 1px solid #e4e7ed;
  padding: 16px;
  background: #fafafa;
  overflow-y: auto;
  
  .panel-title {
    font-size: 14px;
    font-weight: 600;
    color: #303133;
    margin-bottom: 12px;
  }
  
  .component-item {
    display: flex;
    align-items: center;
    gap: 8px;
    padding: 10px 12px;
    margin-bottom: 6px;
    background: #fff;
    border: 1px solid #e4e7ed;
    border-radius: 4px;
    cursor: grab;
    transition: all 0.3s;
    font-size: 13px;
    
    &:hover {
      border-color: #409EFF;
      box-shadow: 0 2px 8px rgba(64,158,255,0.2);
      transform: translateX(2px);
    }
    
    .el-icon {
      font-size: 16px;
      color: #409EFF;
      flex-shrink: 0;
    }
    
    span {
      color: #606266;
    }
  }
}

.canvas-container {
  flex: 1;
  position: relative;
  background: #f5f7fa;
  overflow: hidden;
  
  .canvas-toolbar {
    position: absolute;
    top: 16px;
    left: 50%;
    transform: translateX(-50%);
    z-index: 100;
    display: flex;
    align-items: center;
    gap: 12px;
    background: #fff;
    padding: 8px 16px;
    border-radius: 8px;
    box-shadow: 0 2px 12px rgba(0,0,0,0.15);
  }
  
  .canvas {
    width: 100%;
    height: 100%;
    overflow: auto;
    position: relative;
    
    .canvas-grid {
      position: absolute;
      top: 0;
      left: 0;
      width: 3000px;
      height: 2000px;
      background-image: 
        linear-gradient(rgba(64,158,255,0.1) 1px, transparent 1px),
        linear-gradient(90deg, rgba(64,158,255,0.1) 1px, transparent 1px);
      background-size: 20px 20px;
      pointer-events: none;
      z-index: 0;
    }
    
    .canvas-content {
      position: relative;
      min-width: 2000px;
      min-height: 1500px;
      transform-origin: top left;
      transition: transform 0.3s;
      z-index: 1;
    }
  }
}

.widget {
  position: absolute;
  border: 2px solid transparent;
  background: #fff;
  box-shadow: 0 2px 12px rgba(0,0,0,0.1);
  cursor: move;
  transition: border-color 0.3s;
  
  &.selected {
    border-color: #409EFF;
    z-index: 10;
  }
  
  .widget-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 8px 12px;
    background: #f5f7fa;
    border-bottom: 1px solid #e4e7ed;
    cursor: move;
    
    .widget-title {
      font-size: 13px;
      font-weight: 600;
      color: #606266;
    }
    
    .widget-actions {
      .el-button {
        padding: 4px;
      }
    }
  }
  
  .widget-content {
    height: calc(100% - 40px);
    padding: 12px;
    overflow: hidden;
    
    .chart-placeholder, .card-placeholder, .table-placeholder {
      height: 100%;
      display: flex;
      flex-direction: column;
      
      .chart-title, .table-title {
        font-size: 14px;
        font-weight: 600;
        color: #303133;
        margin-bottom: 12px;
      }
    }
    
    .card-placeholder {
      justify-content: center;
      align-items: center;
      
      .card-value {
        font-size: 36px;
        font-weight: bold;
        color: #409EFF;
      }
      
      .card-label {
        font-size: 14px;
        color: #909399;
        margin-top: 8px;
      }
    }
    
    .unknown-component {
      height: 100%;
      display: flex;
      flex-direction: column;
      justify-content: center;
      align-items: center;
      color: #F56C6C;
      
      .el-icon {
        font-size: 32px;
        margin-bottom: 8px;
      }
    }
  }
}

.property-panel {
  width: 340px;
  border-left: 1px solid #e4e7ed;
  background: #fff;
  overflow-y: auto;
}
</style>
