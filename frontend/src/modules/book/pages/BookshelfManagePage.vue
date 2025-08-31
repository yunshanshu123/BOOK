<template>
  <LayoutDefault>
    <div class="page-container">
      <!-- 左侧切换按钮 -->
      <div class="side-buttons">
        <button 
          @click="activePage = 'bookshelf'" 
          :class="{ active: activePage === 'bookshelf' }"
        >
          书籍管理
        </button>
        <button 
          @click="activePage = 'shelf-manage'" 
          :class="{ active: activePage === 'shelf-manage' }"
        >
          书架管理
        </button>
      </div>
      
      <!-- 右侧内容区 -->
      <div class="content-area">
        <BookshelfManage v-if="activePage === 'bookshelf'" />
        <BookshelfManage2 v-if="activePage === 'shelf-manage'" />
      </div>
    </div>
  </LayoutDefault>
</template>

<script setup>
import { ref } from 'vue';
import LayoutDefault from '@/shared/components/layouts/LayoutDefault.vue';
import BookshelfManage from '@/modules/book/components/BookshelfManage.vue';
import BookshelfManage2 from '@/modules/book/components/BookshelfManage2.vue';

const activePage = ref('bookshelf');
</script>

<style scoped>
.page-container {
  display: flex;
  height: 100%;
 
}

.side-buttons {
  width: 150px;
  padding: 20px 10px;
  background: #f5f5f5;
  display: flex;
  flex-direction: column;
  gap: 10px;
  height: 100%; /* 关键属性1：继承父元素高度 */
  min-height: 100vh; /* 关键属性2：至少覆盖整个视口高度 */
}

.side-buttons button {
  padding: 10px;
  border: none;
  background: #e0e0e0;
  cursor: pointer;
  border-radius: 4px;
  text-align: center;
}

.side-buttons button.active {
  background: #1890ff;
  color: white;
}

.side-buttons button:hover {
  background: #d0d0d0;
}

.side-buttons button.active:hover {
  background: #40a9ff;
}

.content-area {
  flex: 1;
  padding: 20px;
}
</style>