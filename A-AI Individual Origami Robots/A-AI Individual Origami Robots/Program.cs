using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace A_AI_Individual_Origami_Robots
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> OrigamiRobotIdentifiers = new List<string>
            {
                "Esfnawefin",
                "seghwfawfh",
                "rgUnsreiuj",
                "seguonnapd",
                "oaInwFubag",
                "yuiauebveu",
                "auoWenuvse",
                "aeoifgbnse",
                "aiweFnauia",
                "aiewpgnbeu",
                "eaiosnegfn",
                "zxcmvVjefk",
                "mefwsigbva",
                "pczxcvazaw",
                "mnbqlwknee",
                "ZCnbakeuuh",
                "AEUfbesuEW",
                "oawiNFobea",
                "aseoinsevu",
                "AOWEiufgbn",
                "ZXCOBQafun"
            };


            IndividualOrigamiRobotManager manager = new IndividualOrigamiRobotManager();
            manager.TrainRobotModel(OrigamiRobotIdentifiers);

            List<OrigamiRobot> robots = new List<OrigamiRobot>
            {
                new OrigamiRobot(new Vector3(1,1,1)),
                new OrigamiRobot(new Vector3(2,7,4)),
                new OrigamiRobot(new Vector3(0,9,2))
            };

            manager.DetectIncomingObject(robots);

            List<List<Vector3>> coordinatesList = new List<List<Vector3>>
            {
                new List<Vector3>
                {
                    new Vector3(0,0,0),
                    new Vector3(1,0,0),
                    new Vector3(0,0,1)
                },
                new List<Vector3>
                {
                    new Vector3(3,0,2),
                    new Vector3(1,0,1),
                    new Vector3(2,2,2)
                },
                new List<Vector3>
                {
                    new Vector3(0,0,0),
                    new Vector3(1,1,1),
                    new Vector3(2,2,2)
                }
            };

            manager.MatchRobotToCoordinates(robots, coordinatesList);
        }
    }
}
