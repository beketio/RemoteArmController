using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;

namespace ArmController.Core.Input
{
    internal class GamepadInputData
    {
        private readonly Dictionary<GamepadInput, double> inputMap = new Dictionary<GamepadInput, double>();

        public GamepadInputData()
        {
            foreach (GamepadInput input in Enum.GetValues(typeof(GamepadInput)))
                inputMap.Add(input, 0);
        }

        public GamepadInputData(Dictionary<GamepadInput, double> inputMap)
        {
            this.inputMap = inputMap;
        }

        public double Get(GamepadInput input)
        {
            return inputMap[input];
        }

        public void Set(GamepadInput input, double value)
        {
            inputMap[input] = value;
        }

        public void Add(GamepadInput input, double valueToAdd)
        {
            inputMap[input] += valueToAdd;
        }

        public GamepadInputData CloneData()
        {
            Dictionary<GamepadInput, double> mapClone = new Dictionary<GamepadInput, double>();
            foreach (KeyValuePair<GamepadInput, double> entry in inputMap)
                mapClone.Add(entry.Key, entry.Value);
            return new GamepadInputData(mapClone);
        }

        public InputData ToInputData()
        {
            double xRot = Trig.DegreeToRadian(Get(GamepadInput.Pitch));
            double yRot = Trig.DegreeToRadian(Get(GamepadInput.Yaw));
            double zRot = Trig.DegreeToRadian(Get(GamepadInput.Roll));

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
                Position = new Vector3D(Get(GamepadInput.XPos), Get(GamepadInput.YPos), Get(GamepadInput.ZPos)),
                Rotation = xMat * yMat * zMat,
                Tool0 = Get(GamepadInput.Tool0),
                Tool1 = Get(GamepadInput.Tool1)
            };

            return inputData;
        }
    }
}
