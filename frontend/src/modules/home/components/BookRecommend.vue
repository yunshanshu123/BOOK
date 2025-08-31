<template>
  <section class="book-recommend">
    <h2 class="section-title">推荐图书</h2>
    <div class="book-list">
      <div v-for="(book, index) in books" :key="index" class="book-card">
        <img :src="book.cover" class="book-cover" alt="book cover" />
        <div class="book-info">
          <h3 class="book-title">{{ book.title }}</h3>
          <p class="book-author">{{ book.author }}</p>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
const books = [
  {
    title: '人工智能导论',
    author: '周志华',
    cover: '/images/book1.jpg'
  },
  {
    title: '深入理解计算机系统',
    author: 'Randal E. Bryant',
    cover: '/images/book2.jpg'
  },
  {
    title: '数据结构与算法分析',
    author: 'Mark Allen Weiss',
    cover: '/images/book3.jpg'
  }
]
</script>

<style scoped>
.book-recommend {
  padding: 2rem 1rem;
  background-color: #ffffff;
}

.section-title {
  text-align: center;
  font-size: 1.5rem;
  font-weight: bold;
  color: #004b8d;
  margin-bottom: 1.5rem;
}

.book-list {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(160px, 1fr));
  gap: 1.5rem;
  max-width: 1000px;
  margin: auto;
}

.book-card {
  background-color: #f5faff;
  border-radius: 12px;
  overflow: hidden;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.05);
  transition: transform 0.2s;
}

.book-card:hover {
  transform: translateY(-4px);
}

.book-cover {
  width: 100%;
  height: 200px;
  object-fit: cover;
}

.book-info {
  padding: 0.75rem;
}

.book-title {
  font-size: 1rem;
  font-weight: 600;
  color: #004b8d;
}

.book-author {
  font-size: 0.9rem;
  color: #666;
}
</style>
