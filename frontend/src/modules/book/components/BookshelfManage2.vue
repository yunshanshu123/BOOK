<template>
    <!-- 在 .shelf-management 顶部添加 -->
  <div class="header-section">
    <h1>书架管理系统</h1>
    <p>管理图书馆书架信息与位置</p>
  </div>

  <div class="shelf-management">
    
    <!-- 替换原来的按钮和筛选部分 -->
    <div class="action-bar">
      <button class="add-btn" @click="handleAdd">+ 新增书架</button>
      
      <div class="filter-container">
         <span class="filter-label">筛选条件</span>
        <select v-model="filterBuildingId" class="filter-select" @change="handleFilter">
          <option value="">全部建筑</option>
          <option v-for="building in buildingOptions" :key="building" :value="building">
            {{ building === 1 ? '总图书馆' : '德文图书馆' }}
          </option>
        </select>
        
        <select v-model="filterFloor" class="filter-select" @change="handleFilter">
          <option value="">全部楼层</option>
          <option v-for="floor in floorOptions" :key="floor" :value="floor">
            {{ floor }}层
          </option>
        </select>
      </div>
    </div>

    <!-- 书架表格 -->
    <div v-if="loading" class="loading">加载中...</div>
    <div v-else class="shelf-table-container">
      <table class="shelf-table">
        <thead>
          <tr>
            <th>书架ID</th>
            <th>楼宇</th>
            <th>书架编码</th>
            <th>楼层</th>
            <th>区域</th>
            <th>操作</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="shelf in shelves" :key="shelf.SHELFID">
            <!-- 书架ID -->
            <td>
              <template v-if="shelf.SHELFID">
                {{ shelf.SHELFID }}
              </template>
              <template v-else>
                无信息
              </template>
            </td>
            
            <!-- 建筑 -->

            <td>
              <template v-if="shelf.BUILDINGID">
                {{ shelf.BUILDINGID === 1 ? '总图书馆' : '德文图书馆' }}
              </template>
              <template v-else>
                无信息
              </template>
            </td>
            
            <!-- 书架编码 -->
            <td>
              <template v-if="shelf.SHELFCODE">
                {{ shelf.SHELFCODE }}
              </template>
              <template v-else>
                无信息
              </template>
            </td>
            
            <!-- 楼层 -->
            <td>
              <template v-if="shelf.FLOOR">
                {{ shelf.FLOOR }}
              </template>
              <template v-else>
                无信息
              </template>
            </td>
            
            <!-- 区域 -->
            <td>
              <template v-if="shelf.ZONE">
                {{ shelf.ZONE }}
              </template>
              <template v-else>
                无信息
              </template>
            </td>
            
            <!-- 操作 -->
            <td>
              
               <button class="delete-btn" @click="handleDelete(shelf.SHELFID)">删除</button>
            </td>
          </tr>
          <tr v-if="shelves.length === 0">
            <td colspan="6" class="no-data">暂无书架数据</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>

        <!-- 新增书架弹窗 (原生实现) -->
    <div v-if="showDialog" class="modal-mask">
      <div class="modal-container">
        <div class="modal-header">
          <h3>新增书架</h3>
          <button class="close-btn" @click="showDialog = false">&times;</button>
        </div>
<div class="modal-body">
  <div class="form-group">
    <label>建筑:</label>
    <select v-model="newShelfForm.BUILDINGID" @change="updateShelfCode; updateOptions()">
      <option value="">请选择建筑</option>
      <option value="1">总图书馆</option>
      <option value="2">德文图书馆</option>
    </select>
  </div>

  <div class="form-group">
    <label>楼层:</label>
    <select v-model="newShelfForm.FLOOR" @change="updateShelfCode">
      <option value="">请选择楼层</option>
      <option v-for="floor in availableFloors" :key="floor" :value="floor">
        {{ floor }}层
      </option>
    </select>
  </div>
  
  <div class="form-group">
    <label>区域:</label>
    <select v-model="newShelfForm.ZONE" @change="updateShelfCode">
      <option value="">请选择区域</option>
      <option v-for="zone in availableZones" :key="zone" :value="zone">
        {{ zone }}区
      </option>
    </select>
  </div>
  
  <div class="form-group">
    <label>序号:</label>
    <select v-model="newShelfForm.SEQUENCE" @change="updateShelfCode">
      <option value="">请选择序号</option>
      <option v-for="seq in availableSequences" :key="seq" :value="seq">
        {{ seq.toString().padStart(3, '0') }}号
      </option>
    </select>
  </div>
  
  <div class="form-group">
    <label>书架编码:</label>
    <input v-model="newShelfForm.SHELFCODE" type="text" placeholder="自动生成" readonly class="shelf-code-display">
  </div>
</div>
        <div class="modal-footer">
          <button class="cancel-btn" @click="showDialog = false">取消</button>
          <button class="confirm-btn" @click="confirmAdd">确认</button>
        </div>
      </div>
    </div>
</template>

<script setup>
import { ref, reactive,onMounted } from 'vue'
import { getShelf,addShelf,deleteShelf,checkShelfHasBooks } from '@/modules/book/api.js'

// 添加筛选相关的响应式变量
const filterBuildingId = ref('')
const filterFloor = ref('')

// 添加建筑和楼层选项
const buildingOptions = [1, 2] // 根据实际情况调整
const floorOptions = Array.from({length: 14}, (_, i) => i + 1) // 1-14层

// 添加筛选处理函数
const handleFilter = () => {
  fetchShelves()
}


const showDialog = ref(false) // 控制弹窗显示
const newShelfForm = reactive({
  BUILDINGID: '',
  SHELFCODE: '',
  FLOOR: '',
  ZONE: ''
})
const shelves = ref([])
const loading = ref(false)


// 修改 fetchShelves 函数，添加筛选参数
const fetchShelves = async () => {
  try {
    loading.value = true
    const response = await getShelf('0')
    let filteredData = response.data || []
    
    // 应用筛选
    if (filterBuildingId.value) {
      filteredData = filteredData.filter(item => item.BUILDINGID == filterBuildingId.value)
    }
    if (filterFloor.value) {
      filteredData = filteredData.filter(item => item.FLOOR == filterFloor.value)
    }
    
    shelves.value = filteredData
  } catch (error) {
    console.error('获取书架列表失败:', error)
    shelves.value = []
  } finally {
    loading.value = false
  }
}
const handleEdit = (shelf) => {
  console.log('编辑书架:', shelf)
  // 这里可以添加编辑逻辑
}

// 添加可用的选项
const availableFloors = ref([])
const availableZones = ref([])
const availableSequences = ref([])

// 根据选择的建筑更新选项
const updateOptions = () => {
  if (newShelfForm.BUILDINGID === '1') {
    // 总图书馆：14层，每层4区(A~D)，每区10个书架
    availableFloors.value = Array.from({length: 14}, (_, i) => i + 1)
    availableZones.value = ['A', 'B', 'C', 'D']
    availableSequences.value = Array.from({length: 10}, (_, i) => i + 1)
  } else if (newShelfForm.BUILDINGID === '2') {
    // 德文图书馆：2层，每层2区(A~B)，每区5个书架
    availableFloors.value = Array.from({length: 2}, (_, i) => i + 1)
    availableZones.value = ['A', 'B']
    availableSequences.value = Array.from({length: 5}, (_, i) => i + 1)
  } else {
    availableFloors.value = []
    availableZones.value = []
    availableSequences.value = []
  }
  
  // 重置其他字段
  newShelfForm.FLOOR = ''
  newShelfForm.ZONE = ''
  newShelfForm.SEQUENCE = ''
  newShelfForm.SHELFCODE = ''
}

// 修改自动生成书架编码的函数
const updateShelfCode = () => {
  if (newShelfForm.FLOOR && newShelfForm.ZONE && newShelfForm.SEQUENCE) {
    const floor = newShelfForm.FLOOR.toString().padStart(2, '0')
    const sequence = newShelfForm.SEQUENCE.toString().padStart(3, '0')
    newShelfForm.SHELFCODE = `${floor}${newShelfForm.ZONE}-${sequence}`
  } else {
    newShelfForm.SHELFCODE = ''
  }
}

// 在 resetEditForm 或类似函数中也要重置这些选项
const resetForm = () => {
  newShelfForm.BUILDINGID = ''
  newShelfForm.FLOOR = ''
  newShelfForm.ZONE = ''
  newShelfForm.SEQUENCE = ''
  newShelfForm.SHELFCODE = ''
  availableFloors.value = []
  availableZones.value = []
  availableSequences.value = []
}
const handleAdd = () => {
  Object.assign(newShelfForm, {
    BUILDINGID: '',
    SHELFCODE: '',
    FLOOR: '',
    ZONE: ''
  })
  showDialog.value = true
}

const confirmAdd = async () => {
  // 简单验证
  if (!newShelfForm.BUILDINGID || !newShelfForm.SHELFCODE || 
      !newShelfForm.FLOOR || !newShelfForm.ZONE) {
    alert('请填写完整信息')
    return
  }
  
  try {
    // 调用新增API
    await addShelf(
      Number(newShelfForm.BUILDINGID),  // 确保转换为number
      newShelfForm.SHELFCODE,
      Number(newShelfForm.FLOOR),      // 确保转换为number
      newShelfForm.ZONE
    )
    alert('新增书架成功')
    showDialog.value = false
    fetchShelves() // 刷新列表
  } catch (error) {
    console.error('新增书架失败:', error)
    alert('新增书架失败: ' + (error.response?.data?.message || error.message))
  }
}

const handleDelete = async (shelfId) => {
  if (!confirm('确定要删除这个书架吗？')) return;
  
  try {
    // 转换为数字类型
    const numericShelfId = Number(shelfId);

    if (isNaN(numericShelfId)) {
      throw new Error('书架ID格式不正确');
    }

    // 先检查书架是否有书
    
    const response = await checkShelfHasBooks(numericShelfId);
    const hasBooks = response.data; // 需要确认数据结构
    if (hasBooks) {
      alert('书架上有图书，请先移除所有图书再删除');
      return;
    }
    // 调用删除API
    await deleteShelf(numericShelfId);
    alert('删除成功');
    fetchShelves(); // 刷新列表
  } catch (error) {
    console.error('删除失败:', error);
    alert('删除失败: ' + (error.response?.data?.message || error.message));
  }
}
onMounted(() => {
  fetchShelves()
})
</script>

<style scoped>
.shelf-management {
  padding: 20px;
}

h2 {
  margin-bottom: 20px;
  color: #333;
}
.add-btn {
  padding: 8px 16px;
  background-color: #67c23a;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.3s;
}
.loading {
  padding: 20px;
  text-align: center;
  color: #666;
}

.shelf-table-container {
  overflow-x: auto;
}

.shelf-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 20px;
}

.shelf-table th,
.shelf-table td {
  padding: 12px 15px;
  border: 1px solid #ddd;
  text-align: left;
}

.shelf-table th {
  background-color: #f5f5f5;
  font-weight: bold;
}

.shelf-table tr:hover {
  background-color: #f9f9f9;
}

.no-data {
  text-align: center;
  color: #999;
  padding: 20px;
}

.edit-btn {
  padding: 6px 12px;
  background-color: #1890ff;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.edit-btn:hover {
  background-color: #40a9ff;
}

/* 新增弹窗样式 */
.modal-mask {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-container {
  background: white;
  border-radius: 8px;
  width: 400px;
  max-width: 90%;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33);
}

.modal-header {
  padding: 15px 20px;
  border-bottom: 1px solid #eee;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.modal-header h3 {
  margin: 0;
}

.close-btn {
  background: none;
  border: none;
  font-size: 20px;
  cursor: pointer;
}

.modal-body {
  padding: 20px;
}

.form-group {
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
}

.form-group input {
  width: 100%;
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.modal-footer {
  padding: 15px 20px;
  border-top: 1px solid #eee;
  text-align: right;
}

.cancel-btn, .confirm-btn {
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
}

.cancel-btn {
  background-color: #f5f5f5;
  margin-right: 10px;
}

.confirm-btn {
  background-color: #1890ff;
  color: white;
  border: none;
}

.delete-btn {
  padding: 6px 12px;
  background-color: #f56c6c;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s;
  margin-left: 8px; /* 与编辑按钮间距 */
}

.delete-btn:hover {
  background-color: #f78989;
}
.action-bar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
  flex-wrap: wrap;
  gap: 15px;
}
.filter-label {
  font-weight: 600;
  color: #475569;
  white-space: nowrap;
  display: flex;
  align-items: center;
  margin-right: 8px;
}
/* 替换现有的筛选样式 */
.filter-container {
  display: flex;
  gap: 15px;
  margin-bottom: 25px;
  background: white;
  padding: 15px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.03);
}

.filter-select {
  padding: 10px 15px;
  border: 1px solid #e2e8f0;
  border-radius: 6px;
  min-width: 140px;
  background-color: #f8fafc;
  transition: all 0.2s;
}

.filter-select:focus {
  outline: none;
  border-color: #4facfe;
  box-shadow: 0 0 0 3px rgba(79, 172, 254, 0.2);
}

.shelf-code-display {
  background-color: #f5f5f5;
  color: #666;
  font-weight: bold;
}

/* 添加这些样式 */
.header-section {
  background: linear-gradient(135deg, #2575fc 0%, #e7e9eeff 100%);
  color: white;
  padding: 20px;
  border-radius: 8px;
  margin-bottom: 25px;
}

.header-section h1 {
  margin: 0 0 8px 0;
  font-size: 28px;
}

.header-section p {
  margin: 0;
  opacity: 0.9;
}

.shelf-management {
  padding: 25px;
  background-color: #f8fafc;
  min-height: 100vh;
}
</style>