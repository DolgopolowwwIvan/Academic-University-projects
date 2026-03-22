#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/wait.h>
#include <fcntl.h>
#include <string.h>
#include <math.h>

#define TEMP_FILE "temp_result.txt"
#define MAX_ITER 100
#define EPSILON 1e-5

// Структура для обмена данными между процессами
typedef struct {
    pid_t pid;           // Идентификатор процесса-отправителя
    double exp_value;
    int status;
} Message;

// Функция для вычисления exp(x) разложением в ряд Тейлора
double calculate_exp_taylor(double x) {
    double sum = 1.0;  // Начальное значение (член ряда для n=0)
    double term = 1.0;  // Текущий член ряда
    
    for (int n = 1; n < MAX_ITER; n++) {
        term *= x / n;  // Вычисление следующего члена ряда
        sum += term;
        
        // Проверка на достижение точности
        if (term < EPSILON && term > -EPSILON)
            break;
    }
    
    return sum;
}

// Функция для вычисления плотности распределения Рэлея
double rayleigh_pdf(double x, double sigma) {
    if (x < 0 || sigma <= 0)
        return 0.0;
    
    return (x / (sigma * sigma)) * exp(-x * x / (2 * sigma * sigma));
}

int main() {
    double x, sigma;
    pid_t pid;
    int status;
    FILE *fp;
    Message msg;
    
    printf("Введите значение x: ");
    scanf("%lf", &x);
    printf("Введите значение sigma (>0): ");
    scanf("%lf", &sigma);
    
    if (sigma <= 0) {
        printf("Ошибка: sigma должна быть положительной\n");
        return 1;
    }
    
    printf("Родительский процесс (PID=%d) начал работу\n", getpid());
    
    // Создание временного файла
    fp = fopen(TEMP_FILE, "w");
    if (fp == NULL) {
        perror("Ошибка создания временного файла");
        return 1;
    }
    fclose(fp);
    
    // Создание дочернего процесса
    pid = fork();
    
    if (pid == -1) {
        perror("Ошибка fork");
        return 1;
    }
    else if (pid == 0) {
        // Дочерний процесс - вычисляет exp(-x^2/(2*sigma^2))
        printf("Дочерний процесс (PID=%d) начал вычисление экспоненты\n", getpid());
        
        double exponent = -x * x / (2 * sigma * sigma);
        double exp_value = calculate_exp_taylor(exponent);
        
        printf("Дочерний процесс вычислил exp(%.6f) = %.10f\n", exponent, exp_value);
        
        // Запись результата во временный файл
        fp = fopen(TEMP_FILE, "w");
        if (fp == NULL) {
            perror("Дочерний процесс: ошибка открытия файла");
            msg.pid = getpid();
            msg.exp_value = 0.0;
            msg.status = 1;
        } else {
            msg.pid = getpid();
            msg.exp_value = exp_value;
            msg.status = 0;
            fwrite(&msg, sizeof(Message), 1, fp);
            fclose(fp);
            printf("Дочерний процесс записал результат во временный файл\n");
        }
        
        printf("Дочерний процесс завершает работу\n");
        exit(0);
    }
    else {
        printf("Родительский процесс породил дочерний процесс с PID=%d\n", pid);
        printf("Родительский процесс ожидает появления данных во временном файле...\n");
        
        // Опрос временного файла
        int file_ready = 0;
        int attempts = 0;
        
        while (!file_ready && attempts < 100) {
            sleep(1);  // Пауза 1 секунда между опросами
            
            fp = fopen(TEMP_FILE, "r");
            if (fp != NULL) {
                // Проверяем, есть ли данные в файле
                if (fread(&msg, sizeof(Message), 1, fp) == 1) {
                    file_ready = 1;
                }
                fclose(fp);
            }
            
            attempts++;
            if (!file_ready) {
                printf("Попытка %d: данные еще не готовы...\n", attempts);
            }
        }
        
        if (!file_ready) {
            printf("Ошибка: превышено время ожидания данных от дочернего процесса\n");
        } else {
            printf("Родительский процесс получил данные от дочернего процесса (PID=%d)\n", msg.pid);
            
            if (msg.status == 0) {
                // Вычисление плотности распределения Рэлея
                double pdf_value = (x / (sigma * sigma)) * msg.exp_value;
                double check_value = rayleigh_pdf(x, sigma);  // Для проверки
                
                printf("\n========== РЕЗУЛЬТАТЫ ==========\n");
                printf("Параметры: x = %.6f, sigma = %.6f\n", x, sigma);
                printf("Вычисленное значение exp: %.10f\n", msg.exp_value);
                printf("Плотность распределения Рэлея: %.10f\n", pdf_value);
                printf("Контрольное значение (math.h): %.10f\n", check_value);
                printf("Погрешность: %.10f\n", fabs(pdf_value - check_value));
                printf("================================\n");
            } else {
                printf("Дочерний процесс сообщил об ошибке\n");
            }
        }

        wait(&status);
        printf("Дочерний процесс завершился с кодом %d\n", WEXITSTATUS(status));
        
        remove(TEMP_FILE);
        printf("Временный файл удален\n");
    }
    
    printf("Родительский процесс завершает работу\n");
    return 0;
}