/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      // 在这里为管理员界面添加专属颜色
      colors: {
        'admin-bg': '#f0f4f8',        // 管理员界面的主背景色 (淡蓝灰色)
        'admin-header': '#ffffff',     // 头部背景色 (白色)
        'admin-primary': '#3b82f6',    // 主题蓝色 (按钮、高亮)
        'admin-text-primary': '#1e293b', // 主要文字颜色 (深灰色)
        'admin-text-secondary': '#64748b',// 次要文字颜色 (灰色)
        'admin-card': '#ffffff',       // 卡片背景色 (白色)
        'admin-border': '#e2e8f0',      // 边框颜色
      }
    },
  },
  plugins: [],
}