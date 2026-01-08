import matplotlib.pyplot as plt
import numpy as np
from scipy.stats import norm
import matplotlib

# Русские шрифты
matplotlib.rcParams['font.family'] = 'DejaVu Sans'

# Данные вашего варианта
n = 300
x_min, x_max = -5, 5
l = 10
n_i = [5, 13, 26, 46, 58, 60, 46, 27, 13, 6]
h = (x_max - x_min) / l

# Расчеты для гистограммы
p_i = [ni / n for ni in n_i]
f_i = [pi / h for pi in p_i]
bins = np.linspace(x_min, x_max, l + 1)

# Оценки параметров (метод моментов)
a_normal = 0.03783
sigma_normal = 1.92230
a_uniform = -3.29115
b_uniform = 3.36681

# Создание данных для гистограммы
data = []
for i, count in enumerate(n_i):
    data.extend(np.random.uniform(bins[i], bins[i+1], count))

# Построение графика
plt.figure(figsize=(14, 8))

# Гистограмма
plt.hist(data, bins=bins, density=True, alpha=0.7, color='lightblue', 
         edgecolor='black', linewidth=1.2, label='Эмпирическая плотность')

# Теоретическая нормальная плотность
x_normal = np.linspace(x_min, x_max, 1000)
y_normal = norm.pdf(x_normal, a_normal, sigma_normal)
plt.plot(x_normal, y_normal, 'r-', linewidth=2, label=f'Нормальное N({a_normal:.2f}, {sigma_normal:.2f}²)')

# Теоретическая равномерная плотность
x_uniform = np.linspace(a_uniform, b_uniform, 100)
y_uniform = np.full_like(x_uniform, 1/(b_uniform - a_uniform))
plt.plot(x_uniform, y_uniform, 'g-', linewidth=2, label=f'Равномерное U({a_uniform:.2f}, {b_uniform:.2f})')

# Настройки графика
plt.xlabel('x', fontsize=12)
plt.ylabel('Плотность вероятности f(x)', fontsize=12)
plt.title('Гистограмма с теоретическими плотностями распределений (Вариант 8)', fontsize=14)
plt.grid(True, alpha=0.3)
plt.legend()
plt.xlim(x_min, x_max)

# Добавление информации о параметрах
textstr = '\n'.join((
    f'Нормальное распределение:',
    f'  a = {a_normal:.4f}',
    f'  σ = {sigma_normal:.4f}',
    f'Равномерное распределение:',
    f'  a = {a_uniform:.4f}',
    f'  b = {b_uniform:.4f}'))

props = dict(boxstyle='round', facecolor='wheat', alpha=0.8)
plt.text(0.02, 0.98, textstr, transform=plt.gca().transAxes, fontsize=10,
         verticalalignment='top', bbox=props)

plt.tight_layout()
plt.show()

# Вывод параметров
print("ПАРАМЕТРЫ РАСПРЕДЕЛЕНИЙ (Метод моментов):")
print("="*50)
print(f"Нормальное распределение N(a, σ²):")
print(f"  a = {a_normal:.5f}")
print(f"  σ = {sigma_normal:.5f}")
print(f"  Плотность: f_N(x) = 1/({sigma_normal:.5f}·√(2π)) · exp(-(x-{a_normal:.5f})²/(2·{sigma_normal**2:.5f}))")
print()
print(f"Равномерное распределение U(a, b):")
print(f"  a = {a_uniform:.5f}")
print(f"  b = {b_uniform:.5f}")
print(f"  Плотность: f_U(x) = 1/({b_uniform - a_uniform:.5f}) = {1/(b_uniform - a_uniform):.5f}")
print(f"  Область определения: [{a_uniform:.5f}, {b_uniform:.5f}]")