#include <winsock2.h>
#include <ws2tcpip.h>
#include <iphlpapi.h>
#include <icmpapi.h>
#include <stdio.h>

const int n = 1024;

#pragma comment(lib, "iphlpapi.lib")
#pragma comment(lib, "ws2_32.lib")

int main()
{
    HANDLE h_icmp_file;
    unsigned long ip_addr = INADDR_NONE;
    DWORD dw_ret_val = 0;
    char send_data[n];
    LPVOID reply_buffer = NULL;
    DWORD reply_size = 0;
    char send_address[n];
    char host[n];
    WORD sock_ver;
    WSADATA wsa_data;
    
    sock_ver = MAKEWORD(2, 2);
    WSAStartup(sock_ver, &wsa_data);
    
    printf("Enter adress: ");
    scanf("%s", send_address);
    
    printf("Enter data: ");
    scanf("%s", send_data);
    
    struct hostent *remote_host;
    remote_host = gethostbyname(send_address);
    
    if(remote_host != NULL)
    {
        strcpy(host, inet_ntoa(*((struct in_addr*)remote_host->h_addr_list[0])));
        ip_addr = inet_addr(host);
        
        h_icmp_file = IcmpCreateFile();
        reply_size = sizeof(ICMP_ECHO_REPLY) + sizeof(send_data);
        reply_buffer = (VOID*) malloc(reply_size);
        
        dw_ret_val = IcmpSendEcho(h_icmp_file, ip_addr, send_data, sizeof(send_data), NULL, reply_buffer, reply_size, 1000);
        
        if (dw_ret_val != 0)
        {
            PICMP_ECHO_REPLY p_echo_reply = (PICMP_ECHO_REPLY)reply_buffer;
            struct in_addr reply_addr;
            reply_addr.S_un.S_addr = p_echo_reply->Address;
            
            printf("Sent echo request to %s\n", host);
            
            if(!p_echo_reply->Status) 
                printf("Status was success\n");
            
            printf("Received %ld icmp message ", dw_ret_val);
            printf("from %s\n", inet_ntoa(reply_addr));
            printf("Roundtrip time = %ld milliseconds\n", p_echo_reply->RoundTripTime);
        }
        else
        {
            printf("Error: no reply received\n");
        }
        
        free(reply_buffer);
        IcmpCloseHandle(h_icmp_file);
    }
    else
    {
        printf("Error: could not resolve hostname\n");
    }
    
    WSACleanup();
    return 0;
}