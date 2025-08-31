<template>
  <header :class="['navbar', { scrolled: isScrolled }]">
    <div class="navbar-container">
      <router-link to="/" class="logo">Library System</router-link>

      <nav class="nav-links">
        <router-link to="/" class="nav-item">首页</router-link>
        <router-link to="/books" class="nav-item">图书资源</router-link>

        <div class="nav-item dropdown">
          <span>借阅服务</span>
          <div class="dropdown-menu">
            <router-link to="/services/rules">借阅规则</router-link>
            <router-link to="/services/reserve">图书预约</router-link>
            <router-link to="/services/renew">图书续借</router-link>
          </div>
        </div>

        <div class="nav-item dropdown">
          <span>我的图书馆</span>
          <div class="dropdown-menu">
            <router-link to="/user/borrowed">当前借阅</router-link>
            <router-link to="/user/favorites">收藏图书</router-link>

            <!-- 添加我的书单 -->
            <router-link to="/user/booklist">我的书单</router-link>


            <router-link to="/user/history">借阅历史</router-link>
          </div>
        </div>

        <div class="nav-item dropdown">
          <span>管理员操作</span>
          <div class="dropdown-menu">
            <!-- This is now the single entry point to the admin area -->
            <router-link to="/admin/dashboard">管理中心</router-link>
          </div>
        </div>

        
        <router-link to="/about" class="nav-item">关于我们</router-link>
      </nav>

      <div class="auth-btn">
        <router-link to="/login" class="login">登录</router-link>
      </div>
    </div>
  </header>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue'

const showServices = ref(false)
const showLibrary = ref(false)
const isScrolled = ref(false)
let   scrollEl     = null   // 实际滚动的容器（#app）

function handleScroll () {
  if (scrollEl) {
    isScrolled.value = scrollEl.scrollTop > 0
  }
}

onMounted(() => {
  // Vue 在挂载后，#app 已经存在于 DOM
  scrollEl = document.getElementById('app') || window
  // 只在 #app 上监听
  scrollEl.addEventListener('scroll', handleScroll, { passive: true })
  handleScroll()            // 初始化一次，保证刷新后状态正确
})

onUnmounted(() => {
  scrollEl && scrollEl.removeEventListener('scroll', handleScroll)
})
</script>


<style>
/* ---------- NAVBAR 基础 ---------- */
.navbar {
  position: fixed;
  top: 0;
  width: 100%;
  z-index: 1000;
  background-color: transparent;
  transition:
    background 0.3s ease,
    box-shadow 0.3s ease,
    height 1s ease;
  box-shadow: none;
  height: 4rem;
}

/* 顶部渐变遮罩增强可读性 */
.navbar::before {
  content: "";
  position: absolute;
  inset: 0;
  background: linear-gradient(rgba(0,0,0,0.6), rgba(0,0,0,0));
  pointer-events: none;
  opacity: 1;
  transition: opacity .3s ease;
}

.navbar.scrolled {
  background-color: #004b8d;
  box-shadow: 0 2px 6px rgba(0,0,0,.1);
  height: 3.5rem;
}
.navbar.scrolled::before { opacity: 0; }

.navbar-container {
  max-width: 1200px;
  margin: auto;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.75rem 1rem;
}

.logo {
  font-size: 1.5rem;
  font-weight: 700;
  color: #fff;
  text-decoration: none;
}

/* ---------- 链接区 ---------- */
.nav-links {
  display: flex;
  gap: 1rem;
  align-items: center;
}

.nav-item,
.nav-item span {
  width: 120px;
  text-align: center;
  color: #fff;
  font-size: 0.85rem;
  font-weight: 700;
  text-decoration: none;
  padding: 0.3rem 0.5rem;
  border-radius: 6px;
  position: relative;
  cursor: pointer;
  line-height: 1;
  text-shadow: 0 0 4px rgba(0,0,0,0.6);
}

/* 下划线动画 */
.nav-item::after {
  content: "";
  position: absolute;
  left: 50%;                  /* 居中起点 */
  transform: translateX(-50%) scaleX(0);  /* 居中 + 缩放隐藏 */
  bottom: 0.1rem;
  height: 2px;
  width: 50%;                 /* 控制下划线宽度（比如 50%） */
  background: #ffffff;
  opacity: 0;
  transition: transform 0.25s ease, opacity 0.25s ease;
  transform-origin: center;
  pointer-events: auto;       /* 允许伪元素捕获鼠标事件 */
}

.nav-item:hover::after {
  transform: translateX(-50%) scaleX(1);
  opacity: 1;
}


.nav-item:hover { background: transparent; }

/* ---------- 下拉菜单 ---------- */
.dropdown {
  position: relative;
}

.dropdown:hover .dropdown-menu {
  display: flex;
  flex-direction: column;
}

.dropdown-menu {
  display: none;
  position: absolute;
  top: calc(100% + 0.1rem);
  left: 0%;
  transform: translateX(-50%);
  background: #deeaff;
  border: 1px solid #dce6f1;
  text-shadow: 0 0 1px rgba(0, 0, 0, 0.08);

  flex-direction: column;
  min-width: 120px;
  z-index: 10;
  text-align: center;
}

.dropdown-menu a {
  color: #000;
  text-decoration: none;
  padding: 0.8rem 0.75rem;
  font-size: 0.85rem;
  font-weight: 600;
  line-height: 1.2;
  width: 100%;              /* 强制每个链接占满整行 */
  box-sizing: border-box;   /* 使 padding 不影响 width */
}

.dropdown-menu a:hover {
  background: #ffffff;
}

/* ---------- 登录按钮 ---------- */
.auth-btn .login {
  color: #004b8d;
  background: #fff;
  padding: 0.4rem 1rem;
  border-radius: 6px;
  text-decoration: none;
  font-weight: 600;
  font-size: 0.9rem;
}

.auth-btn .login:hover { background: #f0f0f0; }
</style>
