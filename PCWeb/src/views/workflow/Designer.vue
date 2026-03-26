<template>
  <div class="workflow-designer-container">
    <div class="designer-header">
      <div class="header-left">
        <el-button @click="backToList">
          <el-icon><ArrowLeft /></el-icon>
          返回
        </el-button>
        <span class="workflow-title">{{ workflowName || '新建工作流' }}</span>
      </div>
      <div class="header-right">
        <el-button @click="saveWorkflow">
          <el-icon><Save /></el-icon>
          保存
        </el-button>
        <el-button type="primary" @click="publishWorkflow">
          <el-icon><Upload /></el-icon>
          发布
        </el-button>
      </div>
    </div>

    <div class="designer-body">
      <!-- 左侧组件面板 -->
      <div class="component-panel">
        <div class="panel-title">组件</div>
        <el-divider />
        <div class="component-section">
          <div class="section-title">基础节点</div>
          <div
            class="component-item"
            v-for="node in basicNodes"
            :key="node.type"
            draggable="true"
            @dragstart="onDragStart($event, node)"
          >
            <el-icon :component="node.icon" />
            <span>{{ node.name }}</span>
          </div>
        </div>
        <el-divider />
        <div class="component-section">
          <div class="section-title">网关</div>
          <div
            class="component-item"
            v-for="node in gatewayNodes"
            :key="node.type"
            draggable="true"
            @dragstart="onDragStart($event, node)"
          >
            <el-icon :component="node.icon" />
            <span>{{ node.name }}</span>
          </div>
        </div>
        <el-divider />
        <div class="component-section">
          <div class="section-title">其他</div>
          <div
            class="component-item"
            v-for="node in otherNodes"
            :key="node.type"
            draggable="true"
            @dragstart="onDragStart($event, node)"
          >
            <el-icon :component="node.icon" />
            <span>{{ node.name }}</span>
          </div>
        </div>
      </div>

      <!-- 中间画布 -->
      <div class="canvas-container">
        <div class="canvas-toolbar">
          <el-button-group>
            <el-button @click="undo" :disabled="historyIndex <= 0">
              <el-icon><RefreshLeft /></el-icon>
            </el-button>
            <el-button @click="redo" :disabled="historyIndex >= history.length - 1">
              <el-icon><RefreshRight /></el-icon>
            </el-button>
            <el-divider direction="vertical" />
            <el-button @click="zoomOut">
              <el-icon><ZoomOut /></el-icon>
            </el-button>
            <el-button @click="resetZoom">{{ Math.round(zoomLevel * 100) }}%</el-button>
            <el-button @click="zoomIn">
              <el-icon><ZoomIn /></el-icon>
            </el-button>
            <el-divider direction="vertical" />
            <el-button @click="autoLayout">
              <el-icon><Arrange /></el-icon>
              自动布局
            </el-button>
          </el-button-group>
          <div class="toolbar-right">
            <el-button @click="toggleGrid">
              <el-icon><Grid /></el-icon>
              网格：{{ showGrid ? '开' : '关' }}
            </el-button>
            <el-button @click="validateWorkflow">
              <el-icon><CircleCheck /></el-icon>
              验证
            </el-button>
            <el-button @click="clearCanvas">
              <el-icon><Delete /></el-icon>
              清空
            </el-button>
            <el-button @click="exportWorkflow">
              <el-icon><Download /></el-icon>
              导出
            </el-button>
            <el-button @click="importTrigger.click()">
              <el-icon><Upload /></el-icon>
              导入
            </el-button>
            <input
              ref="importTrigger"
              type="file"
              accept=".json"
              style="display: none"
              @change="onImportFile"
            />
          </div>
        </div>
        <div class="canvas" ref="canvasRef" @dragover="onDragOver" @drop="onDrop" @click="onCanvasClick" @mouseup="onMouseUp">
          <!-- 网格背景 -->
          <div class="canvas-grid" v-if="showGrid" :style="{ transform: `scale(${zoomLevel})` }"></div>
          
          <!-- SVG 连线层 -->
          <svg class="canvas-connections" :style="{ transform: `scale(${zoomLevel})` }">
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
          
          <div class="canvas-content" :style="{ transform: `scale(${zoomLevel})` }">
            <!-- 开始节点 -->
            <div class="node start-node" :style="getNodePosition(startNode)">
              <div class="node-header">
                <el-icon><Play /></el-icon>
                <span>开始</span>
              </div>
              <div class="node-connector" @mousedown="startConnection(startNode)">
                <el-icon><Plus /></el-icon>
              </div>
            </div>

            <!-- 动态节点 -->
            <div
              v-for="node in nodes"
              :key="node.id"
              class="node"
              :class="['node-' + node.type, { selected: selectedNode?.id === node.id }]"
              :style="getNodePosition(node)"
              :data-node-id="node.id"
              @click="selectNode(node)"
              @mouseup.stop="onNodeMouseUp(node)"
            >
              <div class="node-header">
                <el-icon :component="getNodeIcon(node.type)" />
                <span>{{ node.name }}</span>
                <div class="node-actions">
                  <el-button class="action-btn" type="text" @click.stop="copyNode(node)" title="复制">
                    <el-icon><CopyDocument /></el-icon>
                  </el-button>
                  <el-button class="delete-btn" type="text" @click.stop="deleteNode(node.id)" title="删除">
                    <el-icon><Close /></el-icon>
                  </el-button>
                </div>
              </div>
              <div class="node-body">
                <div v-if="node.type === 'approver'" class="node-info">
                  <span v-if="node.approverType === 'person'">
                    指定成员：{{ node.approverNames?.join('、') || '未设置' }}
                  </span>
                  <span v-else-if="node.approverType === 'role'">
                    指定角色：{{ node.roleNames?.join('、') || '未设置' }}
                  </span>
                  <span v-else>
                    发起人自选
                  </span>
                </div>
                <div v-else-if="node.type === 'condition'" class="node-info">
                  <span>条件：{{ node.conditionExpression || '未设置' }}</span>
                </div>
                <div v-else-if="node.type === 'copyer'" class="node-info">
                  <span>抄送人：{{ node.copyerNames?.join('、') || '未设置' }}</span>
                </div>
                <div v-else-if="['exclusive', 'parallel', 'inclusive'].includes(node.type)" class="node-info">
                  <span>{{ getGatewayName(node.type) }}</span>
                </div>
                <div v-else-if="node.type === 'script'" class="node-info">
                  <span>脚本：{{ node.scriptContent ? '已配置' : '未设置' }}</span>
                </div>
              </div>
              <div class="node-connector" @mousedown.stop="startConnection(node)">
                <el-icon><Plus /></el-icon>
              </div>
            </div>

            <!-- 结束节点 -->
            <div class="node end-node" :style="getNodePosition(endNode)">
              <div class="node-header">
                <el-icon><CircleCheck /></el-icon>
                <span>结束</span>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- 右侧属性面板 -->
      <div class="property-panel" v-if="selectedNode">
        <div class="panel-title">属性设置</div>
        <el-divider />
        
        <el-form label-position="top" size="small">
          <el-form-item label="节点名称">
            <el-input v-model="selectedNode.name" />
          </el-form-item>

          <!-- 审批人设置 -->
          <el-form-item label="审批人类型" v-if="selectedNode.type === 'approver'">
            <el-select v-model="selectedNode.approverType" placeholder="请选择">
              <el-option label="指定成员" value="person" />
              <el-option label="指定角色" value="role" />
              <el-option label="发起人自选" value="select" />
            </el-select>
          </el-form-item>

          <el-form-item label="选择成员" v-if="selectedNode.type === 'approver' && selectedNode.approverType === 'person'">
            <el-select v-model="selectedNode.approverIds" multiple placeholder="请选择成员">
              <el-option label="张三" value="user1" />
              <el-option label="李四" value="user2" />
              <el-option label="王五" value="user3" />
            </el-select>
          </el-form-item>

          <el-form-item label="选择角色" v-if="selectedNode.type === 'approver' && selectedNode.approverType === 'role'">
            <el-select v-model="selectedNode.roleIds" multiple placeholder="请选择角色">
              <el-option label="部门经理" value="role1" />
              <el-option label="财务" value="role2" />
              <el-option label="人事" value="role3" />
            </el-select>
          </el-form-item>

          <!-- 抄送人设置 -->
          <el-form-item label="抄送人" v-if="selectedNode.type === 'copyer'">
            <el-select v-model="selectedNode.copyerIds" multiple placeholder="请选择抄送人">
              <el-option label="张三" value="user1" />
              <el-option label="李四" value="user2" />
            </el-select>
          </el-form-item>

          <!-- 条件设置 -->
          <el-form-item label="条件表达式" v-if="selectedNode.type === 'condition'">
            <el-input
              v-model="selectedNode.conditionExpression"
              type="textarea"
              :rows="3"
              placeholder="例如：amount > 1000"
            />
          </el-form-item>

          <!-- 网关节点设置 -->
          <el-form-item label="网关类型" v-if="['exclusive', 'parallel', 'inclusive'].includes(selectedNode.type)">
            <el-alert
              :title="getGatewayDescription(selectedNode.type)"
              type="info"
              :closable="false"
              show-icon
            />
          </el-form-item>

          <!-- 脚本任务设置 -->
          <el-form-item label="脚本内容" v-if="selectedNode.type === 'script'">
            <el-input
              v-model="selectedNode.scriptContent"
              type="textarea"
              :rows="5"
              placeholder="请输入 JavaScript 代码"
            />
          </el-form-item>

          <el-form-item>
            <el-button type="primary" @click="updateNode">更新节点</el-button>
          </el-form-item>
        </el-form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import {
  Back as ArrowLeft,
  FolderChecked as Save,
  UploadFilled as Upload,
  ZoomOut,
  FullScreen,
  ZoomIn,
  VideoPlay as Play,
  CircleCheckFilled as CircleCheck,
  Close,
  User,
  UserFilled,
  Connection,
  Share,
  QuestionFilled,
  RefreshLeft,
  RefreshRight,
  Download,
  Grid as Arrange,
  Timer,
  Document as DocIcon,
  Flag,
  Plus,
  Grid,
  Delete,
  CopyDocument,
  Scissor
} from '@element-plus/icons-vue'
import { ElMessage } from 'element-plus'

const router = useRouter()
const route = useRoute()

const canvasRef = ref<HTMLDivElement | null>(null)
const zoomLevel = ref(1)
const selectedNode = ref<any>(null)
const workflowName = ref('')
const importTrigger = ref<HTMLInputElement | null>(null)
const showGrid = ref(true)

// 撤销/重做历史
const history = ref<any[]>([])
const historyIndex = ref(-1)

// 连线相关
const connections = ref<any[]>([])
const isConnecting = ref(false)
const connectionStart = ref<any>(null)
const tempConnection = ref<any>(null)

// 复制/粘贴
const clipboard = ref<any>(null)
const copiedNode = ref<any>(null)

// 快捷键
const shortcuts = ref<Map<string, Function>>(new Map())

// 节点模板分类
const basicNodes = [
  { type: 'approver', name: '审批人', icon: UserFilled },
  { type: 'copyer', name: '抄送人', icon: Share },
  { type: 'notifier', name: '通知器', icon: QuestionFilled }
]

const gatewayNodes = [
  { type: 'exclusive', name: '排他网关', icon: Connection },
  { type: 'parallel', name: '并行网关', icon: Connection },
  { type: 'inclusive', name: '包容网关', icon: Connection }
]

const otherNodes = [
  { type: 'condition', name: '条件分支', icon: Timer },
  { type: 'script', name: '脚本任务', icon: DocIcon },
  { type: 'endevent', name: '结束事件', icon: Flag }
]

// 节点数据
const startNode = reactive({ id: 'start', type: 'start', x: 100, y: 200 })
const endNode = reactive({ id: 'end', type: 'end', x: 800, y: 200 })
const nodes = ref<any[]>([])

let nodeIdCounter = 1
let draggedNode: any = null

// 获取节点图标
const getNodeIcon = (type: string) => {
  const iconMap: Record<string, any> = {
    approver: UserFilled,
    condition: Timer,
    copyer: Share,
    notifier: QuestionFilled,
    exclusive: Connection,
    parallel: Connection,
    inclusive: Connection,
    script: DocIcon,
    endevent: Flag
  }
  return iconMap[type] || User
}

// 获取节点位置
const getNodePosition = (node: any) => {
  return {
    left: `${node.x}px`,
    top: `${node.y}px`
  }
}

// 拖拽开始
const onDragStart = (event: DragEvent, node: any) => {
  draggedNode = { ...node, id: `node_${nodeIdCounter++}`, x: 0, y: 0 }
  event.dataTransfer?.setData('application/json', JSON.stringify(draggedNode))
}

// 拖拽中
const onDragOver = (event: DragEvent) => {
  event.preventDefault()
}

// 拖拽结束
const onDrop = (event: DragEvent) => {
  event.preventDefault()
  if (!canvasRef.value || !draggedNode) return

  const rect = canvasRef.value.getBoundingClientRect()
  const x = event.clientX - rect.left - 50
  const y = event.clientY - rect.top - 30

  draggedNode.x = x
  draggedNode.y = y
  
  // 初始化节点属性
  if (draggedNode.type === 'approver') {
    draggedNode.approverType = 'select'
    draggedNode.approverIds = []
    draggedNode.roleIds = []
    draggedNode.approverNames = []
    draggedNode.roleNames = []
  } else if (draggedNode.type === 'copyer') {
    draggedNode.copyerIds = []
    draggedNode.copyerNames = []
  } else if (draggedNode.type === 'condition') {
    draggedNode.conditionExpression = ''
  } else if (draggedNode.type === 'exclusive' || draggedNode.type === 'parallel' || draggedNode.type === 'inclusive') {
    draggedNode.gatewayConditions = []
  } else if (draggedNode.type === 'script') {
    draggedNode.scriptContent = ''
  }

  nodes.value.push(draggedNode)
  saveToHistory()
  draggedNode = null
}

// 选择节点
const selectNode = (node: any) => {
  selectedNode.value = node
}

// 删除节点
const deleteNode = (nodeId: string) => {
  nodes.value = nodes.value.filter(n => n.id !== nodeId)
  // 同时删除相关连线
  connections.value = connections.value.filter(c => c.from.id !== nodeId && c.to.id !== nodeId)
  if (selectedNode.value?.id === nodeId) {
    selectedNode.value = null
  }
  saveToHistory()
  ElMessage.success('节点已删除')
}

// 开始连线
const startConnection = (node: any) => {
  isConnecting.value = true
  connectionStart.value = node
}

// 结束连线
const endConnection = (targetNode: any) => {
  if (!isConnecting.value || !connectionStart.value) return
  
  // 不能自己连自己
  if (connectionStart.value.id === targetNode.id) {
    isConnecting.value = false
    connectionStart.value = null
    return
  }
  
  // 检查是否已存在
  const exists = connections.value.some(c => 
    c.from.id === connectionStart.value?.id && c.to.id === targetNode.id
  )
  
  if (!exists) {
    connections.value.push({
      id: `conn_${Date.now()}`,
      from: connectionStart.value,
      to: targetNode
    })
    saveToHistory()
    ElMessage.success('连接已创建')
  }
  
  isConnecting.value = false
  connectionStart.value = null
}

// 画布点击事件
const onCanvasClick = (event: MouseEvent) => {
  const target = event.target as HTMLElement
  // 如果点击的是画布背景（不是节点），结束连线
  if (target.classList.contains('canvas') || target.classList.contains('canvas-content') || 
      target.classList.contains('canvas-grid') || target.classList.contains('canvas-connections')) {
    if (isConnecting.value) {
      isConnecting.value = false
      connectionStart.value = null
    }
  }
}

// 节点鼠标松开事件
const onNodeMouseUp = (node: any) => {
  if (isConnecting.value) {
    endConnection(node)
  }
}

// 鼠标松开事件
const onMouseUp = (event: MouseEvent) => {
  if (isConnecting.value) {
    isConnecting.value = false
    connectionStart.value = null
  }
}

// 获取连线 SVG 路径
const getConnectionPath = (from: any, to: any) => {
  const startX = from.x + 200 // 节点宽度
  const startY = from.y + 40  // 节点高度的一半
  const endX = to.x
  const endY = to.y + 40
  
  // 贝塞尔曲线
  const controlPoint1X = startX + (endX - startX) / 2
  const controlPoint1Y = startY
  const controlPoint2X = endX - (endX - startX) / 2
  const controlPoint2Y = endY
  
  return `M ${startX} ${startY} C ${controlPoint1X} ${controlPoint1Y}, ${controlPoint2X} ${controlPoint2Y}, ${endX} ${endY}`
}

// 选择连线
const selectConnection = (conn: any) => {
  if (confirm('确定删除此连接吗？')) {
    connections.value = connections.value.filter(c => c.id !== conn.id)
    saveToHistory()
    ElMessage.success('连接已删除')
  }
}

// 切换网格
const toggleGrid = () => {
  showGrid.value = !showGrid.value
}

// 更新节点
const updateNode = () => {
  if (!selectedNode.value) return
  
  // 更新名称映射
  if (selectedNode.value.type === 'approver') {
    selectedNode.value.approverNames = selectedNode.value.approverIds?.map((id: string) => {
      const names: Record<string, string> = { user1: '张三', user2: '李四', user3: '王五' }
      return names[id]
    }).filter(Boolean)
    
    selectedNode.value.roleNames = selectedNode.value.roleIds?.map((id: string) => {
      const names: Record<string, string> = { role1: '部门经理', role2: '财务', role3: '人事' }
      return names[id]
    }).filter(Boolean)
  }
  
  if (selectedNode.value.type === 'copyer') {
    selectedNode.value.copyerNames = selectedNode.value.copyerIds?.map((id: string) => {
      const names: Record<string, string> = { user1: '张三', user2: '李四' }
      return names[id]
    }).filter(Boolean)
  }
  
  saveToHistory()
  ElMessage.success('节点已更新')
}

// 撤销/重做
const saveToHistory = () => {
  const state = JSON.stringify({
    nodes: nodes.value,
    startNode,
    endNode
  })
  // 移除当前索引之后的历史
  history.value = history.value.slice(0, historyIndex.value + 1)
  history.value.push(state)
  historyIndex.value = history.value.length - 1
}

const undo = () => {
  if (historyIndex.value > 0) {
    historyIndex.value--
    const state = JSON.parse(history.value[historyIndex.value])
    nodes.value = state.nodes
    Object.assign(startNode, state.startNode)
    Object.assign(endNode, state.endNode)
    ElMessage.success('已撤销')
  }
}

const redo = () => {
  if (historyIndex.value < history.value.length - 1) {
    historyIndex.value++
    const state = JSON.parse(history.value[historyIndex.value])
    nodes.value = state.nodes
    Object.assign(startNode, state.startNode)
    Object.assign(endNode, state.endNode)
    ElMessage.success('已重做')
  }
}

// 缩放控制
const zoomIn = () => {
  zoomLevel.value = Math.min(zoomLevel.value + 0.1, 2)
}

const zoomOut = () => {
  zoomLevel.value = Math.max(zoomLevel.value - 0.1, 0.5)
}

const resetZoom = () => {
  zoomLevel.value = 1
}

// 自动布局
const autoLayout = () => {
  const startX = 100
  const startY = 100
  const gapX = 250
  const gapY = 150
  
  // 开始节点
  startNode.x = startX
  startNode.y = startY + 200
  
  // 按顺序排列节点
  nodes.value.forEach((node, index) => {
    node.x = startX + gapX * (Math.floor(index / 3) + 1)
    node.y = startY + (index % 3) * gapY + 100
  })
  
  // 结束节点
  const lastX = Math.max(...nodes.value.map(n => n.x), startX) + gapX
  endNode.x = lastX
  endNode.y = startY + 200
  
  ElMessage.success('自动布局完成')
}

// 导出工作流
const exportWorkflow = () => {
  const workflowData = {
    name: workflowName.value || '未命名工作流',
    nodes: [startNode, ...nodes.value, endNode],
    exportTime: new Date().toISOString()
  }
  
  const blob = new Blob([JSON.stringify(workflowData, null, 2)], { type: 'application/json' })
  const url = URL.createObjectURL(blob)
  const a = document.createElement('a')
  a.href = url
  a.download = `${workflowName.value || 'workflow'}.json`
  a.click()
  URL.revokeObjectURL(url)
  
  ElMessage.success('工作流已导出')
}

// 导入工作流
const onImportFile = (event: Event) => {
  const target = event.target as HTMLInputElement
  const file = target.files?.[0]
  if (!file) return
  
  const reader = new FileReader()
  reader.onload = (e) => {
    try {
      const data = JSON.parse(e.target?.result as string)
      if (data.name) workflowName.value = data.name
      if (data.nodes && Array.isArray(data.nodes)) {
        const importedNodes = data.nodes.filter((n: any) => !['start', 'end'].includes(n.type))
        nodes.value = importedNodes
        if (data.nodes.find((n: any) => n.type === 'start')) {
          Object.assign(startNode, data.nodes.find((n: any) => n.type === 'start'))
        }
        if (data.nodes.find((n: any) => n.type === 'end')) {
          Object.assign(endNode, data.nodes.find((n: any) => n.type === 'end'))
        }
      }
      ElMessage.success('工作流已导入')
    } catch (error) {
      ElMessage.error('导入失败：文件格式错误')
    }
  }
  reader.readAsText(file)
  target.value = ''
}

// 保存工作流
const saveWorkflow = () => {
  const workflowData = {
    name: workflowName.value || '未命名工作流',
    nodes: [startNode, ...nodes.value, endNode],
    saveTime: new Date().toISOString()
  }
  console.log('保存工作流:', workflowData)
  saveToHistory()
  ElMessage.success('工作流已保存')
}

// 发布工作流
const publishWorkflow = () => {
  if (nodes.value.length === 0) {
    ElMessage.warning('请至少添加一个节点')
    return
  }
  
  const workflowData = {
    name: workflowName.value || '未命名工作流',
    nodes: [startNode, ...nodes.value, endNode],
    publishTime: new Date().toISOString()
  }
  console.log('发布工作流:', workflowData)
  saveToHistory()
  ElMessage.success('工作流已发布')
}

// 返回列表
const backToList = () => {
  router.push('/workflow/list')
}

// 生命周期
onMounted(() => {
  setupShortcuts()
  saveToHistory() // 初始化历史记录
})

// 获取网关名称
const getGatewayName = (type: string) => {
  const names: Record<string, string> = {
    exclusive: '排他网关',
    parallel: '并行网关',
    inclusive: '包容网关'
  }
  return names[type] || '网关'
}

// 获取网关描述
const getGatewayDescription = (type: string) => {
  const descriptions: Record<string, string> = {
    exclusive: '排他网关：只有一个输出路径会被执行（条件判断）',
    parallel: '并行网关：所有输出路径会同时执行',
    inclusive: '包容网关：满足条件的多个输出路径会执行'
  }
  return descriptions[type] || ''
}

// 验证工作流
const validateWorkflow = () => {
  const errors: string[] = []
  
  // 检查是否有节点
  if (nodes.value.length === 0) {
    errors.push('工作流至少需要一个节点')
  }
  
  // 检查节点名称
  nodes.value.forEach(node => {
    if (!node.name || node.name.trim() === '') {
      errors.push(`节点 ${node.id} 的名称不能为空`)
    }
  })
  
  // 检查是否有孤立节点（没有连线）
  if (connections.value.length === 0 && nodes.value.length > 0) {
    errors.push('节点之间没有连接')
  }
  
  // 检查是否有节点没有入度或出度
  const nodeIds = [startNode.id, ...nodes.value.map(n => n.id), endNode.id]
  const hasIncoming = new Set(nodeIds)
  const hasOutgoing = new Set(nodeIds)
  
  connections.value.forEach(conn => {
    hasOutgoing.add(conn.from.id)
    hasIncoming.add(conn.to.id)
  })
  
  // 开始节点应该有出度
  if (!hasOutgoing.has(startNode.id) && nodes.value.length > 0) {
    errors.push('开始节点没有连接到任何节点')
  }
  
  // 结束节点应该有入度
  if (!hasIncoming.has(endNode.id) && nodes.value.length > 0) {
    errors.push('没有节点连接到结束节点')
  }
  
  if (errors.length === 0) {
    ElMessage.success('✓ 工作流验证通过')
  } else {
    ElMessage.error(errors.join('\n'))
  }
  
  return errors.length === 0
}

// 清空画布
const clearCanvas = async () => {
  try {
    await ElMessageBox.confirm('确定清空画布吗？此操作不可恢复！', '警告', {
      type: 'warning'
    })
    nodes.value = []
    connections.value = []
    selectedNode.value = null
    saveToHistory()
    ElMessage.success('画布已清空')
  } catch (error: any) {
    if (error !== 'cancel') console.error('清空失败:', error)
  }
}

// 复制节点
const copyNode = (node: any) => {
  copiedNode.value = JSON.parse(JSON.stringify(node))
  ElMessage.success('节点已复制')
}

// 粘贴节点
const pasteNode = () => {
  if (!copiedNode.value) {
    ElMessage.warning('没有可粘贴的节点')
    return
  }
  
  const newNode = JSON.parse(JSON.stringify(copiedNode.value))
  newNode.id = `node_${Date.now()}`
  newNode.x += 20
  newNode.y += 20
  newNode.name = `${newNode.name} (副本)`
  
  nodes.value.push(newNode)
  saveToHistory()
  ElMessage.success('节点已粘贴')
}

// 删除选中节点
const deleteSelectedNode = () => {
  if (!selectedNode.value) {
    ElMessage.warning('请先选择节点')
    return
  }
  deleteNode(selectedNode.value.id)
  selectedNode.value = null
}

// 键盘快捷键
const setupShortcuts = () => {
  document.addEventListener('keydown', (event) => {
    // Ctrl+C 复制
    if (event.ctrlKey && event.key === 'c' && selectedNode.value) {
      event.preventDefault()
      copyNode(selectedNode.value)
    }
    
    // Ctrl+V 粘贴
    if (event.ctrlKey && event.key === 'v') {
      event.preventDefault()
      pasteNode()
    }
    
    // Delete 删除
    if (event.key === 'Delete' && selectedNode.value) {
      event.preventDefault()
      deleteSelectedNode()
    }
    
    // Ctrl+Z 撤销
    if (event.ctrlKey && event.key === 'z') {
      event.preventDefault()
      if (event.shiftKey) {
        redo()
      } else {
        undo()
      }
    }
    
    // Ctrl+Y 重做
    if (event.ctrlKey && event.key === 'y') {
      event.preventDefault()
      redo()
    }
    
    // Ctrl+S 保存
    if (event.ctrlKey && event.key === 's') {
      event.preventDefault()
      saveWorkflow()
    }
    
    // 方向键移动节点
    if (selectedNode.value && ['ArrowUp', 'ArrowDown', 'ArrowLeft', 'ArrowRight'].includes(event.key)) {
      event.preventDefault()
      const step = event.shiftKey ? 10 : 1
      switch (event.key) {
        case 'ArrowUp': selectedNode.value.y -= step; break
        case 'ArrowDown': selectedNode.value.y += step; break
        case 'ArrowLeft': selectedNode.value.x -= step; break
        case 'ArrowRight': selectedNode.value.x += step; break
      }
    }
  })
}
</script>

<style scoped lang="less">
.workflow-designer-container {
  height: 100%;
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
  
  .header-left {
    display: flex;
    align-items: center;
    gap: 12px;
    
    .workflow-title {
      font-size: 18px;
      font-weight: 600;
      color: #303133;
    }
  }
  
  .header-right {
    display: flex;
    gap: 12px;
  }
}

.designer-body {
  flex: 1;
  display: flex;
  overflow: hidden;
}

.component-panel {
  width: 220px;
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
  
  .component-section {
    margin-bottom: 16px;
    
    .section-title {
      font-size: 12px;
      font-weight: 600;
      color: #909399;
      margin-bottom: 8px;
      text-transform: uppercase;
    }
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
    
    .toolbar-right {
      display: flex;
      gap: 8px;
    }
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
      width: 2000px;
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
      width: 2000px;
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
      min-width: 1000px;
      min-height: 600px;
      transform-origin: top left;
      transition: transform 0.3s;
      z-index: 2;
    }
  }
}

.node {
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
  
  .node-header {
    display: flex;
    align-items: center;
    gap: 8px;
    padding: 12px 16px;
    background: #f5f7fa;
    border-bottom: 1px solid #e4e7ed;
    border-radius: 8px 8px 0 0;
    font-weight: 600;
    color: #303133;
    
    .el-icon {
      font-size: 18px;
    }
    
    .node-actions {
      margin-left: auto;
      display: flex;
      gap: 4px;
      opacity: 0;
      transition: opacity 0.3s;
      
      .action-btn, .delete-btn {
        padding: 4px;
        font-size: 14px;
        
        &:hover {
          background: rgba(0,0,0,0.1);
        }
      }
      
      .delete-btn:hover {
        color: #F56C6C;
      }
    }
    
    &:hover .node-actions {
      opacity: 1;
    }
  }
  
  .node-connector {
    position: absolute;
    right: -12px;
    top: 50%;
    transform: translateY(-50%);
    width: 24px;
    height: 24px;
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
      font-size: 14px;
    }
    
    &:hover {
      background: #67C23A;
      transform: translateY(-50%) scale(1.2);
    }
  }
  
  &:hover .node-connector {
    opacity: 1;
  }
  
  .node-body {
    padding: 12px 16px;
    
    .node-info {
      font-size: 13px;
      color: #606266;
      line-height: 1.5;
    }
  }
  
  &.start-node {
    border-color: #67C23A;
    
    .node-header {
      background: #f0f9eb;
      color: #67C23A;
    }
  }
  
  &.end-node {
    border-color: #909399;
    
    .node-header {
      background: #f4f4f5;
      color: #909399;
    }
  }
  
  &.node-approver {
    border-color: #409EFF;
    
    .node-header {
      background: #ecf5ff;
      color: #409EFF;
    }
  }
  
  &.node-condition {
    border-color: #E6A23C;
    
    .node-header {
      background: #fdf6ec;
      color: #E6A23C;
    }
  }
  
  &.node-copyer {
    border-color: #909399;
    
    .node-header {
      background: #f4f4f5;
      color: #909399;
    }
  }
}

.property-panel {
  width: 300px;
  border-left: 1px solid #e4e7ed;
  padding: 16px;
  background: #fafafa;
  
  .panel-title {
    font-size: 14px;
    font-weight: 600;
    color: #303133;
    margin-bottom: 12px;
  }
}
</style>
