# RemoteArmController
Software to control a servo powered robot arm from a desktop application. It's designed for the [SainSmart 6-Axis Desktop Robotic Arm](https://www.sainsmart.com/products/6-axis-desktop-robotic-arm-assembled) but it should be usable with any other 6 axis servo powered arm by tweaking the arm length and degree of motion parameters. The board used is an ESP8266.

[Watch the video demo!](https://www.youtube.com/watch?v=ORoi-Ku_lN4)

## Desktop Application



### Input methods
- XInput Controller
- Vive Controller
- Leap Motion (Under construction)

## ESP8266

Communicates with the desktop application over UDP. Requires a PWM servo controller.

# Required Hardware

- [SainSmart 6-Axis Desktop Robotic Arm](https://www.sainsmart.com/products/6-axis-desktop-robotic-arm-assembled) or other similar 6 axis arm.

- ESP8266 Development Board: ([NodeMCU](https://www.amazon.com/s?k=esp8266+nodemcu&i=electronics), Adafruit Feather Huzzah, Wemos D1 and many others)
  - Note that the printed board pins numbers don't match the internal GPIO  pin numbers. Additionally, not all GPIO pins can be used for I2C. [More details here.](https://randomnerdtutorials.com/esp8266-pinout-reference-gpios/)

- [I2C PWM Servo Driver](https://www.adafruit.com/product/815)

- [5V Power Supply](https://smile.amazon.com/ALITOVE-Converter-100-240V-Transformer-5-5x2-5mm/dp/B0852HL336/)

# Setup

## Hardware

Connect the PWM servo driver to the ESP8266 board to by connecting the follwing pins:

- VIN (or 5V) on the ESP to V+ on the driver (the side pins).
- GND on the ESP to GND on the driver (the side pins).
- 3.3V on the ESP to VCC on the driver.
- D1 (GPIO 5) on the ESP to SCL on the driver.
- D2 (GPIO 4) on the ESP to SDA on the driver.

To power the ESP and driver, connect a 5V power supply to the screw terminals V+ and GNV. Do not power everything with USB


## Configuration

Edit `src/configuration.h` and provide your network ssid and password.

## Installation

(Under construction)

## Connecting

# Libraries

[Adafruit PCA9685 PWM Servo Driver Library](https://github.com/adafruit/Adafruit-PWM-Servo-Driver-Library)

