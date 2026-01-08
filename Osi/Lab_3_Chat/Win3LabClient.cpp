#pragma comment(lib, "ws2_32.lib")
#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdio.h>
#include <iostream>
#include <string>
#include <windows.h>

using namespace std;

const int BUFFER_SIZE = 1024;

HANDLE consoleMutex;
bool isReceiving = true;

void SafePrint(const string& message) {
    WaitForSingleObject(consoleMutex, INFINITE);
    cout << message;
    cout.flush();
    ReleaseMutex(consoleMutex);
}

void ClearInputLine() {
    cout << "\r";
    for (int i = 0; i < 80; i++) cout << " ";
    cout << "\r";
    cout.flush();
}

void ShowPrompt() {
    WaitForSingleObject(consoleMutex, INFINITE);
    cout << "> ";
    cout.flush();
    ReleaseMutex(consoleMutex);
}

string GetLocalIP() {
    char hostname[256];
    if (gethostname(hostname, sizeof(hostname)) == SOCKET_ERROR) {
        return "127.0.0.1";
    }

    struct hostent* host = gethostbyname(hostname);
    if (host == NULL) return "127.0.0.1";

    return inet_ntoa(*(struct in_addr*)*host->h_addr_list);
}

DWORD WINAPI ReceiveMessages(LPVOID arg) {
    SOCKET clientSocket = *(SOCKET*)arg;
    char buffer[BUFFER_SIZE];

    while (true) {
        int bytesReceived = recv(clientSocket, buffer, BUFFER_SIZE - 1, 0);

        if (bytesReceived <= 0) {
            SafePrint("\nDisconnected from server\n");
            isReceiving = false;
            break;
        }

        buffer[bytesReceived] = '\0';
        ClearInputLine();
        SafePrint(string(buffer));

        if (isReceiving) {
            ShowPrompt();
        }
    }

    return 0;
}

int main() {
    WSADATA wsaData;
    int retVal = WSAStartup(MAKEWORD(2, 2), &wsaData);
    if (retVal != 0) {
        printf("WSAStartup failed: %d\n", retVal);
        return 1;
    }

    consoleMutex = CreateMutex(NULL, FALSE, NULL);

    SOCKET clientSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
    if (clientSocket == INVALID_SOCKET) {
        printf("Unable to create socket: %d\n", WSAGetLastError());
        WSACleanup();
        return 1;
    }

    string localIP = GetLocalIP();
    printf("Your local IP: %s\n", localIP.c_str());

    string serverInput;
    cout << "Enter server IP (localhost for same PC): ";
    getline(cin, serverInput);

    int serverPort;
    cout << "Enter server port: ";
    cin >> serverPort;
    cin.ignore();

    string serverIP;
    if (serverInput == "localhost") {
        serverIP = "127.0.0.1";
    }
    else {
        serverIP = serverInput;
    }

    sockaddr_in serverAddr;
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_port = htons(serverPort);
    serverAddr.sin_addr.s_addr = inet_addr(serverIP.c_str());

    printf("Connecting to %s:%d...\n", serverIP.c_str(), serverPort);
    retVal = connect(clientSocket, (sockaddr*)&serverAddr, sizeof(serverAddr));
    if (retVal == SOCKET_ERROR) {
        printf("Unable to connect to server %s:%d\n", serverIP.c_str(), serverPort);
        printf("Make sure server is running and firewall allows connections\n");
        closesocket(clientSocket);
        WSACleanup();
        return 1;
    }

    printf("Connected to server successfully!\n");

    char buffer[BUFFER_SIZE];
    int bytesReceived = recv(clientSocket, buffer, BUFFER_SIZE - 1, 0);
    if (bytesReceived > 0) {
        buffer[bytesReceived] = '\0';
        printf("%s", buffer);
    }

    string username;
    cout << "Enter your name: ";
    getline(cin, username);
    send(clientSocket, username.c_str(), username.length(), 0);

    printf("Welcome to the chat, %s!\n", username.c_str());
    printf("Type your messages (type 'exit' to quit):\n\n");

    HANDLE receiveThread = CreateThread(NULL, 0, ReceiveMessages, &clientSocket, 0, NULL);
    if (receiveThread == NULL) {
        printf("Unable to create receive thread\n");
        closesocket(clientSocket);
        WSACleanup();
        return 1;
    }

    string message;
    ShowPrompt();

    while (true) {
        getline(cin, message);

        if (message == "exit") {
            message += "\n";
            send(clientSocket, message.c_str(), message.length(), 0);
            break;
        }

        ClearInputLine();
        SafePrint("You: " + message + "\n");

        message += "\n";
        int sent = send(clientSocket, message.c_str(), message.length(), 0);
        if (sent == SOCKET_ERROR) {
            SafePrint("Send error - connection lost\n");
            break;
        }

        if (isReceiving) {
            ShowPrompt();
        }
    }

    isReceiving = false;
    shutdown(clientSocket, SD_BOTH);
    WaitForSingleObject(receiveThread, INFINITE);
    CloseHandle(receiveThread);
    closesocket(clientSocket);
    CloseHandle(consoleMutex);
    WSACleanup();

    printf("Press Enter to exit...");
    getchar();
    return 0;
}