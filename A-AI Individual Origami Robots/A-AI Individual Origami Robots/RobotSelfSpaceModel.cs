using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_AI_Individual_Origami_Robots
{
    class RobotSelfSpaceModel
    {
        private readonly int Index;
        private readonly int AsciiValue;

        public RobotSelfSpaceModel(int Index, int AsciiValue)
        {
            this.Index = Index;
            this.AsciiValue = AsciiValue;
        }

        public int GetIndex()
        {
            return Index;
        }

        public int GetAsciiValue()
        {
            return AsciiValue;
        }
    }
}
