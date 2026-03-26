<template>
  <div class="etl-designer-container">
    <!-- 顶部导航 -->
    <div class="designer-header">
      <div class="header-left">
        <el-button @click="backToList">
          <el-icon><Back /></el-icon>
          返回
        </el-button>
        <span class="workflow-title">{{ workflowName || '新建 ETL 工作流' }}</span>
      </div>
      <div class="header-center">
        <el-input
          v-model="workflowName"
          placeholder="输入工作流名称"
          style="width: 300px"
          clearable
        >
          <template #prefix>
            <el-icon><Edit /></el-icon>
          </template>
        </el-input>
      </div>
      <div class="header-right">
        <el-button @click="saveWorkflow">
          <el-icon><Save /></el-icon>
          保存
        </el-button>
        <el-button type="primary" @click="runWorkflow">
          <el-icon><VideoPlay /></el-icon>
          运行
        </el-button>
      </div>
    </div>

    <div class="designer-body">
      <!-- 左侧工具栏 -->
      <div class="toolbar-panel">
        <div class="toolbar-section">
          <div class="section-title">数据抽取</div>
          <div
            class="tool-item"
            v-for="tool in extractTools"
            :key="tool.type"
            draggable="true"
            @dragstart="onDragStart($event, tool)"
          >
            <el-icon :component="tool.icon" />
            <span>{{ tool.name }}</span>
          </div>
        </div>
        
        <el-divider />
        
        <div class="toolbar-section">
          <div class="section-title">数据处理</div>
          <div
            class="tool-item"
            v-for="tool in processTools"
            :key="tool.type"
            draggable="true"
            @dragstart="onDragStart($event, tool)"
          >
            <el-icon :component="tool.icon" />
            <span>{{ tool.name }}</span>
          </div>
        </div>
        
        <el-divider />
        
        <div class="toolbar-section">
          <div class="section-title">数据加载</div>
          <div
            class="tool-item"
            v-for="tool in loadTools"
            :key="tool.type"
            draggable="true"
            @dragstart="onDragStart($event, tool)"
          >
            <el-icon :component="tool.icon" />
            <span>{{ tool.name }}</span>
          </div>
        </div>
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
          <el-button @click="autoLayout">
            <el-icon><Arrange /></el-icon>
            自动布局
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
          
          <!-- SVG 连线层 -->
          <svg class="canvas-connections" :style="{ transform: `scale(${canvasZoom})` }">
            <defs>
              <marker id="arrowhead" markerWidth="10" markerHeight="7" refX="9" refY="3.5" orient="auto">
                <polygon points="0 0, 10 3.5, 0 7" fill="#409EFF" />
              </marker>
            </defs>
            <path
              v-for="conn in connections"
              :key="conn.id"
              :d="getConnectionPath(conn.from, conn.to)"
              stroke="#409EFF"
              stroke-width="2"
              fill="none"
              marker-end="url(#arrowhead)"
              class="connection-path"
              @click="selectConnection(conn)"
            />
          </svg>
          
          <div class="canvas-content" :style="{ transform: `scale(${canvasZoom})` }">
            <!-- 任务节点 -->
            <div
              v-for="task in tasks"
              :key="task.id"
              class="task-node"
              :class="['task-' + task.type, { selected: selectedTask?.id === task.id }]"
              :style="getTaskPosition(task)"
              @click="selectTask(task)"
            >
              <div class="task-header">
                <el-icon :component="getTaskIcon(task.type)" />
                <span class="task-name">{{ task.name }}</span>
                <el-button class="delete-btn" type="text" @click.stop="deleteTask(task.id)">
                  <el-icon><Close /></el-icon>
                </el-button>
              </div>
              <div class="task-body">
                <div class="task-info">{{ getTaskDescription(task) }}</div>
              </div>
              <!-- 连接点 -->
              <div class="connector input" @mousedown.stop="startConnection(task, 'input')">
                <el-icon><Plus /></el-icon>
              </div>
              <div class="connector output" @mousedown.stop="startConnection(task, 'output')">
                <el-icon><Plus /></el-icon>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- 右侧配置面板 -->
      <div class="config-panel">
        <el-tabs v-model="activeTab" type="border-card">
          <el-tab-pane label="任务配置" name="task">
            <el-form label-position="top" size="small" v-if="selectedTask">
              <el-form-item label="任务编码">
                <el-input v-model="selectedTask.code" disabled />
              </el-form-item>
              
              <el-form-item label="任务名称">
                <el-input v-model="selectedTask.name" />
              </el-form-item>
              
              <el-form-item label="任务类型">
                <el-tag>{{ getTaskTypeName(selectedTask.type) }}</el-tag>
              </el-form-item>
              
              <el-form-item label="超时时间">
                <el-input-number v-model="selectedTask.config.timeout" :min="0" :step="60" />
                <span class="unit">秒</span>
              </el-form-item>
              
              <el-form-item label="重试次数">
                <el-input-number v-model="selectedTask.config.retryCount" :min="0" :max="10" />
              </el-form-item>
              
              <el-form-item label="重试间隔">
                <el-input-number v-model="selectedTask.config.retryInterval" :min="0" :step="10" />
                <span class="unit">秒</span>
              </el-form-item>
            </el-form>
          </el-tab-pane>
          
          <el-tab-pane label="数据源配置" name="source">
            <el-form label-position="top" size="small" v-if="selectedTask">
              <el-form-item label="源数据源">
                <el-select v-model="selectedTask.config.sourceDb" placeholder="选择数据源" style="width: 100%">
                  <el-option label="MySQL-生产库" value="mysql_prod" />
                  <el-option label="MySQL-测试库" value="mysql_test" />
                  <el-option label="Oracle-数仓" value="oracle_dw" />
                </el-select>
              </el-form-item>
              
              <el-form-item label="目标数据源">
                <el-select v-model="selectedTask.config.targetDb" placeholder="选择目标库" style="width: 100%">
                  <el-option label="MySQL-生产库" value="mysql_prod" />
                  <el-option label="MySQL-测试库" value="mysql_test" />
                  <el-option label="Oracle-数仓" value="oracle_dw" />
                </el-select>
              </el-form-item>
              
              <el-form-item label="目标表">
                <el-input v-model="selectedTask.config.targetTable" placeholder="输入表名" />
              </el-form-item>
              
              <el-form-item label="源表/SQL">
                <el-input
                  v-model="selectedTask.config.sourceSql"
                  type="textarea"
                  :rows="4"
                  placeholder="SELECT * FROM table WHERE ..."
                />
              </el-form-item>
              
              <el-form-item label="过滤条件">
                <el-input v-model="selectedTask.config.whereClause" placeholder="WHERE 条件" />
              </el-form-item>
              
              <el-form-item label="批量大小">
                <el-input-number v-model="selectedTask.config.batchSize" :min="100" :step="100" />
              </el-form-item>
            </el-form>
          </el-tab-pane>
          
          <el-tab-pane label="高级配置" name="advanced">
            <el-form label-position="top" size="small" v-if="selectedTask">
              <el-form-item label="清空目标">
                <el-switch v-model="selectedTask.config.truncateTarget" />
              </el-form-item>
              
              <el-form-item label="启用并发">
                <el-switch v-model="selectedTask.config.enableParallel" />
              </el-form-item>
              
              <el-form-item label="并发数">
                <el-input-number v-model="selectedTask.config.parallelCount" :min="1" :max="10" />
              </el-form-item>
              
              <el-form-item label="错误容忍">
                <el-select v-model="selectedTask.config.errorTolerance" placeholder="选择容忍度">
                  <el-option label="零容忍" value="none" />
                  <el-option label="容忍 1%" value="1pct" />
                  <el-option label="容忍 5%" value="5pct" />
                  <el-option label="容忍 10%" value="10pct" />
                </el-select>
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
import { ElMessage, ElMessageBox } from 'element-plus'
import {
  Back,
  FolderChecked as Save,
  VideoPlay,
  Edit,
  ZoomOut,
  ZoomIn,
  Grid,
  Grid as Arrange,
  Close,
  Plus,
  Document,
  Connection,
  DataAnalysis,
  Filter,
  Upload,
  Download,
  Refresh,
  Warning,
  Folder
} from '@element-plus/icons-vue'

const router = useRouter()
const canvasRef = ref<HTMLDivElement | null>(null)
const workflowName = ref('')
const canvasZoom = ref(1)
const showGrid = ref(true)
const activeTab = ref('task')
const selectedTask = ref<any>(null)
const selectedConnection = ref<any>(null)
const tasks = ref<any[]>([])
const connections = ref<any[]>([])

// 工具定义
const extractTools = [
  { type: 'sql_extract', name: 'SQL 抽取', icon: Document },
  { type: 'api_extract', name: 'API 拉取', icon: Upload },
  { type: 'file_extract', name: '文件抽取', icon: Folder }
]

const processTools = [
  { type: 'data_transform', name: '数据转换', icon: DataAnalysis },
  { type: 'data_quality', name: '数据质量', icon: Filter },
  { type: 'sql_aggregate', name: 'SQL 聚合', icon: Document },
  { type: 'stored_procedure', name: '存储过程', icon: Refresh }
]

const loadTools = [
  { type: 'api_load', name: 'API 推送', icon: Download },
  { type: 'db_load', name: '数据库加载', icon: Connection }
]

// 拖拽相关
let draggedTool: any = null
let isConnecting = false
let connectionStart: any = null
let connectionType: 'input' | 'output' | null = null

// 获取任务图标
const getTaskIcon = (type: string) => {
  const icons: Record<string, any> = {
    sql_extract: Document,
    api_extract: Upload,
    file_extract: Folder,
    data_transform: DataAnalysis,
    data_quality: Filter,
    sql_aggregate: Document,
    stored_procedure: Refresh,
    api_load: Download,
    db_load: Connection
  }
  return icons[type] || Document
}

// 获取任务类型名称
const getTaskTypeName = (type: string) => {
  const names: Record<string, string> = {
    sql_extract: 'SQL 抽取',
    api_extract: 'API 拉取',
    file_extract: '文件抽取',
    data_transform: '数据转换',
    data_quality: '数据质量',
    sql_aggregate: 'SQL 聚合',
    stored_procedure: '存储过程',
    api_load: 'API 推送',
    db_load: '数据库加载'
  }
  return names[type] || '未知类型'
}

// 获取任务描述
const getTaskDescription = (task: any) => {
  if (task.type === 'sql_extract') {
    return `SQL: ${task.config?.sourceSql?.substring(0, 30) || '未配置'}...`
  }
  return `${getTaskTypeName(task.type)}`
}

// 获取任务位置
const getTaskPosition = (task: any) => {
  return {
    left: `${task.x}px`,
    top: `${task.y}px`
  }
}

// 拖拽开始
const onDragStart = (event: DragEvent, tool: any) => {
  draggedTool = { ...tool }
  event.dataTransfer?.setData('application/json', JSON.stringify(tool))
}

const onDragOver = (event: DragEvent) => {
  event.preventDefault()
}

const onDrop = (event: DragEvent) => {
  event.preventDefault()
  if (!canvasRef.value || !draggedTool) return
  
  const rect = canvasRef.value.getBoundingClientRect()
  const x = (event.clientX - rect.left) / canvasZoom.value - 100
  const y = (event.clientY - rect.top) / canvasZoom.value - 30
  
  const newTask = {
    id: `task_${Date.now()}`,
    code: `task_${Date.now()}`,
    type: draggedTool.type,
    name: draggedTool.name,
    x: Math.max(0, x),
    y: Math.max(0, y),
    width: 200,
    height: 80,
    config: {
      timeout: 300,
      retryCount: 0,
      retryInterval: 60,
      sourceDb: '',
      targetDb: '',
      targetTable: '',
      sourceSql: '',
      whereClause: '',
      batchSize: 1000,
      truncateTarget: false,
      enableParallel: false,
      parallelCount: 1,
      errorTolerance: 'none'
    }
  }
  
  tasks.value.push(newTask)
  selectTask(newTask)
  draggedTool = null
}

const onCanvasClick = () => {
  selectedTask.value = null
  selectedConnection.value = null
}

// 选择任务
const selectTask = (task: any) => {
  selectedTask.value = task
  selectedConnection.value = null
  activeTab.value = 'task'
}

// 删除任务
const deleteTask = (id: string) => {
  tasks.value = tasks.value.filter(t => t.id !== id)
  connections.value = connections.value.filter(c => c.from.id !== id && c.to.id !== id)
  if (selectedTask.value?.id === id) {
    selectedTask.value = null
  }
  ElMessage.success('任务已删除')
}

// 开始连线
const startConnection = (task: any, type: 'input' | 'output') => {
  isConnecting = true
  connectionStart = task
  connectionType = type
}

// 结束连线
const endConnection = (targetTask: any) => {
  if (!isConnecting || !connectionStart) return
  
  // 不能自己连自己
  if (connectionStart.id === targetTask.id) {
    isConnecting = false
    connectionStart = null
    connectionType = null
    return
  }
  
  // 输入只能连接到输出
  if (connectionType === 'input') {
    const exists = connections.value.some(c => c.from.id === targetTask.id && c.to.id === connectionStart.id)
    if (!exists) {
      connections.value.push({
        id: `conn_${Date.now()}`,
        from: targetTask,
        to: connectionStart
      })
      ElMessage.success('连接已创建')
    }
  } else {
    const exists = connections.value.some(c => c.from.id === connectionStart.id && c.to.id === targetTask.id)
    if (!exists) {
      connections.value.push({
        id: `conn_${Date.now()}`,
        from: connectionStart,
        to: targetTask
      })
      ElMessage.success('连接已创建')
    }
  }
  
  isConnecting = false
  connectionStart = null
  connectionType = null
}

// 选择连线
const selectConnection = (conn: any) => {
  selectedConnection.value = conn
  selectedTask.value = null
  if (confirm('确定删除此连接吗？')) {
    connections.value = connections.value.filter(c => c.id !== conn.id)
    ElMessage.success('连接已删除')
  }
}

// 获取连线 SVG 路径
const getConnectionPath = (from: any, to: any) => {
  const startX = from.x + from.width
  const startY = from.y + from.height / 2
  const endX = to.x
  const endY = to.y + to.height / 2
  
  // 贝塞尔曲线
  const controlPoint1X = startX + (endX - startX) / 2
  const controlPoint1Y = startY
  const controlPoint2X = endX - (endX - startX) / 2
  const controlPoint2Y = endY
  
  return `M ${startX} ${startY} C ${controlPoint1X} ${controlPoint1Y}, ${controlPoint2X} ${controlPoint2Y}, ${endX} ${endY}`
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

// 自动布局
const autoLayout = () => {
  const gapX = 300
  const gapY = 150
  const cols = 3
  
  tasks.value.forEach((task, index) => {
    const col = index % cols
    const row = Math.floor(index / cols)
    task.x = 100 + col * gapX
    task.y = 100 + row * gapY
  })
  
  ElMessage.success('自动布局完成')
}

// 保存/运行
const saveWorkflow = () => {
  const workflowData = {
    name: workflowName.value,
    tasks: tasks.value,
    connections: connections.value,
    saveTime: new Date().toISOString()
  }
  console.log('保存工作流:', workflowData)
  ElMessage.success('工作流已保存')
}

const runWorkflow = () => {
  if (tasks.value.length === 0) {
    ElMessage.warning('请先添加任务')
    return
  }
  ElMessage.info('工作流运行功能开发中')
}

const backToList = () => {
  router.push('/etl/task')
}

// 组件卸载时清理
onUnmounted(() => {
  tasks.value = []
  connections.value = []
})

// 初始化
onMounted(() => {
  // 可以添加示例任务
})
</script>

<style scoped lang="less">
.etl-designer-container {
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
  
  .workflow-title {
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

.toolbar-panel {
  width: 200px;
  border-right: 1px solid #e4e7ed;
  padding: 16px;
  background: #fafafa;
  overflow-y: auto;
  
  .toolbar-section {
    margin-bottom: 16px;
    
    .section-title {
      font-size: 13px;
      font-weight: 600;
      color: #606266;
      margin-bottom: 12px;
    }
    
    .tool-item {
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
    
    .canvas-connections {
      position: absolute;
      top: 0;
      left: 0;
      width: 3000px;
      height: 2000px;
      pointer-events: none;
      z-index: 1;
      
      .connection-path {
        pointer-events: stroke;
        cursor: pointer;
        
        &:hover {
          stroke: #67C23A;
          stroke-width: 3;
        }
      }
    }
    
    .canvas-content {
      position: relative;
      min-width: 2000px;
      min-height: 1500px;
      transform-origin: top left;
      transition: transform 0.3s;
      z-index: 2;
    }
  }
}

.task-node {
  position: absolute;
  width: 200px;
  background: #fff;
  border: 2px solid #e4e7ed;
  border-radius: 8px;
  box-shadow: 0 2px 12px rgba(0,0,0,0.1);
  cursor: pointer;
  transition: all 0.3s;
  
  &:hover {
    box-shadow: 0 4px 16px rgba(0,0,0,0.15);
  }
  
  &.selected {
    border-color: #409EFF;
    box-shadow: 0 0 0 2px rgba(64,158,255,0.3);
  }
  
  .task-header {
    display: flex;
    align-items: center;
    gap: 8px;
    padding: 10px 12px;
    background: #f5f7fa;
    border-bottom: 1px solid #e4e7ed;
    border-radius: 8px 8px 0 0;
    
    .el-icon {
      font-size: 16px;
      color: #409EFF;
    }
    
    .task-name {
      flex: 1;
      font-size: 13px;
      font-weight: 600;
      color: #303133;
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
    }
    
    .delete-btn {
      opacity: 0;
      transition: opacity 0.3s;
    }
    
    &:hover .delete-btn {
      opacity: 1;
    }
  }
  
  .task-body {
    padding: 10px 12px;
    
    .task-info {
      font-size: 12px;
      color: #909399;
      line-height: 1.4;
      overflow: hidden;
      text-overflow: ellipsis;
      display: -webkit-box;
      -webkit-line-clamp: 2;
      -webkit-box-orient: vertical;
    }
  }
  
  .connector {
    position: absolute;
    width: 20px;
    height: 20px;
    border-radius: 50%;
    background: #409EFF;
    color: #fff;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: crosshair;
    opacity: 0;
    transition: opacity 0.3s;
    z-index: 10;
    
    .el-icon {
      font-size: 12px;
    }
    
    &:hover {
      background: #67C23A;
      transform: scale(1.2);
    }
    
    &.input {
      left: -10px;
      top: 50%;
      transform: translateY(-50%);
    }
    
    &.output {
      right: -10px;
      top: 50%;
      transform: translateY(-50%);
    }
  }
  
  &:hover .connector {
    opacity: 1;
  }
  
  // 不同类型节点的颜色
  &.task-sql_extract, &.task-api_extract, &.task-file_extract {
    border-left: 4px solid #67C23A;
  }
  
  &.task-data_transform, &.task-data_quality, &.task-sql_aggregate, &.task-stored_procedure {
    border-left: 4px solid #409EFF;
  }
  
  &.task-api_load, &.task-db_load {
    border-left: 4px solid #E6A23C;
  }
}

.config-panel {
  width: 360px;
  border-left: 1px solid #e4e7ed;
  background: #fff;
  overflow-y: auto;
  
  .unit {
    margin-left: 8px;
    color: #909399;
    font-size: 13px;
  }
}
</style>
