import numpy as np
import math
import itertools
import tkinter as tk
from tkinter import ttk, filedialog, messagebox

# -------------------- Проверочная матрица H --------------------
H_orig = np.array([
    [0, 0, 0, 0, 0, 0, 0, 1, 1],
    [0, 0, 0, 1, 1, 1, 1, 0, 0],
    [0, 1, 1, 0, 0, 1, 1, 0, 0],
    [1, 0, 1, 0, 1, 0, 1, 0, 1]
], dtype=int)

H_work = H_orig.copy()
n = H_work.shape[1]
r = H_work.shape[0]
k = n - r  # 5

# -------------------- Вспомогательные функции для GF(2) --------------------
def mat2(A):
    return np.mod(np.array(A, dtype=int), 2)

def gf2_rank(A):
    A = mat2(A.copy())
    rows, cols = A.shape
    rank = 0
    for col in range(cols):
        pivot = None
        for row in range(rank, rows):
            if A[row, col] == 1:
                pivot = row
                break
        if pivot is None:
            continue
        if pivot != rank:
            A[[pivot, rank]] = A[[rank, pivot]]
        for rrr in range(rows):
            if rrr != rank and A[rrr, col] == 1:
                A[rrr] ^= A[rank]
        rank += 1
        if rank == rows:
            break
    return rank

def gf2_inv_square(A):
    A = mat2(A.copy())
    nA = A.shape[0]
    Aug = np.concatenate((A, np.eye(nA, dtype=int)), axis=1)
    row = 0
    for col in range(nA):
        piv = None
        for rrr in range(row, nA):
            if Aug[rrr, col] == 1:
                piv = rrr
                break
        if piv is None:
            continue
        if piv != row:
            Aug[[piv, row]] = Aug[[row, piv]]
        for rrr in range(nA):
            if rrr != row and Aug[rrr, col] == 1:
                Aug[rrr] ^= Aug[row]
        row += 1
    left = Aug[:, :nA]
    if not np.array_equal(left, np.eye(nA, dtype=int)):
        raise ValueError("Matrix not invertible over GF(2)")
    inv = Aug[:, nA:]
    return mat2(inv)

def nullspace_basis_gf2(H):
    Hm = mat2(H)
    ncols = Hm.shape[1]
    sols = []
    for i in range(2 ** ncols):
        v = np.array(list(map(int, format(i, f'0{ncols}b'))), dtype=int)
        if np.array_equal(mat2(Hm @ v), np.zeros(Hm.shape[0], dtype=int)):
            sols.append(v)
    basis = []
    for v in sols:
        if np.all(v == 0):
            continue
        if not basis:
            basis.append(v.copy())
        else:
            M = np.vstack(basis + [v])
            if gf2_rank(M) > gf2_rank(np.vstack(basis)):
                basis.append(v.copy())
        if len(basis) == n - Hm.shape[0]:
            break
    desired_k = n - gf2_rank(Hm)
    for v in sols:
        if len(basis) >= desired_k:
            break
        if np.all(v == 0):
            continue
        M = np.vstack(basis + [v])
        if gf2_rank(M) > gf2_rank(np.vstack(basis)) if basis else 1:
            basis.append(v.copy())
    if not basis:
        return np.zeros((0, n), dtype=int)
    return mat2(np.vstack(basis))

G_rows = nullspace_basis_gf2(H_work)
if G_rows.shape[0] != k:
    P_try = mat2(H_work[:, :k].T)
    G_rows = mat2(np.concatenate((np.eye(k, dtype=int), P_try), axis=1))

from itertools import combinations
cols_for_identity = None
for cols in combinations(range(n), k):
    sub = G_rows[:, list(cols)]
    if gf2_rank(sub) == k:
        cols_for_identity = list(cols)
        break
if cols_for_identity is None:
    raise RuntimeError("Не найдено k независимых столбцов в G_rows")

perm = cols_for_identity + [c for c in range(n) if c not in cols_for_identity]
G_perm = G_rows[:, perm]
H_perm = H_work[:, perm]
H_used = mat2(H_perm)

A = mat2(G_perm[:, :k])
A_inv = gf2_inv_square(A)
G_sys = mat2(A_inv @ G_perm)
G = G_sys.copy()

inv_perm = [0] * n
for i, orig_col in enumerate(perm):
    inv_perm[orig_col] = i
perm_array = np.array(perm, dtype=int)
inv_perm_array = np.array(inv_perm, dtype=int)

# -------------------- Фиксированные таблицы --------------------
number_to_bits_map = {i: [int(x) for x in f"{i:05b}"] for i in range(32)}
bits_to_number_map = {tuple(v): k for k, v in number_to_bits_map.items()}

def number_to_bits(num):
    return number_to_bits_map[num]

def bits_to_number(bits):
    return bits_to_number_map[tuple(bits)]

# -------------------- Кодирование / Декодирование --------------------
def encode_bits_stream(bits):
    bits = list(map(int, bits))
    encoded_orig = []
    codeword_blocks_orig = []
    for i in range(0, len(bits), k):
        block = bits[i:i + k]
        if len(block) < k:
            block = block + [0] * (k - len(block))
        block_arr = np.array(block, dtype=int)  # без реверса
        cw_sys = mat2(block_arr @ G)
        cw_orig = cw_sys[inv_perm_array].astype(int)
        encoded_orig.extend(cw_orig.tolist())
        codeword_blocks_orig.append(cw_orig.copy())
    return np.array(encoded_orig, dtype=int), codeword_blocks_orig

def syndrome_of_block_orig(block_orig):
    blk_orig = np.array(block_orig, dtype=int)
    blk_sys = blk_orig[perm_array]
    s = mat2(H_used @ blk_sys)
    return s

def decode_bits_stream(encoded_bits_orig):
    bits = list(map(int, encoded_bits_orig))
    decoded = []
    syndromes = []
    corrected_positions = []
    uncorrectable_blocks = []
    for block_idx in range(0, len(bits), n):
        blk_orig = bits[block_idx:block_idx + n]
        if len(blk_orig) < n:
            blk_orig = blk_orig + [0] * (n - len(blk_orig))
        blk_orig_arr = np.array(blk_orig, dtype=int)
        blk_sys = blk_orig_arr[perm_array].copy()
        s = mat2(H_used @ blk_sys).tolist()
        syndromes.append(s)
        if not any(s):
            info_bits = blk_sys[:k].tolist()
            decoded.extend(info_bits)
        else:
            found = None
            for j in range(n):
                if np.array_equal(H_used[:, j] % 2, np.array(s, dtype=int)):
                    found = j
                    break
            if found is not None:
                blk_sys[found] ^= 1
                orig_pos = perm_array[found]
                corrected_positions.append(block_idx + int(orig_pos))
                info_bits = blk_sys[:k].tolist()
                decoded.extend(info_bits)
            else:
                uncorrectable_blocks.append(block_idx // n)
                info_bits = blk_sys[:k].tolist()
                decoded.extend(info_bits)
    return decoded, syndromes, corrected_positions, uncorrectable_blocks

# -------------------- Исследование границ --------------------
def run_research():
    all_msgs = np.array(list(itertools.product([0, 1], repeat=k)), dtype=int)
    codewords_sys = mat2(all_msgs @ G)
    d_min = 3
    t = (d_min - 1) // 2
    res = f"Параметры кода: n = {n}, k = {k}\n"
    res += f"Минимальное кодовое расстояние d_min = {d_min}\n"
    res += f"Число исправляемых ошибок t = {t}\n\n"
    V = sum(math.comb(n, i) for i in range(t + 1))
    right = 2 ** n / V
    ok_h = (2 ** k <= right)
    r_min = math.ceil(math.log2(V))
    res += "--- Граница Хэмминга ---\n"
    res += f"Проверка: 2^k = {2 ** k} ≤ 2^n / ΣC(n,i) = {right:.6f}  --> {ok_h}\n"
    res += f"Минимально требуемое r: r_min = {r_min}\n\n"
    plot = n * (2 ** (k - 1)) / (2 ** k - 1)
    res += "--- Граница Плоткина ---\n"
    res += f"Проверка: d_min ≤ {plot:.6f}  --> {d_min <= plot}\n\n"
    right_vg = sum(math.comb(n - 1, i) for i in range(d_min - 1))
    left_vg = 2 ** (n - k)
    res += "--- Граница Варшамова–Гильберта ---\n"
    res += f"Проверка: 2^(n−k) = {left_vg} > ΣC(n−1,i) = {right_vg}  --> {left_vg > right_vg}\n"
    return res

# -------------------- GUI --------------------
class HammingApp(tk.Tk):
    def __init__(self):
        super().__init__()
        self.title("Кодирование Хэмминга")
        self.geometry("900x560")
        self.tab_control = ttk.Notebook(self)
        self.tab_encode = ttk.Frame(self.tab_control)
        self.tab_decode = ttk.Frame(self.tab_control)
        self.tab_research = ttk.Frame(self.tab_control)
        self.tab_control.add(self.tab_encode, text='Кодирование')
        self.tab_control.add(self.tab_decode, text='Декодирование')
        self.tab_control.add(self.tab_research, text='Исследования')
        self.tab_control.pack(expand=1, fill='both')
        self.create_encode_tab()
        self.create_decode_tab()
        self.create_research_tab()

    def create_encode_tab(self):
        tk.Label(self.tab_encode, text="Выберите файл с числами (0–31) для кодирования").pack(pady=8)
        tk.Button(self.tab_encode, text="Кодировать файл", command=self.encode_file).pack()
        self.encode_text = tk.Text(self.tab_encode, height=20, width=105)
        self.encode_text.pack(padx=10, pady=10)

    def encode_file(self):
        path = filedialog.askopenfilename(title="Выберите файл с числами (0–31)")
        if not path:
            return
        with open(path, 'r') as f:
            data = f.read().strip().replace(",", " ").split()
        try:
            nums = [int(x) for x in data]
            if any(n < 0 or n > 31 for n in nums):
                raise ValueError
        except ValueError:
            messagebox.showerror("Ошибка", "Файл должен содержать числа от 0 до 31.")
            return
        bits = []
        for n_val in nums:
            bits.extend(number_to_bits(n_val))
        encoded_bits_orig, codeword_blocks_orig = encode_bits_stream(bits)
        synds = [syndrome_of_block_orig(cw).tolist() for cw in codeword_blocks_orig]
        self.encode_text.delete(1.0, tk.END)
        self.encode_text.insert(tk.END, "Закодированное сообщение (биты):\n")
        self.encode_text.insert(tk.END, ''.join(map(str, encoded_bits_orig.tolist())) + "\n\n")
        self.encode_text.insert(tk.END, "Синдромы по блокам:\n")
        self.encode_text.insert(tk.END, str(synds) + "\n")
        out_path = path.rsplit(".", 1)[0] + "_encoded.txt"
        with open(out_path, 'w') as f:
            f.write(''.join(map(str, encoded_bits_orig.tolist())))
        messagebox.showinfo("Готово", f"Закодированное сообщение сохранено в:\n{out_path}")

    def create_decode_tab(self):
        tk.Label(self.tab_decode, text="Выберите файл с закодированными битами").pack(pady=8)
        tk.Button(self.tab_decode, text="Декодировать файл", command=self.decode_file).pack()
        self.decode_text = tk.Text(self.tab_decode, height=20, width=105)
        self.decode_text.pack(padx=10, pady=10)

    def decode_file(self):
        path = filedialog.askopenfilename(title="Выберите файл с закодированными битами")
        if not path:
            return
        with open(path, 'r') as f:
            data = f.read().strip().replace(" ", "").replace("\n", "")
        if any(ch not in "01" for ch in data):
            messagebox.showerror("Ошибка", "Файл должен содержать только 0 и 1.")
            return
        bits_list = [int(b) for b in data]
        decoded_bits, synds, corrected_positions, uncorrectable = decode_bits_stream(bits_list)
        nums = []
        for i in range(0, len(decoded_bits), k):
            grp = decoded_bits[i:i + k]
            if len(grp) == k:
                nums.append(bits_to_number(grp))
        self.decode_text.delete(1.0, tk.END)
        self.decode_text.insert(tk.END, f"Декодированные числа:\n{nums}\n\n")
        self.decode_text.insert(tk.END, f"Синдромы блоков:\n{synds}\n\n")
        self.decode_text.insert(tk.END, f"Исправленные позиции: {corrected_positions}\n\n")
        self.decode_text.insert(tk.END, f"Неисправимые блоки: {uncorrectable}\n")

    def create_research_tab(self):
        tk.Label(self.tab_research, text="Проверка параметров и границ кода").pack(pady=8)
        tk.Button(self.tab_research, text="Выполнить анализ", command=self.do_research).pack()
        self.research_text = tk.Text(self.tab_research, height=20, width=105)
        self.research_text.pack(padx=10, pady=10)

    def do_research(self):
        result = run_research()
        self.research_text.delete(1.0, tk.END)
        self.research_text.insert(tk.END, result)

if __name__ == "__main__":
    app = HammingApp()
    app.mainloop()


