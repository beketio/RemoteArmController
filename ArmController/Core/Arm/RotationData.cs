using System;
using System.Collections.Generic;
using System.Text;

namespace ArmController.Core.Arm
{
    public class RotationData
    {
        private readonly float[] joint;
        private readonly float[] tool;

        public RotationData(int numJoints, int numTools)
        {
            joint = new float[numJoints];
            tool = new float[numTools];
        }

        public void SetJoint(int index, float value)
        {
            joint[index] = value;
        }

        public void SetJoint(int index, double value)
        {
            SetJoint(index, Convert.ToSingle(value));
        }

        public void SetTool(int index, float value)
        {
            tool[index] = value;
        }

        public void SetTool(int index, double value)
        {
            SetTool(index, Convert.ToSingle(value));
        }

        public int GetNumJoints()
        {
            return joint.Length;
        }

        public int GetNumTools()
        {
            return tool.Length;
        }

        public float GetJoint(int index)
        {
            return joint[index];
        }

        public float GetTool(int index)
        {
            return tool[index];
        }

        public float[] ToArray()
        {
            int jlength = joint.Length;
            float[] result = new float[jlength + tool.Length];
            for (int i = 0; i < joint.Length; i++)
                result[i] = joint[i];
            for (int i = 0; i < tool.Length; i++)
                result[jlength + i] = tool[i];
            return result;
        }

        public override string ToString()
        {
            String result = "";
            for(int i = 0; i < joint.Length; i++)
                result += i+": " + joint[i] + ", ";
            return result;
        }
    }
}
