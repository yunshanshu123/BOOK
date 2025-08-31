<template>
  <div class="notice-card">
    <h3 class="title">最新通知</h3>
    <ul class="list">
      <li class="item" v-for="(n, i) in notices" :key="i">{{ n }}</li>
    </ul>
  </div>
</template>

<script setup>
const notices = [
  '图书馆将在8月10日进行系统维护。',
  '暑期开放时间已更新，请前往公告查看。',
  '欢迎新同学入馆，请及时激活借阅权限。'
]
</script>

<style scoped>
.notice-card {
  color: black;
  background: white;
  padding: 16px;
  border-radius: 12px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.05);
}
.title {
  font-size: 16px;
  font-weight: bold;
  margin-bottom: 12px;
}
.list {
  padding-left: 16px;
}
.item {
  font-size: 14px;
  color: #444;
  margin-bottom: 6px;
}
</style>
