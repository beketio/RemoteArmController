#pragma once

#include <math.h>
#include <Configuration.h>
#include <DataStructs.h>

namespace
{

const double pi = 3.141592653589793;
const double pi_2 = 1.570796326794896;
const float pif = 3.1415927f;
const float pi_2f = 1.5707963f;

const ServoData minPos = {MIN_SERVO_0, MIN_SERVO_1, MIN_SERVO_2, MIN_SERVO_3, MIN_SERVO_4,
                          MIN_SERVO_5, MIN_SERVO_6, MIN_SERVO_7, MIN_SERVO_8, MIN_SERVO_9};
const ServoData maxPos = {MAX_SERVO_0, MAX_SERVO_1, MAX_SERVO_2, MAX_SERVO_3, MAX_SERVO_4,
                          MAX_SERVO_5, MAX_SERVO_6, MAX_SERVO_7, MAX_SERVO_8, MAX_SERVO_9};

const float armLength0 = ARM_LENGTH_0;
const float armLength1 = ARM_LENGTH_1;
const float armLength2 = ARM_LENGTH_2;

class Arm
{

public:
    void update(PositionData position);
    PositionData currentPosition();
    RotationData currentRotation();
    ServoData currentServo();

private:
    void translatePosition(PositionData *position);
    void transformRotation(RotationData *rotation);

    RotationData positionToRotation(PositionData position);
    ServoData rotationToServo(RotationData rotation);

    double lawOfCosines(double a, double b, double c);
};

} // namespace