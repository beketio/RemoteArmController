using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmController.Core.Arm;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;

namespace ArmController.Core.Input
{

    internal class GamepadMovement
    {
        public double XPos { get; set; }

        public double YPos { get; set; }

        public double ZPos { get; set; }

        public double Pitch { get; set; }

        public double Yaw { get; set; }

        public double Roll { get; set; }

        public double Tool0 { get; set; }

        public double Tool1 { get; set; }

        public bool Reset { get; set; }

        public InputData ToInputData()
        {
            double xRot = Trig.DegreeToRadian(Pitch);
            double yRot = Trig.DegreeToRadian(Yaw);
            double zRot = Trig.DegreeToRadian(Roll);

            // Rotations from -pi to pi
            if (Math.Abs(xRot) > Math.PI)
                xRot = LinearUtils.FullRotate(xRot);
            if (Math.Abs(yRot) > Math.PI)
                yRot = LinearUtils.FullRotate(yRot);
            if (Math.Abs(zRot) > Math.PI)
                zRot = LinearUtils.FullRotate(zRot);

            Matrix<double> xMat = LinearUtils.GetXRotationMatrix(xRot);
            Matrix<double> yMat = LinearUtils.GetYRotationMatrix(yRot);
            Matrix<double> zMat = LinearUtils.GetZRotationMatrix(zRot);

            InputData inputData = new InputData
            {
                Position = new Vector3D(XPos, YPos, ZPos),
                Rotation = xMat * yMat * zMat,
                Tool0 = this.Tool0,
                Tool1 = this.Tool1,
                Reset = this.Reset
            };

            return inputData;
        }
    }
}
