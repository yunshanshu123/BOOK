<template>
  <div class="book-booklist-page">
    <div class="page-header">
      <h1 class="text-2xl font-bold">我的书单</h1>
      <button 
        class="create-btn"
        @click="createBooklist">
        <i class="fas fa-plus mr-1"></i>新建书单
      </button>
    </div>
    
    <div class="booklist-container">
      <!-- 书单列表 -->
      <div v-if="booklists.length" class="booklist-grid">
        <div v-for="booklist in booklists" :key="booklist.id" class="booklist-card">
          <h3>{{ booklist.name }}</h3>
          <p>{{ booklist.bookCount }} 本书</p>
        </div>
      </div>
      
      <!-- 空状态 -->
      <div v-else class="empty-state">
        <i class="fas fa-book-open text-4xl text-gray-300 mb-4"></i>
        <p>您还没有创建任何书单</p>
        <button 
          class="create-btn mt-4"
          @click="createBooklist">
          <i class="fas fa-plus mr-1"></i>创建第一个书单
        </button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'BookBooklistPage',
  data() {
    return {
      booklists: [] // 初始为空
    }
  },
  mounted() {
    // 这里可以添加加载书单数据的逻辑
    // this.loadBooklists();
  },
  methods: {
    createBooklist() {
      console.log("创建新书单逻辑");
      // 实际项目中这里会有创建书单的API调用
    },
    loadBooklists() {
      // 示例数据
      this.booklists = [
        { id: 1, name: '编程学习', bookCount: 5 },
        { id: 2, name: '科幻小说', bookCount: 3 },
        { id: 3, name: '历史读物', bookCount: 7 }
      ]
    }
  }
}
</script>

<style scoped>
.book-booklist-page {
  max-width: 1200px;
  margin: 2rem auto;
  padding: 0 1.5rem;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
}

.create-btn {
  background-color: #3b82f6;
  color: white;
  padding: 0.5rem 1rem;
  border-radius: 0.25rem;
  font-weight: 500;
  transition: background-color 0.2s;
}

.create-btn:hover {
  background-color: #2563eb;
}

.empty-state {
  text-align: center;
  padding: 3rem;
  background-color: #f9fafb;
  border-radius: 0.5rem;
  color: #6b7280;
}

.booklist-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 1.5rem;
}

.booklist-card {
  background: white;
  border-radius: 0.5rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05);
  padding: 1.5rem;
  transition: transform 0.2s;
}

.booklist-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 15px rgba(0, 0, 0, 0.1);
}
</style>