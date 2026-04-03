#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/wait.h>
#include <signal.h>
#include <string.h>

#pragma pack(push, 1)
struct Message {
    int process_num;
    char text[40];
};
#pragma pack(pop)

volatile sig_atomic_t sig_flag = 0;

void sig_handler(int signum) {
    (void)signum;
    sig_flag = 1;
}

int main() {
    int fd[2];
    pid_t pid1, pid2;
    struct Message msg;

    signal(SIGUSR1, sig_handler);

    printf("[P0] Основной процесс начал работу.\n");

    if (pipe(fd) == -1) {
        perror("Ошибка создания pipe");
        exit(1);
    }
    printf("[P0] Программный канал К1 успешно создан.\n");

    pid1 = fork();
    if (pid1 < 0) {
        perror("Ошибка fork 1");
        exit(1);
    }

    if (pid1 > 0) {
        close(fd[1]);
        printf("[P0] Порожден процесс P1 (PID: %d).\n", pid1);
        printf("[P0] Ожидание и обработка данных из канала...\n");

        while (read(fd[0], &msg, sizeof(msg)) > 0) {
            printf("\n[P0 ЧИТАЕТ]: Процесс №%d, Сообщение: '%s'\n", 
                   msg.process_num, msg.text);
        }
        close(fd[0]);

        printf("\n[P0] Ожидание завершения дочернего процесса P1...\n");
        waitpid(pid1, NULL, 0);
        printf("[P0] Процесс P1 завершен. Основной процесс P0 завершает работу.\n");

    } else {
        printf("[P1] Процесс P1 начал работу (PID: %d).\n", getpid());
        
        pid2 = fork();
        if (pid2 < 0) {
            perror("Ошибка fork 2");
            exit(1);
        }

        if (pid2 > 0) {
            close(fd[0]);
            printf("[P1] Порожден процесс P2 (PID: %d).\n", pid2);

            msg.process_num = 1;
            strncpy(msg.text, "Данные от P1 (Этап I)", 40);
            write(fd[1], &msg, sizeof(msg));
            printf("[P1] Подготовил данные (I) и записал в канал.\n");

            sleep(1); 
            
            printf("[P1] Посылка сигнала процессу P2 для старта...\n");
            kill(pid2, SIGUSR1);

            printf("[P1] Ожидание сигнала от P2...\n");
            while (!sig_flag) {
                pause(); 
            }
            sig_flag = 0;
            printf("[P1] Сигнал от P2 получен.\n");

            msg.process_num = 1;
            strncpy(msg.text, "Данные от P1 (Этап III)", 40);
            write(fd[1], &msg, sizeof(msg));
            printf("[P1] Подготовил данные (III) и записал в канал.\n");

            printf("[P1] Ожидание завершения процесса P2...\n");
            waitpid(pid2, NULL, 0);
            
            close(fd[1]);
            printf("[P1] Процесс P2 завершен. Завершение работы P1.\n");

        } else {
            close(fd[0]);
            printf("[P2] Процесс P2 начал работу (PID: %d). Ожидание сигнала от P1...\n", getpid());

            while (!sig_flag) {
                pause(); 
            }
            sig_flag = 0;
            printf("[P2] Сигнал от P1 получен.\n");

            msg.process_num = 2;
            strncpy(msg.text, "Данные от P2 (Этап II)", 40);
            write(fd[1], &msg, sizeof(msg));
            printf("[P2] Подготовил данные (II) и записал в канал.\n");

            printf("[P2] Посылка сигнала процессу P1...\n");
            kill(getppid(), SIGUSR1);

            close(fd[1]);
            printf("[P2] Завершение работы P2.\n");
            exit(0);
        }
        exit(0);
    }
    return 0;
}
