<template>
  <h3 class="title">Tổng quan</h3>

  <div class="cards">
    <div v-for="s in stats" :key="s.label" class="card">
      <div class="label">{{ s.label }}</div>
      <div class="value">{{ s.value }}</div>
    </div>
  </div>
</template>

<script setup lang="ts">
const stats = [
  { label: 'Sản phẩm', value: 0 },
  { label: 'Bài viết', value: 0 },
  { label: 'Liên hệ mới', value: 0 },
  { label: 'Người dùng', value: 0 },
];
</script>

<style scoped lang="scss">
.title {
  margin: 0 0 20px;
}

.cards {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 16px;

  .card {
    background: #fff;
    border: 1px solid #dee2e6;
    border-radius: 6px;
    padding: 16px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.02); 

    .label {
      color: #6c757d;
      font-size: 14px;
    }

    .value {
      font-size: 28px;
      font-weight: 700;
      color: #212529;
      margin-top: 4px;
    }
  }
}
</style>