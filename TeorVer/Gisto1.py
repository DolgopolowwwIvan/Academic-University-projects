import matplotlib.pyplot as plt
import numpy as np

# Данные вашего варианта
n = 300
l = 10
x_min = -5
x_max = 5
n_i = [5, 13, 26, 46, 58, 60, 46, 27, 13, 6]
alpha = 0.05

# Ширина интервала
h = (x_max - x_min) / l

# Расчет вероятностей и плотностей
p_i = [ni / n for ni in n_i]
f_i = [pi / h for pi in p_i]

# Границы интервалов
bins = [x_min + i * h for i in range(l + 1)]

# Данные для гистограммы (повторяем каждое значение n_i раз)
data = []
for i, count in enumerate(n_i):
    # Середина интервала
    mid_point = (bins[i] + bins[i+1]) / 2
    # Добавляем mid_point count раз
    data.extend([mid_point] * count)

# Построение гистограммы
plt.figure(figsize=(12, 8))

# Гистограмма частотной плотности
plt.hist(data, bins=bins, density=True, alpha=0.7, color='lightblue', 
         edgecolor='black', linewidth=1.2, label='Гистограмма частотной плотности')

# Настройки графика
plt.xlabel('x', fontsize=12)
plt.ylabel('$\\hat{f}(x)$', fontsize=12)
plt.title('Гистограмма частотной плотности (Вариант 8)', fontsize=14)
plt.grid(True, alpha=0.3)
plt.legend()

# Подписи на оси x
plt.xticks(bins, rotation=45)

# Добавление значений плотности на график
midpoints = [(bins[i] + bins[i+1]) / 2 for i in range(l)]
for i, (mid, density) in enumerate(zip(midpoints, f_i)):
    plt.text(mid, density + 0.005, f'{density:.4f}', 
             ha='center', va='bottom', fontsize=9)

plt.tight_layout()
plt.show()

# Вывод таблицы с данными
print("Таблица для гистограммы:")
print("i | Интервал        | n_i | p̂_i    | f̂_i")
print("-" * 55)
for i in range(l):
    interval = f"[{bins[i]:.1f}, {bins[i+1]:.1f})" if i < l-1 else f"[{bins[i]:.1f}, {bins[i+1]:.1f}]"
    print(f"{i+1:2} | {interval:14} | {n_i[i]:3} | {p_i[i]:.5f} | {f_i[i]:.5f}")

print("-" * 55)
print(f"{'Σ':2} | {'':14} | {sum(n_i):3} | {sum(p_i):.5f} | {sum(f_i):.5f}")

# Дополнительная информация
print(f"\nОбщее количество наблюдений: n = {n}")
print(f"Ширина интервала: h = {h}")
print(f"Уровень значимости: α = {alpha}")
print(f"Проверка площади гистограммы: Σ(f̂_i × h) = {sum(f_i) * h:.6f}")