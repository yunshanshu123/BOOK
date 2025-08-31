<!-- frontend/src/modules/book/pages/BookCommentsPage.vue -->
<template>
  <LayoutDefault>
    <div class="book-comments-page">
      <BookComments />
    </div>
  </LayoutDefault>
</template>

<script setup>
import LayoutDefault from '@/shared/components/layouts/LayoutDefault.vue'
import BookComments from '@/modules/book/components/BookComments.vue'
</script>

<style scoped>
.book-comments-page {
  min-height: calc(100vh - 120px);
  background-color: #fff;
}
</style>