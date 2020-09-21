#include <Udper.h>

/********************************** START SETUP UDP*****************************************/
Udper::Udper(int port, bool debug) {
	if (debug) {
		Serial.print("Setup UDP on port ");
		Serial.println(udp_port);
	}
	header[4] = '\0';
}

Udper::~Udper() {
	udpClient.stop();
}

void Udper::Setup()
{
	udpClient.begin(udp_port);
}

int Udper::NextPacket() {
	for(int i = 0; i < 4; i ++)
		header[i] = 0;
	int packetSize = udpClient.parsePacket();
	if (packetSize)
	{
		udpClient.read(udpPacket, 500);
		for (int i = 0; i < 4; i++) {
			header[i] = udpPacket[i];
		}
		header[4] = '\0';
		//Serial.print("Pac rec, header: ");
		//Serial.println(header);
		if (strcmp(header, check_alive) == 0) {
			Serial.println("CHeck alive!");
			byte pacc[header_size];
			int j = 0;
			for (int i = 0; i < header_size; i++) {
				pacc[j++] = return_alive[i];
			}
			//Serial.println((char*)pacc);
			udpClient.beginPacket(udpClient.remoteIP(), udpClient.remotePort());
			udpClient.write(pacc, j);
			udpClient.endPacket();
		}
		else if (strcmp(header, header_start) == 0) {
			Serial.printf("start!: %s\n", header);
			//char version[5];
			//for (int i = 0; i < 4; i++) {
			//	header[i] = udpPacket[i];
			//}
			byte packet[header_size];
			int j2 = 0;
			for (int i = 0; i < header_size; i++) {
				packet[j2++] = return_start[i];
			}
			udpClient.beginPacket(udpClient.remoteIP(), udpClient.remotePort());
			udpClient.write(packet, j2);
			udpClient.endPacket();
			lastPacketNum = 255;
		}
		else {
			byte packetNum = udpPacket[4];
			// Check if within 64 packets of last
			byte diff = packetNum - lastPacketNum;
			if (diff > 64) {
				Serial.println("Out of order packet recieved, discarding!");
				
				Serial.print(packetNum);
				Serial.print(" last: ");
				Serial.print(lastPacketNum);
				Serial.print(" diff: ");
				Serial.println(diff);
				udpClient.flush();
				return 0;
			}
			lastPacketNum = packetNum;
			//udpClient.flush();
			return packetSize - header_size - 1;
		}
	}
	//udpClient.flush();
	return 0;
}

char* Udper::GetHeader()
{
	return header;
}


byte* Udper::GetData()
{
	return &udpPacket[header_size + 1];
}