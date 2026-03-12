"""
Метод Гаусса (покоординатный спуск)
Реализация для лабораторной работы №2
"""

import numpy as np
import matplotlib.pyplot as plt
from typing import Callable, Tuple, List


def gauss_method(
    f: Callable[[np.ndarray], float],
    x0: np.ndarray,
    epsilon: float = 1e-6,
    max_iter: int = 10000,
    verbose: bool = False
) -> Tuple[np.ndarray, List[np.ndarray], int]:
    """
    Метод Гаусса (покоординатный спуск)
    
    Args:
        f: целевая функция
        x0: начальное приближение
        epsilon: точность
        max_iter: максимальное число итераций
        verbose: выводить прогресс
        
    Returns:
        x_opt: найденная точка минимума
        trajectory: траектория спуска
        iterations: число итераций
    """
    x = x0.copy()
    n = len(x)
    trajectory = [x.copy()]
    iterations = 0
    
    while iterations < max_iter:
        x_prev = x.copy()
        
        # Циклический покоординатный спуск
        for i in range(n):
            # Минимизация по i-й координате при фиксированных остальных
            def f_1d(lambda_i: float) -> float:
                x_temp = x.copy()
                x_temp[i] = lambda_i
                return f(x_temp)
            
            # Используем метод золотого сечения для одномерной минимизации
            # Начальный интервал поиска адаптивный
            step = max(1.0, abs(x[i]) * 0.5)
            lambda_opt = golden_section(f_1d, x[i] - step, x[i] + step, epsilon * 0.1)
            x[i] = lambda_opt
            trajectory.append(x.copy())
        
        iterations += 1
        
        # Проверка критерия остановки
        f_diff = abs(f(x) - f(x_prev))
        x_diff = np.linalg.norm(x - x_prev)
        
        if verbose and iterations % 10 == 0:
            print(f"Iter {iterations}: f={f(x):.6f}, |x-x_prev|={x_diff:.2e}")
        
        if f_diff < epsilon and x_diff < epsilon:
            break
    
    if verbose:
        print(f"Сошлось за {iterations} итераций")
        print(f"Оптимальная точка: {x}")
        print(f"Значение функции: {f(x):.6f}")
    
    return x, trajectory, iterations


def golden_section(
    f: Callable[[float], float],
    a: float,
    b: float,
    epsilon: float = 1e-6,
    max_iter: int = 100
) -> float:
    """
    Метод золотого сечения для одномерной минимизации
    
    Args:
        f: функция одной переменной
        a, b: границы интервала
        epsilon: точность
        max_iter: максимальное число итераций
        
    Returns:
        точка минимума
    """
    phi = (1 + np.sqrt(5)) / 2  # Золотое сечение
    resphi = 2 - phi  # 1 / phi^2
    
    x1 = a + resphi * (b - a)
    x2 = b - resphi * (b - a)
    f1 = f(x1)
    f2 = f(x2)
    
    for _ in range(max_iter):
        if abs(b - a) < epsilon:
            break
        
        if f1 < f2:
            b = x2
            x2 = x1
            f2 = f1
            x1 = a + resphi * (b - a)
            f1 = f(x1)
        else:
            a = x1
            x1 = x2
            f1 = f2
            x2 = b - resphi * (b - a)
            f2 = f(x2)
    
    return (a + b) / 2


# Тестовые функции
def quadratic_function(x: np.ndarray) -> float:
    """Квадратичная функция f1(x) = 10(x1 + x2 - 10)² + (x1 - x2 + 4)²"""
    return 10 * (x[0] + x[1] - 10)**2 + (x[0] - x[1] + 4)**2


def rosenbrock_function(x: np.ndarray) -> float:
    """Функция Розенброка f2(x) = 100(x2 - x1²)² + (1 - x1)²"""
    return 100 * (x[1] - x[0]**2)**2 + (1 - x[0])**2


def plot_trajectory(
    f: Callable[[np.ndarray], float],
    trajectory: List[np.ndarray],
    title: str,
    x_range: Tuple[float, float, float],
    y_range: Tuple[float, float, float]
):
    """Построение траектории спуска"""
    x_vals = np.linspace(x_range[0], x_range[1], 100)
    y_vals = np.linspace(y_range[0], y_range[1], 100)
    X, Y = np.meshgrid(x_vals, y_vals)
    Z = np.zeros_like(X)
    
    for i in range(X.shape[0]):
        for j in range(X.shape[1]):
            Z[i, j] = f(np.array([X[i, j], Y[i, j]]))
    
    trajectory = np.array(trajectory)
    
    plt.figure(figsize=(10, 8))
    plt.contour(X, Y, Z, levels=50, alpha=0.6)
    plt.plot(trajectory[:, 0], trajectory[:, 1], 'ro-', linewidth=1, markersize=4, label='Траектория')
    plt.plot(trajectory[0, 0], trajectory[0, 1], 'go', markersize=10, label='Начало')
    plt.plot(trajectory[-1, 0], trajectory[-1, 1], 'b*', markersize=15, label='Конец')
    plt.colorbar(label='f(x)')
    plt.xlabel('x1')
    plt.ylabel('x2')
    plt.title(title)
    plt.legend()
    plt.grid(True)
    plt.show()


if __name__ == "__main__":
    print("=" * 60)
    print("Метод Гаусса (покоординатный спуск)")
    print("=" * 60)
    
    # Тест 1: Квадратичная функция
    print("\n--- Тест 1: Квадратичная функция ---")
    x0_quad = np.array([0.0, 0.0])
    x_opt_quad, traj_quad, iter_quad = gauss_method(
        quadratic_function, x0_quad, epsilon=1e-6, verbose=True
    )
    
    plot_trajectory(
        quadratic_function, traj_quad,
        "Метод Гаусса: Квадратичная функция",
        (-5, 15, 100), (-5, 15, 100)
    )
    
    # Тест 2: Функция Розенброка
    print("\n--- Тест 2: Функция Розенброка ---")
    x0_rosen = np.array([-1.2, 1.0])
    x_opt_rosen, traj_rosen, iter_rosen = gauss_method(
        rosenbrock_function, x0_rosen, epsilon=1e-6, verbose=True
    )
    
    plot_trajectory(
        rosenbrock_function, traj_rosen,
        "Метод Гаусса: Функция Розенброка",
        (-2, 2, 100), (-1, 3, 100)
    )
    
    # Результаты
    print("\n" + "=" * 60)
    print("Сводные результаты:")
    print("=" * 60)
    print(f"Квадратичная функция:")
    print(f"  Начальная точка: {x0_quad}")
    print(f"  Найденный минимум: {x_opt_quad}")
    print(f"  Значение функции: {quadratic_function(x_opt_quad):.6e}")
    print(f"  Число итераций: {iter_quad}")
    
    print(f"\nФункция Розенброка:")
    print(f"  Начальная точка: {x0_rosen}")
    print(f"  Найденный минимум: {x_opt_rosen}")
    print(f"  Значение функции: {rosenbrock_function(x_opt_rosen):.6e}")
    print(f"  Число итераций: {iter_rosen}")
