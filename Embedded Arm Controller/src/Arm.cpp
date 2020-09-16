#include <Arm.h>

void Arm::update(PositionData position)
{
    RotationData rotation = positionToRotation(position);
}

RotationData Arm::positionToRotation(PositionData position)
{
    translatePosition(&position);

    RotationData rotation;

    double xyDistance = hypot(position.xpos, position.ypos);
    double baseDistance = hypot(position.zpos, xyDistance);

    if (position.xpos == 0 && position.ypos == 0)
        rotation.r0 = 0;
    else
        rotation.r0 = atan2(position.xpos, position.ypos); // atan(x/y) measure angle from +y axis
    rotation.r1 = lawOfCosines(armLength0, baseDistance, armLength1);
    rotation.r2 = lawOfCosines(armLength0, armLength1, baseDistance);

    float xrot = position.xrot + pi - (rotation.r1 + rotation.r2);
    float yrot = position.yrot;
    float zrot = -position.zrot;

    double func = cos(xrot) * sin(zrot);
    double divpos = asin(func / (1 + func));
    double divneg = asin(func / (1 - func));
    rotation.r3 = (divpos + divneg) / 2;
    rotation.r4 = asin(cos(zrot) * sin(xrot));
    rotation.r5 = (divpos - divneg) / 2;
}

ServoData Arm::rotationToServo(RotationData rotation)
{
}

void Arm::translatePosition(PositionData *position)
{
    PositionData pos = *position;

    double xOffset = armLength2;
    xOffset *= cos(pos.xrot);
    xOffset *= sin(pos.zrot);

    double yOffset = armLength2;
    xOffset *= cos(pos.xrot);
    xOffset *= cos(pos.zrot);

    double zOffset = armLength2;
    xOffset *= sin(pos.xrot);
    xOffset *= cos(pos.zrot);

    pos.xpos += (float)xOffset;
    pos.ypos += (float)yOffset;
    pos.zpos += (float)zOffset;
}

void Arm::transformRotation(RotationData *rotation)
{
    RotationData rot = *rotation;
}

double Arm::lawOfCosines(double a, double b, double c)
{
    return acos((a * a + b * b - c * c) / 2 * a * b);
}