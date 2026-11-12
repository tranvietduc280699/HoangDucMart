<template>
  <div class="product">
    <h2>Quản lý sản phẩm</h2>
    <BaseTable :columns="productColumns" :data="rows" show-selection>
      <template #status="{ row }">
        <el-tag :type="row.isActive ? 'success' : 'info'">
          {{ row.isActive ? 'Hoạt động' : 'Ngừng' }}
        </el-tag>
      </template>

      <template #actions="{ row }">
        <el-button size="small" @click="edit(row)">Sửa</el-button>
        <el-button size="small" type="danger" plain @click="remove(row)">Xóa</el-button>
      </template>
    </BaseTable>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import BaseTable from '@/components/common/DataTable.vue'
import { productColumns } from './product.columns.js'
import { productApi } from '@/api/productApi/productApi.js';

const rows = ref([]);

// Hàm gọi API lấy danh sách sản phẩm
const fetchProducts = async () => {
  try {
    const response = await productApi.getAllProducts()
    if (response && response.length > 0) {
      rows.value = response
    }
  } catch (error) {
    console.error('Error fetching products:', error)
  }
}

onMounted(() => {
  fetchProducts()
})

</script>