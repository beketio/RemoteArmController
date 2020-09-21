#include <Arm.h>

Arm::Arm()
{
    pwm.begin();
    pwm.setPWMFreq(FREQUENCY);
    
    ArmPosition restingPosition;
    restingPosition.numArms = 7;
    for(int i = 0; i < 7; i++)
        restingPosition.arm[i] = resting[i];
    SetPosition(&restingPosition);
}

Arm::~Arm()
{
    
}

bool Arm::SetPosition(ArmPosition* position)
{
    if(!ValidRotation(position))
        return false;
    ToPulseWidth(position);
    targetChanged = true;
    
}

void Arm::Update()
{
    if(targetChanged)
    {
        for(int i = 0; i < servoPosition.numServos; i++)
            pwm.setPWM(i, 0, servoPosition.servo[i]);
        targetChanged = false;
    }
}

bool Arm::ValidRotation(ArmPosition* position)
{
    ArmPosition pos = *position;
    for(int i = 0; i < pos.numArms; i++)
        if(pos.arm[i] > armMax[i] || pos.arm[i] < armMin[i])
            return false;
    
    int r2max = GetR2Max(pos.arm[1]);
    if(pos.arm[2] > r2max)
        return false;
    
    return true;
}

int Arm::GetR2Max(int r1)
{
    return 180 - r1;
}

int Arm::PulseWidth(float angle)
{
    float pulseWide = (angle) * (MAX_PULSE_WIDTH - MIN_PULSE_WIDTH) / 180.0f + MIN_PULSE_WIDTH;
    return int(pulseWide / 1000000 * FREQUENCY * 4096);
}

void Arm::ToPulseWidth(ArmPosition* position)
{
    servoPosition.numServos = position->numArms;
    for(int i = 0; i < position-> numArms; i++)
        servoPosition.servo[i] = PulseWidth(position->arm[i]);
}