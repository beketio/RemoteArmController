#pragma once

#include <Adafruit_PWMServoDriver.h>
#include "configuration.h"
#include "arm_structs.h"

#define PULSE_WIDTH_MIN       650
#define PULSE_WIDTH_MAX       2350
#define PWM_FREQUENCY         50

static const int armMin[] = { SERVO_0_MIN, SERVO_1_MIN, SERVO_2_MIN,
                              SERVO_3_MIN, SERVO_4_MIN, SERVO_5_MIN,
                              SERVO_6_MIN };

static const int armMax[] = { SERVO_0_MAX, SERVO_1_MAX, SERVO_2_MAX,
                              SERVO_3_MAX, SERVO_4_MAX, SERVO_5_MAX,
                              SERVO_6_MAX };

static const int resting[] = { SERVO_0_REST, SERVO_1_REST, SERVO_2_REST,
                               SERVO_3_REST, SERVO_4_REST, SERVO_5_REST,
                               SERVO_6_REST };

class robot_arm {
    public:
    robot_arm();
    ~robot_arm();

    bool SetPosition(ArmPosition* position);
    void Update();

    private:
    bool ValidRotation(ArmPosition* position);
    int GetR2Max(int r1);
    int PulseWidth(float angle);
    void ToPulseWidth(ArmPosition* position);

    Adafruit_PWMServoDriver pwm;
    ArmPosition targetPosition {};
    PwmData servoPosition {};
    bool targetChanged = false;
};