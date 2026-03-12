"""
Сравнительный анализ методов прямого поиска
Гаусса, Хука-Дживса, Розенброка
"""

import numpy as np
import matplotlib.pyplot as plt
from typing import Callable, Tuple, List, Dict
import sys
import os

# Добавляем пути к модулям методов
sys.path.append(os.path.join(os.path.dirname(__file__), '../task1_gauss'))
sys.path.append(os.path.join(os.path.dirname(__file__), '../task2_hooke_jeeves'))
sys.path.append(os.path.join(os.path.dirname(__file__), '../task3_rosenbrock'))

from gauss_method import gauss_method
from hooke_jeeves import hooke_jeeves
from rosenbrock_method import rosenbrock_method


# Тестовые функции
def quadratic_function(x: np.ndarray) -> float:
    """Квадратичная функция f1(x) = 10(x1 + x2 - 10)² + (x1 - x2 + 4)²"""
    return 10 * (x[0] + x[1] - 10)**2 + (x[0] - x[1] + 4)**2


def rosenbrock_function(x: np.ndarray) -> float:
    """Функция Розенброка f2(x) = 100(x2 - x1²)² + (1 - x1)²"""
    return 100 * (x[1] - x[0]**2)**2 + (1 - x[0])**2


def run_all_methods(
    f: Callable[[np.ndarray], float],
    x0: np.ndarray,
    epsilon: float = 1e-6,
    verbose: bool = False
) -> Dict[str, Tuple[np.ndarray, List[np.ndarray], int]]:
    """Запуск всех методов на одной функции"""
    results = {}
    
    if verbose:
        print(f"\n{'='*60}")
        print(f"Тестирование функции с начальной точкой {x0}")
        print(f"{'='*60}")
    
    # Метод Гаусса
    if verbose:
        print("\n--- Метод Гаусса ---")
    results['Gauss'] = gauss_method(f, x0, epsilon=epsilon, verbose=verbose)
    
    # Метод Хука-Дживса
    if verbose:
        print("\n--- Метод Хука-Дживса ---")
    results['Hooke-Jeeves'] = hooke_jeeves(f, x0, delta0=0.5, epsilon=epsilon, verbose=verbose)
    
    # Метод Розенброка
    if verbose:
        print("\n--- Метод Розенброка ---")
    results['Rosenbrock'] = rosenbrock_method(f, x0, epsilon=epsilon, verbose=verbose)
    
    return results


def plot_comparison(
    f: Callable[[np.ndarray], float],
    results: Dict[str, Tuple[np.ndarray, List[np.ndarray], int]],
    title: str,
    x_range: Tuple[float, float, float],
    y_range: Tuple[float, float, float]
):
    """Построение сравнительных траекторий"""
    x_vals = np.linspace(x_range[0], x_range[1], 100)
    y_vals = np.linspace(y_range[0], y_range[1], 100)
    X, Y = np.meshgrid(x_vals, y_vals)
    Z = np.zeros_like(X)
    
    for i in range(X.shape[0]):
        for j in range(X.shape[1]):
            Z[i, j] = f(np.array([X[i, j], Y[i, j]]))
    
    fig, axes = plt.subplots(1, 3, figsize=(18, 6))
    
    for idx, (name, (x_opt, trajectory, iterations)) in enumerate(results.items()):
        ax = axes[idx]
        trajectory = np.array(trajectory)
        
        ax.contour(X, Y, Z, levels=50, alpha=0.6)
        ax.plot(trajectory[:, 0], trajectory[:, 1], 'ro-', linewidth=1, markersize=3, label='Траектория')
        ax.plot(trajectory[0, 0], trajectory[0, 1], 'go', markersize=8, label='Начало')
        ax.plot(trajectory[-1, 0], trajectory[-1, 1], 'b*', markersize=12, label='Конец')
        ax.set_xlabel('x1')
        ax.set_ylabel('x2')
        ax.set_title(f'{name}\nИтераций: {iterations}, f={f(x_opt):.2e}')
        ax.legend()
        ax.grid(True)
    
    plt.suptitle(title)
    plt.tight_layout()
    plt.show()


def plot_convergence(
    f: Callable[[np.ndarray], float],
    results: Dict[str, Tuple[np.ndarray, List[np.ndarray], int]],
    title: str
):
    """Построение графиков сходимости"""
    plt.figure(figsize=(10, 6))
    
    for name, (x_opt, trajectory, iterations) in results.items():
        f_values = [f(point) for point in trajectory]
        plt.semilogy(range(len(f_values)), f_values, label=name, linewidth=2)
    
    plt.xlabel('Итерация')
    plt.ylabel('log(f(x))')
    plt.title(title)
    plt.legend()
    plt.grid(True)
    plt.show()


def analyze_convergence_by_epsilon(
    f: Callable[[np.ndarray], float],
    x0: np.ndarray,
    epsilons: List[float]
) -> Dict[str, List[int]]:
    """Исследование сходимости при разных точностях"""
    convergence_data = {'Gauss': [], 'Hooke-Jeeves': [], 'Rosenbrock': []}
    
    print(f"\n{'='*60}")
    print("Исследование сходимости при различных точностях")
    print(f"{'='*60}")
    print(f"{'Epsilon':<15} {'Gauss':<15} {'Hooke-Jeeves':<15} {'Rosenbrock':<15}")
    print('-' * 60)
    
    for eps in epsilons:
        # Гаусс
        _, _, iter_g = gauss_method(f, x0, epsilon=eps, verbose=False)
        convergence_data['Gauss'].append(iter_g)
        
        # Хук-Дживс
        _, _, iter_hj = hooke_jeeves(f, x0, delta0=0.5, epsilon=eps, verbose=False)
        convergence_data['Hooke-Jeeves'].append(iter_hj)
        
        # Розенброк
        _, _, iter_r = rosenbrock_method(f, x0, epsilon=eps, verbose=False)
        convergence_data['Rosenbrock'].append(iter_r)
        
        print(f"{eps:<15.0e} {iter_g:<15} {iter_hj:<15} {iter_r:<15}")
    
    return convergence_data


def plot_epsilon_convergence(
    epsilons: List[float],
    convergence_data: Dict[str, List[int]],
    title: str
):
    """График зависимости числа итераций от точности"""
    plt.figure(figsize=(10, 6))
    
    for name, iterations in convergence_data.items():
        plt.loglog(epsilons, iterations, 'o-', label=name, linewidth=2, markersize=8)
    
    plt.xlabel('Точность (epsilon)')
    plt.ylabel('Число итераций')
    plt.title(title)
    plt.legend()
    plt.grid(True)
    plt.show()


def print_comparison_table(
    results: Dict[str, Tuple[np.ndarray, List[np.ndarray], int]],
    f: Callable[[np.ndarray], float]
):
    """Вывод таблицы сравнения"""
    print(f"\n{'='*80}")
    print(f"{'Метод':<20} {'Оптимальная точка':<25} {'f(x)':<15} {'Итерации':<10}")
    print('-' * 80)
    
    for name, (x_opt, _, iterations) in results.items():
        x_str = f"[{x_opt[0]:.4f}, {x_opt[1]:.4f}]"
        print(f"{name:<20} {x_str:<25} {f(x_opt):<15.2e} {iterations:<10}")
    
    print('='*80)


def main():
    """Основная функция анализа"""
    
    # Параметры
    x0_quad = np.array([0.0, 0.0])
    x0_rosen = np.array([-1.2, 1.0])
    
    # ==================== Квадратичная функция ====================
    print("\n" + "="*80)
    print("АНАЛИЗ КВАДРАТИЧНОЙ ФУНКЦИИ")
    print("="*80)
    
    results_quad = run_all_methods(quadratic_function, x0_quad, verbose=True)
    
    print_comparison_table(results_quad, quadratic_function)
    
    plot_comparison(
        quadratic_function, results_quad,
        "Сравнение методов: Квадратичная функция",
        (-5, 15, 100), (-5, 15, 100)
    )
    
    plot_convergence(
        quadratic_function, results_quad,
        "Сходимость методов: Квадратичная функция"
    )
    
    # Исследование сходимости при разных точностях
    epsilons = [1e-2, 1e-4, 1e-6, 1e-8]
    conv_data_quad = analyze_convergence_by_epsilon(quadratic_function, x0_quad, epsilons)
    plot_epsilon_convergence(
        epsilons, conv_data_quad,
        "Зависимость числа итераций от точности: Квадратичная функция"
    )
    
    # ==================== Функция Розенброка ====================
    print("\n" + "="*80)
    print("АНАЛИЗ ФУНКЦИИ РОЗЕНБРОКА")
    print("="*80)
    
    results_rosen = run_all_methods(rosenbrock_function, x0_rosen, verbose=True)
    
    print_comparison_table(results_rosen, rosenbrock_function)
    
    plot_comparison(
        rosenbrock_function, results_rosen,
        "Сравнение методов: Функция Розенброка",
        (-2, 2, 100), (-1, 3, 100)
    )
    
    plot_convergence(
        rosenbrock_function, results_rosen,
        "Сходимость методов: Функция Розенброка"
    )
    
    # Исследование сходимости при разных точностях
    conv_data_rosen = analyze_convergence_by_epsilon(rosenbrock_function, x0_rosen, epsilons)
    plot_epsilon_convergence(
        epsilons, conv_data_rosen,
        "Зависимость числа итераций от точности: Функция Розенброка"
    )
    
    # ==================== Выводы ====================
    print("\n" + "="*80)
    print("ОБЩИЕ ВЫВОДЫ")
    print("="*80)
    print("""
1. Метод Гаусса (покоординатный спуск):
   + Прост в реализации
   + Гарантирует сходимость для выпуклых функций
   - Медленная сходимость для овражных функций
   - Зависимость от выбора начальной точки

2. Метод Хука-Дживса:
   + Эффективнее метода Гаусса за счет поиска по образцу
   + Адаптивный выбор шага
   + Хорошая сходимость на гладких функциях
   - Может требовать настройки параметров

3. Метод Розенброка:
   + Самый эффективный из трех методов
   + Адаптивное обновление направлений поиска
   + Хорошо справляется с овражными функциями
   - Более сложная реализация

Рекомендации:
- Для простых функций можно использовать любой метод
- Для сложных овражных функций предпочтительнее метод Розенброка
- Выбор точности одномерного поиска существенно влияет на сходимость
    """)


if __name__ == "__main__":
    main()
