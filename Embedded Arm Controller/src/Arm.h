#pragma once

#include <Adafruit_PWMServoDriver.h>
#include <Structs.h>


#define MIN_PULSE_WIDTH       650
#define MAX_PULSE_WIDTH       2350
#define DEFAULT_PULSE_WIDTH   1500
#define FREQUENCY             50

static const int armMin[] = {0, 45, 10, 0, 0, 0, 0};
static const int armMax[] = {180, 170, 140, 180, 180, 180, 97};
static const int resting[] = { 90, 160, 10, 90, 90, 90, 0 };

class Arm {
    public:
    Arm();
    ~Arm();

    bool SetPosition(ArmPosition* position);
    void Update();

    private:
    bool ValidRotation(ArmPosition* position);
    int GetR2Max(int r1);
    int PulseWidth(float angle);
    void ToPulseWidth(ArmPosition* position);

    Adafruit_PWMServoDriver pwm = Adafruit_PWMServoDriver();
    ArmPosition targetPosition;
    PwmData servoPosition;
    bool targetChanged = false;

};