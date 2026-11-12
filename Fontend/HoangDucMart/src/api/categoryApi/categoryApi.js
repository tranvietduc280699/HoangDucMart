import {api} from '../api.js';

export const categoryApi = {
    // GET: chỉ URL
    getAll: (q) => api.get('/api/admin/categories' + (q ? '?q=' + encodeURIComponent(q) : '')),

    getAllCategories: () => api.get('/api/Category'),
}