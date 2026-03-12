import { useState, useEffect } from 'react'
import { useAuth, useToast } from '../App'
import { productsApi, categoriesApi } from '../services/api'
import Modal from '../components/Modal'

interface Product {
  id: number
  article: string | null
  name: string
  description: string | null
  price: number
  quantity: number
  categoryId: number
  categoryName: string
}

interface Category {
  id: number
  name: string
}

function Products() {
  const { isAdmin } = useAuth()
  const { showToast } = useToast()
  const [products, setProducts] = useState<Product[]>([])
  const [categories, setCategories] = useState<Category[]>([])
  const [loading, setLoading] = useState(true)
  const [search, setSearch] = useState('')
  const [selectedCategory, setSelectedCategory] = useState<number | ''>('')
  const [selectedProducts, setSelectedProducts] = useState<number[]>([])

  // Modal states
  const [showAddProduct, setShowAddProduct] = useState(false)
  const [showMoveProduct, setShowMoveProduct] = useState(false)
  const [showAdvancedSearch, setShowAdvancedSearch] = useState(false)
  const [editingProduct, setEditingProduct] = useState<Product | null>(null)
  const [movingProduct, setMovingProduct] = useState<Product | null>(null)

  // Form states
  const [formData, setFormData] = useState({
    categoryId: 0,
    name: '',
    article: '',
    description: '',
    price: '',
    quantity: ''
  })
  const [moveCategoryId, setMoveCategoryId] = useState<number>(0)

  // Search filters
  const [filters, setFilters] = useState({
    name: '',
    categoryId: '' as number | '',
    article: '',
    minPrice: '',
    maxPrice: '',
    inStock: '' as 'all' | 'true' | 'false'
  })

  useEffect(() => {
    loadData()
  }, [])

  const loadData = async () => {
    try {
      const [productsRes, categoriesRes] = await Promise.all([
        productsApi.getAll(),
        categoriesApi.getAll()
      ])
      setProducts(productsRes.data)
      setCategories(categoriesRes.data)
    } catch (error) {
      showToast('Ошибка загрузки данных', 'error')
    } finally {
      setLoading(false)
    }
  }

  const handleSearch = async () => {
    setLoading(true)
    try {
      const params: any = {}
      if (search) params.search = search
      if (selectedCategory) params.categoryId = selectedCategory

      const response = await productsApi.getAll(params.categoryId, params.search)
      setProducts(response.data)
    } catch (error) {
      showToast('Ошибка поиска', 'error')
    } finally {
      setLoading(false)
    }
  }

  const handleAdvancedSearch = async () => {
    setLoading(true)
    try {
      const params: any = {}
      if (filters.name) params.name = filters.name
      if (filters.categoryId) params.categoryId = filters.categoryId
      if (filters.article) params.article = filters.article
      if (filters.minPrice) params.minPrice = filters.minPrice
      if (filters.maxPrice) params.maxPrice = filters.maxPrice
      if (filters.inStock === 'true') params.inStock = true
      else if (filters.inStock === 'false') params.inStock = false

      const response = await productsApi.search(params)
      setProducts(response.data)
      setShowAdvancedSearch(false)
    } catch (error) {
      showToast('Ошибка поиска', 'error')
    } finally {
      setLoading(false)
    }
  }

  const resetFilters = () => {
    setFilters({
      name: '',
      categoryId: '',
      article: '',
      minPrice: '',
      maxPrice: '',
      inStock: 'all'
    })
    loadData()
  }

  const handleAddProduct = async () => {
    if (!formData.name || !formData.categoryId || !formData.price || formData.quantity === '') {
      showToast('Заполните обязательные поля', 'error')
      return
    }

    try {
      await productsApi.create({
        categoryId: formData.categoryId,
        name: formData.name,
        article: formData.article || undefined,
        description: formData.description || undefined,
        price: parseFloat(formData.price),
        quantity: parseInt(formData.quantity)
      })
      showToast('Товар добавлен', 'success')
      setShowAddProduct(false)
      resetForm()
      loadData()
    } catch (error: any) {
      showToast(error.response?.data?.message || 'Ошибка добавления товара', 'error')
    }
  }

  const handleUpdateProduct = async () => {
    if (!editingProduct) return

    try {
      await productsApi.update(editingProduct.id, {
        name: formData.name,
        article: formData.article || undefined,
        description: formData.description || undefined,
        price: parseFloat(formData.price),
        quantity: parseInt(formData.quantity),
        categoryId: formData.categoryId
      })
      showToast('Товар обновлён', 'success')
      setEditingProduct(null)
      resetForm()
      loadData()
    } catch (error: any) {
      showToast(error.response?.data?.message || 'Ошибка обновления товара', 'error')
    }
  }

  const handleDeleteProduct = async (id: number) => {
    if (!confirm('Удалить товар?')) return

    try {
      await productsApi.delete(id)
      showToast('Товар удалён', 'success')
      loadData()
    } catch (error) {
      showToast('Ошибка удаления товара', 'error')
    }
  }

  const handleMoveProduct = async () => {
    if (!movingProduct || !moveCategoryId) return

    try {
      await productsApi.move(movingProduct.id, moveCategoryId)
      showToast('Товар перемещён', 'success')
      setShowMoveProduct(false)
      setMovingProduct(null)
      setMoveCategoryId(0)
      loadData()
    } catch (error: any) {
      showToast(error.response?.data?.message || 'Ошибка перемещения товара', 'error')
    }
  }

  const handleMoveBulk = async () => {
    if (!selectedProducts.length || !moveCategoryId) return

    try {
      const response = await productsApi.moveBulk(selectedProducts, moveCategoryId)
      showToast(`Перемещено товаров: ${response.data.movedCount}`, 'success')
      setSelectedProducts([])
      setShowMoveProduct(false)
      setMoveCategoryId(0)
      loadData()
    } catch (error) {
      showToast('Ошибка перемещения товаров', 'error')
    }
  }

  const resetForm = () => {
    setFormData({
      categoryId: 0,
      name: '',
      article: '',
      description: '',
      price: '',
      quantity: ''
    })
  }

  const openEditModal = (product: Product) => {
    setEditingProduct(product)
    setFormData({
      categoryId: product.categoryId,
      name: product.name,
      article: product.article || '',
      description: product.description || '',
      price: product.price.toString(),
      quantity: product.quantity.toString()
    })
  }

  const openMoveModal = (product: Product) => {
    setMovingProduct(product)
    setMoveCategoryId(categories.find(c => c.id !== product.categoryId)?.id || 0)
  }

  const toggleProductSelection = (id: number) => {
    setSelectedProducts(prev =>
      prev.includes(id) ? prev.filter(p => p !== id) : [...prev, id]
    )
  }

  const toggleSelectAll = () => {
    if (selectedProducts.length === products.length) {
      setSelectedProducts([])
    } else {
      setSelectedProducts(products.map(p => p.id))
    }
  }

  const activeFiltersCount = Object.values(filters).filter(
    v => v !== '' && v !== 'all'
  ).length

  return (
    <div className="container mx-auto px-4 py-6">
      {/* Breadcrumbs */}
      <div className="mb-4 text-sm text-gray-600">
        <span>Главная</span>
        <span className="mx-2">/</span>
        <span className="text-gray-800">Товары</span>
      </div>

      {/* Search Bar */}
      <div className="card mb-6">
        <div className="flex flex-wrap gap-4 items-end">
          <div className="flex-1 min-w-[200px]">
            <input
              type="text"
              value={search}
              onChange={e => setSearch(e.target.value)}
              placeholder="Поиск по названию или артикулу..."
              className="input-field"
              onKeyDown={e => e.key === 'Enter' && handleSearch()}
            />
          </div>
          <select
            value={selectedCategory}
            onChange={e => setSelectedCategory(e.target.value ? parseInt(e.target.value) : '')}
            className="input-field w-auto"
          >
            <option value="">Все категории</option>
            {categories.map(cat => (
              <option key={cat.id} value={cat.id}>{cat.name}</option>
            ))}
          </select>
          <button onClick={handleSearch} className="btn-primary">
            Найти
          </button>
          <button
            onClick={() => setShowAdvancedSearch(true)}
            className={`btn-secondary ${activeFiltersCount > 0 ? 'bg-blue-500' : ''}`}
          >
            Расширенный поиск {activeFiltersCount > 0 && `(${activeFiltersCount})`}
          </button>
          {isAdmin && (
            <button onClick={() => setShowAddProduct(true)} className="btn-primary">
              + Добавить товар
            </button>
          )}
        </div>
      </div>

      {/* Products Table */}
      <div className="card overflow-hidden">
        <table className="w-full">
          <thead className="bg-gray-50">
            <tr>
              {isAdmin && (
                <th className="px-4 py-3 text-left">
                  <input
                    type="checkbox"
                    checked={selectedProducts.length === products.length && products.length > 0}
                    onChange={toggleSelectAll}
                    className="w-4 h-4"
                  />
                </th>
              )}
              <th className="px-4 py-3 text-left text-sm font-medium text-gray-600">Артикул</th>
              <th className="px-4 py-3 text-left text-sm font-medium text-gray-600">Название</th>
              <th className="px-4 py-3 text-left text-sm font-medium text-gray-600">Категория</th>
              <th className="px-4 py-3 text-right text-sm font-medium text-gray-600">Цена</th>
              <th className="px-4 py-3 text-right text-sm font-medium text-gray-600">Количество</th>
              {isAdmin && (
                <th className="px-4 py-3 text-center text-sm font-medium text-gray-600">Действия</th>
              )}
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-200">
            {loading ? (
              <tr>
                <td colSpan={isAdmin ? 7 : 5} className="px-4 py-8 text-center text-gray-500">
                  Загрузка...
                </td>
              </tr>
            ) : products.length === 0 ? (
              <tr>
                <td colSpan={isAdmin ? 7 : 5} className="px-4 py-8 text-center text-gray-500">
                  Товары не найдены
                </td>
              </tr>
            ) : (
              products.map(product => (
                <tr key={product.id} className="hover:bg-gray-50">
                  {isAdmin && (
                    <td className="px-4 py-3">
                      <input
                        type="checkbox"
                        checked={selectedProducts.includes(product.id)}
                        onChange={() => toggleProductSelection(product.id)}
                        className="w-4 h-4"
                      />
                    </td>
                  )}
                  <td className="px-4 py-3 text-sm">{product.article || '-'}</td>
                  <td className="px-4 py-3 font-medium">{product.name}</td>
                  <td className="px-4 py-3 text-sm">{product.categoryName}</td>
                  <td className="px-4 py-3 text-sm text-right">{product.price.toFixed(2)} ₽</td>
                  <td className="px-4 py-3 text-sm text-right">
                    <span className={product.quantity === 0 ? 'text-red-500' : ''}>
                      {product.quantity}
                    </span>
                  </td>
                  {isAdmin && (
                    <td className="px-4 py-3">
                      <div className="flex justify-center gap-2">
                        <button
                          onClick={() => openEditModal(product)}
                          className="text-blue-500 hover:text-blue-700 text-sm"
                        >
                          Редактировать
                        </button>
                        <button
                          onClick={() => openMoveModal(product)}
                          className="text-green-500 hover:text-green-700 text-sm"
                        >
                          Переместить
                        </button>
                        <button
                          onClick={() => handleDeleteProduct(product.id)}
                          className="text-red-500 hover:text-red-700 text-sm"
                        >
                          Удалить
                        </button>
                      </div>
                    </td>
                  )}
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {/* Bulk Move Button */}
      {isAdmin && selectedProducts.length > 0 && (
        <div className="fixed bottom-6 right-6">
          <button
            onClick={() => {
              setMovingProduct(null)
              setMoveCategoryId(categories[0]?.id || 0)
              setShowMoveProduct(true)
            }}
            className="btn-primary shadow-lg"
          >
            Переместить выбранные ({selectedProducts.length})
          </button>
        </div>
      )}

      {/* Add/Edit Product Modal */}
      <Modal
        isOpen={showAddProduct || !!editingProduct}
        onClose={() => {
          setShowAddProduct(false)
          setEditingProduct(null)
          resetForm()
        }}
        title={editingProduct ? 'Редактирование товара' : 'Добавление товара'}
      >
        <div className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Категория *
            </label>
            <select
              value={formData.categoryId}
              onChange={e => setFormData({ ...formData, categoryId: parseInt(e.target.value) })}
              className="input-field"
            >
              <option value={0}>Выберите категорию</option>
              {categories.map(cat => (
                <option key={cat.id} value={cat.id}>{cat.name}</option>
              ))}
            </select>
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Название товара *
            </label>
            <input
              type="text"
              value={formData.name}
              onChange={e => setFormData({ ...formData, name: e.target.value })}
              className="input-field"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Артикул
            </label>
            <input
              type="text"
              value={formData.article}
              onChange={e => setFormData({ ...formData, article: e.target.value })}
              className="input-field"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Описание
            </label>
            <textarea
              value={formData.description}
              onChange={e => setFormData({ ...formData, description: e.target.value })}
              className="input-field"
              rows={3}
            />
          </div>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Цена *
              </label>
              <input
                type="number"
                step="0.01"
                min="0"
                value={formData.price}
                onChange={e => setFormData({ ...formData, price: e.target.value })}
                className="input-field"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Количество *
              </label>
              <input
                type="number"
                min="0"
                value={formData.quantity}
                onChange={e => setFormData({ ...formData, quantity: e.target.value })}
                className="input-field"
              />
            </div>
          </div>
          <div className="flex justify-end gap-3 pt-4">
            <button
              onClick={() => {
                setShowAddProduct(false)
                setEditingProduct(null)
                resetForm()
              }}
              className="btn-secondary"
            >
              Отмена
            </button>
            <button
              onClick={editingProduct ? handleUpdateProduct : handleAddProduct}
              className="btn-primary"
            >
              Сохранить
            </button>
          </div>
        </div>
      </Modal>

      {/* Move Product Modal */}
      <Modal
        isOpen={showMoveProduct}
        onClose={() => {
          setShowMoveProduct(false)
          setMovingProduct(null)
          setMoveCategoryId(0)
        }}
        title={movingProduct ? 'Перемещение товара' : 'Перемещение товаров'}
      >
        <div className="space-y-4">
          {movingProduct && (
            <div className="p-3 bg-gray-50 rounded">
              <p className="font-medium">{movingProduct.name}</p>
              <p className="text-sm text-gray-600">Текущая категория: {movingProduct.categoryName}</p>
            </div>
          )}
          {!movingProduct && selectedProducts.length > 0 && (
            <p className="text-gray-600">Перемещение {selectedProducts.length} товаров</p>
          )}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Новая категория
            </label>
            <select
              value={moveCategoryId}
              onChange={e => setMoveCategoryId(parseInt(e.target.value))}
              className="input-field"
            >
              {categories
                .filter(cat => movingProduct ? cat.id !== movingProduct.categoryId : true)
                .map(cat => (
                  <option key={cat.id} value={cat.id}>{cat.name}</option>
                ))}
            </select>
          </div>
          <div className="flex justify-end gap-3 pt-4">
            <button
              onClick={() => {
                setShowMoveProduct(false)
                setMovingProduct(null)
              }}
              className="btn-secondary"
            >
              Отмена
            </button>
            <button
              onClick={movingProduct ? handleMoveProduct : handleMoveBulk}
              className="btn-primary"
            >
              Переместить
            </button>
          </div>
        </div>
      </Modal>

      {/* Advanced Search Modal */}
      <Modal
        isOpen={showAdvancedSearch}
        onClose={() => setShowAdvancedSearch(false)}
        title="Расширенный поиск"
      >
        <div className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Название товара
            </label>
            <input
              type="text"
              value={filters.name}
              onChange={e => setFilters({ ...filters, name: e.target.value })}
              className="input-field"
              placeholder="Частичное совпадение"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Категория
            </label>
            <select
              value={filters.categoryId}
              onChange={e => setFilters({ ...filters, categoryId: e.target.value ? parseInt(e.target.value) : '' })}
              className="input-field"
            >
              <option value="">Все категории</option>
              {categories.map(cat => (
                <option key={cat.id} value={cat.id}>{cat.name}</option>
              ))}
            </select>
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Артикул
            </label>
            <input
              type="text"
              value={filters.article}
              onChange={e => setFilters({ ...filters, article: e.target.value })}
              className="input-field"
              placeholder="Точное совпадение"
            />
          </div>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Цена от
              </label>
              <input
                type="number"
                step="0.01"
                min="0"
                value={filters.minPrice}
                onChange={e => setFilters({ ...filters, minPrice: e.target.value })}
                className="input-field"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Цена до
              </label>
              <input
                type="number"
                step="0.01"
                min="0"
                value={filters.maxPrice}
                onChange={e => setFilters({ ...filters, maxPrice: e.target.value })}
                className="input-field"
              />
            </div>
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Наличие на складе
            </label>
            <div className="flex gap-4">
              <label className="flex items-center">
                <input
                  type="radio"
                  name="inStock"
                  value="all"
                  checked={filters.inStock === 'all'}
                  onChange={e => setFilters({ ...filters, inStock: e.target.value as any })}
                  className="mr-2"
                />
                Все
              </label>
              <label className="flex items-center">
                <input
                  type="radio"
                  name="inStock"
                  value="true"
                  checked={filters.inStock === 'true'}
                  onChange={e => setFilters({ ...filters, inStock: e.target.value as any })}
                  className="mr-2"
                />
                В наличии
              </label>
              <label className="flex items-center">
                <input
                  type="radio"
                  name="inStock"
                  value="false"
                  checked={filters.inStock === 'false'}
                  onChange={e => setFilters({ ...filters, inStock: e.target.value as any })}
                  className="mr-2"
                />
                Нет в наличии
              </label>
            </div>
          </div>
          <div className="flex justify-end gap-3 pt-4">
            <button onClick={resetFilters} className="btn-secondary">
              Сбросить
            </button>
            <button onClick={handleAdvancedSearch} className="btn-primary">
              Найти
            </button>
          </div>
        </div>
      </Modal>
    </div>
  )
}

export default Products
