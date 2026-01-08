#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <cstring>
#include <stdio.h>
#include <iostream>
#include <string>

using namespace std;

int main() {
    int clientSock = socket(AF_INET, SOCK_STREAM, 0);
    if (clientSock == -1) {
        printf("Unable to create socket\n");
        return 1;
    }

    string ip;
    int port;
    cout << "Enter server IP: ";
    getline(cin, ip);
    cout << "Enter server port: ";
    cin >> port;
    cin.ignore();

    struct sockaddr_in serverInfo;
    serverInfo.sin_family = AF_INET;
    serverInfo.sin_port = htons(port);

    if (inet_pton(AF_INET, ip.c_str(), &serverInfo.sin_addr) <= 0) {
        printf("Invalid IP address: %s\n", ip.c_str());
        close(clientSock);
        return 1;
    }

    printf("Connecting to %s:%d...\n", ip.c_str(), port);
    int retVal = connect(clientSock, (struct sockaddr*)&serverInfo, sizeof(serverInfo));
    if (retVal == -1) {
        printf("Unable to connect to server %s:%d\n", ip.c_str(), port);
        close(clientSock);
        return 1;
    }

    printf("Connection established successfully\n");

    string message;
    cout << "Enter text to send: ";
    getline(cin, message);

    retVal = send(clientSock, message.c_str(), message.length(), 0);
    if (retVal == -1) {
        printf("Unable to send\n");
        close(clientSock);
        return 1;
    }

    printf("Data sent. Waiting for response...\n");

    char buffer[1024];
    retVal = recv(clientSock, buffer, sizeof(buffer) - 1, 0);
    if (retVal == -1) {
        printf("Unable to receive data\n");
    } else if (retVal == 0) {
        printf("Server closed connection\n");
    } else {
        buffer[retVal] = '\0';
        printf("Server response: %s\n", buffer);
    }

    close(clientSock);

    printf("Press Enter to exit...");
    getchar();
    return 0;
}