#include <Arduino.h>
#include <Wire.h>

#include <ESP8266WiFi.h>
//#include <ESP8266mDNS.h>
#include <ArduinoOTA.h>

#include "configuration.h"
#include "robot_arm.h"
#include "udp_server.h"

const bool debug = true;

// WIFI
const char* wifi_ssid = WIFI_SSID;
const char* wifi_password = WIFI_PASSWORD;

// OTA
const char* ota_name = OTA_NAME;
const char* ota_password = OTA_PASSWORD;
const int ota_port = OTA_PORT;

void SetupWifi();
void SetupOta();
float FromBytes(const byte* source);

//IPAddress ipAddress;
//WiFiClient wifiClient;

RobotArm arm = RobotArm();
UdpServer udp = UdpServer();

void setup() {
    Serial.begin(115200);
    Serial.println("\nBegin");
    SetupWifi();
    SetupOta();
    udp.Setup(UDP_PORT);
}

void loop() {
    if (WiFi.status() != WL_CONNECTED) {
        delay(10);
        Serial.print("WIFI Disconnected. Attempting reconnection.");
        SetupWifi();
        return;
    }
    ArduinoOTA.handle();
    yield();
    int data_size = udp.NextPacket();
    char* header = udp.GetHeader();

    static unsigned long last_time = 0;

    if(data_size && strcmp(header, "data") == 0)
    {
        uint8_t *data = udp.GetData();
        int num_arms = data[0];
        int num_tools = data[1];
        ArmPosition received_position {};
        received_position.num_arms = num_arms + num_tools;
        data = &data[2];
        int actual_size = min(received_position.num_arms * 4, data_size - 2);
        int arm_index = 0;
        for(int i = 0; i < actual_size; i += 4)
            received_position.arm_pos[arm_index++] = FromBytes(&data[i]);
        bool valid_position = arm.SetPosition(&received_position);
        unsigned long current_time = millis();
        // only print once per second
        if(current_time - last_time > 1000)
        {
            if(valid_position)
            {
                Serial.print("Pos: ");
                for(int i = 0; i < received_position.num_arms; i++)
                {
                    Serial.print(received_position.arm_pos[i]);
                    Serial.print(", ");
                }
                Serial.println();
            }
            else
            {
                Serial.println("Invalid!");
                for(int i = 0; i < received_position.num_arms; i++)
                {
                    Serial.print(received_position.arm_pos[i]);
                    Serial.print(", ");
                }
            }
            last_time = current_time;
        }
    }
    arm.Update();
}

void SetupWifi() {

	delay(10);
	// We start by connecting to a WiFi network
	if (debug) {
		Serial.println();
		Serial.print("Connecting to ");
		Serial.println(wifi_ssid);
	}

	WiFi.mode(WIFI_STA);
	WiFi.begin(wifi_ssid, wifi_password);

	while (WiFi.status() != WL_CONNECTED) {
		delay(500);
		Serial.print(".");
	}

	// ipAddress = WiFi.localIP();

	if (debug) {
		Serial.println();
		Serial.println("WiFi connected");
		Serial.print("IP address: ");
		Serial.println(WiFi.localIP());
	}
}

void SetupOta() {
	ArduinoOTA.setPort(ota_port);
	ArduinoOTA.setHostname(ota_name);
	ArduinoOTA.setPassword(ota_password);
	ArduinoOTA.onStart([]() {
		Serial.println("Starting");
	});
	ArduinoOTA.onEnd([]() {
		Serial.println("\nEnd");
	});
	ArduinoOTA.onProgress([](unsigned int progress, unsigned int total) {
		Serial.printf("Progress: %u%%\r", (progress / (total / 100)));
	});
	ArduinoOTA.onError([](ota_error_t error) {
		Serial.printf("Error[%u]: ", error);
		if (error == OTA_AUTH_ERROR) Serial.println("Auth Failed");
		else if (error == OTA_BEGIN_ERROR) Serial.println("Begin Failed");
		else if (error == OTA_CONNECT_ERROR) Serial.println("Connect Failed");
		else if (error == OTA_RECEIVE_ERROR) Serial.println("Receive Failed");
		else if (error == OTA_END_ERROR) Serial.println("End Failed");
	});
	ArduinoOTA.begin();

	if (debug) {
		Serial.print("Setup OTA: ");
		Serial.print(ota_name);
		Serial.print(":");
		Serial.println(ota_port);
	}
}

float FromBytes(const byte* source)
{
	return *((float*) source);
}
