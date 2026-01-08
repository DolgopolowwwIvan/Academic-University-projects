#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <cstring>
#include <netdb.h>
#include <stdio.h>
#include <iostream>
#include <string>
#include <cctype>

using namespace std;

string processString(const string& input) {
    string result;
    if (input.empty()) return result;
    
    result += input[0];
    
    for (size_t i = 1; i < input.size(); i++) {
        if (isupper(input[i]) && islower(input[i - 1])) {
            result += ' ';
        }
        result += input[i];
    }
    
    return result;
}

int main(void) {
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
    
    int servSock = socket(AF_INET, SOCK_STREAM, 0);
    if (servSock == -1) {
        printf("Unable to create socket\n");
        return 1;
    }
    
    int opt = 1;
    if (setsockopt(servSock, SOL_SOCKET, SO_REUSEADDR, &opt, sizeof(opt)) < 0) {
        printf("Setsockopt error\n");
        close(servSock);
        return 1;
    }
    
    struct sockaddr_in sin;
    sin.sin_family = AF_INET;
    sin.sin_port = htons(serverPort);
    sin.sin_addr.s_addr = INADDR_ANY;
    
    int retVal = bind(servSock, (struct sockaddr*)&sin, sizeof(sin));
    if (retVal == -1) {
        printf("Unable to bind to port %d\n", serverPort);
        close(servSock);
        return 1;
    }
    
    printf("Server started. Waiting for connections on port %d...\n", serverPort);
    fflush(stdout);
    
    retVal = listen(servSock, 10);
    if (retVal == -1) {
        printf("Unable to listen\n");
        close(servSock);
        return 1;
    }
    
    while (true) {
        struct sockaddr_in from;
        socklen_t fromlen = sizeof(from);
        
        int clientSock = accept(servSock, (struct sockaddr*)&from, &fromlen);
        if (clientSock == -1) {
            printf("Unable to accept connection\n");
            continue;
        }
        
        char clientIP[INET_ADDRSTRLEN];
        inet_ntop(AF_INET, &(from.sin_addr), clientIP, INET_ADDRSTRLEN);
        printf("New client connected from %s:%d\n", clientIP, ntohs(from.sin_port));
        fflush(stdout);
        
        char buffer[1024];
        retVal = recv(clientSock, buffer, sizeof(buffer) - 1, 0);
        if (retVal == -1) {
            printf("Unable to receive data\n");
            close(clientSock);
            continue;
        } else if (retVal == 0) {
            printf("Client disconnected\n");
            close(clientSock);
            continue;
        }
        
        buffer[retVal] = '\0';
        printf("Received from %s: %s\n", clientIP, buffer);
        
        string receivedStr(buffer);
        string processedStr = processString(receivedStr);
        
        printf("Processed string: %s\n", processedStr.c_str());
        fflush(stdout);
        
        retVal = send(clientSock, processedStr.c_str(), processedStr.length(), 0);
        if (retVal == -1) {
            printf("Unable to send response\n");
        } else {
            printf("Sent to %s: %s\n", clientIP, processedStr.c_str());
        }
        
        close(clientSock);
        printf("Connection with %s closed\n\n", clientIP);
        fflush(stdout);
    }
    
    close(servSock);
    return 0;
}