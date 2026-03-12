import { Link, useLocation, useNavigate } from 'react-router-dom'
import { useAuth } from '../App'

function Header() {
  const { user, logout, isAdmin, isSuperAdmin } = useAuth()
  const location = useLocation()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  return (
    <header className="bg-white shadow-md">
      <div className="container mx-auto px-4 py-4">
        <div className="flex items-center justify-between">
          <div className="flex items-center space-x-6">
            <Link to="/" className="text-xl font-bold text-blue-500">
              Прайс-лист фирмы
            </Link>
            <nav className="flex space-x-4">
              <Link
                to="/"
                className={`px-3 py-2 rounded ${
                  location.pathname === '/'
                    ? 'bg-blue-100 text-blue-600'
                    : 'text-gray-600 hover:text-blue-500'
                }`}
              >
                Товары
              </Link>
              {isAdmin && (
                <Link
                  to="/categories"
                  className={`px-3 py-2 rounded ${
                    location.pathname === '/categories'
                      ? 'bg-blue-100 text-blue-600'
                      : 'text-gray-600 hover:text-blue-500'
                  }`}
                >
                  Категории
                </Link>
              )}
              {isSuperAdmin && (
                <Link
                  to="/users"
                  className={`px-3 py-2 rounded ${
                    location.pathname === '/users'
                      ? 'bg-blue-100 text-blue-600'
                      : 'text-gray-600 hover:text-blue-500'
                  }`}
                >
                  Пользователи
                </Link>
              )}
            </nav>
          </div>
          <div className="flex items-center space-x-4">
            <div className="text-sm text-gray-600">
              <span className="font-medium">{user?.login}</span>
              <span className="mx-2">|</span>
              <span className="text-gray-500">{user?.role}</span>
            </div>
            <button
              onClick={handleLogout}
              className="text-gray-600 hover:text-red-500 transition"
            >
              Выйти
            </button>
          </div>
        </div>
      </div>
    </header>
  )
}

export default Header
