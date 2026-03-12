import { useState, useEffect } from 'react'
import { useToast } from '../App'
import { usersApi } from '../services/api'
import Modal from '../components/Modal'

interface User {
  id: number
  email: string
  login: string
  role: string
  createdAt: string
}

function Users() {
  const { showToast } = useToast()
  const [users, setUsers] = useState<User[]>([])
  const [loading, setLoading] = useState(true)
  const [showAddModal, setShowAddModal] = useState(false)
  const [formData, setFormData] = useState({
    email: '',
    login: '',
    role: 'Admin'
  })
  const [tempPassword, setTempPassword] = useState<string | null>(null)

  useEffect(() => {
    loadUsers()
  }, [])

  const loadUsers = async () => {
    try {
      const response = await usersApi.getAll()
      setUsers(response.data)
    } catch (error) {
      showToast('Ошибка загрузки пользователей', 'error')
    } finally {
      setLoading(false)
    }
  }

  const handleAddAdmin = async () => {
    if (!formData.email.trim()) {
      showToast('Email обязателен', 'error')
      return
    }

    try {
      const response = await usersApi.createAdmin({
        email: formData.email,
        login: formData.login || undefined,
        role: formData.role
      })
      showToast(`Пользователь создан. Временный пароль: ${response.data.tempPassword}`, 'success')
      setTempPassword(response.data.tempPassword)
      setShowAddModal(false)
      resetForm()
      loadUsers()
    } catch (error: any) {
      showToast(error.response?.data?.message || 'Ошибка создания пользователя', 'error')
    }
  }

  const handleUpdateRole = async (userId: number, newRole: string) => {
    try {
      await usersApi.updateRole(userId, newRole)
      showToast('Роль обновлена', 'success')
      loadUsers()
    } catch (error: any) {
      showToast(error.response?.data?.message || 'Ошибка обновления роли', 'error')
    }
  }

  const resetForm = () => {
    setFormData({ email: '', login: '', role: 'Admin' })
    setTempPassword(null)
  }

  const getRoleBadgeClass = (role: string) => {
    switch (role) {
      case 'SuperAdmin':
        return 'bg-purple-100 text-purple-700'
      case 'Admin':
        return 'bg-blue-100 text-blue-700'
      default:
        return 'bg-gray-100 text-gray-700'
    }
  }

  return (
    <div className="container mx-auto px-4 py-6">
      {/* Breadcrumbs */}
      <div className="mb-4 text-sm text-gray-600">
        <span>Главная</span>
        <span className="mx-2">/</span>
        <span className="text-gray-800">Пользователи</span>
      </div>

      {/* Page Header */}
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold">Управление пользователями</h1>
        <button onClick={() => setShowAddModal(true)} className="btn-primary">
          + Добавить администратора
        </button>
      </div>

      {/* Users Table */}
      <div className="card overflow-hidden">
        <table className="w-full">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-4 py-3 text-left text-sm font-medium text-gray-600">Логин</th>
              <th className="px-4 py-3 text-left text-sm font-medium text-gray-600">Email</th>
              <th className="px-4 py-3 text-left text-sm font-medium text-gray-600">Роль</th>
              <th className="px-4 py-3 text-left text-sm font-medium text-gray-600">Добавлен</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-200">
            {loading ? (
              <tr>
                <td colSpan={4} className="px-4 py-8 text-center text-gray-500">
                  Загрузка...
                </td>
              </tr>
            ) : users.length === 0 ? (
              <tr>
                <td colSpan={4} className="px-4 py-8 text-center text-gray-500">
                  Пользователи не найдены
                </td>
              </tr>
            ) : (
              users.map(user => (
                <tr key={user.id} className="hover:bg-gray-50">
                  <td className="px-4 py-3 font-medium">{user.login}</td>
                  <td className="px-4 py-3 text-sm">{user.email}</td>
                  <td className="px-4 py-3">
                    <select
                      value={user.role}
                      onChange={e => handleUpdateRole(user.id, e.target.value)}
                      className={`px-3 py-1 rounded-full text-sm border-0 cursor-pointer ${getRoleBadgeClass(user.role)}`}
                    >
                      <option value="User">User</option>
                      <option value="Admin">Admin</option>
                      <option value="SuperAdmin">SuperAdmin</option>
                    </select>
                  </td>
                  <td className="px-4 py-3 text-sm text-gray-600">
                    {new Date(user.createdAt).toLocaleDateString('ru-RU')}
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {/* Add Admin Modal */}
      <Modal
        isOpen={showAddModal}
        onClose={() => {
          setShowAddModal(false)
          resetForm()
        }}
        title="Добавление администратора"
      >
        <div className="space-y-4">
          {tempPassword && (
            <div className="p-3 bg-green-100 text-green-700 rounded">
              Пользователь создан. Временный пароль: <strong>{tempPassword}</strong>
            </div>
          )}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Email *
            </label>
            <input
              type="email"
              value={formData.email}
              onChange={e => setFormData({ ...formData, email: e.target.value })}
              className="input-field"
              placeholder="example@mail.ru"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Логин (необязательно)
            </label>
            <input
              type="text"
              value={formData.login}
              onChange={e => setFormData({ ...formData, login: e.target.value })}
              className="input-field"
              placeholder="Если не указан, будет создан автоматически"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Уровень прав
            </label>
            <select
              value={formData.role}
              onChange={e => setFormData({ ...formData, role: e.target.value })}
              className="input-field"
            >
              <option value="Admin">Администратор (Admin)</option>
              <option value="SuperAdmin">Супер-администратор (SuperAdmin)</option>
            </select>
          </div>
          <div className="flex justify-end gap-3 pt-4">
            <button
              onClick={() => {
                setShowAddModal(false)
                resetForm()
              }}
              className="btn-secondary"
            >
              Отмена
            </button>
            <button onClick={handleAddAdmin} className="btn-primary">
              Назначить
            </button>
          </div>
        </div>
      </Modal>
    </div>
  )
}

export default Users
