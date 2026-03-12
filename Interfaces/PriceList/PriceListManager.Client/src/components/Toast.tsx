interface ToastProps {
  message: string
  type: 'success' | 'error'
}

function Toast({ message, type }: ToastProps) {
  return (
    <div
      className={`mb-2 px-4 py-3 rounded-lg shadow-lg ${
        type === 'success' ? 'bg-green-500' : 'bg-red-500'
      } text-white`}
    >
      {message}
    </div>
  )
}

export default Toast
