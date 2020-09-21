#include <Arduino.h>
#include <Wire.h>

#include <ESP8266WiFi.h>
//#include <ESP8266mDNS.h>
#include <ArduinoOTA.h>

#include "Configuration.h"
#include "Arm.h"
#include "UdpServer.h"

const bool debug = true;

// WIFI
const char* wifi_ssid = WIFI_SSID;
const char* wifi_password = WIFI_PASSWORD;

// OTA
const char* ota_name = OTA_NAME;
const char* ota_password = OTA_PASSWORD;
const int ota_port = 8266;

void SetupWifi();
void SetupOta();
float FromBytes(byte* source);

//IPAddress ipAddress;
//WiFiClient wifiClient;

Arm arm = Arm();
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
    int dataSize = udp.NextPacket();
    char* header = udp.GetHeader();

    static unsigned long lastTime = 0;

    if(dataSize && strcmp(header, "data") == 0)
    {
        uint8_t * data = udp.GetData();
        int narms = data[0];
        int ntools = data[1];
        ArmPosition pos {};
        pos.numArms = narms + ntools;
        data = &data[2];
        int actualSize = min(pos.numArms * 4, dataSize - 2);
        int armi = 0;
        for(int i = 0; i < actualSize; i += 4)
            pos.arm[armi++] = FromBytes(&data[i]);
        bool valid = arm.SetPosition(&pos);
        unsigned long currentTime = millis();
        // only print once per second
        if(currentTime - lastTime > 1000)
        {
            if(valid)
            {
                Serial.print("Pos: ");
                for(int i = 0; i < pos.numArms; i++)
                {
                    Serial.print(pos.arm[i]);
                    Serial.print(", ");
                }
                Serial.println();
            }
            else
            {
                Serial.println("Invalid!");
                for(int i = 0; i < pos.numArms; i++)
                {
                    Serial.print(pos.arm[i]);
                    Serial.print(", ");
                }
            }
            lastTime = currentTime;
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

float FromBytes(byte* source)
{
	float *fp = (float*) source;
	float f = *fp;
	return f;
}
