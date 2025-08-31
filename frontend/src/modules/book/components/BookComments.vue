<!-- frontend/src/modules/book/components/BookComments.vue -->

<template>
  <div class="comments-container">
    <h2>图书评论</h2>

    <div class="isbn-search">
      <input v-model="searchISBN" placeholder="请输入图书ISBN" @keyup.enter="searchComments" />
      <button @click="searchComments">搜索评论</button>
    </div>

    <!-- 新增评论表单 -->
    <div v-if="searchISBN" class="add-comment">
      <h3>发表评论</h3>
      <form @submit.prevent="submitComment">
        <div class="form-group">
          <label for="rating">评分（1-5）：</label>
          <select id="rating" v-model.number="newComment.rating" required>
            <option value="1">1</option>
            <option value="2">2</option>
            <option value="3">3</option>
            <option value="4">4</option>
            <option value="5">5</option>
          </select>
        </div>
        <div class="form-group">
          <label for="review">评论内容：</label>
          <textarea id="review" v-model="newComment.reviewContent" required></textarea>
        </div>
        <button type="submit">提交评论</button>
      </form>
    </div>

    <div v-if="loading" class="loading">加载中...</div>
    <div v-else-if="error" class="error">{{ error }}</div>
    <div v-else-if="comments.length > 0" class="comments-list">
      <div v-for="comment in comments" :key="comment.CommentID" class="comment-item">
        <div class="comment-header">
          <span class="reader-id">读者ID: {{ comment.ReaderID }}</span>
          <span class="rating">评分: {{ comment.RATING }}/5</span>
        </div>
        <div class="comment-content">{{ comment.ReviewContent }}</div>
        <div class="comment-time">{{ formatDate(comment.CreateTime) }}</div>
      </div>
    </div>
    <div v-else class="no-comments">暂无评论</div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRoute } from 'vue-router'
import { getCommentsByISBN, addComment } from '@/modules/book/api.js'

const route = useRoute()
const searchISBN = ref('')
const comments = ref([])
const loading = ref(false)
const error = ref('')

const newComment = ref({
  readerId: '1', // TODO: 从用户登录信息中获取
  rating: 5,
  reviewContent: ''
})

// 格式化日期函数
function formatDate(dateString) {
  const date = new Date(dateString)
  return date.toLocaleString('zh-CN')
}

// 从 URL 获取 ISBN 并自动搜索
function initISBNFromRoute() {
  if (route.query.isbn) {
    searchISBN.value = route.query.isbn
    searchComments()
  }
}

// 获取评论
async function searchComments() {
  if (!searchISBN.value.trim()) {
    error.value = '请输入ISBN'
    return
  }

  loading.value = true
  error.value = ''

  try {
    const response = await getCommentsByISBN(searchISBN.value)
    comments.value = response.data || []
  } catch (err) {
    error.value = '获取评论失败: ' + (err.response?.data?.message || err.message)
  } finally {
    loading.value = false
  }
}

// 提交新评论
async function submitComment() {
  if (!searchISBN.value.trim()) {
    error.value = '请先搜索图书'
    return
  }

  const commentData = {
    ReaderID: newComment.value.readerId,
    ISBN: searchISBN.value,
    Rating: newComment.value.rating,
    ReviewContent: newComment.value.reviewContent,
    // 注意：后端应该自动设置 CreateTime，不需要从前端传递
    Status: '正常'
  }

  try {
    const response = await addComment(commentData)
    if (response.status === 200) {
      newComment.value.reviewContent = ''
      await searchComments() // 重新加载评论
    }
  } catch (err) {
    error.value = '提交评论失败: ' + (err.response?.data?.message || err.message)
  }
}

// 初始化
initISBNFromRoute()
</script>

<style scoped>
.add-comment {
  margin-top: 30px;
  padding: 20px;
  background-color: #f5f7fa;
  border-radius: 8px;
}

.add-comment h3 {
  margin-bottom: 15px;
}

.form-group {
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
}

.form-group select,
.form-group textarea {
  width: 100%;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.form-group textarea {
  resize: vertical;
  min-height: 100px;
}

button[type='submit'] {
  background-color: #409eff;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  cursor: pointer;
}

button[type='submit']:hover {
  background-color: #337ecc;
}
</style>