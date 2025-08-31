import http from '@/services/http.js'

// 获取所有采购分析排名数据
export function getPurchaseAnalysis() {
  return http.get('/admin/purchase-analysis')
}

// 获取采购日志列表
export function getPurchaseLogs() {
  return http.get('/admin/purchase-analysis/logs')
}

// 添加一条新的采购日志
export function addPurchaseLog(logText) {
  return http.post('/admin/purchase-analysis/logs', { logText })
}

// 获取所有待处理的举报
export function getPendingReports() {
  return http.get('/admin/reports/pending');
}

export function handleReport(reportId, action, commentId) {
  // action can be 'approve' or 'reject'
  return http.put(`/admin/reports/${reportId}/handle`, { action, commentId }) // 在请求体中加入 commentId
}

// 获取所有公告（管理用）
export function getAllAnnouncements() {
  return http.get('/admin/announcements')
}

// 创建新公告
export function createAnnouncement(data) {
  return http.post('/admin/announcements', data)
}

// 更新公告
export function updateAnnouncement(id, data) {
  return http.put(`/admin/announcements/${id}`, data)
}

// 下架公告
export function takedownAnnouncement(id) {
  return http.put(`/admin/announcements/${id}/takedown`)
}