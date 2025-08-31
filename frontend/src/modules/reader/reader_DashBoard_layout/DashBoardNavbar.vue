<template>
  <div class="navbar">
    <div class="title">图书馆个人主页</div>
    <div class="user" @click="toggleDropdown">
      <img :src="avatar" class="avatar" />
      <span>您好，{{ nickname }}</span>
      <span class="arrow">▼</span>

      <div v-if="showDropdown" class="dropdown">
        <div class="dropdown-item" @click="toggleAvatarCard">👤 修改头像</div>
        <div class="dropdown-item" @click="toggleProfileCard">✏️ 完善资料</div>
        <div class="dropdown-item" @click="handleLogout">⏻ 退出登录</div>
      </div>
    </div>
  </div>

  <!-- 修改头像弹窗 -->
  <div v-if="avatarCard" class="modal-overlay" @click.self="toggleAvatarCard">
    <div class="modal-content">
      <ModifyAvatarCard @close="toggleAvatarCard" />
    </div>
  </div>

  <!-- 修改资料弹窗 -->
  <ProfileEditCard
    :is-visible="profileCard"
    :initial-data="initialProfileData"
    @close="toggleProfileCard"
    @save="handleSaveProfile"
  />
</template>

<script setup>
import { computed, ref } from 'vue'
import { logout, updateMyProfile } from '@/modules/reader/api.js'
import { useRouter } from 'vue-router'
import ModifyAvatarCard from '@/modules/reader/components/ModifyAvatarCard.vue'
import ProfileEditCard from '@/modules/reader/components/ProfileEditCard.vue'
import { useUserStore } from '@/stores/user.js'

const userStore = useUserStore()
const router = useRouter()

const baseAvatarUrl = 'http://localhost:5000/avatars/'

const nickname = computed(() => userStore.nickName)
const avatar = computed(() => baseAvatarUrl + userStore.avatar)

const showDropdown = ref(false)
const avatarCard = ref(false)
const profileCard = ref(false)

const toggleDropdown = () => {
  showDropdown.value = !showDropdown.value
}

const toggleAvatarCard = () => {
  avatarCard.value = !avatarCard.value
}

const toggleProfileCard = () => {
  profileCard.value = !profileCard.value
}

// 资料编辑初始值
const initialProfileData = computed(() => ({
  userName: userStore.userName,
  fullName: userStore.fullName,
  nickName: userStore.nickName,
  //password: ''
}))

// 保存资料信息
const handleSaveProfile = async (formData) => {

  const user= userStore.user

  // 示例更新 store（根据你实际的字段来）：
  if(formData.userName) {
    user.userName = formData.userName
  }
  if(formData.fullName) {
    user.fullName = formData.fullName
  }
  if(formData.nickName) {
    user.nickName = formData.nickName
  }
  const saveProfileRes = await updateMyProfile(user)

  userStore.setUser(user)

  alert(saveProfileRes.data)
  // 关闭弹窗
  toggleProfileCard()
}

// 退出登录
const handleLogout = async () => {
  const confirmed = window.confirm("确定要退出登录吗？")
  if (!confirmed) return

  try {
    await logout()
    localStorage.removeItem('token')
    localStorage.removeItem('user')
    await router.push('/')
  } catch (err) {
    alert("退出失败，请稍后再试")
    console.error(err)
  }
}
</script>

<style scoped>
.navbar {
  background: white;
  padding: 16px 24px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid #ddd;
  position: relative;
}
.title {
  color: black;
  font-size: 20px;
  font-weight: bold;
}
.user {
  display: flex;
  align-items: center;
  font-size: 14px;
  color: #666;
  cursor: pointer;
  position: relative;
}
.avatar {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  margin-right: 8px;
  object-fit: cover;
}
.arrow {
  margin-left: 4px;
  font-size: 12px;
}
.dropdown {
  position: absolute;
  top: 48px;
  right: 0;
  width: 120px;
  background: white;
  border: 1px solid #ddd;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
  border-radius: 4px;
  z-index: 10;
}
.dropdown-item {
  padding: 10px 16px;
  font-size: 14px;
  color: #333;
  cursor: pointer;
}
.dropdown-item:hover {
  background-color: #f5f5f5;
}
.logout {
  border-top: 1px solid #eee;
}
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background-color: rgba(0, 0, 0, 0.4);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
}
.modal-content {
  background: white;
  border-radius: 12px;
  padding: 24px;
  box-shadow: 0 4px 12px rgba(0,0,0,0.2);
  max-width: 400px;
  width: 90%;
  position: relative;
}
</style>
