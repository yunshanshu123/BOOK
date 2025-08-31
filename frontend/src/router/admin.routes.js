// 导入新的仪表盘页面
import AdminDashboardPage from '@/modules/admin/pages/AdminDashboardPage.vue'
import CategoryManagePage from '@/modules/book/pages/CategoryManagePage.vue'
import PurchaseAnalyticsPage from '@/modules/admin/pages/PurchaseAnalyticsPage.vue'
import ReportHandlingPage from '@/modules/admin/pages/ReportHandlingPage.vue';
import AnnouncementManagePage from '@/modules/admin/pages/AnnouncementManagePage.vue'

export default [
  // 新增：管理员仪表盘/主页，取代旧的 example-route
  {
    // 将路径从 'example-route' 更改为更有意义的路径
    path: '/admin/dashboard',
    name: 'AdminDashboard',
    component: AdminDashboardPage,
    meta: {
      layout: 'Admin', // ★ 指定使用管理员布局
      requiresAuth: true,
      requiresAdmin: true,
      title: '管理员仪表盘'
    }
  },

  // 修改：为分类管理页面也指定管理员布局
  {
    path: '/admin/category',
    name: 'CategoryManage',
    component: CategoryManagePage,
    meta: {
      layout: 'Admin', // ★ 指定使用管理员布局
      requiresAuth: true,
      requiresAdmin: true,
      title: '分类管理'
    }
  },

  {
    path: '/bookshelf',
    name: 'BookshelfManage',
    component: () => import('@/modules/book/pages/BookshelfManagePage.vue'),
    meta: {
      layout: 'Admin', // ★ 指定使用管理员布局
      requiresAuth: true,
      title: '书架管理',
    }
  },

  {
    path: '/admin/analytics',
    name: 'PurchaseAnalytics',
    component: PurchaseAnalyticsPage,
    meta: {
      layout: 'Admin',
      requiresAuth: true,
      requiresAdmin: true,
      title: '采购分析'
    }
  },

  {
    path: '/admin/reports',
    name: 'ReportHandling',
    component: ReportHandlingPage,
    meta: {
      layout: 'Admin',
      requiresAuth: true,
      requiresAdmin: true,
      title: '举报处理'
    }
  },

  {
    path: '/admin/announcements',
    name: 'AnnouncementManage',
    component: AnnouncementManagePage,
    meta: {
      layout: 'Admin',
      requiresAuth: true,
      requiresAdmin: true,
      title: '公告发布'
    }
  }
]