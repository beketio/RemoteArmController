using ArmController.Core.Arm;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmController.Core
{

    class GamepadMovement : IGamepadInputListener
    {
        private double xPos;
        private double yPos;
        private double zPos;
        private double pitch;
        private double yaw;
        private double roll;
        private double tool0;
        private double tool1;

        private double newXPos;
        private double newYPos;
        private double newZPos;
        private double newPitch;
        private double newYaw;
        private double newRoll;
        private double newTool0;
        private double newTool1;

        private double minX;
        private double maxX;
        private double minY;
        private double maxY;
        private double minZ;
        private double maxZ;

        private RobotArm arm;

        public GamepadMovement(double minX, double maxX, double minY, double maxY, double minZ, double maxZ)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
            this.minZ = minZ;
            this.maxZ = maxZ;
         
        }

        public GamepadMovement(RobotArm arm)
        {
            this.arm = arm;
        }

        public double XPos
        {
            get { return xPos; }
            set { newXPos = value; }
        }

        public double YPos
        {
            get { return yPos; }
            set { newYPos = value; }
        }

        public double ZPos
        {
            get { return zPos; }
            set { newZPos = value; }
        }

        public double Pitch
        {
            get { return pitch; }
            set { newPitch = value; }
        }

        public double Yaw
        {
            get { return yaw; }
            set { newYaw = value; }
        }

        public double Roll
        {
            get { return roll; }
            set { newRoll = value; }
        }

        public double Tool0
        {
            get { return tool0; }
            set { newTool0 = value; }
        }

        public double Tool1
        {
            get { return tool1; }
            set { newTool1 = value; }
        }

        public void update()
        {
            bool valid = false;
            if(arm != null)
            {
                Vector3D position = new Vector3D(newXPos, newYPos, newZPos);

                double xRot = Trig.DegreeToRadian(newPitch);
                double yRot = Trig.DegreeToRadian(newYaw);
                double zRot = Trig.DegreeToRadian(newRoll);
                // The roll loops
                if (Math.Abs(zRot) > Math.PI)
                    zRot = LinearUtils.FullRotate(zRot);

                Matrix<double> xMat = LinearUtils.GetXRotationMatrix(xRot);
                Matrix<double> yMat = LinearUtils.GetYRotationMatrix(yRot);
                Matrix<double> zMat = LinearUtils.GetZRotationMatrix(zRot);

                Matrix<double> rotation = xMat * yMat * zMat;

                //valid = arm.Update(position, rotation);
            }
            else
            {
                if (newXPos < minX)
                    newXPos = minX;
                if (newXPos > maxX)
                    newXPos = maxX;
                if (newYPos < minY)
                    newYPos = minY;
                if (newYPos > maxY)
                    newYPos = maxY;
                if (newZPos < minZ)
                    newZPos = minZ;
                if (newZPos > maxZ)
                    newZPos = maxZ;

                if (newPitch > 180)
                    newPitch = 180;
                if (newPitch < -180)
                    newPitch = -180;
                if (newYaw > 180)
                    newYaw -= 360;
                if (newYaw < -180)
                    newYaw += 360;
                if (newRoll > 180)
                    newRoll -= 360;
                if (newRoll < -180)
                    newRoll += 360;

                if (newTool0 > 1)
                    newTool0 = 1;
                if (newTool0 < 0)
                    newTool0 = 0;
                if (newTool1 > 1)
                    newTool1 = 1;
                if (newTool1 < 0)
                    newTool1 = 0;

                valid = true;

            }
            if(valid)
            {
                xPos = newXPos;
                yPos = newYPos;
                zPos = newZPos;
                pitch = newPitch;
                yaw = newYaw;
                roll = newRoll;
                tool0 = newTool0;
                tool1 = newTool1;
            }
        }

        
    }
}
