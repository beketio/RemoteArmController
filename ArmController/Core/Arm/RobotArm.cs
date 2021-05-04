using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;

namespace ArmController.Core.Arm
{
    public class RobotArm
    {
        private const int MinYBuffer = 100;
        private const double MinY = -95;

        private readonly int[] min = { 0, 45, 10, 0, 0, 0 };
        private readonly int[] max = { 180, 170, 140, 180, 180, 180 };
        private readonly int[] resting = { 90, 160, 10, 90, 90, 90 };

        private readonly Stopwatch restingTimer = new Stopwatch();

        // private ICommunicator communicator;

        private int numJoints = 6;
        private int numTools = 1;
        private double[] armLength = { 220, 5, 255, 50 };


        public RobotArm()
        {
            restingTimer.Start();
        }

        public RotationData Update(InputData inputData)
        {
            if (inputData == null || inputData.Reset)
                return Update(GetResting());

            Vector3D position = inputData.Position;
            Matrix<double> rotation = inputData.Rotation;

            // Convert from meters to mm
            Vector3D mmPosition = position.ScaleBy(1000);

            double[] joints = new double[numJoints];
            double[] tools = new double[numTools];

            // Check to see it will hit floor. If within buffer distance, just set Y to floor. Otherwise, set invalid.
            if (mmPosition.Y < MinY)
                if (MinY - mmPosition.Y < MinYBuffer)
                    mmPosition = new Vector3D(mmPosition.X, MinY, mmPosition.Z);
                else
                    return Update(new RotationData(0, 0));

            // Check to see if tool will hit base, move forward if it will
            // TODO: Instead of adjusting the Z all the time, determine whether to adjust Z or X based on position
            if (Math.Abs(mmPosition.X) < 30 && mmPosition.Y < 100 && Math.Abs(mmPosition.Z) < 30)
                mmPosition = new Vector3D(mmPosition.X, mmPosition.Y, -30);

            // Get the position at the base of the tool
            Vector3D wristPosition = TranslatePosition(mmPosition, rotation);

            // Console.WriteLine("translated: " + wristPosition.ToString());

            // Check to see if the wrist will hit floor too
            if (wristPosition.Y < MinY)
                if (MinY - wristPosition.Y < MinYBuffer)
                    wristPosition = new Vector3D(wristPosition.X, MinY, wristPosition.Z);
                else
                    return Update(new RotationData(0, 0));

            // No need to check to see if the wrist hits the base (It's physically impossible)

            // old
            // double xzDistance = LinearUtils.Hypot(wristPosition.X, wristPosition.Z);
            // double baseDistance = LinearUtils.Hypot(wristPosition.Y, xzDistance);

            // Distance between base (origin) and position
            double baseDistance = wristPosition.Length;

            // Angle between XZ plane and position
            double baseAngle = Math.Asin(wristPosition.Y / baseDistance);

            // First joint: Angle of position from -Z axis
            joints[0] = Math.Atan2(wristPosition.X, -wristPosition.Z);

            //
            joints[1] = baseAngle + LinearUtils.LawOfCosines(armLength[0], baseDistance, armLength[2]);
            joints[2] = LinearUtils.LawOfCosines(armLength[0], armLength[2], baseDistance) - (Math.PI - joints[1]);

            // Adjust rotation matrix to account for wrist angle
            double yrot = joints[0];
            Matrix<double> ymat = LinearUtils.GetYRotationMatrix(yrot);
            double xrot = -joints[2];
            Matrix<double> xmat = LinearUtils.GetXRotationMatrix(xrot);
            Matrix<double> fixedRot = xmat * ymat * rotation; // Might need to reverse order?

            // Get ZXZ Euler angles from adjusted rotationa matrix
            Vector3D[] euler = LinearUtils.RotationToEulerZXZ(fixedRot);

            // Determine best euler
            if (euler.Length > 1)
            {
                if (Math.Abs(euler[0].Y) < Math.PI / 2)
                {
                    joints[3] = euler[0].X; // Z
                    joints[4] = euler[0].Y; // X'
                    joints[5] = euler[0].Z; // Z''
                }
                else if (Math.Abs(euler[1].Y) < Math.PI / 2)
                {
                    joints[3] = euler[1].X; // Z
                    joints[4] = euler[1].Y; // X'
                    joints[5] = euler[1].Z; // Z''
                }
            }
            else
            {
                joints[3] = euler[0].X; // Z
                joints[4] = euler[0].Y; // X'
                joints[5] = euler[0].Z; // Z''
            }

            // Map Z to (-pi .. pi)
            if (Math.Abs(joints[3]) > Math.PI)
            {
                joints[3] = LinearUtils.FullRotate(joints[3]);
            }

            // If Z'' is positive, rotate Z
            // I have no idea why but this works so don't touch it
            if (joints[5] > 0)
                joints[3] = LinearUtils.HalfRotate(joints[3]);

            // If upside down, map Z to (-pi/2 .. pi/2) and flip X'
            if (Math.Abs(joints[3]) > Math.PI / 2)
            {
                joints[3] = LinearUtils.HalfRotate(joints[3]);
                joints[4] = -joints[4];
            }

            // Map Z'' to (-pi .. pi)
            if (Math.Abs(joints[5]) > Math.PI)
                joints[5] = LinearUtils.FullRotate(joints[5]);

            // And Map Z'' to (-pi/2 .. pi/2) This is because the gripper works upside down
            if (Math.Abs(joints[5]) > Math.PI / 2)
                joints[5] = LinearUtils.HalfRotate(joints[5]);

            // Console.WriteLine("Euler Fix: " + joints[3] + ", " + joints[4] + ", " + joints[5]);
            RotationData data = GetRotationData(joints, tools);

            return Update(data);
        }

        // Remove at some point
        public RotationData Update(Vector3D position, Matrix<double> rotation)
        {
            InputData data = new InputData
            {
                Position = position,
                Rotation = rotation
            };

            return Update(data);
        }

        /*private void oldcode()
        {
            double sign = joints[3];

            Vector3D axisAngle = LinearUtils.RotationMatrixToAxisAngle(rotation);

            //joints[3] = LinearUtils.HalfRotate(joints[3]);

            if (Math.Abs(joints[3]) > Math.PI / 2)
            {
                joints[3] = LinearUtils.HalfRotate(joints[3]);
                joints[5] = LinearUtils.HalfRotate(joints[5]);
            }

            if (axisAngle.X < 0)
                joints[4] = -joints[4];

            if (Math.Abs(joints[5]) > Math.PI / 2)
                joints[5] = LinearUtils.HalfRotate(joints[5]);

            joints[3] = -joints[3];
            joints[5] = -joints[5];

            /*joints[0] = 0;
            for (int i = 1; i < 3; i++)
                joints[i] = Trig.DegreeToRadian(resting[i]);



            /*if (joints[5] > 0)
            {
                joints[5] = Math.PI - joints[5];
                // joints[4] = -joints[4];
            }
            else if (joints[5] < 0)
            {
                joints[5] = Math.PI + joints[5];
                //joints[4] = -joints[4];
            }

            /*joints[3] = -joints[3];

            if (joints[3] > Math.PI / 2)
            {
                joints[3] -= Math.PI;
               // joints[4] = -joints[4];
            }
            else if (joints[3] < -Math.PI / 2)
            {
                joints[3] += Math.PI;
                //joints[4] = -joints[4];
            }

            if (joints[5] > Math.PI / 2)
            {
                joints[5] -= Math.PI;
                // joints[4] = -joints[4];
            }
            else if (joints[5] < -Math.PI / 2)
            {
                joints[5] += Math.PI;
                //joints[4] = -joints[4];
            }
            /*
            // Arm still works upside down
            if (joints[5] > Math.PI / 2)
                joints[5] -= Math.PI;
            else if (joints[5] < -Math.PI / 2)
                joints[5] += Math.PI;
        }*/

        private RotationData Update(RotationData data)
        {
            if (RotationIsValid(data))
            {
               // communicator?.Update(data);
                // Console.WriteLine("update");
                // Console.WriteLine(data);
                restingTimer.Restart();
                return data;
            }
            else if (restingTimer.ElapsedMilliseconds > 2000)
            {
                // communicator?.Update(GetResting());
                restingTimer.Reset();
                return GetResting();
            }
            else
            {
                // Console.WriteLine("invalid");
                // Console.WriteLine(data);
                return null;
            }
        }

        private Vector3D TranslatePosition(Vector3D position, Matrix<double> rotation)
        {
            if (armLength.Length < 4 || armLength[3] == 0)
                return position;
            Vector3D delta = new Vector3D(0, 0, armLength[3]);
            delta = delta.TransformBy(rotation);
            return position + delta;
        }

        private RotationData GetRotationData(double[] joints, double[] tools)
        {
            RotationData data = new RotationData(numJoints, numTools); // 6 Joints, 2 Tools

            double[] newJoint = new double[numJoints];

            newJoint[0] = -joints[0] + (Math.PI / 2);
            newJoint[1] = joints[1]; // Probably needs to be adjusted
            newJoint[2] = -joints[2] + Trig.DegreeToRadian(15); // Needs to be adjusted
            newJoint[3] = -joints[3] + (Math.PI / 2);
            newJoint[4] = joints[4] + (Math.PI / 2); // Needs to be adjusted
            newJoint[5] = -joints[5] + (Math.PI / 2);
            // newJoint[6] = joints[6] + Math.PI / 2; // Needs to be adjusted

            for (int i = 0; i < numJoints; i++)
                data.SetJoint(i, Math.Round(Trig.RadianToDegree(newJoint[i]), 2));

            return data;
        }

        private bool RotationIsValid(RotationData data)
        {
            if (data.GetNumJoints() < numJoints || data.GetNumTools() < numTools)
                return false;

            // Return if out of bounds
            for (int i = 0; i < numJoints; i++)
            {
                float joint = data.GetJoint(i);
                if (float.IsNaN(joint))
                    return false;
                else if (joint < min[i])
                    if (min[i] - joint < 5)
                        data.SetJoint(i, min[i]); // If within 5 degrees, just set to min
                    else
                        return false;
                else if (joint > max[i])
                    if (joint - max[i] < 5)
                        data.SetJoint(i, max[i]); // If within 5 degrees, just set to max
                    else
                        return false;
            }

            // Check special case
            float j2max = 180 - data.GetJoint(1);
            if (data.GetJoint(2) > j2max)
                return false;

            return true;
        }

        private RotationData GetResting()
        {
            RotationData data = new RotationData(numJoints, numTools);
            for (int i = 0; i < numJoints; i++)
                data.SetJoint(i, resting[i]);
            return data;
        }


    }
}
