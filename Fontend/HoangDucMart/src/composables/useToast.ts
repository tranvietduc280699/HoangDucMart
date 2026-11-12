import { ref } from 'vue';

const message = ref('');
const type = ref<'success' | 'error' | 'info'>('info');

let timeout: ReturnType<typeof setTimeout> | null = null;

export function useToast() {
  const showToast = (
    text: string,
    toastType: 'success' | 'error' | 'info' = 'success'
  ) => {
    message.value = text;
    type.value = toastType;

    if (timeout) clearTimeout(timeout);

    timeout = setTimeout(() => {
      message.value = '';
    }, 3000);
  };

  return {
    message,
    type,
    showToast
  };
}