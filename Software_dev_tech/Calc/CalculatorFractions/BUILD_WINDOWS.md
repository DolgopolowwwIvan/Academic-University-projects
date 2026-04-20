# Инструкция по сборке для Windows

## Вариант 1: C++ Builder (рекомендуется)

### Требования
- Embarcadero C++ Builder 2022 или новее
- Windows 10/11

### Шаги сборки

1. **Создайте новый проект VCL Forms Application**
   - File → New → Other → C++ Builder Projects → VCL Forms Application

2. **Добавьте файлы исходного кода в проект**
   - Project → Add to Project...
   - Выберите все файлы из папки `src/`:
     - UFrac.h, UFrac.cpp
     - UEditor.h, UEditor.cpp
     - UMemory.h
     - UProc.h
     - UControl.h, UControl.cpp
     - UClcPnl.h, UClcPnl.cpp

3. **Настройте форму**
   - Откройте файл `UClcPnl.dfm` в дизайнере форм
   - Или перетащите компоненты вручную согласно описанию ниже

4. **Компоненты формы**

   **TStaticText (2 шт.):**
   - `DisplayLabel` - отображение текущей дроби (большой, справа)
   - `MemoryStatusLabel` - состояние памяти (M или пусто)

   **TBitButton (28 шт.):**
   
   | Имя | Caption | Назначение |
   |-----|---------|------------|
   | BtnMC | MC | Очистить память |
   | BtnMR | MR | Восстановить из памяти |
   | BtnMPlus | M+ | Добавить к памяти |
   | BtnMS | MS | Сохранить в память |
   | BtnC | C | Сброс калькулятора |
   | BtnCE | CE | Очистить текущий ввод |
   | BtnBackspace | <- | Удалить символ |
   | BtnDiv | / | Деление |
   | Btn7-9, 4-6, 1-3, 0 | 0-9 | Цифры |
   | BtnMul | * | Умножение |
   | BtnSub | - | Вычитание |
   | BtnAdd | + | Сложение |
   | BtnEqual | = | Равно |
   | BtnSqr | Sqr | Квадрат |
   | BtnRev | Rev | Обратное |
   | BtnSign | +/- | Смена знака |
   | BtnSeparator | / | Разделитель дроби |

   **TMainMenu:**
   - Правка → Копировать, Вставить
   - Вид → Дробь, Число
   - Справка → О программе

5. **Подключите обработчики событий**
   - `FormCreate` → `FormCreate`
   - `FormKeyPress` → `FormKeyPress`
   - `OnClick` для всех кнопок → `ButtonClick`
   - Команды меню → соответствующие обработчики

6. **Скомпилируйте проект**
   - Project → Build или Ctrl+F9

## Вариант 2: Visual Studio 2022 (консольная версия)

### Требования
- Visual Studio 2022
- MSVC с поддержкой C++17

### Шаги сборки

1. **Создайте новый проект**
   - File → New → Project → Console App
   - Language: C++

2. **Добавьте файлы исходного кода**
   - Скопируйте файлы из `src/` в папку проекта
   - Добавьте их в проект (Solution Explorer → Add → Existing Item)

3. **Настройте стандарт C++**
   - Project Properties → C/C++ → Language
   - C++ Language Standard: ISO C++17 Standard

4. **Скомпилируйте**
   - Build → Build Solution или Ctrl+Shift+B

## Вариант 3: Qt (кроссплатформенная GUI версия)

Для создания GUI версии на Qt потребуется дополнительная работа по адаптации интерфейса.

## Структура проекта

```
CalculatorFractions/
├── src/
│   ├── UFrac.h           # Класс TFrac - простая дробь
│   ├── UFrac.cpp
│   ├── UEditor.h         # Класс TEditor - ввод/редактирование
│   ├── UEditor.cpp
│   ├── UMemory.h         # Шаблон TMemory - память
│   ├── UProc.h           # Шаблон TProc - процессор
│   ├── UControl.h        # Класс TCtrl - управление
│   ├── UControl.cpp
│   ├── UClcPnl.h         # Класс TClcPnl - интерфейс
│   ├── UClcPnl.cpp
│   ├── UClcPnl.dfm       # Форма для C++ Builder
│   └── main.cpp          # Главный файл с тестами
├── CMakeLists.txt        # Для CMake (Linux)
├── README.md             # Основная документация
└── BUILD_WINDOWS.md      # Эта инструкция
```

## Тестирование

После сборки запустите программу. Консольная версия автоматически выполнит тесты.

### Проверка основных функций

1. **Простая операция:** 5/1 + 2/1 = 7/1
2. **Функция Sqr:** 5/1 Sqr = 25/1
3. **Функция Rev:** 4/1 Rev = 1/4
4. **Память:** 
   - Введите 5/1, нажмите MS
   - Введите 3/1, нажмите +
   - Нажмите MR, должно быть 5/1
   - Нажмите =, результат 8/1
5. **Сложное выражение:** 6/1 Sqr + 2/1 Sqr / 10/1 + 6/1 = 10/1

## Горячие клавиши

| Клавиша | Действие |
|---------|----------|
| 0-9 | Ввод цифр |
| / или | | Разделитель дроби |
| + - * / | Операции |
| = или Enter | Равно |
| Escape или C | Сброс |
| Backspace | Удалить символ |
| S | Sqr (квадрат) |
| R | Rev (обратное) |
| Ctrl+C | Копировать |
| Ctrl+V | Вставить |
