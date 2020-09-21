#include <Arduino.h>
#include <Wire.h>

#include <ESP8266WiFi.h>
#include <ESP8266mDNS.h>
#include <ArduinoOTA.h>
 
#include <Arm.h>
#include <Udper.h>

const bool debug = true;

//WIFI
const char* wifi_ssid = "ssid"; //type your WIFI information inside the quotes
const char* wifi_password = "password";

//OTA
const char* ota_name = "Robot Arm";
const char* ota_password = "ota password";
const int ota_port = 8266;

const int udp_porty = 4210;

IPAddress ipAddress;
WiFiClient wifiClient;

Arm arm = Arm();
Udper udp = Udper(udp_porty, true);



/********************************** START SETUP WIFI*****************************************/
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

	ipAddress = WiFi.localIP();

	if (debug) {
		Serial.println();
		Serial.println("WiFi connected");
		Serial.print("IP address: ");
		Serial.println(WiFi.localIP());
	}
}

/********************************** START SETUP OTA*****************************************/
void SetupOta() {
	//OTA SETUP
	ArduinoOTA.setPort(ota_port);
	// Hostname defaults to esp8266-[ChipID]
	ArduinoOTA.setHostname(ota_name);

	// No authentication by default
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


void setup() {
  Serial.begin(115200);
  Serial.println("\nBegin");
  SetupWifi();
  SetupOta();
  udp.Setup();
}

float FromBytes(byte* source)
{
	float *fp = (float*)source;
	float f = *fp;
	return f;
}

long lastTime = 0;

/********************************** START MAIN LOOP*****************************************/
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

	if(dataSize && strcmp(header, "data") == 0)
	{
		byte* data = udp.GetData();
		int narms = data[0];
		int ntools = data[1];
		ArmPosition pos;
		pos.numArms = narms + ntools;
		data = &data[2];
		int actualSize = min(pos.numArms * 4, dataSize - 2);
		int armi = 0;
		for(int i = 0; i < actualSize; i += 4)
		pos.arm[armi++] = FromBytes(&data[i]);
		bool valid = arm.SetPosition(&pos);
		long currentTime = millis();
		//only print once per second
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