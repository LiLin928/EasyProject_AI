<template>
  <div class="report-canvas-container">
    <!-- 顶部工具栏 -->
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
    </div>
    
    <!-- 画布区域 -->
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
        <!-- 报表组件 -->
        <div
          v-for="widget in widgets"
          :key="widget.id"
          class="report-widget"
          :class="{ selected: selectedWidget?.id === widget.id, dragging: isDragging && draggingWidget?.id === widget.id }"
          :style="getWidgetPosition(widget)"
          @click.stop="selectWidget(widget)"
          @mousedown.stop="startDragWidget($event, widget)"
        >
          <div class="widget-header">
            <span class="widget-title">{{ widget.name }}</span>
            <el-button class="delete-btn" type="text" @click.stop="deleteWidget(widget.id)">
              <el-icon><Close /></el-icon>
            </el-button>
          </div>
          <div class="widget-content">
            <div v-if="widget.type === 'table'" class="widget-preview">
              <el-table :data="mockTableData" size="small" :show-header="false">
                <el-table-column prop="name" label="名称" />
                <el-table-column prop="value" label="数值" />
              </el-table>
            </div>
            <div v-else-if="widget.type === 'chart'" class="widget-preview">
              <div class="chart-placeholder">
                <el-icon><DataAnalysis /></el-icon>
                <span>图表预览</span>
              </div>
            </div>
            <div v-else-if="widget.type === 'card'" class="widget-preview">
              <div class="card-value">1,234</div>
              <div class="card-label">{{ widget.name }}</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { ElMessage } from 'element-plus'
import { ZoomOut, ZoomIn, Grid, Close, DataAnalysis } from '@element-plus/icons-vue'

const canvasRef = ref<HTMLDivElement | null>(null)
const canvasZoom = ref(1)
const showGrid = ref(true)
const selectedWidget = ref<any>(null)

// 拖拽相关
const isDragging = ref(false)
const draggingWidget = ref<any>(null)
const dragOffset = ref({ x: 0, y: 0 })
const draggedComponent = ref<any>(null)

// 组件列表
const widgets = ref<any[]>([
  {
    id: 'widget_1',
    type: 'table',
    name: '数据表格',
    x: 50,
    y: 50,
    width: 400,
    height: 300,
    config: {}
  },
  {
    id: 'widget_2',
    type: 'card',
    name: '指标卡片',
    x: 500,
    y: 50,
    width: 200,
    height: 150,
    config: {}
  }
])

// 模拟数据
const mockTableData = [
  { name: '项目 1', value: 100 },
  { name: '项目 2', value: 200 },
  { name: '项目 3', value: 150 }
]

// 获取组件位置
const getWidgetPosition = (widget: any) => {
  return {
    left: `${widget.x}px`,
    top: `${widget.y}px`,
    width: `${widget.width}px`,
    height: `${widget.height}px`
  }
}

// 拖拽开始
const onDragStart = (event: DragEvent, comp: any) => {
  draggedComponent.value = { ...comp }
  event.dataTransfer?.setData('application/json', JSON.stringify(comp))
}

const onDragOver = (event: DragEvent) => {
  event.preventDefault()
}

const onDrop = (event: DragEvent) => {
  event.preventDefault()
  if (!canvasRef.value || !draggedComponent.value) return
  
  const rect = canvasRef.value.getBoundingClientRect()
  const x = (event.clientX - rect.left) / canvasZoom.value - 100
  const y = (event.clientY - rect.top) / canvasZoom.value - 50
  
  const newWidget = {
    id: `widget_${Date.now()}`,
    type: draggedComponent.value.type,
    name: draggedComponent.value.name,
    x: Math.max(0, x),
    y: Math.max(0, y),
    width: 300,
    height: 200,
    config: {
      dataType: 'static',
      showTitle: true
    }
  }
  
  widgets.value.push(newWidget)
  selectWidget(newWidget)
  draggedComponent.value = null
}

// 组件拖拽
const startDragWidget = (event: MouseEvent, widget: any) => {
  isDragging.value = true
  draggingWidget.value = widget
  
  const rect = canvasRef.value?.getBoundingClientRect()
  if (rect) {
    dragOffset.value = {
      x: (event.clientX - rect.left) / canvasZoom.value - widget.x,
      y: (event.clientY - rect.top) / canvasZoom.value - widget.y
    }
  }
}

// 鼠标移动
const onMouseMove = (event: MouseEvent) => {
  if (!canvasRef.value) return
  
  const rect = canvasRef.value.getBoundingClientRect()
  
  // 处理组件拖拽
  if (isDragging.value && draggingWidget.value) {
    const x = (event.clientX - rect.left) / canvasZoom.value - dragOffset.value.x
    const y = (event.clientY - rect.top) / canvasZoom.value - dragOffset.value.y
    
    draggingWidget.value.x = Math.max(0, x)
    draggingWidget.value.y = Math.max(0, y)
  }
}

// 鼠标松开
const onMouseUp = () => {
  if (isDragging.value) {
    isDragging.value = false
    draggingWidget.value = null
  }
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
  ElMessage.success('组件已删除')
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

// 暴露方法给父组件
defineExpose({
  onDragStart,
  onMouseMove,
  onMouseUp
})
</script>

<style scoped lang="less">
.report-canvas-container {
  flex: 1;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.canvas-toolbar {
  height: 40px;
  background: #fff;
  border-bottom: 1px solid #e4e7ed;
  display: flex;
  align-items: center;
  padding: 0 16px;
  gap: 16px;
}

.canvas {
  flex: 1;
  overflow: auto;
  position: relative;
  background: #f5f7fa;
  
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
  
  .report-widget {
    position: absolute;
    background: #fff;
    border: 2px solid #e4e7ed;
    border-radius: 4px;
    box-shadow: 0 2px 12px rgba(0,0,0,0.1);
    cursor: move;
    user-select: none;
    
    &.selected {
      border-color: #409EFF;
      box-shadow: 0 0 0 2px rgba(64,158,255,0.3);
    }
    
    &.dragging {
      opacity: 0.8;
      box-shadow: 0 8px 24px rgba(0,0,0,0.2);
      z-index: 100 !important;
    }
    
    .widget-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 8px 12px;
      background: #f5f7fa;
      border-bottom: 1px solid #e4e7ed;
      border-radius: 4px 4px 0 0;
      
      .widget-title {
        font-size: 13px;
        font-weight: 600;
        color: #303133;
      }
      
      .delete-btn {
        opacity: 0;
        transition: opacity 0.3s;
        
        &:hover {
          color: #F56C6C;
        }
      }
      
      &:hover .delete-btn {
        opacity: 1;
      }
    }
    
    .widget-content {
      height: calc(100% - 36px);
      padding: 12px;
      overflow: auto;
      
      .widget-preview {
        height: 100%;
        
        .chart-placeholder {
          height: 100%;
          display: flex;
          flex-direction: column;
          justify-content: center;
          align-items: center;
          color: #909399;
          
          .el-icon {
            font-size: 48px;
            margin-bottom: 8px;
          }
        }
        
        .card-value {
          font-size: 32px;
          font-weight: bold;
          color: #409EFF;
          text-align: center;
          padding: 20px 0;
        }
        
        .card-label {
          text-align: center;
          color: #909399;
          font-size: 13px;
        }
      }
    }
  }
}
</style>
