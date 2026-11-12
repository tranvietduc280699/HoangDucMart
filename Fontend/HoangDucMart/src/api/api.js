const BASE_URL = import.meta.env.VITE_API_URL ?? '';

async function request(url, options = {}) {
  const token = localStorage.getItem('accessToken');

  const res = await fetch(BASE_URL + url, {
    ...options,
    headers: {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...options.headers,
    },
  });

  // Token hết hạn / không hợp lệ → xóa và về trang đăng nhập
  if (res.status === 401 && window.location.pathname !== '/login') {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('user');
    window.location.href = '/login';
    return;
  }

  let data = null;
  try { data = await res.json(); } catch { }

  if (!res.ok) {
    throw new Error(data?.message || data?.title || 'Lỗi HTTP: ' + res.status);
  }
  return data;
}

export const api = {
  get: (url) => request(url),
  post: (url, body) => request(url, { method: 'POST', body: JSON.stringify(body) }),
  put: (url, body) => request(url, { method: 'PUT', body: JSON.stringify(body) }),
  delete: (url) => request(url, { method: 'DELETE' }),
};