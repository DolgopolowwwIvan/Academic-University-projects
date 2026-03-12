"""
Метод Розенброка
Реализация для лабораторной работы №2
"""

import numpy as np
import matplotlib.pyplot as plt
from typing import Callable, Tuple, List


def rosenbrock_method(
    f: Callable[[np.ndarray], float],
    x0: np.ndarray,
    epsilon: float = 1e-6,
    max_iter: int = 1000,
    verbose: bool = False
) -> Tuple[np.ndarray, List[np.ndarray], int]:
    """
    Метод Розенброка
    
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
    
    # Инициализация ортогональных направлений (единичные векторы)
    S = np.eye(n)
    trajectory = [x.copy()]
    iterations = 0
    
    while iterations < max_iter:
        iterations += 1
        x_prev = x.copy()
        x_before_search = x.copy()
        
        lambdas = np.zeros(n)
        
        # Минимизация по каждому направлению
        for i in range(n):
            def f_1d(lambda_val: float) -> float:
                return f(x + lambda_val * S[:, i])
            
            # Адаптивный интервал поиска
            step = max(1.0, abs(lambdas[i]) * 2 if i > 0 else 1.0)
            lambda_i = golden_section(f_1d, -step, step, epsilon * 0.1)
            lambdas[i] = lambda_i
            x = x + lambda_i * S[:, i]
            trajectory.append(x.copy())
        
        # Проверка критерия остановки
        f_diff = abs(f(x) - f(x_prev))
        x_diff = np.linalg.norm(x - x_prev)
        
        if verbose and iterations % 10 == 0:
            print(f"Iter {iterations}: f={f(x):.6f}, |dx|={x_diff:.2e}, lambdas={lambdas}")
        
        if f_diff < epsilon and x_diff < epsilon:
            break
        
        # Обновление направлений (процедура Грама-Шмидта)
        # Вычисляем векторы перемещений
        D = np.zeros((n, n))
        for i in range(n):
            D[:, i] = np.sum([lambdas[j] * S[:, j] for j in range(i, n)], axis=0)
        
        # Ортогонализация Грама-Шмидта
        new_S = np.zeros((n, n))
        
        for i in range(n):
            if np.linalg.norm(D[:, i]) < 1e-10:
                # Если вектор слишком мал, сохраняем старое направление
                new_S[:, i] = S[:, i].copy()
                continue
            
            if i == 0:
                new_S[:, 0] = D[:, 0] / np.linalg.norm(D[:, 0])
            else:
                # Вычитаем проекции на предыдущие векторы
                B = D[:, i].copy()
                for j in range(i):
                    if np.linalg.norm(new_S[:, j]) > 1e-10:
                        proj = np.dot(D[:, i], new_S[:, j]) / np.dot(new_S[:, j], new_S[:, j])
                        B = B - proj * new_S[:, j]
                
                if np.linalg.norm(B) > 1e-10:
                    new_S[:, i] = B / np.linalg.norm(B)
                else:
                    # Если B слишком мал, используем предыдущее направление
                    new_S[:, i] = S[:, i].copy()
        
        S = new_S
    
    if verbose:
        print(f"Сошлось за {iterations} итераций")
        print(f"Оптимальная точка: {x}")
        print(f"Значение функции: {f(x):.6f}")
    
    return x, trajectory, iterations


def get_palmer_direction(A_prev: np.ndarray, A_curr: np.ndarray) -> np.ndarray:
    """
    Вычисление направления по формуле Палмера для вырожденного случая
    
    Args:
        A_prev: предыдущий вектор A
        A_curr: текущий вектор A
        
    Returns:
        новое направление
    """
    norm_A_prev2 = np.dot(A_prev, A_prev)
    norm_A_curr2 = np.dot(A_curr, A_curr)
    
    numerator = np.sqrt(norm_A_curr2 * norm_A_prev2 - np.dot(A_curr, A_prev)**2)
    denominator = np.dot(A_curr, A_prev) * np.sqrt(norm_A_prev2 - norm_A_curr2)
    
    # Если знаменатель близок к нулю, возвращаем ортогональный вектор
    if abs(denominator) < 1e-10:
        # Создаем ортогональный вектор
        if abs(A_prev[0]) > abs(A_prev[1]):
            ortho = np.array([-A_prev[1], A_prev[0]])
        else:
            ortho = np.array([A_prev[1], -A_prev[0]])
        return ortho / np.linalg.norm(ortho)
    
    factor = numerator / denominator
    return A_curr + factor * A_prev


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
    print("Метод Розенброка")
    print("=" * 60)
    
    # Тест 1: Квадратичная функция
    print("\n--- Тест 1: Квадратичная функция ---")
    x0_quad = np.array([0.0, 0.0])
    x_opt_quad, traj_quad, iter_quad = rosenbrock_method(
        quadratic_function, x0_quad, epsilon=1e-6, verbose=True
    )
    
    plot_trajectory(
        quadratic_function, traj_quad,
        "Метод Розенброка: Квадратичная функция",
        (-5, 15, 100), (-5, 15, 100)
    )
    
    # Тест 2: Функция Розенброка
    print("\n--- Тест 2: Функция Розенброка ---")
    x0_rosen = np.array([-1.2, 1.0])
    x_opt_rosen, traj_rosen, iter_rosen = rosenbrock_method(
        rosenbrock_function, x0_rosen, epsilon=1e-6, verbose=True
    )
    
    plot_trajectory(
        rosenbrock_function, traj_rosen,
        "Метод Розенброка: Функция Розенброка",
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
