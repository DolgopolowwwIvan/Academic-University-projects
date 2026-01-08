#include <iostream>
#include <fstream>
#include <string>
#include <cstdlib>
#include <vector>
#include <stdexcept>
#include <limits>

using namespace std;

void show_help() {
    cout << "Программа для вычисления выражения (Cx·y)/(x·By) + Ax·By\n\n"
         << "Использование:\n"
         << "  ./program               - интерактивный ввод данных\n"
         << "  ./program --help        - показать эту справку\n"
         << "  ./program --file <path> - ввод данных из файла\n\n"
         << "Формат файла:\n"
         << "  Первые 2 строк - матрица A (2x2)\n"
         << "  Следующие 2 строк - матрица B (2x2)\n"
         << "  Следующие 2 строк - матрица C (2x2)\n"
         << "  Следующие 2 значений - вектор x\n"
         << "  Последние 2 значений - вектор y\n\n"
         << "Разработчики: Долгополов Иван\n";
}

vector<vector<int> > read_matrix_from_file(ifstream& file, size_t size) {
    vector<vector<int> > matrix(size, vector<int>(size));
    for (size_t i = 0; i < size; ++i) {
        for (size_t j = 0; j < size; ++j) {
            if (!(file >> matrix[i][j])) {
                throw runtime_error("Ошибка чтения матрицы из файла");
            }
        }
    }
    return matrix;
}

vector<int> read_vector_from_file(ifstream& file, size_t size) {
    vector<int> vec(size);
    for (size_t i = 0; i < size; ++i) {
        if (!(file >> vec[i])) {
            throw runtime_error("Ошибка чтения вектора из файла");
        }
    }
    return vec;
}

vector<vector<int> > input_matrix(size_t size) {
    vector<vector<int> > matrix(size, vector<int>(size));
    for (size_t i = 0; i < size; ++i) {
        for (size_t j = 0; j < size; ++j) {
            cout << "a[" << i << "][" << j << "]: ";
            while (!(cin >> matrix[i][j])) {
                cout << "Ошибка ввода! Введите целое число: ";
                cin.clear();
                cin.ignore(numeric_limits<streamsize>::max(), '\n');
            }
        }
    }
    return matrix;
}

vector<int> input_vector(size_t size) {
    vector<int> vec(size);
    for (size_t i = 0; i < size; ++i) {
        cout << "vec[" << i << "]: ";
        while (!(cin >> vec[i])) {
            cout << "Ошибка ввода! Введите целое число: ";
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
        }
    }
    return vec;
}

vector<int> mat_vec_mult(const vector<vector<int> >& matrix, const vector<int>& vec) {
    size_t size = vec.size();
    vector<int> result(size, 0);
    
    for (size_t i = 0; i < size; ++i) {
        for (size_t j = 0; j < size; ++j) {
            result[i] += matrix[j][i] * vec[j];
        }
    }
    return result;
}

int dot_product(const vector<int>& a, const vector<int>& b) {
    int result = 0;
    for (size_t i = 0; i < a.size(); ++i) {
        result += a[i] * b[i];
    }
    return result;
}

int main(int argc, char* argv[]) {
    setlocale(LC_ALL, "RU");
    const size_t SIZE = 2;
    bool file_input = false;
    string filename;

    if (argc > 1) {
        string arg = argv[1];
        if (arg == "--help") {
            show_help();
            return 0;
        } else if (arg == "--file" && argc > 2) {
            file_input = true;
            filename = argv[2];
        } else {
            cerr << "Неизвестный аргумент. Используйте --help для справки.\n";
            return 1;
        }
    }

    try {
        vector<vector<int> > A, B, C;
        vector<int> x, y;

        if (file_input) {
            ifstream file(filename.c_str());
            if (!file.is_open()) {
                throw runtime_error("Не удалось открыть файл: " + filename);
            }

            A = read_matrix_from_file(file, SIZE);
            B = read_matrix_from_file(file, SIZE);
            C = read_matrix_from_file(file, SIZE);
            x = read_vector_from_file(file, SIZE);
            y = read_vector_from_file(file, SIZE);
        } else {
            cout << "Ввод матрицы A (" << SIZE << "x" << SIZE << "):\n";
            A = input_matrix(SIZE);
            
            cout << "\nВвод матрицы B (" << SIZE << "x" << SIZE << "):\n";
            B = input_matrix(SIZE);
            
            cout << "\nВвод матрицы C (" << SIZE << "x" << SIZE << "):\n";
            C = input_matrix(SIZE);
            
            cout << "\nВвод вектора x (" << SIZE << " элементов):\n";
            x = input_vector(SIZE);
            
            cout << "\nВвод вектора y (" << SIZE << " элементов):\n";
            y = input_vector(SIZE);
        }

        vector<int> Ax = mat_vec_mult(A, x);
        vector<int> By = mat_vec_mult(B, y);
        vector<int> Cx = mat_vec_mult(C, x);

        int Ax_By = dot_product(Ax, By);
        int Cx_y = dot_product(Cx, y);
        int x_By = dot_product(x, By);

        if (x_By == 0) {
            throw runtime_error("Ошибка: деление на ноль (x·By = 0)");
        }

        int result = (Cx_y / x_By) + Ax_By;

        cout << "\nРезультаты:\n";
        cout << "Ax·By = " << Ax_By << endl;
        cout << "Cx·y = " << Cx_y << endl;
        cout << "x·By = " << x_By << endl;
        cout << "Итоговый результат: " << result << endl;

    } catch (const exception& e) {
        cerr << "Ошибка: " << e.what() << endl;
        return 1;
    }

    return 0;
}