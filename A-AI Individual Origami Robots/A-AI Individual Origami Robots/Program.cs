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
                "1A120","1A011","2C102","0B212","2C022","0B102","1B110","1C022","2B000","1C202",
                "0B220","1A001","0B110","1B221","2A201","2B111","0A121","2B001","0A011","2C122",
                "1A201","2A012","0C020","0A202","1C210","0A122","1C102","2B210","0C100","1B111",
                "1C020","2B221","0A001","0C111","2A002","0A222","0A112","1C120","1B002","2A010",
                "0C120","2A200","1C010","0A120","1C200","2B211","2A120","0C101","1B212","0C021",
                "1B102","1C211","2B022","0A002","0C212","1B222","2A000","1B112","2A220","2B002",
                "0A110","1C121","0B000","2A011","0C121","0C011","0A221","1C201","2B012","1A121",
                "2C202","1B210","0C122","1B100","1C012","2B020","0B100","2C210","0B020","2A221",
                "2B100","0A111","1A222","0B001","1A112","0C012","2A222","2C010","1A122","2C200",
                "0B021","1B201","0C010","0B101","2C011","1B211","2A022","1B101","2B212","0A200",
                "1A110","0C110","2B220","0A000","1C021","1A220","2C201","0B022","2B112","1A210",
                "1A100","2C121","1C122","1A221","2C012","2C222","1A111","0B010","2B101","1A101",
                "2B021","0A012","0A022","2B120","2C220","1C110","0A212","0C201","2B202","1C000",
                "1B022","2A110","1C011","2B121","2A111","1B012","0A210","1B020","2A100","0A020",
                "1C001","0C222","1C112","2B122","1A002","0C202","1B010","2B200","0A211","2C120",
                "2A101","0A021","0B211","1A022","2A212","0A102","2C110","0C000","1B011","0C220",
                "0B011","0B201","1B121","2A102","0A100","1A020","2A210","1C221","2C111","0C001",
                "0C221","1C002","0B202","2C112","0B122","0A101","1A021","0B200","1A211","2C021",
                "2C002","2B011","1A012","2C100","0B121","0B221","1C111","1B202","2A020","2C101",
                "2A021","1B122","2C211","0C112","1B200","2B110","0C002","0A201","1A212","2A122",
                "1B220","2C000","0C210","2C001","1A102","0B112","2A202","1A200","0C211","1B000",
                "0B120","2C212","2C020","1C220","1A202","0B111","1C101","2B222","0A010","0C022",
                "1C212","0B210","0B222","1B001","0B002","0C200","2A112","1A010","0A220","2B102",
                "1A000","2C221","0B012","1B021","2A001","2B201","1B120","2A211","1C222","2B010",
                "0C102","1C100","2A121"
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
