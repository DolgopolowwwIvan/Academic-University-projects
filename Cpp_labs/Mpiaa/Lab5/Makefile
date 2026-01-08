# Компилятор и флаги
CXX = g++
CXXFLAGS = -std=c++17 -Wall -Wextra -g

# Директория для объектных файлов и исполняемого файла
BUILD_DIR = build
BIN = $(BUILD_DIR)/project
TEST_BIN = $(BUILD_DIR)/test_project

# Файлы проекта
SRC = main.cpp graph.cpp tsp.cpp
HEADERS = graph.h tsp.h
OBJS = $(addprefix $(BUILD_DIR)/, $(SRC:.cpp=.o))

# Библиотеки
CATCH2_FLAGS = -DCATCH_CONFIG_MAIN

# Правило по умолчанию
all: $(BIN) $(TEST_BIN)

# Компиляция основного приложения
$(BIN): $(OBJS)
	$(CXX) $(CXXFLAGS) -o $@ $^

# Общие правила для объектных файлов
$(BUILD_DIR)/%.o: %.cpp $(HEADERS)
	@mkdir -p $(BUILD_DIR)
	$(CXX) $(CXXFLAGS) -c $< -o $@

# Запуск проекта
run: $(BIN)
	@echo "Запуск основного приложения..."
	./$(BIN)

# Очистка
clean:
	@echo "Очистка..."
	rm -rf $(BUILD_DIR)

# Пересборка
rebuild: clean all
