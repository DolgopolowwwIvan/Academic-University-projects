#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <cstring>
#include <netdb.h>
#include <stdio.h>
#include <iostream>
#include <string>
#include <vector>
#include <pthread.h>

using namespace std;

const int MAX_CLIENTS = 50;
const int BUFFER_SIZE = 1024;

vector<int> clients;
vector<string> clientNames;
pthread_mutex_t clientsMutex = PTHREAD_MUTEX_INITIALIZER;

void BroadcastMessage(const string& message, int senderSocket = -1) {
    pthread_mutex_lock(&clientsMutex);
    
    for (size_t i = 0; i < clients.size(); i++) {
        if (clients[i] != senderSocket && clients[i] != -1) {
            send(clients[i], message.c_str(), message.length(), 0);
        }
    }
    
    pthread_mutex_unlock(&clientsMutex);
}


struct ClientParams {
    int socket;
    sockaddr_in addr;
};

void* HandleClient(void* arg) {
    
    ClientParams* params = (ClientParams*)arg;
    int clientSocket = params->socket;
    sockaddr_in clientAddr = params->addr;
    delete params; 
    
    char clientIP[INET_ADDRSTRLEN];
    inet_ntop(AF_INET, &(clientAddr.sin_addr), clientIP, INET_ADDRSTRLEN);
    int clientPort = ntohs(clientAddr.sin_port);
    
    printf("New client connected: %s:%d\n", clientIP, clientPort);
    
   
    string nameRequest = "Enter your name: ";
    send(clientSocket, nameRequest.c_str(), nameRequest.length(), 0);
    
    char username[32];
    int bytesReceived = recv(clientSocket, username, 31, 0);
    if (bytesReceived <= 0) {
        close(clientSocket);
        return NULL;
    }
    username[bytesReceived] = '\0';
    
    
    if (strlen(username) > 0 && username[strlen(username)-1] == '\n') {
        username[strlen(username)-1] = '\0';
    }
    
    
    pthread_mutex_lock(&clientsMutex);
    clients.push_back(clientSocket);
    clientNames.push_back(username);
    pthread_mutex_unlock(&clientsMutex);
    
    
    string joinMessage = string(username) + " joined the chat!\n";
    printf("%s", joinMessage.c_str());
    fflush(stdout);
    BroadcastMessage(joinMessage, clientSocket);
    
    char buffer[BUFFER_SIZE];
    
    while (true) {
        bytesReceived = recv(clientSocket, buffer, BUFFER_SIZE - 1, 0);
        
        if (bytesReceived <= 0) {
            break;
        }
        
        buffer[bytesReceived] = '\0';
        
        
        if (strncmp(buffer, "exit", 4) == 0) {
            break;
        }
        
        
        string clientName;
        pthread_mutex_lock(&clientsMutex);
        for (size_t i = 0; i < clients.size(); i++) {
            if (clients[i] == clientSocket) {
                clientName = clientNames[i];
                break;
            }
        }
        pthread_mutex_unlock(&clientsMutex);
        
        
        string messageText = buffer;
        if (!messageText.empty() && messageText[messageText.length()-1] == '\n') {
            messageText = messageText.substr(0, messageText.length()-1);
        }
        
        string fullMessage = clientName + ": " + messageText + "\n";
        printf("%s", fullMessage.c_str());
        fflush(stdout);
        BroadcastMessage(fullMessage, clientSocket);
    }
    
    
    string clientName;
    pthread_mutex_lock(&clientsMutex);
    for (size_t i = 0; i < clients.size(); i++) {
        if (clients[i] == clientSocket) {
            clientName = clientNames[i];
            
            clients.erase(clients.begin() + i);
            clientNames.erase(clientNames.begin() + i);
            break;
        }
    }
    pthread_mutex_unlock(&clientsMutex);
    
    
    string leaveMessage = clientName + " left the chat!\n";
    printf("%s", leaveMessage.c_str());
    fflush(stdout);
    BroadcastMessage(leaveMessage);
    
    close(clientSocket);
    printf("Client disconnected: %s:%d\n", clientIP, clientPort);
    fflush(stdout);
    
    return NULL;
}

int main() {
   
    int serverPort;
    cout << "Enter server port: ";
    cin >> serverPort;
    cin.ignore();
    
    
    char hostname[256];
    if (gethostname(hostname, sizeof(hostname)) == 0) {
        struct hostent* host = gethostbyname(hostname);
        if (host && host->h_addr_list[0]) {
            cout << "Server IP: " << inet_ntoa(*(struct in_addr*)host->h_addr_list[0]) << endl;
        }
    }
    cout << "Server port: " << serverPort << endl;
    
    
    int serverSocket = socket(AF_INET, SOCK_STREAM, 0);
    if (serverSocket == -1) {
        printf("Unable to create socket\n");
        return 1;
    }
    
    int enable = 1;
    setsockopt(serverSocket, SOL_SOCKET, SO_REUSEADDR, &enable, sizeof(enable));
    
    sockaddr_in serverAddr;
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_port = htons(serverPort);
    serverAddr.sin_addr.s_addr = INADDR_ANY;
    
    int retVal = bind(serverSocket, (sockaddr*)&serverAddr, sizeof(serverAddr));
    if (retVal == -1) {
        printf("Unable to bind socket\n");
        close(serverSocket);
        return 1;
    }
    
    retVal = listen(serverSocket, 10);
    if (retVal == -1) {
        printf("Unable to listen\n");
        close(serverSocket);
        return 1;
    }
    
    printf("Chat server started. Waiting for connections on port %d...\n", serverPort);
    fflush(stdout);
    
    while (true) {
        sockaddr_in clientAddr;
        socklen_t clientAddrSize = sizeof(clientAddr);
        
        int clientSocket = accept(serverSocket, (sockaddr*)&clientAddr, &clientAddrSize);
        if (clientSocket == -1) {
            printf("Unable to accept connection\n");
            continue;
        }
        

        ClientParams* params = new ClientParams;
        params->socket = clientSocket;
        params->addr = clientAddr;
        
        pthread_t clientThread;
        if (pthread_create(&clientThread, NULL, HandleClient, params) != 0) {
            printf("Unable to create thread\n");
            close(clientSocket);
            delete params;
            continue;
        }
        
        pthread_detach(clientThread);
    }
    
    close(serverSocket);
    pthread_mutex_destroy(&clientsMutex);
    return 0;
}