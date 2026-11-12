<template>
  <Transition name="toast-fade">
    <div class="toast-banner" :class="type" v-if="message">
      <span>{{ message }}</span>
    </div>
  </Transition>
</template>

<script setup lang="ts">
import { defineProps, type PropType } from 'vue';
// nhận dữ liệu từ component cha thông qua props
const props = defineProps({
  message: {
    type: String,
    required: true
  },
  type: {
    type: String as PropType<'success' | 'error' | 'info'>,
    default: 'success'
  }
});

</script>

<style scoped lang="scss">
.toast-banner {
  position: fixed;
  top: 20px;
  left: 50%;
  transform: translateX(-50%);
  z-index: 99999;
  padding: 12px 20px;
  border-radius: 6px;
  color: #fff;
  font-weight: 500;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  transition: all 0.3s ease;

  &.success { background-color: #198754; } 
  &.error { background-color: #dc3545; }  
  &.info { background-color: #0dcaf0; color: #000; } 
}

.toast-fade-enter-active, .toast-fade-leave-active {
  transition: opacity 0.3s, transform 0.3s;
}
.toast-fade-enter-from, .toast-fade-leave-to {
  opacity: 0;
  transform: translateY(-10px);
}
</style>