import { BorrowingTest } from '@/modules/reader';

export default [
  //导出我的图书馆页面路由
  {
    path: '/my/home/dashboard',
    name: 'my-home-dashboard',
    component: () => import('@/modules/reader/pages/DashBoardHome_Page.vue'),
    meta: { requiresAuth: true }
  },
  // 其他reader模块路由...
  {
    path: '/reader/borrowing',
    name: 'BorrowingTest',
    component: BorrowingTest,
    meta: {
      title: '借阅记录管理',
      //requiresAuth: true // 如果需要登录验证
    }
  }
];
