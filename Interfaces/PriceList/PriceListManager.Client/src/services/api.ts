import axios from 'axios'

const api = axios.create({
  baseURL: '/api',
  headers: {
    'Content-Type': 'application/json'
  }
})

api.interceptors.request.use(config => {
  const token = localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

api.interceptors.response.use(
  response => response,
  error => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token')
      localStorage.removeItem('user')
      window.location.href = '/login'
    }
    return Promise.reject(error)
  }
)

// Auth
export const authApi = {
  login: (login: string, password: string) =>
    api.post('/auth/login', { login, password }),
  logout: () => api.post('/auth/logout')
}

// Categories
export const categoriesApi = {
  getAll: () => api.get('/categories'),
  getById: (id: number) => api.get(`/categories/${id}`),
  create: (data: { name: string; description?: string }) =>
    api.post('/categories', data),
  update: (id: number, data: { name: string; description?: string }) =>
    api.put(`/categories/${id}`, data),
  delete: (id: number) => api.delete(`/categories/${id}`)
}

// Products
export const productsApi = {
  getAll: (categoryId?: number, search?: string) =>
    api.get('/products', { params: { categoryId, search } }),
  getById: (id: number) => api.get(`/products/${id}`),
  search: (params: {
    name?: string
    categoryId?: number
    article?: string
    minPrice?: number
    maxPrice?: number
    inStock?: boolean
  }) => api.get('/products/search', { params }),
  create: (data: {
    categoryId: number
    name: string
    article?: string
    description?: string
    price: number
    quantity: number
  }) => api.post('/products', data),
  update: (id: number, data: {
    name: string
    article?: string
    description?: string
    price: number
    quantity: number
    categoryId: number
  }) => api.put(`/products/${id}`, data),
  delete: (id: number) => api.delete(`/products/${id}`),
  move: (id: number, targetCategoryId: number) =>
    api.post(`/products/${id}/move`, { targetCategoryId }),
  moveBulk: (productIds: number[], targetCategoryId: number) =>
    api.post('/products/move-bulk', { productIds, targetCategoryId }),
  getMovements: (id: number) => api.get(`/products/${id}/movements`)
}

// Users
export const usersApi = {
  getAll: () => api.get('/users'),
  getById: (id: number) => api.get(`/users/${id}`),
  search: (query: string) => api.get('/users/search', { params: { query } }),
  createAdmin: (data: { email: string; login?: string; role: string }) =>
    api.post('/users/admin', data),
  updateRole: (id: number, role: string) =>
    api.put(`/users/${id}/role`, { role })
}

export default api
