<script setup lang="ts">
import AdminHeader from '@/components/admins/AdminHeader.vue';
import AdminSidebar from '@/components/admins/AdminSidebar.vue';
</script>

<template>
  <div class="admin-root">
    <AdminHeader />
    <div class="admin-body">
      <AdminSidebar />
      <main class="admin-content">
        <router-view />
      </main>
    </div>
  </div>
</template>

<style scoped>
.admin-root { 
    min-height: 100vh; 
    background: #f4f6f9; 
}
.admin-body { 
    display: flex; 
}
.admin-content { 
    flex: 1; 
    padding: 24px; 
    min-width: 0; 
}
</style>