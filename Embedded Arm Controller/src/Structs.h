#pragma once

#include <Arduino.h>

struct ArmPosition
{
    int numArms;
    float arm[20];
};

struct PwmData
{
    int numServos;
    int servo[20];
};

