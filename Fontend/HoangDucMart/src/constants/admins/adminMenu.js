// ready: true  = đã làm, có thể vào được
// ready: false = chưa làm, tạm thời khóa
// roles: vai trò được thấy mục này (khớp URA_ROLES.ROLE_NAME)
export const adminMenu = [
  { title: null, items: [
    { label: 'Tổng quan', to: '/admin', ready: true, roles: ['Admin', 'Manager', 'Staff'] },
  ]},

  { title: 'Hàng hóa', items: [
    { label: 'Loại sản phẩm', to: '/admin/category', ready: true, roles: ['Admin', 'Manager'] },
    { label: 'Sản phẩm',      to: '/admin/product',  ready: true, roles: ['Admin', 'Manager', 'Staff'] },
  ]},

  { title: 'Bán hàng', items: [
    { label: 'Đơn hàng',  to: '/admin/order', ready: true , roles: ['Admin', 'Manager', 'Staff'] },
    { label: 'Giỏ hàng',  to: '/admin/cart',  ready: true, roles: ['Admin', 'Manager'] },
  ]},

  { title: 'Tài chính', items: [
    { label: 'Công nợ', to: '/admin/debt', ready: true, roles: ['Admin', 'Manager'] },
  ]},

  { title: 'Hệ thống', items: [
    { label: 'Người dùng', to: '/admin/user', ready: true, roles: ['Admin'] },
    { label: 'Vai trò',    to: '/admin/role', ready: true, roles: ['Admin'] },
  ]},
];