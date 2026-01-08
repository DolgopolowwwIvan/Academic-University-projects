import tkinter as tk
from tkinter import ttk, filedialog, messagebox
import heapq, json, math
from itertools import combinations


#Кодирование по Хаффману


class Node:
    def __init__(self, char, freq):
        self.char = char
        self.freq = freq
        self.left = None
        self.right = None

    def __lt__(self, other):
        return self.freq < other.freq


def build_huffman_tree(text):
    freq = {}
    for ch in text:
        freq[ch] = freq.get(ch, 0) + 1

    heap = [Node(ch, f) for ch, f in freq.items()]
    heapq.heapify(heap)

    while len(heap) > 1:
        left = heapq.heappop(heap)
        right = heapq.heappop(heap)
        new = Node(None, left.freq + right.freq)
        new.left, new.right = left, right
        heapq.heappush(heap, new)

    return heap[0]


def build_codes(node, prefix="", code={}):
    if node:
        if node.char is not None:
            code[node.char] = prefix
        build_codes(node.left, prefix + "0", code)
        build_codes(node.right, prefix + "1", code)
    return code


def add_parity_bits(bits):
    result = []
    for i in range(0, len(bits), 8):
        block = bits[i:i+8]
        parity = str(block.count('1') % 2)
        result.append(block + parity)
    return ''.join(result)


def encode_text(text):
    tree = build_huffman_tree(text)
    codes = build_codes(tree)
    bits = ''.join(codes[ch] for ch in text)
    bits_with_parity = add_parity_bits(bits)
    return bits_with_parity, codes



#Декодирование

def check_and_fix_parity(bits):
    corrected = ""
    errors = []
    for i in range(0, len(bits), 9):
        block = bits[i:i+9]
        if not block:
            continue
        data, parity = block[:-1], block[-1]
        if data.count('1') % 2 != int(parity):
            errors.append(i // 9)
        corrected += data
    return corrected, errors


def decode_bits(bits, codes):
    reverse = {v: k for k, v in codes.items()}
    result = ""
    buff = ""
    for b in bits:
        buff += b
        if buff in reverse:
            result += reverse[buff]
            buff = ""
    return result

#Исследования
def hamming_distance(a, b):
    min_len = min(len(a), len(b))
    diff = sum(x != y for x, y in zip(a[:min_len], b[:min_len]))
    diff += abs(len(a) - len(b))
    return diff


def min_distance(codes):
    codewords = list(codes.values())
    if len(codewords) < 2:
        return 0
    dmin = float('inf')
    for a, b in combinations(codewords, 2):
        d = hamming_distance(a, b)
        dmin = min(dmin, d)
    return int(dmin if dmin != float('inf') else 0)



def comb(n, r):
    return math.comb(n, r)


def hamming_bound(n, d):
    t = (d - 1) // 2
    denom = sum(comb(n, i) for i in range(t + 1))
    return 2 ** n / denom


def plotkin_bound(n, k):
    return (n * 2 ** (k - 1)) / (2 ** k - 1)


def varshamov_gilbert_bound(n, k, d):
    lhs = 2 ** (n - k)
    rhs = sum(comb(n-1, i) for i in range(d - 2))
    return lhs > rhs, lhs, rhs


def analyze_code_bounds(codes):
    codewords = list(codes.values())
    n = max(len(cw) for cw in codewords)
    M = len(codewords)
    k = math.log2(M)
    dmin = min_distance(codes)

    result_lines = [
        f"Исследование кода Хаффмана:",
        f"Количество кодовых слов M = {M}",
        f"Длина максимального слова n = {n}",
        f"Минимальное кодовое расстояние d_min = {dmin}",
        "",
        f"Граница Хэмминга: M ≤ {hamming_bound(n, dmin):.2f} → {'Выполняется' if M <= hamming_bound(n, dmin) else 'Не выполняется'}",
        f"Граница Плоткина: d ≤ {plotkin_bound(n, k):.2f} → {'Выполняется' if dmin <= plotkin_bound(n, k) else 'Не выполняется'}",
    ]
    vg_ok, lhs, rhs = varshamov_gilbert_bound(n, int(k), dmin)
    result_lines.append(f"Граница Варшамова–Гильберта: {lhs:.2f} > {rhs:.2f} → {'Выполняется' if vg_ok else 'Не выполняется'}")

    return "\n".join(result_lines)


def analyze_code(codes):
    """Старый интерфейс для совместимости, я пока оставлю."""
    return analyze_code_bounds(codes)



#Интерфейс приложения


def do_encode():
    inp = filedialog.askopenfilename(title="Выберите входной файл")
    if not inp: return
    out = filedialog.asksaveasfilename(title="Сохранить закодированный файл как")
    if not out: return
    text = open(inp, "r", encoding="utf-8").read()
    encoded, codes = encode_text(text)
    with open(out, "w") as f:
        f.write(encoded)
    with open(out + ".codes", "w") as f:
        json.dump(codes, f)
    messagebox.showinfo("Кодирование", f"Файл закодирован!\nКоды сохранены в {out}.codes")

    # Показываем содержимое закодированного сообщения во вкладке Кодирование
    tab1_text.delete("1.0", tk.END)
    tab1_text.insert(tk.END, encoded)


def do_decode():
    inp = filedialog.askopenfilename(title="Выберите файл для декодирования")
    if not inp: return
    codes_file = filedialog.askopenfilename(title="Выберите файл кодов (.codes)")
    if not codes_file: return
    out = filedialog.asksaveasfilename(title="Сохранить результат как")
    if not out: return

    bits = open(inp, "r").read().strip()
    codes = json.load(open(codes_file))
    corrected_bits, errors = check_and_fix_parity(bits)
    text = decode_bits(corrected_bits, codes)
    with open(out, "w", encoding="utf-8") as f:
        f.write(text)
    messagebox.showinfo("Декодирование", f"Файл успешно декодирован.\nОшибки в блоках: {errors}")

    # Показываем содержимое декодированного сообщения во вкладке Декодирование
    tab2_text.delete("1.0", tk.END)
    tab2_text.insert(tk.END, text)


def do_analysis(text_widget):
    code_file = filedialog.askopenfilename(title="Выберите файл кодов (.codes)")
    if not code_file: return
    codes = json.load(open(code_file))
    result = analyze_code_bounds(codes)
    text_widget.delete("1.0", tk.END)
    text_widget.insert(tk.END, result)


# GUI setup
root = tk.Tk()
root.title("Кодирование Хаффмана с проверкой на чётность")

notebook = ttk.Notebook(root)
tab1 = ttk.Frame(notebook)
tab2 = ttk.Frame(notebook)
tab3 = ttk.Frame(notebook)

notebook.add(tab1, text="Кодирование")
notebook.add(tab2, text="Декодирование")
notebook.add(tab3, text="Исследования")
notebook.pack(expand=True, fill="both")

# Вкладка 1 — Кодирование
ttk.Label(tab1, text="Выберите файл и закодируйте сообщение").pack(pady=10)
ttk.Button(tab1, text="Кодировать файл", command=do_encode).pack(pady=10)

tab1_text = tk.Text(tab1, height=15, width=70)
tab1_text.pack(pady=10, padx=10)

# Вкладка 2 — Декодирование
ttk.Label(tab2, text="Выберите файл для декодирования").pack(pady=10)
ttk.Button(tab2, text="Декодировать файл", command=do_decode).pack(pady=10)

tab2_text = tk.Text(tab2, height=15, width=70)
tab2_text.pack(pady=10, padx=10)

# Вкладка 3 — Исследования
ttk.Label(tab3, text="Исследование кодов Хаффмана").pack(pady=10)
text_box = tk.Text(tab3, height=15, width=70)
text_box.pack(pady=10)
ttk.Button(tab3, text="Выполнить анализ", command=lambda: do_analysis(text_box)).pack(pady=5)

root.mainloop()