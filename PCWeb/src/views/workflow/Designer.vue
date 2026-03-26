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
        <div
          class="component-item"
          v-for="node in nodeTemplates"
          :key="node.type"
          draggable="true"
          @dragstart="onDragStart($event, node)"
        >
          <el-icon :component="node.icon" />
          <span>{{ node.name }}</span>
        </div>
      </div>

      <!-- 中间画布 -->
      <div class="canvas-container">
        <div class="canvas-toolbar">
          <el-button-group>
            <el-button @click="zoomOut">
              <el-icon><ZoomOut /></el-icon>
            </el-button>
            <el-button @click="resetZoom">
              <el-icon><FullScreen /></el-icon>
            </el-button>
            <el-button @click="zoomIn">
              <el-icon><ZoomIn /></el-icon>
            </el-button>
          </el-button-group>
        </div>
        <div class="canvas" ref="canvasRef" @dragover="onDragOver" @drop="onDrop">
          <div class="canvas-content" :style="{ transform: `scale(${zoomLevel})` }">
            <!-- 开始节点 -->
            <div class="node start-node" :style="getNodePosition(startNode)">
              <div class="node-header">
                <el-icon><Play /></el-icon>
                <span>开始</span>
              </div>
            </div>

            <!-- 动态节点 -->
            <div
              v-for="node in nodes"
              :key="node.id"
              class="node"
              :class="['node-' + node.type, { selected: selectedNode?.id === node.id }]"
              :style="getNodePosition(node)"
              @click="selectNode(node)"
            >
              <div class="node-header">
                <el-icon :component="getNodeIcon(node.type)" />
                <span>{{ node.name }}</span>
                <el-button class="delete-btn" type="text" @click.stop="deleteNode(node.id)">
                  <el-icon><Close /></el-icon>
                </el-button>
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
                <div v-if="node.type === 'condition'" class="node-info">
                  <span>条件：{{ node.conditionExpression || '未设置' }}</span>
                </div>
                <div v-if="node.type === 'copyer'" class="node-info">
                  <span>抄送人：{{ node.copyerNames?.join('、') || '未设置' }}</span>
                </div>
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
  QuestionFilled
} from '@element-plus/icons-vue'
import { ElMessage } from 'element-plus'

const router = useRouter()
const route = useRoute()

const canvasRef = ref<HTMLDivElement | null>(null)
const zoomLevel = ref(1)
const selectedNode = ref<any>(null)
const workflowName = ref('')

// 节点模板
const nodeTemplates = [
  { type: 'approver', name: '审批人', icon: UserFilled },
  { type: 'condition', name: '条件分支', icon: Connection },
  { type: 'copyer', name: '抄送人', icon: Share },
  { type: 'notifier', name: '通知器', icon: QuestionFilled }
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
    condition: Connection,
    copyer: Share,
    notifier: QuestionFilled
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
  draggedNode.approverType = 'select'
  draggedNode.approverIds = []
  draggedNode.roleIds = []
  draggedNode.approverNames = []
  draggedNode.roleNames = []
  draggedNode.copyerIds = []
  draggedNode.copyerNames = []
  draggedNode.conditionExpression = ''

  nodes.value.push(draggedNode)
  draggedNode = null
}

// 选择节点
const selectNode = (node: any) => {
  selectedNode.value = node
}

// 删除节点
const deleteNode = (nodeId: string) => {
  nodes.value = nodes.value.filter(n => n.id !== nodeId)
  if (selectedNode.value?.id === nodeId) {
    selectedNode.value = null
  }
  ElMessage.success('节点已删除')
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
  
  ElMessage.success('节点已更新')
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

// 保存工作流
const saveWorkflow = () => {
  const workflowData = {
    name: workflowName.value || '未命名工作流',
    nodes: [startNode, ...nodes.value, endNode]
  }
  console.log('保存工作流:', workflowData)
  ElMessage.success('工作流已保存')
}

// 发布工作流
const publishWorkflow = () => {
  const workflowData = {
    name: workflowName.value || '未命名工作流',
    nodes: [startNode, ...nodes.value, endNode]
  }
  console.log('发布工作流:', workflowData)
  ElMessage.success('工作流已发布')
}

// 返回列表
const backToList = () => {
  router.push('/workflow/list')
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
  width: 200px;
  border-right: 1px solid #e4e7ed;
  padding: 16px;
  background: #fafafa;
  
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
    padding: 12px;
    margin-bottom: 8px;
    background: #fff;
    border: 1px solid #e4e7ed;
    border-radius: 4px;
    cursor: grab;
    transition: all 0.3s;
    
    &:hover {
      border-color: #409EFF;
      box-shadow: 0 2px 8px rgba(64,158,255,0.2);
    }
    
    .el-icon {
      font-size: 18px;
      color: #409EFF;
    }
    
    span {
      font-size: 13px;
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
    right: 16px;
    z-index: 10;
  }
  
  .canvas {
    width: 100%;
    height: 100%;
    overflow: auto;
    
    .canvas-content {
      position: relative;
      min-width: 1000px;
      min-height: 600px;
      transform-origin: top left;
      transition: transform 0.3s;
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
    
    .delete-btn {
      margin-left: auto;
      opacity: 0;
      transition: opacity 0.3s;
    }
    
    &:hover .delete-btn {
      opacity: 1;
    }
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
