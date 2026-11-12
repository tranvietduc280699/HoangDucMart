<template>
  <div class="category">
    <h2>Quản lý loại sản phẩm</h2>
    <BaseTable :columns="categoryColumns" :data="rows" show-selection>
      <template #status="{ row }">
        <el-tag :type="row.isActive ? 'success' : 'info'">
          {{ row.isActive ? 'Hoạt động' : 'Ngừng' }}
        </el-tag>
      </template>

      <!-- <template #actions="{ row }">
        <el-button size="small" @click="edit(row)">Sửa</el-button>
        <el-button size="small" type="danger" plain @click="remove(row)">Xóa</el-button>
      </template> -->
    </BaseTable>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import BaseTable from '@/components/common/DataTable.vue'
import { categoryColumns } from './category.columns.js'
import { categoryApi } from '@/api/categoryApi/categoryApi.js';

const rows = ref([]);

// Hàm gọi API lấy danh sách loại sản phẩm
const fetchCategories = async () => {
  try {
    const response = await categoryApi.getAllCategories()
    if (response && response.length > 0) {
      // nếu những item không có parentId thì gán giá trị mặc định là '--'
      for (let item of response) {
        if (!item.parentId) {
          item.parentId = '--';
        }
      }
      rows.value = response
        
    }
  } catch (error) {
    console.error('Error fetching categories:', error)
  }
}



onMounted(() => {
  fetchCategories()
})
</script>