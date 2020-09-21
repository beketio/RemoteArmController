#pragma once

#include <Arduino.h>
#include <WiFiUdp.h>

static const int udp_port = 4210;
static const int header_size = 4;
static const char* header_start = "strt";
static const char* header_stop = "stop";
static const char* return_start = "redy";
static const char* check_alive = "aliv";
static const char* return_alive = "yalv";

class Udper {

public: 
Udper(int port, bool debug);
~Udper();

void Setup();
int NextPacket();
char* GetHeader();
byte* GetData();

private:
WiFiUDP udpClient;
char header[5];
byte udpPacket[500];
byte lastPacketNum = 255;


};