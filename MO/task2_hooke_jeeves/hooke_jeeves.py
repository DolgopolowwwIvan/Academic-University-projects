"""
Метод Хука-Дживса
Реализация для лабораторной работы №2
"""

import numpy as np
import matplotlib.pyplot as plt
from typing import Callable, Tuple, List


def hooke_jeeves(
    f: Callable[[np.ndarray], float],
    x0: np.ndarray,
    delta0: float = 0.5,
    epsilon: float = 1e-6,
    alpha: float = 2.0,
    beta: float = 0.5,
    max_iter: int = 10000,
    verbose: bool = False
) -> Tuple[np.ndarray, List[np.ndarray], int]:
    """
    Метод Хука-Дживса
    
    Args:
        f: целевая функция
        x0: начальное приближение
        delta0: начальный шаг
        epsilon: точность
        alpha: коэффициент расширения шага
        beta: коэффициент сжатия шага
        max_iter: максимальное число итераций
        verbose: выводить прогресс
        
    Returns:
        x_opt: найденная точка минимума
        trajectory: траектория спуска
        iterations: число итераций
    """
    x = x0.copy()
    x_base = x.copy()
    n = len(x)
    delta = np.full(n, delta0)
    trajectory = [x.copy()]
    iterations = 0
    
    while iterations < max_iter:
        iterations += 1
        
        # Исследующий поиск
        x_new, success = exploratory_search(f, x_base, delta)
        
        if not success:
            # Уменьшаем шаг
            delta *= beta
            if np.max(delta) < epsilon:
                break
            continue
        
        # Поиск по образцу
        S = x_new - x_base  # Направление спуска
        
        # Одномерная минимизация вдоль направления S
        def f_1d(lambda_val: float) -> float:
            return f(x_base + lambda_val * S)
        
        # Находим оптимальный lambda методом золотого сечения
        # Интервал поиска от 0 до 2 * длины направления
        step = np.linalg.norm(S) * 2
        lambda_opt = golden_section(f_1d, 0, max(step, 1.0), epsilon * 0.1)
        x_pattern = x_base + lambda_opt * S
        
        # Сравниваем значения
        if f(x_pattern) < f(x_new):
            x_base = x_pattern
            trajectory.append(x_base.copy())
            # Увеличиваем шаг
            delta *= alpha
        else:
            x_base = x_new
            trajectory.append(x_base.copy())
        
        if verbose and iterations % 10 == 0:
            print(f"Iter {iterations}: f={f(x_base):.6f}, delta={delta[0]:.2e}")
        
        # Проверка критерия остановки
        if np.linalg.norm(delta) < epsilon:
            break
    
    if verbose:
        print(f"Сошлось за {iterations} итераций")
        print(f"Оптимальная точка: {x_base}")
        print(f"Значение функции: {f(x_base):.6f}")
    
    return x_base, trajectory, iterations


def exploratory_search(
    f: Callable[[np.ndarray], float],
    x: np.ndarray,
    delta: np.ndarray
) -> Tuple[np.ndarray, bool]:
    """
    Исследующий поиск вокруг базисной точки
    
    Args:
        f: целевая функция
        x: базисная точка
        delta: вектор шагов
        
    Returns:
        x_new: новая точка
        success: был ли успешен хотя бы один шаг
    """
    x_new = x.copy()
    success = False
    
    for i in range(len(x)):
        f_current = f(x_new)
        
        # Пробный шаг в положительном направлении
        x_temp = x_new.copy()
        x_temp[i] += delta[i]
        f_plus = f(x_temp)
        
        if f_plus < f_current:
            x_new[i] += delta[i]
            success = True
            continue
        
        # Пробный шаг в отрицательном направлении
        x_temp = x_new.copy()
        x_temp[i] -= delta[i]
        f_minus = f(x_temp)
        
        if f_minus < f_current:
            x_new[i] -= delta[i]
            success = True
    
    return x_new, success


def golden_section(
    f: Callable[[float], float],
    a: float,
    b: float,
    epsilon: float = 1e-6,
    max_iter: int = 100
) -> float:
    """
    Метод золотого сечения для одномерной минимизации
    """
    phi = (1 + np.sqrt(5)) / 2
    resphi = 2 - phi
    
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
    print("Метод Хука-Дживса")
    print("=" * 60)
    
    # Тест 1: Квадратичная функция
    print("\n--- Тест 1: Квадратичная функция ---")
    x0_quad = np.array([0.0, 0.0])
    x_opt_quad, traj_quad, iter_quad = hooke_jeeves(
        quadratic_function, x0_quad, delta0=0.5, epsilon=1e-6, verbose=True
    )
    
    plot_trajectory(
        quadratic_function, traj_quad,
        "Метод Хука-Дживса: Квадратичная функция",
        (-5, 15, 100), (-5, 15, 100)
    )
    
    # Тест 2: Функция Розенброка
    print("\n--- Тест 2: Функция Розенброка ---")
    x0_rosen = np.array([-1.2, 1.0])
    x_opt_rosen, traj_rosen, iter_rosen = hooke_jeeves(
        rosenbrock_function, x0_rosen, delta0=0.5, epsilon=1e-6, verbose=True
    )
    
    plot_trajectory(
        rosenbrock_function, traj_rosen,
        "Метод Хука-Дживса: Функция Розенброка",
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
