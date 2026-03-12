import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth, useToast } from '../App'
import { authApi } from '../services/api'

function Login() {
  const [login, setLogin] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)
  const navigate = useNavigate()
  const { login: authLogin } = useAuth()
  const { showToast } = useToast()

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError('')

    if (!login.trim() || !password.trim()) {
      setError('Заполните все поля')
      return
    }

    setLoading(true)
    try {
      const response = await authApi.login(login, password)
      authLogin(response.data.token, response.data.user)
      showToast('Добро пожаловать!', 'success')
      navigate('/')
    } catch (err: any) {
      setError(err.response?.data?.message || 'Неверный логин или пароль')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <div className="card w-full max-w-md">
        <div className="text-center mb-8">
          <h1 className="text-2xl font-bold text-gray-800">
            Прайс-лист фирмы
          </h1>
        </div>
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Логин или Email
            </label>
            <input
              type="text"
              value={login}
              onChange={e => setLogin(e.target.value)}
              className="input-field"
              placeholder="Введите логин или email"
              disabled={loading}
            />
          </div>
          <div className="mb-6">
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Пароль
            </label>
            <input
              type="password"
              value={password}
              onChange={e => setPassword(e.target.value)}
              className="input-field"
              placeholder="Введите пароль"
              disabled={loading}
            />
          </div>
          {error && (
            <div className="mb-4 p-3 bg-red-100 text-red-700 rounded">
              {error}
            </div>
          )}
          <button
            type="submit"
            disabled={loading}
            className="btn-primary w-full"
          >
            {loading ? 'Вход...' : 'Войти'}
          </button>
        </form>
      </div>
    </div>
  )
}

export default Login
