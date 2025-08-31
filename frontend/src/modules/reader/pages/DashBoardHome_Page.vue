<template>
  <layout>
    <div class="page-body">
      <!-- 用户信息卡片 -->
      <div class="user-card">
        <div class="user-info">
          <img :src="avatar" class="avatar" />
          <div class="info">
            <div><strong>用户名：</strong>{{ userName }}</div>
            <div><strong>真实姓名：</strong>{{ fullName }}</div>
            <div><strong>昵称：</strong>{{ nickName }}</div>
            <div><strong>信誉分：</strong>{{ creditScore }}</div>
            <div><strong>账户状态：</strong><span :class="statusClass">{{ accountStatus }}</span></div>
            <div><strong>权限：</strong>{{ permission }}</div>
          </div>
        </div>
        <div class="user-icons">
          <div v-for="item in icons" :key="item.label" class="icon-item">
            <div class="icon-circle" />
            <div class="icon-label">{{ item.label }}</div>
          </div>
        </div>
      </div>


      <!-- 信息统计卡片 -->
      <div class="info-cards">
        <DashBoardInfoCard title="当前借阅" count="0" className="card-red" />
        <DashBoardInfoCard title="超期图书" count="0" className="card-orange" />
        <DashBoardInfoCard title="委托到书" count="0" className="card-green" />
        <DashBoardInfoCard title="预约到书" count="0" className="card-blue" />
      </div>

      <!-- 通知列表 -->
      <DashBoardNotificationList />
    </div>
  </layout>
</template>

<script setup>
import layout from '@/modules/reader/reader_DashBoard_layout/layout.vue'
import DashBoardInfoCard from '../components/DashBoardInfoCard.vue'
import DashBoardNotificationList from '../components/DashBoardNotificationList.vue'

import {useUserStore} from '@/stores/user.js'
import { computed } from 'vue'

// 引入用户 store
const userStore = useUserStore()

const baseAvatarUrl = 'http://localhost:5000/avatars/'
const avatar = computed(() => baseAvatarUrl + userStore.avatar)

const userName = computed(() => userStore.userName)
const fullName = computed(() => userStore.fullName)
const nickName = computed(() => userStore.nickName)
const creditScore = computed(() => userStore.creditScore)
const accountStatus = computed(() => userStore.accountStatus)
const permission = computed(() => userStore.permission)

const statusClass = computed(() => {
  return accountStatus.value === '正常' ? 'status-normal' : 'status-blocked'
})

const icons = [
  { label: '可借阅' },
  { label: '可预约' },
  { label: '可委托' }
]
</script>

<style scoped>
.layout {
  display: flex;
}
.main-content {
  flex: 1;
  background: #f8f9fa;
  min-height: 100vh;
}
.page-body {
  padding: 24px;
  display: flex;
  flex-direction: column;
  gap: 24px;
}
.user-card {
  background: white;
  border-radius: 12px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  padding: 24px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}
.user-info {
  display: flex;
  align-items: center;
  gap: 16px;
}
.avatar {
  width: 64px;
  height: 64px;
  border-radius: 50%;
  background: #ccc;
}
.user-icons {
  display: flex;
  gap: 32px;
}
.icon-item {
  text-align: center;
}
.icon-circle {
  width: 56px;
  height: 56px;
  border-radius: 50%;
  border: 2px solid #ccc;
  margin: 0 auto 8px;
}
.icon-label {
  font-size: 14px;
  color: #555;
}
.info-cards {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 16px;
}

.info {
  color: black;
  font-size: 14px;
  line-height: 24px;
}
.status-normal {
  color: green;
  font-weight: bold;
}
.status-blocked {
  color: red;
  font-weight: bold;
}

</style>
