<template>
  <div class="base-table">
    <el-table
      :data="data"
      :row-key="rowKey"
      :border="border"
      :stripe="stripe"
      :height="height"
      :max-height="maxHeight"
      v-loading="loading"
      :empty-text="emptyText"
    >
      <!-- Checkbox chọn dòng -->
      <el-table-column
        v-if="showSelection"
        type="selection"
        width="50"
        align="center"
      />

      <!-- Số thứ tự -->
      <el-table-column
        v-if="showIndex"
        type="index"
        label="STT"
        width="65"
        align="center"
        fixed="left"
        :index="getIndex"
      />

      <!-- Các cột được khai báo từ component cha -->
      <el-table-column
        v-for="(column, index) in columns"
        :key="column.prop || index"
        :prop="column.prop"
        :label="column.label"
        :width="column.width"
        :min-width="column.minWidth"
        :fixed="column.fixed"
        :align="column.align || 'left'"
        :header-align="column.headerAlign || column.align || 'left'"
        :sortable="column.sortable || false"
        :show-overflow-tooltip="column.showOverflowTooltip ?? true"
      >
        <!-- Nếu có slot cho cột này, hiển thị slot -->
         <template #default="scope">
          <!-- cột có slot: cha truyền <template #status="{ row }"> -->
          <slot v-if="column.slot" :name="column.slot" :row="scope.row" :index="scope.$index" />
          
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script setup>

// Nhận dữ liệu từ component cha
const props = defineProps({

  // Danh sách dữ liệu
  data: {
    type: Array,
    default: () => []
  },

  // Cấu hình các cột
  columns: {
    type: Array,
    default: () => []
  },

  // Trạng thái loading
  loading: {
    type: Boolean,
    default: false
  },

  // Tên trường khóa chính
  rowKey: {
    type: String,
    default: 'id'
  },

  // Hiển thị đường viền
  border: {
    type: Boolean,
    default: true
  },

  // Hiển thị màu xen kẽ
  stripe: {
    type: Boolean,
    default: true
  },

  // Chiều cao bảng
  height: {
    type: [String, Number],
    default: undefined
  },

  maxHeight: {
    type: [String, Number],
    default: undefined
  },

  // Nội dung khi không có dữ liệu
  emptyText: {
    type: String,
    default: 'Không có dữ liệu'
  },

  // Hiển thị STT
  showIndex: {
    type: Boolean,
    default: true
  },

  // Hiển thị checkbox
  showSelection: {
    type: Boolean,
    default: false
  }

})

console.log('props', props)

// Tính số thứ tự
const getIndex = (index) => {
  return index + 1
}

</script>

<style scoped lang="scss">
.base-table {
  width: 100%;
}
</style>
