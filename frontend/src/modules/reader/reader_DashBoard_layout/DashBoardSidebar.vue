<template>
  <div class="sidebar">
    <div class="logo">我的图书馆</div>
    <ul class="nav-list">
      <li>
        <router-link to="/my/dashboard" class="nav-item" >
          首页
        </router-link>
      </li>
      <li>
        <router-link to="/user/borrowed" class="nav-item">
          当前借阅
        </router-link>
      </li>
      <li>
        <router-link to="/user/favorites" class="nav-item" >
          收藏图书
        </router-link>
      </li>
      <li>
        <router-link to="/user/booklist" class="nav-item" >
          我的书单
        </router-link>
      </li>
      <li>
        <router-link to="/user/history" class="nav-item" >
          借阅历史
        </router-link>
      </li>
    </ul>
  </div>
</template>

<script setup>
</script>

<style scoped>
.sidebar {
  width: 220px;
  height: 100vh;
  background: #1e293b;
  color: white;
  padding: 24px 16px;
}
.logo {
  font-size: 20px;
  font-weight: bold;
  margin-bottom: 32px;
}
.nav-list {
  list-style: none;
  padding: 0;
}
.nav-item {
  display: block;
  padding: 12px 8px;
  border-radius: 6px;
  cursor: pointer;
  transition: background 0.3s;
  color: #fff;
  text-decoration: none;
}
.nav-item:hover {
  background: #334155;
}
.nav-item-active {
  background: #475569;
}
</style>
