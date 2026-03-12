import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { useState, useEffect, createContext, useContext } from 'react'
import Login from './pages/Login'
import Products from './pages/Products'
import Categories from './pages/Categories'
import Users from './pages/Users'
import Header from './components/Header'
import Toast from './components/Toast'

export interface User {
  id: number
  email: string
  login: string
  role: string
}

interface AuthContextType {
  user: User | null
  token: string | null
  login: (token: string, user: User) => void
  logout: () => void
  isAdmin: boolean
  isSuperAdmin: boolean
}

export const AuthContext = createContext<AuthContextType>({
  user: null,
  token: null,
  login: () => {},
  logout: () => {},
  isAdmin: false,
  isSuperAdmin: false
})

export const useAuth = () => useContext(AuthContext)

interface ToastMessage {
  id: number
  message: string
  type: 'success' | 'error'
}

interface ToastHookType {
  toasts: ToastMessage[]
  showToast: (message: string, type?: 'success' | 'error') => void
}

export const ToastContext = createContext<ToastHookType>({
  toasts: [],
  showToast: () => {}
})

export const useToast = () => useContext(ToastContext)

function ToastProvider({ children }: { children: React.ReactNode }) {
  const [toasts, setToasts] = useState<ToastMessage[]>([])

  const showToast = (message: string, type: 'success' | 'error' = 'success') => {
    const id = Date.now()
    setToasts(prev => [...prev, { id, message, type }])
    setTimeout(() => {
      setToasts(prev => prev.filter(t => t.id !== id))
    }, 3000)
  }

  return (
    <ToastContext.Provider value={{ toasts, showToast }}>
      {children}
      <div className="fixed bottom-4 right-4 z-50">
        {toasts.map(toast => (
          <Toast key={toast.id} message={toast.message} type={toast.type} />
        ))}
      </div>
    </ToastContext.Provider>
  )
}

function AppContent() {
  const [user, setUser] = useState<User | null>(null)
  const [token, setToken] = useState<string | null>(localStorage.getItem('token'))

  useEffect(() => {
    if (token) {
      const savedUser = localStorage.getItem('user')
      if (savedUser) {
        try {
          setUser(JSON.parse(savedUser))
        } catch {
          localStorage.removeItem('token')
          localStorage.removeItem('user')
          setToken(null)
        }
      }
    }
  }, [token])

  const login = (newToken: string, newUser: User) => {
    localStorage.setItem('token', newToken)
    localStorage.setItem('user', JSON.stringify(newUser))
    setToken(newToken)
    setUser(newUser)
  }

  const logout = () => {
    localStorage.removeItem('token')
    localStorage.removeItem('user')
    setToken(null)
    setUser(null)
  }

  const isAdmin = user?.role === 'Admin' || user?.role === 'SuperAdmin'
  const isSuperAdmin = user?.role === 'SuperAdmin'

  return (
    <AuthContext.Provider value={{ user, token, login, logout, isAdmin, isSuperAdmin }}>
      <BrowserRouter>
        {user && <Header />}
        <div className="min-h-screen">
          <Routes>
            <Route path="/login" element={!user ? <Login /> : <Navigate to="/" />} />
            <Route path="/" element={user ? <Products /> : <Navigate to="/login" />} />
            <Route path="/categories" element={user ? <Categories /> : <Navigate to="/login" />} />
            <Route path="/users" element={user && isSuperAdmin ? <Users /> : <Navigate to="/" />} />
            <Route path="*" element={<Navigate to="/" />} />
          </Routes>
        </div>
      </BrowserRouter>
    </AuthContext.Provider>
  )
}

function App() {
  return (
    <ToastProvider>
      <AppContent />
    </ToastProvider>
  )
}

export default App
