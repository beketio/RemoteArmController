using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Text;
using Valve.VR;

namespace ArmController.Core
{
    class LinearUtils
    {
        public static Vector3D ToPosition(HmdMatrix34_t mat)
        {
            return new Vector3D(mat.m3, mat.m7, mat.m11);
        }


        public static Matrix<double> ToRotation(HmdMatrix34_t mat)
        {
            Matrix<double> rotation = Matrix<double>.Build.Dense(3, 3);
            rotation[0, 0] = mat.m0;
            rotation[0, 1] = mat.m1;
            rotation[0, 2] = mat.m2;
            rotation[1, 0] = mat.m4;
            rotation[1, 1] = mat.m5;
            rotation[1, 2] = mat.m6;
            rotation[2, 0] = mat.m8;
            rotation[2, 1] = mat.m9;
            rotation[2, 2] = mat.m10;
            return rotation;
        }

        public static bool IsRotationMatrix(Matrix<double> matrix)
        {
            if (!(matrix.RowCount == 3 && matrix.ColumnCount == 3))
                return false;
            return ((matrix[0, 2] != 0 || matrix[1, 2] != 0 || matrix[2, 2] != 0) && (matrix[0, 1] != 0 || matrix[1, 1] != 0 || matrix[2, 1] != 0));
        }

        public static bool IssRotationMatrix(Matrix<double> m)
        {
            double epsilon = 0.0001; // margin to allow for rounding errors
            if (Math.Abs(m[0, 0] * m[0, 1] + m[0, 1] * m[1, 1] + m[0, 2] * m[1, 2]) > epsilon) return false;
            if (Math.Abs(m[0, 0] * m[2, 0] + m[0, 1] * m[2, 1] + m[0, 2] * m[2, 2]) > epsilon) return false;
            if (Math.Abs(m[1, 0] * m[2, 0] + m[1, 1] * m[2, 1] + m[1, 2] * m[2, 2]) > epsilon) return false;
            if (Math.Abs(m[0, 0] * m[0, 0] + m[0, 1] * m[0, 1] + m[0, 2] * m[0, 2] - 1) > epsilon) return false;
            if (Math.Abs(m[1, 0] * m[1, 0] + m[1, 1] * m[1, 1] + m[1, 2] * m[1, 2] - 1) > epsilon) return false;
            if (Math.Abs(m[2, 0] * m[2, 0] + m[2, 1] * m[2, 1] + m[2, 2] * m[2, 2] - 1) > epsilon) return false;
            return (Math.Abs(m.Determinant() - 1) < epsilon);
        }

        public static Vector3D[] RotationToEulerZXZ(Matrix<double> matrix)
        {
            if (!IsRotationMatrix(matrix))
                return new Vector3D[] { new Vector3D(0, 0, 0) };

            double x2 = Math.Acos(matrix[2, 2]);
            double x2alt = Math.PI - x2;

            if (Math.Abs(Math.Sin(x2)) > 0.05)
            {
                int sign = Math.Sign(Math.Sin(x2));
                double z3 = Math.Atan2(matrix[2, 0] * sign, matrix[2, 1] * sign);
                sign = Math.Sign(Math.Sin(x2alt));
                double z3alt = Math.Atan2(matrix[2, 0] * sign, matrix[2, 1] * sign);

                sign = Math.Sign(Math.Sin(z3));
                double z1 = -Math.Atan2(matrix[0, 2] * sign, matrix[1, 2] * sign);
                sign = Math.Sign(Math.Sin(z3alt));
                double z1alt = -Math.Atan2(matrix[0, 2] * sign, matrix[1, 2] * sign);

                return new Vector3D[] { new Vector3D(z1, x2, z3), new Vector3D(z1alt, x2alt, z3alt) };

            }

            double z = -Math.Atan2(matrix[0, 2], matrix[1, 2]);

            return new Vector3D[] { new Vector3D(z / 2, x2, z / 2) };

        }

        public static Vector3D RotationMatrixToAxisAngle(Matrix<double> matrix)
        {
            double angle, x, y, z; // variables for result
            double epsilon = 0.01; // margin to allow for rounding errors
            double epsilon2 = 0.1; // margin to distinguish between 0 and 180 degrees
                                   // optional check that input is pure rotation, 'isRotationMatrix' is defined at:
                                   // https://www.euclideanspace.com/maths/algebra/matrix/orthogonal/rotation/
                                   //float[,] m = new float[3, 3];
                                   //m[0, 0] = mat.m0;
                                   // m[0, 1] = mat.m1;
                                   // m[0, 2] = mat.m2;
                                   // m[1, 0] = mat.m4;
                                   // m[1, 1] = mat.m5;
                                   // m[1, 2] = mat.m6;
                                   // m[2, 0] = mat.m8;
                                   // m[2, 1] = mat.m9;
                                   // m[2, 2] = mat.m10;
            if (IsRotationMatrix(matrix))
            {
                if ((Math.Abs(matrix[0, 1] - matrix[1, 0]) < epsilon)
                  && (Math.Abs(matrix[0, 2] - matrix[2, 0]) < epsilon)
                  && (Math.Abs(matrix[1, 2] - matrix[2, 1]) < epsilon))
                {
                    // singularity found
                    // first check for identity matrix which must have +1 for all terms
                    //  in leading diagonaland zero in other terms
                    if ((Math.Abs(matrix[0, 1] + matrix[1, 0]) < epsilon2)
                      && (Math.Abs(matrix[0, 2] + matrix[2, 0]) < epsilon2)
                      && (Math.Abs(matrix[1, 2] + matrix[2, 1]) < epsilon2)
                      && (Math.Abs(matrix[0, 0] + matrix[1, 1] + matrix[2, 2] - 3) < epsilon2))
                    {
                        // this singularity is identity matrix so angle = 0
                        return new Vector3D(0, 0, 0); // zero angle, arbitrary axis
                    }
                    // otherwise this singularity is angle = 180
                    angle = Math.PI;
                    double xx = (matrix[0, 0] + 1) / 2;
                    double yy = (matrix[1, 1] + 1) / 2;
                    double zz = (matrix[2, 2] + 1) / 2;
                    double xy = (matrix[0, 1] + matrix[1, 0]) / 4;
                    double xz = (matrix[0, 2] + matrix[2, 0]) / 4;
                    double yz = (matrix[1, 2] + matrix[2, 1]) / 4;
                    if ((xx > yy) && (xx > zz))
                    { // m[0,0] is the largest diagonal term
                        if (xx < epsilon)
                        {
                            x = 0;
                            y = 0.7071;
                            z = 0.7071;
                        }
                        else
                        {
                            x = Math.Sqrt(xx);
                            y = xy / x;
                            z = xz / x;
                        }
                    }
                    else if (yy > zz)
                    { // m[1,1] is the largest diagonal term
                        if (yy < epsilon)
                        {
                            x = 0.7071;
                            y = 0;
                            z = 0.7071;
                        }
                        else
                        {
                            y = Math.Sqrt(yy);
                            x = xy / y;
                            z = yz / y;
                        }
                    }
                    else
                    { // m[2,2] is the largest diagonal term so base result on this
                        if (zz < epsilon)
                        {
                            x = 0.7071;
                            y = 0.7071;
                            z = 0;
                        }
                        else
                        {
                            z = Math.Sqrt(zz);
                            x = xz / z;
                            y = yz / z;
                        }
                    }
                    return new Vector3D(Convert.ToSingle(angle * x), Convert.ToSingle(angle * y), Convert.ToSingle(angle * z)); // return 180 deg rotation
                }
                // as we have reached here there are no singularities so we can handle normally
                double s = Math.Sqrt((matrix[2, 1] - matrix[1, 2]) * (matrix[2, 1] - matrix[1, 2])
                    + (matrix[0, 2] - matrix[2, 0]) * (matrix[0, 2] - matrix[2, 0])
                    + (matrix[1, 0] - matrix[0, 1]) * (matrix[1, 0] - matrix[0, 1])); // used to normalise
                if (Math.Abs(s) < 0.001) s = 1;
                // prevent divide by zero, should not happen if matrix is orthogonal and should be
                // caught by singularity test above, but I've left it in just in case
                angle = Math.Acos((matrix[0, 0] + matrix[1, 1] + matrix[2, 2] - 1) / 2);
                x = (matrix[2, 1] - matrix[1, 2]) / s;
                y = (matrix[0, 2] - matrix[2, 0]) / s;
                z = (matrix[1, 0] - matrix[0, 1]) / s;
                return new Vector3D(Convert.ToSingle(angle * x), Convert.ToSingle(angle * y), Convert.ToSingle(angle * z));
            }
            return new Vector3D(0, 0, 0);
        }

        public static double Hypot(double a, double b)
        {
            return Math.Sqrt(a * a + b * b);
        }

        public static double Hypot(double a, double b, double c)
        {
            return Math.Sqrt(a * a + b * b + c * c);
        }

        public static double LawOfCosines(double a, double b, double c)
        {
            return Math.Acos((a * a + b * b - c * c) / (2 * a * b));
        }

        public static double HalfRotate(double angle)
        {
            if (angle < 0)
                return angle + Math.PI;
            return angle - Math.PI;
        }

        public static double FullRotate(double angle)
        {
            if (angle < 0)
                return angle + 2 * Math.PI;
            return angle - 2 * Math.PI;
        }

        public static Matrix<double> GetXRotationMatrix(double angle)
        {
            Matrix<double> matrix = Matrix<double>.Build.DenseIdentity(3);
            matrix[1, 1] = Math.Cos(angle);
            matrix[1, 2] = -Math.Sin(angle);
            matrix[2, 1] = Math.Sin(angle);
            matrix[2, 2] = Math.Cos(angle);
            return matrix;
        }

        public static Matrix<double> GetYRotationMatrix(double angle)
        {
            Matrix<double> matrix = Matrix<double>.Build.DenseIdentity(3);
            matrix[0, 0] = Math.Cos(angle);
            matrix[0, 2] = Math.Sin(angle);
            matrix[2, 0] = -Math.Sin(angle);
            matrix[2, 2] = Math.Cos(angle);
            return matrix;
        }

        public static Matrix<double> GetZRotationMatrix(double angle)
        {
            Matrix<double> matrix = Matrix<double>.Build.DenseIdentity(3);
            matrix[0, 0] = Math.Cos(angle);
            matrix[0, 1] = -Math.Sin(angle);
            matrix[1, 0] = Math.Sin(angle);
            matrix[1, 1] = Math.Cos(angle);
            return matrix;
        }

        /* public Quaternion RotationMatrixToQuaternion(HmdMatrix34_t mat)
         {

             if (IsRotationValid(mat))
             {
                 float w = MathF.Sqrt(Math.Max(0, 1 + mat.m0 + mat.m5 + mat.m10)) / 2;
                 float x = MathF.Sqrt(Math.Max(0, 1 + mat.m0 - mat.m5 - mat.m10)) / 2;
                 float y = MathF.Sqrt(Math.Max(0, 1 - mat.m0 + mat.m5 - mat.m10)) / 2;
                 float z = MathF.Sqrt(Math.Max(0, 1 - mat.m0 - mat.m5 + mat.m10)) / 2;

                 _copysign(ref x, -mat.m9 - -mat.m6);
                 _copysign(ref y, -mat.m2 - -mat.m8);
                 _copysign(ref z, mat.m4 - mat.m1);

                 return new Quaternion(w, x, y, z);
             }
             return Quaternion.One;
         }

         private static void _copysign(ref float sizeval, float signval)
         {
             if (signval > 0 != sizeval > 0)
                 sizeval = -sizeval;
         }*/
    }
}
