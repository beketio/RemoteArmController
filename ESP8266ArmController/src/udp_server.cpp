#include <udp_server.h>

UdpServer::UdpServer() {
	strcpy(header, "    ");
	header[header_size] = '\0';
}

UdpServer::~UdpServer() {
    udp_client.stop();
}

void UdpServer::Setup(int port)
{
    udp_client.begin(port);
}

int UdpServer::NextPacket() {
	for(int i = 0; i < header_size; i ++)
		header[i] = 0;
	int packetSize = udp_client.parsePacket();
	if (packetSize)
	{
        udp_client.read(udp_packet, udp_packet_buffer_size);
		memcpy(header, udp_packet, header_size);
		header[header_size] = '\0';
		if (strcmp(header, check_alive) == 0) {
			Serial.println("Check alive!");
			byte pacc[header_size];
			memcpy(pacc, return_alive, header_size);
            udp_client.beginPacket(udp_client.remoteIP(), udp_client.remotePort());
            udp_client.write(pacc, header_size);
            udp_client.endPacket();
		}
		else if (strcmp(header, header_start) == 0) {
			Serial.printf("start!: %s\n", header);
			//char version[5];
			//for (int i = 0; i < 4; i++) {
			//	header[i] = udpPacket[i];
			//}
			uint8_t packet[header_size];
			memcpy(packet, return_start, header_size);
            udp_client.beginPacket(udp_client.remoteIP(), udp_client.remotePort());
            udp_client.write(packet, header_size);
            udp_client.endPacket();
            last_packet_num = 0xFF;
		}
		else {
			uint8_t packetNum = udp_packet[header_size];
			// Check if within 64 packets of last
			uint8_t diff = packetNum - last_packet_num;
			if (diff > 64) {
				Serial.println("Out of order packet received, discarding!");
				Serial.print(packetNum);
				Serial.print(" last: ");
				Serial.print(last_packet_num);
				Serial.print(" diff: ");
				Serial.println(diff);
                udp_client.flush();
				return 0;
			}
			last_packet_num = packetNum;
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
	return &udp_packet[header_size + 1];
}