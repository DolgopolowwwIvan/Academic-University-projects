#include <iostream>
#include <fstream>
#include <cstring>

int main(int argc, char* argv[])
{
    float x, y = 1.0;
    
    if(argc == 3 && strcmp(argv[1], "-f") == 0)
    {
        std::ifstream file(argv[2]);
        if(!file)
        {
            std::cout << "Ошибка: файл не найден!\n";
            return 1;
        }
        file >> x;
    }
    else if(argc == 1)
    {
        std::cout << "Введите x: ";
        std::cin >> x;
    }
    else
    {
        std::cout << "Использование:\n";
        std::cout << "  " << argv[0] << "           - ввод с клавиатуры\n";
        std::cout << "  " << argv[0] << " -f файл   - чтение из файла\n";
        return 1;
    }

    if(x == 1 || x == 3 || x == 7 || x == 15 || x == 31 || x == 63)
    {
        std::cout << "Ошибка: деление на ноль!\n";
        return 1;
    }

    for(int i = 2; i <= 64; i *= 2)
    {
        y *= (x - i) / (x - (i - 1));
    }

    std::cout << "Результат: y = " << y << "\n";
    
    return 0;
}
