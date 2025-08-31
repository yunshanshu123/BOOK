import { defineStore } from 'pinia'
import { ref } from 'vue'
import http from '@/services/http.js' // 确认 http.js 的路径

export const useAnnouncementStore = defineStore('announcements', () => {
  const urgentAnnouncements = ref([])
  const regularAnnouncements = ref([])
  const hasFetched = ref(false)

  async function fetchPublicAnnouncements() {
    if (hasFetched.value) return // 避免重复请求
    try {
      const response = await http.get('/announcements/public')
      urgentAnnouncements.value = response.data.Urgent
      regularAnnouncements.value = response.data.Regular
      hasFetched.value = true
    } catch (error) {
      console.error('Failed to fetch public announcements:', error)
    }
  }

  return { urgentAnnouncements, regularAnnouncements, fetchPublicAnnouncements }
})