import { createRouter, createWebHistory } from 'vue-router';

const router = createRouter({
  history: createWebHistory(),
  routes: [
    //Trang Đăng nhập
    { 
      path: '/login', 
      component: () => import('@/layouts/auth/AuthLayout.vue'),
      children: [
        { path: '', name: 'login', component: () => import('@/features/auth/views/LoginView.vue') }
      ]
    },

    // Khu vực Quản trị (Admin)
    {
      path: '/admin',
      component: () => import('@/layouts/admins/AdminLayout.vue'),
      meta: { requiresAuth: true },
      children: [
        { path: '', name: 'dashboard', component: () => import('@/features/dashboard/DashboardView.vue') },
        { path: 'category', name: 'category', component: () => import('@/features/products/Category.vue') }, 
        { path: 'product', name: 'product', component: () => import('@/features/products/Product.vue') }     
      ]
    },
    { 
      path: '/', 
      redirect: '/admin' 
    },
  ],
});

// Bộ kiểm tra quyền trước khi chuyển trang (Navigation Guard)
router.beforeEach((to) => {
  const isAuthenticated = !!localStorage.getItem('accessToken'); 

  // Nếu route yêu cầu đăng nhập (có meta.requiresAuth) mà chưa đăng nhập -> đá về /login
  if (to.meta.requiresAuth && !isAuthenticated) {
    return { name: 'login' }; 
  }

  // Nếu đã đăng nhập rồi mà cố tình vào trang /login -> đá thẳng vào /admin
  if (to.path === '/login' && isAuthenticated) {
    return { path: '/admin' };
  }
});

export default router;