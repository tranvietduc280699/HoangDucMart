export const productColumns = [
    { 
        prop: 'productName', 
        label: 'Tên sản phẩm', 
        minWidth: 180, 
        fixed: 'left' 
    },
    { 
        prop: 'sku', 
        label: 'Mã SKU', 
        width: 120 
    },
    { 
        prop: 'categoryName', 
        label: 'Loại sản phẩm', 
        width: 140 
    },
    { 
        prop: 'unit', 
        label: 'Đơn vị tính', 
        width: 100, 
        align: 'center' 
    },
    { 
        prop: 'sellPrice', 
        label: 'Giá bán', 
        width: 130, 
        align: 'right' 
    },
    { 
        prop: 'costPrice', 
        label: 'Giá vốn', 
        width: 130, 
        align: 'right' 
    },
    { 
        prop: 'weightGrams', 
        label: 'Trọng lượng (g)', 
        width: 130, 
        align: 'right' 
    },
    { 
        prop: 'qrCode', 
        label: 'Mã QR', 
        width: 130 
    },
    { 
        prop: 'imageUrl', 
        label: 'Hình ảnh', 
        width: 100, 
        slot: 'image' // Dùng slot nếu bạn muốn hiển thị dạng <el-image>
    },
    { 
        prop: 'isFeatured', 
        label: 'Nổi bật', 
        width: 100, 
        align: 'center',
        slot: 'featured' // Dùng slot để hiển thị icon sao hoặc tag
    },
    { 
        prop: 'isActive', 
        label: 'Trạng thái', 
        width: 120, 
        align: 'center',
        slot: 'status' // Dùng slot để hiển thị el-tag Hoạt động / Ngừng
    },
    { 
        label: 'Thao tác', 
        width: 160, 
        align: 'center', 
        fixed: 'right',
        slot: 'actions' // Cột nút Sửa / Xóa
    }
];