import { api } from '../api.js';

export const userApi = {
    // GET: chỉ URL
    getAll: (q) => api.get('/api/admin/users' + (q ? '?q=' + encodeURIComponent(q) : '')),

    // POST: URL + body JSON
    login: (data) => api.post('/api/User/Login', data)
};