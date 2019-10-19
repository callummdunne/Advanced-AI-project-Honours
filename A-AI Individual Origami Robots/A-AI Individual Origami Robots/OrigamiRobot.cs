using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace A_AI_Individual_Origami_Robots
{
    class OrigamiRobot
    {
        public double Energy = 100;
        public bool IsConverged = false;
        public Vector3 CurrentCoordinates; //Center of robot

        public OrigamiRobot(Vector3 CurrentCoordinates)
        {
            this.CurrentCoordinates = CurrentCoordinates;
        }

        public List<string> GetNearestObjects()
        {
            List<string> objects = new List<string>
            {

                //"AAAAA" //Foreign
                "0A002" //Self
            };

            return objects;
        }
    }
}
