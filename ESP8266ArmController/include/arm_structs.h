#pragma once

struct ArmPosition
{
    int num_arms;
    float arm_pos[10];
};

struct PwmData
{
    int num_servos;
    int servo_pwm[10];
};