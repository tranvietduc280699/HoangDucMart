<script setup lang="ts">
// sử dụng toast
import Toast from '@/components/common/Toast.vue';
import { useToast } from '@/composables/useToast';

const { message, type } = useToast();

</script>

<template>
  <Toast :message="message" :type="type" />
  <router-view />
</template>

<style scoped>
</style>
