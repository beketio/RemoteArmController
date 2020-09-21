#include <UdpServer.h>

UdpServer::UdpServer() {
	strcpy(header, "    ");
	header[header_size] = '\0';
}

UdpServer::~UdpServer() {
	udpClient.stop();
}

void UdpServer::Setup(int port)
{
	udpClient.begin(port);
}

int UdpServer::NextPacket() {
	for(int i = 0; i < header_size; i ++)
		header[i] = 0;
	int packetSize = udpClient.parsePacket();
	if (packetSize)
	{
		udpClient.read(udpPacket, udp_packet_buffer_size);
		memcpy(header, udpPacket, header_size);
		header[header_size] = '\0';
		if (strcmp(header, check_alive) == 0) {
			Serial.println("Check alive!");
			byte pacc[header_size];
			memcpy(pacc, return_alive, header_size);
			udpClient.beginPacket(udpClient.remoteIP(), udpClient.remotePort());
			udpClient.write(pacc, header_size);
			udpClient.endPacket();
		}
		else if (strcmp(header, header_start) == 0) {
			Serial.printf("start!: %s\n", header);
			//char version[5];
			//for (int i = 0; i < 4; i++) {
			//	header[i] = udpPacket[i];
			//}
			uint8_t packet[header_size];
			memcpy(packet, return_start, header_size);
			udpClient.beginPacket(udpClient.remoteIP(), udpClient.remotePort());
			udpClient.write(packet, header_size);
			udpClient.endPacket();
			lastPacketNum = 0xFF;
		}
		else {
			uint8_t packetNum = udpPacket[header_size];
			// Check if within 64 packets of last
			uint8_t diff = packetNum - lastPacketNum;
			if (diff > 64) {
				Serial.println("Out of order packet received, discarding!");
				Serial.print(packetNum);
				Serial.print(" last: ");
				Serial.print(lastPacketNum);
				Serial.print(" diff: ");
				Serial.println(diff);
				udpClient.flush();
				return 0;
			}
			lastPacketNum = packetNum;
			return packetSize - header_size - 1;
		}
	}
	return 0;
}

char *UdpServer::GetHeader()
{
	return header;
}

uint8_t *UdpServer::GetData()
{
	return &udpPacket[header_size + 1];
}