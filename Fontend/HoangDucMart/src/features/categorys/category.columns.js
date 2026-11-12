export const categoryColumns = [
  {
    prop: 'categoryName',
    label: 'Tên loại sản phẩm',
    minWidth: 180
  },
  {
    prop: 'description',
    label: 'Mô tả',
    minWidth: 200
  },
  {
    prop: 'parentId',
    label: 'Mã sản phẩm cha',
    minWidth: 160,
    align: 'center'
  },
  // thêm cột mới cho trạng thái
  {
    prop: 'isActive',
    label: 'Trạng thái',
    width: 120,
    slot: 'status',
    align: 'center'
  },
  // thêm chức năng cho các thao tác (sửa, xóa)
  {
    prop: 'actions',
    label: 'Thao tác',
    width: 150,
    slot: 'actions',
    fixed: 'right',
    align: 'center'
  }
]