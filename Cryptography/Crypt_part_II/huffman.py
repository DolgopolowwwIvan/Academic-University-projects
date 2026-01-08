import tkinter as tk
from tkinter import ttk, messagebox, filedialog, scrolledtext
import math
import heapq
import random

# Константы приложения
DEFAULT_WINDOW_WIDTH = 900
DEFAULT_WINDOW_HEIGHT = 700
DEFAULT_SEQUENCE_LENGTH = 1000
ALPHABET_SYMBOLS = ['A', 'B', 'C', 'D', 'E', 'F', '1', '2']
PROBABILITY_P1_VALUES = {
    'A': 0.9, 'B': 0.01, 'C': 0.01, 'D': 0.02, 
    'E': 0.05, 'F': 0.01, '1': 0.0, '2': 0.0
}
PROBABILITY_P2_VALUES = {
    'A': 2**(-5), 'B': 2**(-1), 'C': 2**(-2), 'D': 2**(-3),
    'E': 2**(-4), 'F': 2**(-5), '1': 0.0, '2': 0.0
}
UNKNOWN_SYMBOL_REPLACEMENT = '?'
VALID_BITS = {'0', '1'}

class HuffmanNode:
    """
    Узел дерева Хаффмана.
    
    symbol: символ, связанный с узлом (None для внутренних узлов)
    probability: вероятность символа или суммарная вероятность поддерева
    left: левый дочерний узел
    right: правый дочерний узел
    code: кодовое слово для символа
    """
    
    def __init__(self, symbol=None, probability=0):
        self.symbol = symbol
        self.probability = probability
        self.left = None
        self.right = None
        self.code = ""
    
    def __lt__(self, other):
        """Сравнение узлов по вероятности для работы с кучей."""
        return self.probability < other.probability


class HuffmanEncoder:
    """
    Кодирования текста.

    alphabet: список символов алфавита
    probabilities_p1: распределение вероятностей P1
    probabilities_p2: распределение вероятностей P2  
    current_probabilities: текущее используемое распределение вероятностей
    codes: словарь кодовых слов для символов
    """
    
    def __init__(self):
        self.alphabet = ALPHABET_SYMBOLS
        self.probabilities_p1 = PROBABILITY_P1_VALUES
        self.probabilities_p2 = PROBABILITY_P2_VALUES
        self.current_probabilities = self.probabilities_p1
        self.codes = {}
    
    def set_probabilities(self, prob_set):
        """
        Устанавливает распределение вероятностей для кодирования.
        
        Args:
            prob_set: строка "P1" или "P2" для выбора распределения
        """
        if prob_set == "P1":
            self.current_probabilities = self.probabilities_p1
        else:
            self.current_probabilities = self.probabilities_p2
    
    def set_custom_probabilities(self, probabilities):
        """
        Устанавливает пользовательское распределение вероятностей.
        
        Args:
            probabilities: словарь с вероятностями символов
        """
        self.current_probabilities = probabilities

    def get_uniform_probabilities(self):
        """
        Создает равномерное распределение вероятностей.
        
        Returns:
            Словарь с равномерными вероятностями для символов алфавита
        """
        symbols = ['A', 'B', 'C', 'D', 'E', 'F']
        uniform_prob = 1.0 / len(symbols)
        probabilities = {sym: uniform_prob for sym in symbols}
        probabilities['1'] = 0.0
        probabilities['2'] = 0.0
        return probabilities

    def load_probabilities_from_file(self, filename):
        """
        Загружает вероятности символов из файла.
        
        Args:
            filename: путь к файлу с вероятностями
            
        Returns:
            Кортеж (успех, сообщение)
        """
        try:
            with open(filename, 'r', encoding='utf-8') as file:
                content = file.read().strip()
                probabilities = {}
                for line in content.split('\n'):
                    if ':' in line:
                        symbol, prob_str = line.split(':', 1)
                        symbol = symbol.strip()
                        prob = float(prob_str.strip())
                        probabilities[symbol] = prob
                self.current_probabilities = probabilities
                return True, "Вероятности загружены из файла"
        except FileNotFoundError:
            return False, "Файл не найден"
        except PermissionError:
            return False, "Нет прав доступа к файлу"
        except ValueError as e:
            return False, f"Некорректный формат данных в файле: {str(e)}"
        except Exception as e:
            return False, f"Не удалось загрузить вероятности: {str(e)}"
    
    def build_huffman_tree(self, probabilities):
        """
        Строит дерево Хаффмана на основе вероятностей символов.
        
        Алгоритм:
        1. Создает узлы для каждого символа с положительной вероятностью
        2. Объединяет два узла с наименьшими вероятностями в новый узел
        3. Повторяет шаг 2, пока не останется один корневой узел
        
        Args:
            probabilities: словарь {символ: вероятность}
            
        Returns:
            Корень построенного дерева Хаффмана или None если дерево не может быть построено
        """
        nodes = []
        for symbol, prob in probabilities.items():
            if prob > 0:
                nodes.append(HuffmanNode(symbol, prob))
        
        if not nodes:
            return None
            
        heapq.heapify(nodes)
        
        while len(nodes) > 1:
            left = heapq.heappop(nodes)
            right = heapq.heappop(nodes)
            
            merged = HuffmanNode(probability=left.probability + right.probability)
            merged.left = left
            merged.right = right
            
            heapq.heappush(nodes, merged)
        
        return nodes[0] if nodes else None
    
    def generate_codes(self, node, current_code=""):
        """
        Рекурсивно генерирует кодовые слова для символов дерева Хаффмана.
        
        Args:
            node: текущий узел дерева
            current_code: текущее накопленное кодовое слово
        """
        if node is None:
            return
        
        if node.symbol is not None:
            node.code = current_code
            self.codes[node.symbol] = current_code
            return
        
        self.generate_codes(node.left, current_code + "0")
        self.generate_codes(node.right, current_code + "1")
    
    def calculate_entropy(self, probabilities):
        """
        Вычисляет энтропию источника информации.
        
        Формула: H = -Σ p_i * log2(p_i)
        
        Args:
            probabilities: распределение вероятностей символов
            
        Returns:
            Значение энтропии в битах
        """
        entropy = 0.0
        for prob in probabilities.values():
            if prob > 0:
                entropy -= prob * math.log2(prob)
        return entropy
    
    def calculate_average_length(self, probabilities, codes):
        """
        Вычисляет среднюю длину кодового слова.
        
        Формула: L_avg = Σ p_i * l_i
        
        Args:
            probabilities: распределение вероятностей символов
            codes: словарь кодовых слов
            
        Returns:
            Средняя длина кодового слова в битах
        """
        avg_length = 0.0
        for symbol, prob in probabilities.items():
            if prob > 0 and symbol in codes:
                avg_length += prob * len(codes[symbol])
        return avg_length
    
    def check_kraft_inequality(self, codes):
        """
        Проверяет выполнение неравенства Крафта.
        
        Неравенство Крафта: Σ 2^(-l_i) ≤ 1
        
        Args:
            codes: словарь кодовых слов
            
        Returns:
            Кортеж (сумма Крафта, выполняется ли неравенство)
        """
        kraft_sum = 0.0
        for code in codes.values():
            kraft_sum += 2**(-len(code))
        return kraft_sum, kraft_sum <= 1.0
    
    def encode_text(self, text):
        """
        Кодирует текст с использованием текущего распределения вероятностей.
        
        Args:
            text: исходный текст для кодирования
            
        Returns:
            Словарь с результатами кодирования
            
        Raises:
            ValueError: если не удалось построить дерево Хаффмана
        """
        root = self.build_huffman_tree(self.current_probabilities)
        self.codes = {}
        
        if root is None:
            raise ValueError("Не удалось построить дерево Хаффмана. Проверьте распределение вероятностей.")
            
        self.generate_codes(root)
        
        encoded_chars = []
        unknown_symbols = set()
        
        for char in text:
            upper_char = char.upper()
            if upper_char in self.codes:
                encoded_chars.append(self.codes[upper_char])
            else:
                encoded_chars.append(UNKNOWN_SYMBOL_REPLACEMENT)
                unknown_symbols.add(char)
        
        if unknown_symbols:
            print(f"Предупреждение: неизвестные символы заменены: {unknown_symbols}")
        
        encoded_text = ''.join(encoded_chars)
        
        entropy = self.calculate_entropy(self.current_probabilities)
        avg_length = self.calculate_average_length(self.current_probabilities, self.codes)
        redundancy = avg_length - entropy
        kraft_sum, kraft_ok = self.check_kraft_inequality(self.codes)
        
        results = {
            'codes': self.codes.copy(),
            'encoded_text': encoded_text,
            'entropy': entropy,
            'average_length': avg_length,
            'redundancy': redundancy,
            'kraft_sum': kraft_sum,
            'kraft_ok': kraft_ok
        }
        
        return results


class Node:
    """
    Упрощенный узел для декодера Хаффмана.
    
    Attributes:
        symbol: символ узла
        prob: вероятность символа
        left: левый дочерний узел  
        right: правый дочерний узел
    """
    
    def __init__(self, symbol, prob, left=None, right=None):
        self.symbol = symbol
        self.prob = prob
        self.left = left
        self.right = right

    def __lt__(self, other):
        """Сравнение узлов по вероятности."""
        return self.prob < other.prob


class HuffmanDecoder:
    """
    Декодер Хаффмана для раскодирования текста.
    """
    
    def __init__(self):
        pass
    
    def build_huffman_tree(self, probabilities):
        """
        Строит дерево Хаффмана для декодирования.
        
        Args:
            probabilities: распределение вероятностей символов
            
        Returns:
            Корень дерева Хаффмана или None если дерево не может быть построено
        """
        heap = [Node(sym, prob) for sym, prob in probabilities.items() if prob > 0]
        
        if not heap:
            return None
            
        heapq.heapify(heap)

        while len(heap) > 1:
            n1 = heapq.heappop(heap)
            n2 = heapq.heappop(heap)
            merged = Node(None, n1.prob + n2.prob, n1, n2)
            heapq.heappush(heap, merged)

        return heap[0] if heap else None
    
    def decode(self, encoded_str, root):
        """
        Декодирует битовую последовательность с использованием дерева Хаффмана.
        
        Args:
            encoded_str: закодированная битовая последовательность
            root: корень дерева Хаффмана
            
        Returns:
            Раскодированная строка
            
        Raises:
            ValueError: если закодированная последовательность некорректна
        """
        if root is None:
            raise ValueError("Дерево Хаффмана не построено")
            
        decoded = []
        node = root
        
        for i, bit in enumerate(encoded_str):
            if bit not in VALID_BITS:
                raise ValueError(f"Некорректный бит '{bit}' в позиции {i}")
                
            node = node.left if bit == "0" else node.right
            
            if node is None:
                raise ValueError(f"Некорректная закодированная последовательность на позиции {i}")
                
            if node.symbol is not None:
                decoded.append(node.symbol)
                node = root
        
        if node != root:
            raise ValueError("Закодированная последовательность неполная")
            
        return "".join(decoded)
    
    def load_probabilities_from_file(self, filename):
        """
        Загружает вероятности из файла.
        
        Args:
            filename: путь к файлу с вероятностями
            
        Returns:
            Кортеж (успех, вероятности, сообщение)
        """
        try:
            probabilities = {}
            with open(filename, "r", encoding="utf-8") as f:
                for line_num, line in enumerate(f, 1):
                    line = line.strip()
                    if not line:
                        continue
                        
                    parts = line.split()
                    if len(parts) == 2:
                        symbol, prob = parts
                        probabilities[symbol] = float(prob)
                    elif ':' in line:
                        symbol, prob_str = line.split(':', 1)
                        symbol = symbol.strip()
                        prob = float(prob_str.strip())
                        probabilities[symbol] = prob
                    else:
                        return False, {}, f"Некорректный формат строки {line_num}: {line}"
                        
            return True, probabilities, "Вероятности загружены из файла"
        except FileNotFoundError:
            return False, {}, "Файл не найден"
        except PermissionError:
            return False, {}, "Нет прав доступа к файлу"
        except ValueError as e:
            return False, {}, f"Некорректные числовые данные в файле: {str(e)}"
        except Exception as e:
            return False, {}, f"Не удалось загрузить вероятности: {str(e)}"
    
    def decode_file(self, prob_filename, encoded_filename):
        """
        Декодирует файл с закодированной последовательностью.
        
        Args:
            prob_filename: файл с вероятностями
            encoded_filename: файл с закодированной последовательностью
            
        Returns:
            Кортеж (успех, раскодированный текст, сообщение)
        """
        success, probabilities, message = self.load_probabilities_from_file(prob_filename)
        if not success:
            return False, None, message
        
        try:
            with open(encoded_filename, "r", encoding="utf-8") as f:
                encoded_str = f.read().strip()
        except FileNotFoundError:
            return False, None, "Файл с закодированным текстом не найден"
        except PermissionError:
            return False, None, "Нет прав доступа к файлу с закодированным текстом"
        except Exception as e:
            return False, None, f"Не удалось загрузить закодированный текст: {str(e)}"
        
        if not encoded_str:
            return False, None, "Файл с закодированным текстом пуст"
        
        try:
            root = self.build_huffman_tree(probabilities)
            if root is None:
                return False, None, "Не удалось построить дерево Хаффмана из загруженных вероятностей"
                
            decoded_str = self.decode(encoded_str, root)
            return True, decoded_str, "Текст успешно декодирован"
        except ValueError as e:
            return False, None, f"Ошибка декодирования: {str(e)}"
        except Exception as e:
            return False, None, f"Неожиданная ошибка при декодировании: {str(e)}"


class ResearchModule:
    """
    Модуль для проведения исследовательских экспериментов.
    """
    
    def __init__(self):
        self.encoder = HuffmanEncoder()
    
    def generate_sequence(self, distribution, length=DEFAULT_SEQUENCE_LENGTH):
        """
        Генерирует последовательность символов по заданному распределению.
        
        Args:
            distribution: тип распределения ("uniform", "P1", "P2")
            length: длина генерируемой последовательности
            
        Returns:
            Сгенерированная последовательность символов
            
        Raises:
            ValueError: если указано неизвестное распределение
        """
        symbols = []
        probs = []
        
        if distribution == "uniform":
            prob_dict = self.encoder.get_uniform_probabilities()
        elif distribution == "P1":
            prob_dict = self.encoder.probabilities_p1
        elif distribution == "P2":
            prob_dict = self.encoder.probabilities_p2
        else:
            raise ValueError(f"Неизвестное распределение: {distribution}")
        
        for sym, prob in prob_dict.items():
            if prob > 0:
                symbols.append(sym)
                probs.append(prob)
        
        if not symbols:
            raise ValueError("Нет символов с положительной вероятностью")
        
        total_prob = sum(probs)
        normalized_probs = [p / total_prob for p in probs]
        
        sequence = random.choices(symbols, weights=normalized_probs, k=length)
        return ''.join(sequence)
    
    def research_all_distributions(self):
        """
        Проводит исследование для всех распределений (пункт 3 задания).
        
        Returns:
            Словарь с результатами для каждого распределения
        """
        results = {}
        
        try:
            self.encoder.current_probabilities = self.encoder.get_uniform_probabilities()
            uniform_results = self.encoder.encode_text("")
            results['uniform'] = {
                'codes': uniform_results['codes'],
                'average_length': uniform_results['average_length'],
                'redundancy': uniform_results['redundancy'],
                'kraft_ok': uniform_results['kraft_ok'],
                'kraft_sum': uniform_results['kraft_sum']
            }
        except Exception as e:
            results['uniform'] = {'error': str(e)}
        
        try:
            self.encoder.set_probabilities("P1")
            p1_results = self.encoder.encode_text("")
            results['P1'] = {
                'codes': p1_results['codes'],
                'average_length': p1_results['average_length'],
                'redundancy': p1_results['redundancy'],
                'kraft_ok': p1_results['kraft_ok'],
                'kraft_sum': p1_results['kraft_sum']
            }
        except Exception as e:
            results['P1'] = {'error': str(e)}
        
        try:
            self.encoder.set_probabilities("P2")
            p2_results = self.encoder.encode_text("")
            results['P2'] = {
                'codes': p2_results['codes'],
                'average_length': p2_results['average_length'],
                'redundancy': p2_results['redundancy'],
                'kraft_ok': p2_results['kraft_ok'],
                'kraft_sum': p2_results['kraft_sum']
            }
        except Exception as e:
            results['P2'] = {'error': str(e)}
        
        return results
    
    def research_encoding_lengths(self, sequence_length=DEFAULT_SEQUENCE_LENGTH):
        """
        Исследует зависимость длины кода от распределения (пункт 4 задания).
        
        Args:
            sequence_length: длина тестовых последовательностей
            
        Returns:
            Словарь с длинами закодированных последовательностей
        """
        results = {}
        
        for source_dist in ["uniform", "P1", "P2"]:
            try:
                sequence = self.generate_sequence(source_dist, sequence_length)
                results[source_dist] = {}
                
                for encoding_dist in ["uniform", "P1", "P2"]:
                    try:
                        if encoding_dist == "uniform":
                            self.encoder.current_probabilities = self.encoder.get_uniform_probabilities()
                        else:
                            self.encoder.set_probabilities(encoding_dist)
                        
                        encoding_results = self.encoder.encode_text(sequence)
                        results[source_dist][encoding_dist] = len(encoding_results['encoded_text'])
                    except Exception as e:
                        results[source_dist][encoding_dist] = f"Ошибка: {str(e)}"
                        
            except Exception as e:
                results[source_dist] = {'error': str(e)}
        
        return results
    
    def format_research_results(self, research_results):
        """
        Форматирует результаты исследования распределений для вывода.
        
        Args:
            research_results: результаты исследования
            
        Returns:
            Отформатированная строка с результатами
        """
        output = "РЕЗУЛЬТАТЫ ИССЛЕДОВАНИЯ\n" + "="*50 + "\n\n"
        
        output += "3. АНАЛИЗ РАСПРЕДЕЛЕНИЙ ВЕРОЯТНОСТЕЙ:\n" + "-"*40 + "\n"
        
        for dist_name, results in research_results.items():
            output += f"\n{dist_name.upper()} распределение:\n"
            
            if 'error' in results:
                output += f"  ОШИБКА: {results['error']}\n"
                continue
                
            output += f"  Кодовые слова:\n"
            for sym, code in sorted(results['codes'].items()):
                if results['codes'][sym]:
                    output += f"    {sym}: {code}\n"
            output += f"  Средняя длина: {results['average_length']:.4f} бит\n"
            output += f"  Избыточность: {results['redundancy']:.4f} бит\n"
            output += f"  Сумма Крафта: {results['kraft_sum']:.4f}\n"
            kraft_status = "выполняется" if results['kraft_ok'] else "НЕ выполняется"
            output += f"  Неравенство Крафта: {kraft_status}\n"
        
        return output
    
    def format_length_results(self, length_results):
        """
        Форматирует результаты исследования длин для вывода.
        
        Args:
            length_results: результаты исследования длин
            
        Returns:
            Отформатированная строка с результатами
        """
        output = "\n\n4. ИССЛЕДОВАНИЕ ДЛИН ЗАКОДИРОВАННЫХ ПОСЛЕДОВАТЕЛЬНОСТЕЙ:\n" + "-"*60 + "\n\n"
        
        output += "Распределение исходной последовательности → | "
        for encoding_dist in ["uniform", "P1", "P2"]:
            output += f"{encoding_dist:^15} | "
        output += "\n" + "-"*80 + "\n"
        
        for source_dist, encodings in length_results.items():
            if 'error' in encodings:
                output += f"{source_dist:^43} | "
                for _ in range(3):
                    output += f"{'ОШИБКА':^15} | "
                output += "\n"
                continue
                
            output += f"{source_dist:^43} | "
            for encoding_dist in ["uniform", "P1", "P2"]:
                length = encodings.get(encoding_dist, "N/A")
                output += f"{str(length):^15} | "
            output += "\n"

        return output


class HuffmanApp:
    """
    Основной класс приложения с графическим интерфейсом.
    """
    
    def __init__(self, root):
        self.root = root
        self.root.title("Приложение для кодирования Хаффмана")
        self.root.geometry(f'{DEFAULT_WINDOW_WIDTH}x{DEFAULT_WINDOW_HEIGHT}')
        
        self.encoder = HuffmanEncoder()
        self.decoder = HuffmanDecoder()
        self.researcher = ResearchModule()
        
        self.setup_ui()
    
    def setup_ui(self):
        """Настраивает графический интерфейс приложения."""
        notebook = ttk.Notebook(self.root)
        notebook.pack(fill='both', expand=True, padx=10, pady=10)
        
        encode_frame = ttk.Frame(notebook, padding="10")
        notebook.add(encode_frame, text="Кодирование")
        
        decode_frame = ttk.Frame(notebook, padding="10")
        notebook.add(decode_frame, text="Декодирование")
        
        research_frame = ttk.Frame(notebook, padding="10")
        notebook.add(research_frame, text="Исследования")
        
        self.setup_encode_tab(encode_frame)
        self.setup_decode_tab(decode_frame)
        self.setup_research_tab(research_frame)
    
    def setup_encode_tab(self, parent):
        """Настраивает вкладку кодирования."""
        prob_frame = ttk.LabelFrame(parent, text="Выбор вероятностей", padding="5")
        prob_frame.pack(fill='x', pady=5)
        
        self.prob_var = tk.StringVar(value="P1")
        ttk.Radiobutton(prob_frame, text="P1: {0.9, 0.01, 0.01, 0.02, 0.05, 0.01, 0, 0}", 
                       variable=self.prob_var, value="P1").grid(row=0, column=0, sticky=tk.W)
        ttk.Radiobutton(prob_frame, text="P2: {2^(-5), 2^(-1), 2^(-2), 2^(-3), 2^(-4), 2^(-5), 0, 0}", 
                       variable=self.prob_var, value="P2").grid(row=1, column=0, sticky=tk.W)
        
        file_frame = ttk.LabelFrame(parent, text="Работа с файлами", padding="5")
        file_frame.pack(fill='x', pady=5)
        
        ttk.Button(file_frame, text="Загрузить вероятности из файла", 
                  command=self.load_encode_probabilities).grid(row=0, column=0, padx=5, pady=2)
        ttk.Button(file_frame, text="Загрузить текст для кодирования", 
                  command=self.load_text_to_encode).grid(row=0, column=1, padx=5, pady=2)
        ttk.Button(file_frame, text="Выполнить кодирование", 
                  command=self.encode_text).grid(row=1, column=0, padx=5, pady=2)
        ttk.Button(file_frame, text="Сохранить закодированный текст", 
                  command=self.save_encoded_text).grid(row=1, column=1, padx=5, pady=2)
        
        text_frame = ttk.LabelFrame(parent, text="Текст для кодирования", padding="5")
        text_frame.pack(fill='both', expand=True, pady=5)
        
        self.encode_input_text = scrolledtext.ScrolledText(text_frame, height=6, wrap=tk.WORD)
        self.encode_input_text.pack(fill='both', expand=True)
        
        results_frame = ttk.LabelFrame(parent, text="Результаты кодирования", padding="5")
        results_frame.pack(fill='both', expand=True, pady=5)
        
        results_notebook = ttk.Notebook(results_frame)
        results_notebook.pack(fill='both', expand=True)
        
        codes_frame = ttk.Frame(results_notebook, padding="5")
        results_notebook.add(codes_frame, text="Кодовые слова")
        
        self.codes_text = scrolledtext.ScrolledText(codes_frame, wrap=tk.WORD)
        self.codes_text.pack(fill='both', expand=True)
        
        calc_frame = ttk.Frame(results_notebook, padding="5")
        results_notebook.add(calc_frame, text="Вычисления")
        
        self.calc_text = scrolledtext.ScrolledText(calc_frame, wrap=tk.WORD)
        self.calc_text.pack(fill='both', expand=True)
        
        encoded_frame = ttk.Frame(results_notebook, padding="5")
        results_notebook.add(encoded_frame, text="Закодированный текст")
        
        self.encoded_text = scrolledtext.ScrolledText(encoded_frame, wrap=tk.WORD)
        self.encoded_text.pack(fill='both', expand=True)
    
    def setup_decode_tab(self, parent):
        """Настраивает вкладку декодирования."""
        file_frame = ttk.LabelFrame(parent, text="Работа с файлами", padding="5")
        file_frame.pack(fill='x', pady=5)
        
        ttk.Button(file_frame, text="Загрузить файл с вероятностями", 
                  command=self.load_decode_probabilities).grid(row=0, column=0, padx=5, pady=2)
        ttk.Button(file_frame, text="Загрузить закодированный файл", 
                  command=self.load_encoded_file).grid(row=0, column=1, padx=5, pady=2)
        ttk.Button(file_frame, text="Выполнить декодирование", 
                  command=self.decode_text).grid(row=1, column=0, padx=5, pady=2)
        ttk.Button(file_frame, text="Сохранить декодированный текст", 
                  command=self.save_decoded_text).grid(row=1, column=1, padx=5, pady=2)
        
        decode_results_frame = ttk.LabelFrame(parent, text="Результаты декодирования", padding="5")
        decode_results_frame.pack(fill='both', expand=True, pady=5)
        
        self.decode_output_text = scrolledtext.ScrolledText(decode_results_frame, wrap=tk.WORD, height=15)
        self.decode_output_text.pack(fill='both', expand=True)
    
    def setup_research_tab(self, parent):
        """Настраивает вкладку исследований."""
        research_frame = ttk.LabelFrame(parent, text="Исследования", padding="5")
        research_frame.pack(fill='x', pady=5)
        
        ttk.Button(research_frame, text="Выполнить исследование распределений (п.3)", 
                  command=self.run_distribution_research).grid(row=0, column=0, padx=5, pady=2)
        
        ttk.Button(research_frame, text="Исследовать длины кодов (п.4)", 
                  command=self.run_length_research).grid(row=0, column=1, padx=5, pady=2)
        
        ttk.Label(research_frame, text="Длина последовательности:").grid(row=1, column=0, padx=5, pady=2)
        self.sequence_length_var = tk.StringVar(value=str(DEFAULT_SEQUENCE_LENGTH))
        ttk.Entry(research_frame, textvariable=self.sequence_length_var, width=10).grid(row=1, column=1, padx=5, pady=2)
        
        results_frame = ttk.LabelFrame(parent, text="Результаты исследований", padding="5")
        results_frame.pack(fill='both', expand=True, pady=5)
        
        self.research_text = scrolledtext.ScrolledText(results_frame, wrap=tk.WORD, height=20)
        self.research_text.pack(fill='both', expand=True)
    
    def load_encode_probabilities(self):
        """Загружает вероятности из файла для кодирования."""
        filename = filedialog.askopenfilename(
            title="Выберите файл с вероятностями",
            filetypes=[("Text Files", "*.txt"), ("All Files", "*.*")]
        )
        if filename:
            success, message = self.encoder.load_probabilities_from_file(filename)
            if success:
                messagebox.showinfo("Успех", message)
            else:
                messagebox.showerror("Ошибка", message)
    
    def load_text_to_encode(self):
        """Загружает текст для кодирования из файла."""
        filename = filedialog.askopenfilename(
            title="Выберите файл с текстом для кодирования",
            filetypes=[("Text Files", "*.txt"), ("All Files", "*.*")]
        )
        if filename:
            try:
                with open(filename, 'r', encoding='utf-8') as file:
                    text = file.read().strip()
                self.encode_input_text.delete(1.0, tk.END)
                self.encode_input_text.insert(tk.END, text)
                messagebox.showinfo("Успех", "Текст загружен из файла")
            except FileNotFoundError:
                messagebox.showerror("Ошибка", "Файл не найден")
            except PermissionError:
                messagebox.showerror("Ошибка", "Нет прав доступа к файлу")
            except Exception as e:
                messagebox.showerror("Ошибка", f"Не удалось загрузить текст: {str(e)}")
    
    def encode_text(self):
        """Выполняет кодирование текста."""
        text = self.encode_input_text.get(1.0, tk.END).strip()
        if not text:
            messagebox.showwarning("Предупреждение", "Введите текст для кодирования")
            return
        
        self.encoder.set_probabilities(self.prob_var.get())
        
        try:
            results = self.encoder.encode_text(text)
            
            self.codes_text.delete(1.0, tk.END)
            codes_output = "Кодовые слова:\n"
            for symbol, code in sorted(results['codes'].items()):
                codes_output += f"  {symbol}: {code}\n"
            self.codes_text.insert(tk.END, codes_output)
            
            self.calc_text.delete(1.0, tk.END)
            calc_output = f"Энтропия: {results['entropy']:.4f} бит\n"
            calc_output += f"Средняя длина кодового слова: {results['average_length']:.4f} бит\n"
            calc_output += f"Избыточность: {results['redundancy']:.4f} бит\n"
            calc_output += f"Сумма Крафта: {results['kraft_sum']:.4f}\n"
            kraft_status = "Да" if results['kraft_ok'] else "Нет"
            calc_output += f"Неравенство Крафта выполняется: {kraft_status}\n"
            self.calc_text.insert(tk.END, calc_output)
            
            self.encoded_text.delete(1.0, tk.END)
            self.encoded_text.insert(tk.END, results['encoded_text'])
            
            self.encoded_result = results['encoded_text']
            
        except ValueError as e:
            messagebox.showerror("Ошибка кодирования", str(e))
        except Exception as e:
            messagebox.showerror("Ошибка", f"Неожиданная ошибка при кодировании: {str(e)}")
    
    def save_encoded_text(self):
        """Сохраняет закодированный текст в файл."""
        if hasattr(self, 'encoded_result'):
            filename = filedialog.asksaveasfilename(
                title="Сохранить закодированный текст",
                defaultextension=".txt",
                filetypes=[("Text Files", "*.txt"), ("All Files", "*.*")]
            )
            if filename:
                try:
                    with open(filename, 'w', encoding='utf-8') as file:
                        file.write(self.encoded_result)
                    messagebox.showinfo("Успех", "Закодированный текст сохранен")
                except Exception as e:
                    messagebox.showerror("Ошибка", f"Не удалось сохранить текст: {str(e)}")
        else:
            messagebox.showwarning("Предупреждение", "Сначала выполните кодирование текста")
    
    def load_decode_probabilities(self):
        """Загружает файл с вероятностями для декодирования."""
        self.decode_prob_file = filedialog.askopenfilename(
            title="Выберите файл с вероятностями",
            filetypes=[("Text Files", "*.txt"), ("All Files", "*.*")]
        )
        if self.decode_prob_file:
            messagebox.showinfo("Успех", f"Файл вероятностей загружен:\n{self.decode_prob_file}")
    
    def load_encoded_file(self):
        """Загружает файл с закодированной последовательностью."""
        self.encoded_file = filedialog.askopenfilename(
            title="Выберите закодированный файл",
            filetypes=[("Text Files", "*.txt"), ("All Files", "*.*")]
        )
        if self.encoded_file:
            messagebox.showinfo("Успех", f"Закодированный файл загружен:\n{self.encoded_file}")
    
    def decode_text(self):
        """Выполняет декодирование закодированной последовательности."""
        if not hasattr(self, 'decode_prob_file') or not hasattr(self, 'encoded_file'):
            messagebox.showerror("Ошибка", "Выберите оба файла!")
            return
        
        success, decoded_text, message = self.decoder.decode_file(
            self.decode_prob_file, 
            self.encoded_file
        )
        
        if success:
            self.decode_output_text.delete(1.0, tk.END)
            self.decode_output_text.insert(tk.END, decoded_text)
            self.decoded_result = decoded_text
            messagebox.showinfo("Успех", message)
        else:
            messagebox.showerror("Ошибка", message)
    
    def save_decoded_text(self):
        """Сохраняет декодированный текст в файл."""
        if hasattr(self, 'decoded_result'):
            filename = filedialog.asksaveasfilename(
                title="Сохранить декодированный текст",
                defaultextension=".txt",
                filetypes=[("Text Files", "*.txt"), ("All Files", "*.*")]
            )
            if filename:
                try:
                    with open(filename, 'w', encoding='utf-8') as file:
                        file.write(self.decoded_result)
                    messagebox.showinfo("Успех", "Декодированный текст сохранен")
                except Exception as e:
                    messagebox.showerror("Ошибка", f"Не удалось сохранить текст: {str(e)}")
        else:
            messagebox.showwarning("Предупреждение", "Сначала выполните декодирование")
    
    def run_distribution_research(self):
        """Выполняет исследование распределений (пункт 3)."""
        try:
            results = self.researcher.research_all_distributions()
            
            output = self.researcher.format_research_results(results)
            
            self.research_text.delete(1.0, tk.END)
            self.research_text.insert(tk.END, output)
            
            with open("research_results.txt", "w", encoding="utf-8") as f:
                f.write(output)
                
            messagebox.showinfo("Успех", "Исследование распределений завершено! Результаты сохранены в research_results.txt")
            
        except Exception as e:
            messagebox.showerror("Ошибка", f"Ошибка при исследовании: {str(e)}")
    
    def run_length_research(self):
        """Выполняет исследование длин кодов (пункт 4)."""
        try:
            length = int(self.sequence_length_var.get())
            if length <= 0:
                raise ValueError("Длина последовательности должна быть положительным числом")
                
            results = self.researcher.research_encoding_lengths(length)
            
            output = self.researcher.format_length_results(results)
            
            self.research_text.delete(1.0, tk.END)
            self.research_text.insert(tk.END, output)
            
            with open("length_research.txt", "w", encoding="utf-8") as f:
                f.write(output)
                
            messagebox.showinfo("Успех", "Исследование длин завершено! Результаты сохранены в length_research.txt")
            
        except ValueError as e:
            messagebox.showerror("Ошибка", f"Некорректная длина последовательности: {str(e)}")
        except Exception as e:
            messagebox.showerror("Ошибка", f"Ошибка при исследовании длин: {str(e)}")


def main():
    """Основная функция приложения."""
    root = tk.Tk()
    app = HuffmanApp(root)
    root.mainloop()


if __name__ == "__main__":
    main()