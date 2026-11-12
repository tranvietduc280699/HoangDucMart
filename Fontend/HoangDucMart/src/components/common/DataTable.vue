<template>
  <div class="dt">
    <el-table :data="pagedData" row-key="id" border stripe>
      <el-table-column type="selection" width="45" />
      <el-table-column
        type="index" label="#" width="56" align="center"
        :index="(i: number) => (page - 1) * pageSize + i + 1"
      />
      <el-table-column prop="id" label="Mã (GUID)" width="170" show-overflow-tooltip />
      <el-table-column prop="name" label="Tên" min-width="160" sortable />
      <el-table-column prop="description" label="Mô tả" min-width="200" />

      <el-table-column label="Trạng thái" width="140" align="center">
        <template #default="scope">
          <el-tag :type="scope.row.isActive ? 'success' : 'info'">
            {{ scope.row.isActive ? 'Hoạt động' : 'Ngừng' }}
          </el-tag>
        </template> 
      </el-table-column>

      <el-table-column label="Thao tác" width="160" align="center">
        <template #default="scope">
          <el-button size="small" @click="$emit('edit', scope.row)">Sửa</el-button>
          <el-button size="small" type="danger" plain @click="$emit('remove', scope.row)">Xóa</el-button>
        </template>
      </el-table-column>
    </el-table>

    <div class="pager">
      <el-pagination
        v-model:current-page="page"
        v-model:page-size="pageSize"
        :total="list.length"
        :page-sizes="[10, 20, 50]"
        layout="total, sizes, prev, pager, next"
        background
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';

const props = defineProps({
  data: { 
    type: Array as () => any[], 
    default: () => [] 
  },
});
defineEmits(['edit', 'remove']);

// dữ liệu mẫu tạm: dùng khi trang cha chưa truyền data
const sample = [
  { id: '3f2b8c1e-5a4d-4e7b-9c21-8d6a1f0b7e45', name: 'Thực phẩm', description: 'Các loại thực phẩm và đồ ăn', isActive: true },
  { id: 'a91d4e07-2c3b-47f8-8a15-5b9e6c2d3f10', name: 'Đồ uống', description: 'Các loại nước uống và đồ giải khát', isActive: false },
];

const list = computed(() => (props.data.length ? props.data : sample));

const page = ref(1);
const pageSize = ref(10);

const pagedData = computed(() => {
  const start = (page.value - 1) * pageSize.value;
  return list.value.slice(start, start + pageSize.value);
});
</script>

<style scoped lang="scss">
.dt {
  background: var(--surface, #fff);
  border: 1px solid var(--border, #dee2e6);
  border-radius: 8px;
  padding: 12px;

  :deep(.el-table) {
    --el-table-header-bg-color: #f1f8f4;
    --el-table-header-text-color: #1b5e20;
    --el-table-row-hover-bg-color: #d1e7dd;
    --el-table-border-color: #e3e8e5;
    --el-table-text-color: #333;
    font-size: 14px;
  }

  :deep(.el-table th.el-table__cell) {
    font-weight: 600;
    height: 44px;
  }

  :deep(.el-table td.el-table__cell) {
    padding: 10px 0;
  }

  :deep(.el-table--striped .el-table__body tr.el-table__row--striped td.el-table__cell) {
    background: #fafcfb;
  }

  .pager {
    display: flex;
    justify-content: flex-end;
    padding-top: 12px;
  }

  :deep(.el-pagination.is-background .el-pager li.is-active) {
    background-color: #198754;
    color: #fff;
  }
}
</style>