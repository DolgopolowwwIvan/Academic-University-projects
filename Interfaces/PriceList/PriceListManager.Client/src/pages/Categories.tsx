import { useState, useEffect } from 'react'
import { useToast } from '../App'
import { categoriesApi } from '../services/api'
import Modal from '../components/Modal'

interface Category {
  id: number
  name: string
  description: string | null
  productsCount: number
}

function Categories() {
  const { showToast } = useToast()
  const [categories, setCategories] = useState<Category[]>([])
  const [loading, setLoading] = useState(true)
  const [showAddModal, setShowAddModal] = useState(false)
  const [editingCategory, setEditingCategory] = useState<Category | null>(null)
  const [formData, setFormData] = useState({
    name: '',
    description: ''
  })

  useEffect(() => {
    loadCategories()
  }, [])

  const loadCategories = async () => {
    try {
      const response = await categoriesApi.getAll()
      setCategories(response.data)
    } catch (error) {
      showToast('Ошибка загрузки категорий', 'error')
    } finally {
      setLoading(false)
    }
  }

  const handleAddCategory = async () => {
    if (!formData.name.trim()) {
      showToast('Название категории обязательно', 'error')
      return
    }

    try {
      await categoriesApi.create({
        name: formData.name,
        description: formData.description || undefined
      })
      showToast('Категория добавлена', 'success')
      setShowAddModal(false)
      resetForm()
      loadCategories()
    } catch (error: any) {
      showToast(error.response?.data?.message || 'Ошибка добавления категории', 'error')
    }
  }

  const handleUpdateCategory = async () => {
    if (!editingCategory || !formData.name.trim()) {
      showToast('Название категории обязательно', 'error')
      return
    }

    try {
      await categoriesApi.update(editingCategory.id, {
        name: formData.name,
        description: formData.description || undefined
      })
      showToast('Категория обновлена', 'success')
      setEditingCategory(null)
      resetForm()
      loadCategories()
    } catch (error: any) {
      showToast(error.response?.data?.message || 'Ошибка обновления категории', 'error')
    }
  }

  const handleDeleteCategory = async (id: number) => {
    if (!confirm('Удалить категорию? Товары в этой категории должны отсутствовать.')) {
      return
    }

    try {
      await categoriesApi.delete(id)
      showToast('Категория удалена', 'success')
      loadCategories()
    } catch (error: any) {
      showToast(error.response?.data?.message || 'Не удалось удалить категорию', 'error')
    }
  }

  const resetForm = () => {
    setFormData({ name: '', description: '' })
  }

  const openEditModal = (category: Category) => {
    setEditingCategory(category)
    setFormData({
      name: category.name,
      description: category.description || ''
    })
  }

  return (
    <div className="container mx-auto px-4 py-6">
      {/* Breadcrumbs */}
      <div className="mb-4 text-sm text-gray-600">
        <span>Главная</span>
        <span className="mx-2">/</span>
        <span className="text-gray-800">Категории</span>
      </div>

      {/* Page Header */}
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold">Управление категориями</h1>
        <button onClick={() => setShowAddModal(true)} className="btn-primary">
          + Добавить категорию
        </button>
      </div>

      {/* Categories Grid */}
      {loading ? (
        <div className="text-center py-8 text-gray-500">Загрузка...</div>
      ) : categories.length === 0 ? (
        <div className="text-center py-8 text-gray-500">Категории не найдены</div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {categories.map(category => (
            <div key={category.id} className="card">
              <div className="flex justify-between items-start mb-2">
                <h3 className="text-lg font-semibold">{category.name}</h3>
                <div className="flex gap-2">
                  <button
                    onClick={() => openEditModal(category)}
                    className="text-blue-500 hover:text-blue-700 text-sm"
                  >
                    Редактировать
                  </button>
                  <button
                    onClick={() => handleDeleteCategory(category.id)}
                    className="text-red-500 hover:text-red-700 text-sm"
                  >
                    Удалить
                  </button>
                </div>
              </div>
              <p className="text-gray-600 text-sm mb-3">
                {category.description || 'Описание отсутствует'}
              </p>
              <div className="inline-block px-3 py-1 bg-blue-100 text-blue-700 rounded-full text-sm">
                Товаров: {category.productsCount}
              </div>
            </div>
          ))}
        </div>
      )}

      {/* Add/Edit Category Modal */}
      <Modal
        isOpen={showAddModal || !!editingCategory}
        onClose={() => {
          setShowAddModal(false)
          setEditingCategory(null)
          resetForm()
        }}
        title={editingCategory ? 'Редактирование категории' : 'Добавление категории'}
      >
        <div className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Название категории *
            </label>
            <input
              type="text"
              value={formData.name}
              onChange={e => setFormData({ ...formData, name: e.target.value })}
              className="input-field"
              placeholder="Введите название"
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
              placeholder="Введите описание (необязательно)"
            />
          </div>
          <div className="flex justify-end gap-3 pt-4">
            <button
              onClick={() => {
                setShowAddModal(false)
                setEditingCategory(null)
                resetForm()
              }}
              className="btn-secondary"
            >
              Отмена
            </button>
            <button
              onClick={editingCategory ? handleUpdateCategory : handleAddCategory}
              className="btn-primary"
            >
              Сохранить
            </button>
          </div>
        </div>
      </Modal>
    </div>
  )
}

export default Categories
