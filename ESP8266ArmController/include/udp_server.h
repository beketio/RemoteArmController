#pragma once

#include <Arduino.h>
#include <WiFiUdp.h>
#include <cstring>

#ifndef UDP_PACKET_BUFFER_SIZE
#define UDP_PACKET_BUFFER_SIZE 500
#endif

static const int udp_packet_buffer_size = UDP_PACKET_BUFFER_SIZE;
static const int header_size = 4;
static const char *header_start = "strt";
static const char *header_stop = "stop";
static const char *return_start = "redy";
static const char *check_alive = "aliv";
static const char *return_alive = "yalv";

class UdpServer {

public: 
UdpServer();
~UdpServer();

void Setup(int port);
int NextPacket();
char *GetHeader();
uint8_t *GetData();

private:
WiFiUDP udp_client;
char header[header_size + 1]{};
uint8_t udp_packet[udp_packet_buffer_size]{};
uint8_t last_packet_num = 0xFF;

};