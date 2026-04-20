# Инструкция по сборке для Windows

## Вариант 1: Visual Studio 2022 (рекомендуется)

### Требования
- Visual Studio 2022 (Community, Professional или Enterprise)
- Рабочая нагрузка: ".NET Desktop Development"
- Windows 10/11

### Шаги сборки

1. **Откройте проект в Visual Studio**
   - Запустите Visual Studio 2022
   - File → Open → Project/Solution
   - Выберите файл `CalculatorFractions.csproj`

2. **Проверьте настройки проекта**
   - Правой кнопкой на проекте в Solution Explorer → Properties
   - Убедитесь, что Target Framework: .NET 6.0 (или новее)

3. **Скомпилируйте проект**
   - Build → Build Solution или Ctrl+Shift+B
   - В окне Output должно появиться: "Build succeeded"

4. **Запустите приложение**
   - Debug → Start Debugging или F5
   - Или найдите exe-файл в папке `bin/Debug/net6.0-windows/`

### Публикация (создание standalone exe)

Для создания исполняемого файла, который работает без установки .NET:

```bash
dotnet publish -c Release -r win-x64 --self-contained true
```

Файл будет в папке `bin/Release/net6.0-windows/win-x64/publish/`

## Вариант 2: Командная строка (dotnet CLI)

### Требования
- .NET 6.0 SDK или новее
- Скачать: https://dotnet.microsoft.com/download

### Команды

```bash
# Перейти в папку проекта
cd CalculatorFractions

# Скомпилировать
dotnet build

# Запустить
dotnet run

# Скомпилировать в Release режиме
dotnet build -c Release

# Опубликовать (создать exe)
dotnet publish -c Release -r win-x64 --self-contained true
```

## Вариант 3: Visual Studio Code

### Требования
- Visual Studio Code
- Расширение: C# Dev Kit
- .NET 6.0 SDK

### Шаги

1. Откройте папку `CalculatorFractions` в VS Code
2. Нажмите F5 для запуска с отладкой
3. Или используйте команду: `dotnet run`

## Интерфейс программы

```
┌─────────────────────────────────────────┐
│  Правка  Вид  Справка                  │
├─────────────────────────────────────────┤
│                              [  25/1  ] │  ← Дисплей
├─────────────────────────────────────────┤
│ [M]                                     │  ← Индикатор памяти
├─────────────────────────────────────────┤
│ [MC]  [MR]  [M+]  [MS]                  │  ← Кнопки памяти
│ [C]   [CE]  [←]   [/]                   │  ← Редактирование
│ [7]   [8]   [9]   [*]   [Sqr]           │  ← Цифры и операции
│ [4]   [5]   [6]   [-]   [Rev]           │
│ [1]   [2]   [3]   [+]   [+/-]           │
│ [0]   [/]   [=]                         │  ← Результат
└─────────────────────────────────────────┘
```

## Проверка основных функций

После сборки запустите программу и проверьте работу:

1. **Простая операция:** 5/1 + 2/1 = 7/1 ✓
2. **Функция Sqr:** 5/1 Sqr = 25/1 ✓
3. **Функция Rev:** 4/1 Rev = 1/4 ✓
4. **Память:**
   - Введите 5/1, нажмите MS
   - Введите 3/1, нажмите +
   - Нажмите MR → 5/1
   - Нажмите = → 8/1 ✓
5. **Сложное выражение:** 6/1 Sqr + 2/1 Sqr / 10/1 + 6/1 = 10/1 ✓

## Устранение неполадок

### Ошибка: "Framework not found"
Установите .NET 6.0 SDK с https://dotnet.microsoft.com/download

### Ошибка при компиляции
- Убедитесь, что все файлы в папке `src/`
- Проверьте, что установлен .NET 6.0 SDK
- Попробуйте `dotnet restore` перед сборкой

### Форма не отображается
- Проверьте, что установлена рабочая нагрузка ".NET Desktop Development"
- В Visual Studio: Tools → Get Tools and Features → .NET Desktop Development

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
