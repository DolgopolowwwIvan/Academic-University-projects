# Makefile для компиляции под Wine
CXX = x86_64-w64-mingw32-g++
CXXFLAGS = -std=c++11 -I. -D_USE_MATH_DEFINES -Wall -Wextra -pedantic -static
LDFLAGS = -static

# Исходные файлы
LIB_SOURCES = pearson_distribution.cpp mixture.cpp empiric.cpp data_exporter.cpp
LIB_OBJECTS = $(LIB_SOURCES:.cpp=.o)

MAIN_SOURCES = main.cpp
MAIN_OBJECTS = $(MAIN_SOURCES:.cpp=.o)

TEST_SOURCES = test_pearson.cpp
TEST_OBJECTS = $(TEST_SOURCES:.cpp=.o)

EXECUTABLES = main.exe test_pearson.exe

# Цели по умолчанию
all: $(EXECUTABLES)

# Основная программа
main.exe: $(MAIN_OBJECTS) $(LIB_OBJECTS)
	$(CXX) $(LDFLAGS) $(MAIN_OBJECTS) $(LIB_OBJECTS) -o $@

# Тестовая программа
test_pearson.exe: $(TEST_OBJECTS) $(LIB_OBJECTS)
	$(CXX) $(LDFLAGS) $(TEST_OBJECTS) $(LIB_OBJECTS) -o $@

# Правило для компиляции .cpp в .o
%.o: %.cpp
	$(CXX) $(CXXFLAGS) -c $< -o $@

# Специальное правило для main.cpp (удаляем Windows.h)
main.o: main.cpp
	@echo "Обработка main.cpp для Wine..."
	@cp main.cpp main_wine.cpp
	@sed -i '/#include.*windows\.h/d' main_wine.cpp
	@sed -i '/SetConsoleOutputCP/d' main_wine.cpp
	@sed -i '/#include.*Windows\.h/d' main_wine.cpp
	$(CXX) $(CXXFLAGS) -c main_wine.cpp -o main.o
	@rm -f main_wine.cpp

# Очистка
clean:
	rm -f *.o *.exe main_wine.cpp

# Запуск через Wine
run: main.exe
	wine main.exe

run-test: test_pearson.exe
	wine test_pearson.exe

# Создание тестовых файлов
create-test-files:
	@echo "Создание тестовых файлов..."
	@echo "0.0 1.0 3.0" > test.txt
	@echo "2.0 1.5 4.0" > test2.txt
	@echo "Тестовые файлы созданы: test.txt, test2.txt"

.PHONY: all clean run run-test create-test-files