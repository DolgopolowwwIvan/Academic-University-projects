#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <cstring>
#include <netdb.h>
#include <stdio.h>
#include <iostream>
#include <string>
#include <pthread.h>

using namespace std;

const int BUFFER_SIZE = 1024;

pthread_mutex_t consoleMutex = PTHREAD_MUTEX_INITIALIZER;
bool isReceiving = true;

void SafePrint(const string& message) {
    pthread_mutex_lock(&consoleMutex);
    cout << message;
    cout.flush(); 
    pthread_mutex_unlock(&consoleMutex);
}

void ClearInputLine() {

    cout << "\r\033[K"; 
    cout.flush();
}

void ShowPrompt() {
    pthread_mutex_lock(&consoleMutex);
    cout << "> ";
    cout.flush();
    pthread_mutex_unlock(&consoleMutex);
}

string GetLocalIP() {
    char hostname[256];
    gethostname(hostname, sizeof(hostname));

    struct hostent* host = gethostbyname(hostname);
    if (host == NULL) return "127.0.0.1";

    return inet_ntoa(*(struct in_addr*)*host->h_addr_list);
}

void* ReceiveMessages(void* arg) {
    int clientSocket = *(int*)arg;
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
    
    return NULL;
}

int main() {

    int clientSocket = socket(AF_INET, SOCK_STREAM, 0);
    if (clientSocket == -1) {
        printf("Unable to create socket\n");
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
    } else {
        serverIP = serverInput;
    }

    sockaddr_in serverAddr;
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_port = htons(serverPort);

    if (inet_pton(AF_INET, serverIP.c_str(), &serverAddr.sin_addr) <= 0) {
        printf("Invalid IP address: %s\n", serverIP.c_str());
        close(clientSocket);
        return 1;
    }

    printf("Connecting to %s:%d...\n", serverIP.c_str(), serverPort);
    int retVal = connect(clientSocket, (sockaddr*)&serverAddr, sizeof(serverAddr));
    if (retVal == -1) {
        printf("Unable to connect to server %s:%d\n", serverIP.c_str(), serverPort);
        printf("Make sure server is running and firewall allows connections\n");
        close(clientSocket);
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

    pthread_t receiveThread;
    if (pthread_create(&receiveThread, NULL, ReceiveMessages, &clientSocket) != 0) {
        printf("Unable to create receive thread\n");
        close(clientSocket);
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
        if (sent == -1) {
            SafePrint("Send error - connection lost\n");
            break;
        }
        
        if (isReceiving) {
            ShowPrompt();
        }
    }

    isReceiving = false;
    shutdown(clientSocket, SHUT_RDWR);
    pthread_join(receiveThread, NULL);
    close(clientSocket);
    pthread_mutex_destroy(&consoleMutex);

    printf("Press Enter to exit...");
    getchar();
    return 0;
}