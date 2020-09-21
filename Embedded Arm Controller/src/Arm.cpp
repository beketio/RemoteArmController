#include <Arm.h>

Arm::Arm()
{
    pwm = Adafruit_PWMServoDriver();
    pwm.begin();
    pwm.setPWMFreq(PWM_FREQUENCY);
    
    ArmPosition restingPosition {};
    restingPosition.numArms = 7;
    for(int i = 0; i < 7; i++)
        restingPosition.arm[i] = (float) resting[i];
    SetPosition(&restingPosition);
}

Arm::~Arm()
= default;

bool Arm::SetPosition(ArmPosition* position)
{
    if(!ValidRotation(position))
        return false;
    ToPulseWidth(position);
    targetChanged = true;
    return true;
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
        if(pos.arm[i] > (float) armMax[i] || pos.arm[i] < (float) armMin[i])
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
    float pulseWide = (angle * (PULSE_WIDTH_MAX - PULSE_WIDTH_MIN) / 180.0f) + PULSE_WIDTH_MIN;
    return int(pulseWide / 1000000 * PWM_FREQUENCY * 4096);
}

void Arm::ToPulseWidth(ArmPosition* position)
{
    servoPosition.numServos = position->numArms;
    for(int i = 0; i < position-> numArms; i++)
        servoPosition.servo[i] = PulseWidth(position->arm[i]);
}