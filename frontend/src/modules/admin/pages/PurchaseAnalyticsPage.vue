<template>
  <div class="flex flex-col h-full gap-8">

    <!-- 上半部分：排名数据展示区 (结构不变) -->
    <div class="flex-none">
      <h2 class="text-2xl font-bold mb-6 text-gray-700">图书数据洞察</h2>
      <div v-if="loading" class="text-center text-gray-500 py-10">
        <p>正在加载排名数据...</p>
      </div>
      <div v-else class="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <RankingCard 
          title="图书种类借阅 Top 10" 
          :items="analysisData.topByBorrowCount" 
          metricLabel="次"
          iconColor="text-sky-500"
        />
        <RankingCard 
          title="借阅总时长 Top 10" 
          :items="analysisData.topByBorrowDuration" 
          metricLabel="天"
          iconColor="text-amber-500"
        />
        <RankingCard 
          title="单本图书借阅 Top 10" 
          :items="analysisData.topByInstanceBorrow" 
          metricLabel="次"
          iconColor="text-violet-500"
        />
      </div>
    </div>

    <!-- 下半部分：采购日志记录区 (结构不变) -->
    <div class="flex-grow grid grid-cols-1 lg:grid-cols-2 gap-8">
      <!-- 左侧：输入框 -->
      <div class="bg-white/80 backdrop-blur-md p-6 rounded-lg shadow-md flex flex-col">
        <h3 class="font-bold text-lg mb-4 text-gray-700 flex-none">采购决策记录</h3>
        <textarea 
          v-model="newLogText"
          class="w-full p-3 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition flex-grow"
          placeholder="例如：采购《三体》20本，ISBN: 9787536692930..."
        ></textarea>
        <button 
          @click="submitLog" 
          :disabled="isSubmitting"
          class="mt-4 w-full bg-blue-600 text-white font-bold py-2 px-4 rounded-md hover:bg-blue-700 transition disabled:bg-gray-400 flex-none"
        >
          {{ isSubmitting ? '提交中...' : '提交记录' }}
        </button>
      </div>

      <!-- 右侧：历史记录 -->
      <div class="bg-white/80 backdrop-blur-md p-6 rounded-lg shadow-md flex flex-col">
        <h3 class="font-bold text-lg mb-4 text-gray-700 flex-none">历史记录</h3>
        <div class="flex-grow overflow-y-auto space-y-4 pr-2 border-t pt-4">
          <div v-if="logs.length === 0" class="flex items-center justify-center h-full text-center text-gray-500">
            <p>暂无记录</p>
          </div>
          <div v-for="log in logs" :key="log.LogID" class="text-sm p-3 bg-gray-50/80 rounded-md border">
            <p class="text-gray-800 whitespace-pre-wrap">{{ log.LogText }}</p>
            <p class="text-right text-xs text-gray-400 mt-2">{{ new Date(log.LogDate).toLocaleString() }}</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'; // 1. 引入 computed
import { getPurchaseAnalysis, getPurchaseLogs, addPurchaseLog } from '../api.js';

// ---- 修改 RankingCard 子组件 ----
const RankingCard = {
  props: ['title', 'items', 'metricLabel', 'iconColor'],
  
  // 2. 添加 setup 函数来创建计算属性
  setup(props) {
    // 这个计算属性是核心：它会生成一个长度永远为 10 的数组
    const displayItems = computed(() => {
      const placeholders = Array(10).fill({ isPlaceholder: true });
      const actualItems = props.items || [];
      // 用真实数据覆盖占位符
      actualItems.forEach((item, index) => {
        if (index < 10) {
          placeholders[index] = { ...item, isPlaceholder: false };
        }
      });
      return placeholders;
    });

    return { displayItems };
  },

  // 3. 修改模板，强制循环 10 次
  template: `
    <div class="bg-white/80 backdrop-blur-md p-5 rounded-lg shadow-md flex flex-col">
      <h3 class="font-bold text-lg mb-4 flex items-center flex-none" :class="iconColor">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-2" viewBox="0 0 20 20" fill="currentColor"><path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-8.707l-3-3a1 1 0 00-1.414 0l-3 3a1 1 0 001.414 1.414L9 9.414V13a1 1 0 102 0V9.414l1.293 1.293a1 1 0 001.414-1.414z" clip-rule="evenodd" /></svg>
        {{ title }}
      </h3>
      <div class="flex-grow">
        <ol class="list-none space-y-2 text-sm text-gray-700">
          <!-- 始终循环 displayItems (长度为10) -->
          <li v-for="(item, index) in displayItems" :key="index" class="truncate flex items-center">
            <span class="font-semibold w-6 text-center">{{ index + 1 }}.</span>
            
            <!-- 如果是真实数据，就正常显示 -->
            <div v-if="!item.isPlaceholder">
              <span class="font-semibold ml-2">{{ item.Title }}</span>
              <span class="text-xs ml-2 text-gray-500">({{ item.MetricValue }} {{ metricLabel }})</span>
            </div>
            
            <!-- 如果是占位符，就显示 '暂无数据' -->
            <div v-else class="ml-2 text-gray-400 italic">
              暂无数据
            </div>
          </li>
        </ol>
      </div>
    </div>
  `
};
// ---- RankingCard 子组件修改结束 ----

const loading = ref(true);
const analysisData = ref({});
const logs = ref([]);
const newLogText = ref('');
const isSubmitting = ref(false);

async function fetchData() {
  try {
    loading.value = true;
    const [analysisRes, logsRes] = await Promise.all([
      getPurchaseAnalysis(),
      getPurchaseLogs()
    ]);
    analysisData.value = analysisRes.data;
    logs.value = logsRes.data;
  } catch (error) {
    console.error("Failed to fetch purchase analysis data:", error);
  } finally {
    loading.value = false;
  }
}

async function submitLog() {
  if (!newLogText.value.trim()) {
    alert('记录内容不能为空！');
    return;
  }
  try {
    isSubmitting.value = true;
    await addPurchaseLog(newLogText.value);
    newLogText.value = '';
    const logsRes = await getPurchaseLogs();
    logs.value = logsRes.data;
  } catch (error) {
    console.error("Failed to submit log:", error);
    alert('提交失败，请稍后再试。');
  } finally {
    isSubmitting.value = false;
  }
}

onMounted(fetchData);
</script>

<style scoped>
/* 样式无需改动 */
</style>