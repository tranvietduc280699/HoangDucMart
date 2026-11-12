import {api} from '../api.js';

export const productApi = {
    // GET: chỉ URL
    getAll: (q) => api.get('/api/admin/products' + (q ? '?q=' + encodeURIComponent(q) : '')),

    getAllProducts: () => api.get('/api/Product'),
}