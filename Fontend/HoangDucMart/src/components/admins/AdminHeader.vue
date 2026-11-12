<template>
  <header class="header">
    <div class = "logoCampany">
      <div class="logo"> </div>
      <router-link to="/admin" class="brand">Hoang Duc Mart</router-link>
    </div>

    <div class="right">
      <div class="who" v-if="user">
        <span class="name">{{ user.fullName || user.userName }}</span>
        <span class="role">{{ user.roles?.join(', ') }}</span>
      </div>
      <button class="logout" @click="logout">Đăng xuất</button>
    </div>
  </header>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();

// Lấy dữ liệu user từ localStorage
const user = computed(() => JSON.parse(localStorage.getItem('user') || 'null'));

// Đăng xuất
function logout() {
  localStorage.removeItem('accessToken');
  localStorage.removeItem('user');
  router.push('/login');
}
</script>

<style scoped lang="scss">
.header {
  height: 56px;
  padding: 0 20px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  background: var(--brand-ink);
  border-bottom: 1px solid var(--border);
  .logoCampany{
    display: flex;
    align-items: center;

    .logo {
        width: 40px;
        height: 40px;
        background-image: var(--icon-logo);
        background-repeat: no-repeat;
        background-position: center;
        background-size: contain;
    }
  }

  .brand {
    font-weight: 700;
    font-size: 18px;
    color: var(--brand-text);
    text-decoration: none;
  }

  .right {
    display: flex;
    align-items: center;
    gap: 16px;

    .who {
      display: flex;
      flex-direction: column;
      text-align: right;
      line-height: 1.2;

      .name {
        font-weight: 600;
        font-size: 14px;
      }

      .role {
        font-size: 12px;
        color: var(--muted);
      }
    }

    .logout {
        padding: 6px 14px;
        border: 1px solid var(--brand); 
        border-radius: 4px;
        background: var(--brand);       
        color: var(--brand-ink);        
        font-weight: 500;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover {
            background: var(--brand-hover);          
            border-color: var(--brand-hover);
            color: var(--brand-ink);  
        }
    }
  }
}
</style>