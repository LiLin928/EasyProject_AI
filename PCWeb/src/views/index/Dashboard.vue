<template>
  <div class="dashboard-page">
    <el-row :gutter="20">
      <!-- 数据统计卡片 -->
      <el-col :span="6">
        <el-card shadow="hover">
          <div class="stat-card">
            <div class="stat-title">待审批申请</div>
            <div class="stat-value">12</div>
          </div>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card shadow="hover">
          <div class="stat-card">
            <div class="stat-title">待确认订单</div>
            <div class="stat-value">8</div>
          </div>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card shadow="hover">
          <div class="stat-card">
            <div class="stat-title">待审核发票</div>
            <div class="stat-value">5</div>
          </div>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card shadow="hover">
          <div class="stat-card">
            <div class="stat-title">待付款</div>
            <div class="stat-value">¥ 125,000</div>
          </div>
        </el-card>
      </el-col>
    </el-row>

    <!-- 采购流程 -->
    <el-row :gutter="20" style="margin-top: 20px;">
      <el-col :span="24">
        <el-card>
          <template #header><span>采购流程</span></template>
          <el-steps :active="2" finish-status="success" align-center>
            <el-step title="采购申请" description="提交申请" />
            <el-step title="审批" description="领导审批" />
            <el-step title="采购订单" description="生成订单" />
            <el-step title="收货" description="确认收货" />
            <el-step title="发票" description="录入发票" />
            <el-step title="付款" description="财务付款" />
            <el-step title="结算" description="完成结算" />
          </el-steps>
        </el-card>
      </el-col>
    </el-row>

    <!-- 快捷入口 -->
    <el-row :gutter="20" style="margin-top: 20px;">
      <el-col :span="24">
        <el-card>
          <template #header><span>快捷入口</span></template>
          <el-row :gutter="20">
            <el-col :span="4" v-for="item in shortcuts" :key="item.path">
              <div class="shortcut-item" @click="$router.push(item.path)">
                <el-icon :size="32"><component :is="item.icon" /></el-icon>
                <div class="shortcut-title">{{ item.name }}</div>
              </div>
            </el-col>
          </el-row>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script setup lang="ts">
const shortcuts = [
  { name: '采购申请', path: '/srm/purchase-request', icon: 'Document' },
  { name: '采购订单', path: '/srm/purchase-order', icon: 'ShoppingCart' },
  { name: '供应商', path: '/srm/supplier', icon: 'User' },
  { name: '发票管理', path: '/srm/invoice', icon: 'Tickets' },
  { name: '付款管理', path: '/srm/payment', icon: 'Money' },
  { name: '结算管理', path: '/srm/settlement', icon: 'Wallet' },
  { name: '菜单管理', path: '/basic/menu', icon: 'Menu' },
  { name: '工作流', path: '/workflow/list', icon: 'Connection' }
]
</script>

<style scoped lang="less">
.dashboard-page {
  .stat-card {
    text-align: center;
    padding: 20px 0;
    
    .stat-title {
      font-size: 14px;
      color: #909399;
      margin-bottom: 10px;
    }
    
    .stat-value {
      font-size: 28px;
      font-weight: bold;
      color: #409EFF;
    }
  }
  
  .shortcut-item {
    text-align: center;
    padding: 20px;
    cursor: pointer;
    border-radius: 8px;
    transition: all 0.3s;
    
    &:hover {
      background-color: #f5f7fa;
    }
    
    .shortcut-title {
      margin-top: 10px;
      font-size: 14px;
      color: #606266;
    }
  }
}
</style>
