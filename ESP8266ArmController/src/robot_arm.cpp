#include <robot_arm.h>

RobotArm::RobotArm()
{
    pwm = Adafruit_PWMServoDriver(DRIVER_I2C_ADDRESS);
    pwm.begin();
    pwm.setPWMFreq(PWM_FREQUENCY);
    
    ArmPosition resting_position {};
    resting_position.num_arms = 7;
    for(int i = 0; i < 7; i++)
        resting_position.arm_pos[i] = (float) resting[i];
    SetPosition(&resting_position);
}

RobotArm::~RobotArm()
= default;

bool RobotArm::SetPosition(ArmPosition* position)
{
    if(!ValidRotation(position))
        return false;
    //ToPulseWidth(position);
    target_position.num_arms = position->num_arms;
    for(int i = 0; i < position->num_arms; i++)
    {
        target_position.arm_pos[i] = position->arm_pos[i];
    }
    return true;
}

void RobotArm::Update()
{
    bool pos_changed = false;
    current_position.num_arms = target_position.num_arms;
    for(int i = 0; i < current_position.num_arms; i++)
    {
        // TODO: velocity, acceleration, and jerk control
        if(current_position.arm_pos[i] != target_position.arm_pos[i])
        {
            current_position.arm_pos[i] = target_position.arm_pos[i];
            pos_changed = true;
        }
    }
    if(pos_changed)
    {
        ToPulseWidth(&current_position);
        for(int i = 0; i < servo_position.num_servos; i++)
            pwm.setPWM(i, 0, servo_position.servo_pwm[i]);
    }
}

bool RobotArm::ValidRotation(ArmPosition* position)
{
    ArmPosition pos = *position;
    for(int i = 0; i < pos.num_arms; i++)
        if(pos.arm_pos[i] > (float) arm_max[i] || pos.arm_pos[i] < (float) arm_min[i])
            return false;
    
    float r2max = GetR2Max(pos.arm_pos[1]);
    if(pos.arm_pos[2] > r2max)
        return false;
    
    return true;
}

float RobotArm::GetR2Max(float r1)
{
    return 180.0f - r1;
}

int RobotArm::PulseWidth(float angle)
{
    float pulse_wide = (angle * (PULSE_WIDTH_MAX - PULSE_WIDTH_MIN) / 180.0f) + PULSE_WIDTH_MIN;
    return int(pulse_wide / 1000000 * PWM_FREQUENCY * 4096);
}

void RobotArm::ToPulseWidth(ArmPosition* position)
{
    servo_position.num_servos = position->num_arms;
    for(int i = 0; i < position-> num_arms; i++)
        servo_position.servo_pwm[i] = PulseWidth(position->arm_pos[i]);
}