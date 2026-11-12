<template>
  <aside class="sidebar">
    <div class="group" v-for ="(group, index) in adminMenuGroups" :key="index">
      <div  class="heading" v-if="group.title">
        {{ group.title }}
      </div>

      <template v-for="(item, index) in group.items" :key="index">
        <router-link 
          v-if="item.ready" 
          :to="item.to" 
          class="item" 
          exact-active-class="active"
        >
          {{ item.label }}
        </router-link>
        <span v-else class="item disabled">
          {{ item.label }}
        </span>
      </template>
    </div>
  </aside>
</template>
<script setup lang="ts">
import { computed } from 'vue';
import { adminMenu } from '@/constants/admins/adminMenu.js';

// lấy role của user từ localStorage
const roles = computed(() => {
  const u = JSON.parse(localStorage.getItem('user') || 'null');
  return Array.isArray(u?.roles) ? u.roles : [];
});

const adminMenuGroups = computed(() => {
  return adminMenu
    .map((group: any) => ({
      ...group,
      //(ready: true) VÀ USER CÓ QUYỀN (roles khớp)
      items: group.items.filter((item: any) => item.ready && item.roles.some((r: string) => roles.value.includes(r)))
    }))
    .filter((group: any) => group.items.length > 0);
});

</script>
<style scoped lang="scss">
.sidebar {
  width: 160px;
  flex-shrink: 0;
  padding: 16px;
  min-height: calc(100vh - 56px);
  background: var(--surface);         
  border-right: 1px solid var(--border); 
  display: flex;
  flex-direction: column;
  gap: 8px;

  .group {
    display: flex;
    flex-direction: column;
    gap: 8px;
  }

  .heading {
    margin-top: 8px;
    padding-left: 4px;
    font-size: 12px;
    text-transform: uppercase;
    color: var(--muted);               
  }

  .item {
    display: block;
    padding: 8px 12px;
    border: 1px solid var(--border);    
    border-radius: 4px;
    background: var(--surface);         
    color: var(--text);                
    text-decoration: none;
    transition: all 0.2s ease;

    &:hover {
      background: var(--bg);            
      border-color: var(--line);        
    }

    &.active {
      background: var(--brand);          
      border-color: var(--brand);
      color: var(--brand-ink);         
    }

    &.disabled {
      color: var(--muted);
      background: var(--bg);
      border-color: var(--border);
      cursor: not-allowed;

      &:hover {
        background: var(--bg);
        border-color: var(--border);
      }
    }
  }
}
</style>