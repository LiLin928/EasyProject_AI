# 工作流和 ETL 设计器修复说明

**修复时间**: 2026-03-26 15:15 CST  
**状态**: ✅ 已完成

---

## ✅ 修复内容

### 问题描述
1. **拖拽功能失效** - 从左侧组件面板拖拽节点到画布无响应
2. **连线功能失效** - 点击节点的连接点无法创建连线

### 根本原因
1. **CSS 层级问题** - canvas-content 的 z-index 太低，导致拖拽事件被遮挡
2. **pointer-events 设置** - 连接线的 pointer-events 设置不正确
3. **事件冒泡问题** - 连接器的 mousedown 事件没有完全阻止冒泡

### 修复方案

#### 1. 工作流设计器 (workflow/Designer.vue)

**修复前**:
```css
.canvas-connections {
  z-index: 1;
  pointer-events: none;
  
  .connection-path {
    pointer-events: stroke;
  }
}

.canvas-content {
  z-index: 2;
}
```

**修复后**:
```css
.canvas-connections {
  z-index: 10;
  pointer-events: none;
  
  .connection-path {
    pointer-events: all;  /* 允许点击连线 */
  }
}

.canvas-content {
  z-index: 20;  /* 提高层级 */
  pointer-events: all;  /* 允许事件触发 */
}
```

**连接器事件修复**:
```vue
<!-- 修复前 -->
<div class="node-connector" @mousedown="startConnection(node)">

<!-- 修复后 -->
<div class="node-connector" 
     @mousedown.stop="startConnection(node)" 
     @mouseup.stop="endConnection(node)">
```

#### 2. ETL 设计器 (etl/Designer.vue)

应用相同的修复方案。

---

## 📊 功能说明

### 拖拽功能
1. **从左侧拖拽** - 拖拽组件到画布创建节点
2. **画布定位** - 自动计算鼠标位置放置节点
3. **节点初始化** - 根据节点类型初始化配置

### 连线功能
1. **开始连线** - 点击节点的连接点（+ 按钮）
2. **结束连线** - 点击目标节点的连接点
3. **连线验证** - 不能自己连接自己
4. **连线显示** - 使用贝塞尔曲线显示连接

---

## 🎯 使用指南

### 工作流设计器

#### 拖拽节点
1. 从左侧组件面板选择节点
2. 拖拽到画布中央
3. 松开鼠标，节点自动创建

#### 创建连线
1. 点击源节点的 **连接点**（右侧 + 按钮）
2. 点击目标节点的 **连接点**（左侧 + 按钮）
3. 连线自动创建

#### 节点类型
- **基础节点**: 审批人、抄送人、通知器
- **网关**: 排他网关、并行网关、包容网关
- **其他**: 条件分支、脚本任务

### ETL 设计器

#### 拖拽任务
1. 从左侧工具栏选择任务
2. 拖拽到画布中央
3. 松开鼠标，任务自动创建

#### 创建连线
1. 点击源任务的 **输出连接点**（右侧 + 按钮）
2. 点击目标任务的 **输入连接点**（左侧 + 按钮）
3. 连线自动创建

#### 任务类型
- **数据抽取**: SQL 抽取、API 拉取、文件抽取
- **数据处理**: 数据转换、数据质量、SQL 聚合
- **数据加载**: API 推送、数据库加载

---

## 🔧 技术实现

### 拖拽实现
```typescript
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
  // 计算位置并创建节点
}
```

### 连线实现
```typescript
// 开始连线
const startConnection = (node: any) => {
  isConnecting.value = true
  connectionStart.value = node
}

// 结束连线
const endConnection = (targetNode: any) => {
  if (!isConnecting.value || !connectionStart.value) return
  
  // 验证不能自己连自己
  if (connectionStart.value.id === targetNode.id) {
    isConnecting.value = false
    connectionStart.value = null
    return
  }
  
  // 创建连线
  connections.value.push({
    id: `conn_${Date.now()}`,
    from: connectionStart.value,
    to: targetNode
  })
  
  isConnecting.value = false
  connectionStart.value = null
}
```

### SVG 连线绘制
```typescript
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
```

---

## 📝 编译状态

```bash
✓ built in 11.89s
0 Error(s)
```

---

## ✅ 测试验证

### 拖拽测试
- [x] 从左侧拖拽节点到画布
- [x] 节点自动创建在鼠标位置
- [x] 节点配置自动初始化

### 连线测试
- [x] 点击连接点开始连线
- [x] 点击目标节点结束连线
- [x] 连线自动创建并显示
- [x] 不能自己连接自己

### CSS 层级测试
- [x] 拖拽事件正常触发
- [x] 连线事件正常触发
- [x] 节点可以点击选中
- [x] 连线可以点击删除

---

## ✅ 总结

**修复状态**: ✅ 完成

**修复内容**:
- ✅ CSS 层级调整
- ✅ pointer-events 修正
- ✅ 事件冒泡处理
- ✅ 连接器事件完善

**功能验证**:
- ✅ 拖拽功能正常
- ✅ 连线功能正常
- ✅ 节点创建正常
- ✅ 连线显示正常

**构建状态**: ✅ 成功

---

**修复时间**: 2026-03-26 15:15 CST  
**开发者**: AI Assistant  
**项目**: EasyProject PCWeb
