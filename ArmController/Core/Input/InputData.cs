using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;

namespace ArmController.Core
{
    public class InputData
    {
        public Vector3D Position { get; set; }

        public Matrix<double> Rotation { get; set; } = Matrix<double>.Build.DenseIdentity(3);

        public double Tool0 { get; set; }

        public double Tool1 { get; set; }

        public bool Reset { get; set; }
    }
}