<template>
  <div class="category-management">
    <div class="page-header">
      <h2>Quản lý Danh mục</h2>
      <button class="btn-primary">＋ Thêm danh mục</button>
    </div>
    <div class="table-responsive">
      <DataTable :data=" []" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import DataTable from '@/components/common/DataTable.vue';

</script>

<style scoped lang="scss">
.category-management {
  padding: 16px;

  .page-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;

    h2 {
      font-size: 20px;
      color: var(--brand-text);
    }
  }
}

/* Bảng tùy chỉnh dùng hệ thống biến màu `:root` */
.table-responsive {
  width: 100%;
  overflow-x: auto;
  -webkit-overflow-scrolling: touch;
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.04);
}

.custom-table {
  width: 100%;
  min-width: 600px;
  border-collapse: collapse;
  text-align: left;

  th, td {
    padding: 12px 16px;
    border-bottom: 1px solid var(--border);
    font-size: 14px;
    color: var(--text);
  }

  th {
    background-color: var(--bg);
    font-weight: 600;
    color: var(--brand-text);
  }

  tr:last-child td {
    border-bottom: none;
  }
}

/* Nút bấm hành động */
.btn-primary {
  background: var(--brand);
  color: var(--brand-ink);
  border: none;
  padding: 8px 16px;
  border-radius: 6px;
  font-weight: 500;
  cursor: pointer;
  transition: background 0.2s;

  &:hover {
    background: var(--brand-hover);
  }
}

.btn-secondary {
  background: var(--bg);
  color: var(--text);
  border: 1px solid var(--border);
  padding: 8px 16px;
  border-radius: 6px;
  cursor: pointer;
}

.btn-sm {
  padding: 4px 10px;
  font-size: 12px;
  border-radius: 4px;
  cursor: pointer;
  border: none;
  margin-right: 6px;
}

.btn-edit {
  background-color: var(--blue);
  color: #fff;
  &:hover { background-color: var(--blue-dark); }
}

.btn-delete {
  background-color: var(--warn);
  color: #fff;
  &:hover { opacity: 0.85; }
}

.text-center { text-align: center; }
.fw-bold { font-weight: 600; }

/* Modal Popup Styles */
.modal-overlay {
  position: fixed;
  top: 0; left: 0; width: 100vw; height: 100vh;
  background: rgba(0, 0, 0, 0.4);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 10000;
}

.modal-content {
  background: var(--surface);
  padding: 24px;
  border-radius: 8px;
  width: 100%;
  max-width: 450px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);

  h3 {
    margin-bottom: 16px;
    color: var(--brand-text);
  }

  .form-group {
    margin-bottom: 14px;

    label {
      display: block;
      margin-bottom: 6px;
      font-size: 13px;
      font-weight: 500;
      color: var(--text);
    }

    input, textarea {
      width: 100%;
      padding: 10px;
      border: 1px solid var(--border);
      border-radius: 6px;
      background: var(--bg);
      color: var(--text);
      font-size: 14px;

      &:focus {
        outline: none;
        border-color: var(--brand);
      }
    }

    textarea {
      resize: vertical;
      height: 80px;
    }
  }

  .modal-actions {
    display: flex;
    justify-content: flex-end;
    gap: 8px;
    margin-top: 20px;
  }
}
</style>