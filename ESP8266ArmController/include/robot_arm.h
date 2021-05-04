#pragma once

#include <Adafruit_PWMServoDriver.h>
#include "configuration.h"
#include "arm_structs.h"

#define PULSE_WIDTH_MIN       650
#define PULSE_WIDTH_MAX       2350
#define PWM_FREQUENCY         50

static const int arm_min[] = {SERVO_0_MIN, SERVO_1_MIN, SERVO_2_MIN,
                              SERVO_3_MIN, SERVO_4_MIN, SERVO_5_MIN,
                              SERVO_6_MIN };

static const int arm_max[] = {SERVO_0_MAX, SERVO_1_MAX, SERVO_2_MAX,
                              SERVO_3_MAX, SERVO_4_MAX, SERVO_5_MAX,
                              SERVO_6_MAX };

static const int resting[] = { SERVO_0_REST, SERVO_1_REST, SERVO_2_REST,
                               SERVO_3_REST, SERVO_4_REST, SERVO_5_REST,
                               SERVO_6_REST };

class RobotArm {
    public:
    RobotArm();
    ~RobotArm();

    bool SetPosition(ArmPosition* position);
    void Update();

    private:
    bool ValidRotation(ArmPosition* position);
    float GetR2Max(float r1);
    int PulseWidth(float angle);
    void ToPulseWidth(ArmPosition* position);

    Adafruit_PWMServoDriver pwm;
    ArmPosition current_position {};
    ArmPosition target_position {};
    PwmData servo_position {};
};