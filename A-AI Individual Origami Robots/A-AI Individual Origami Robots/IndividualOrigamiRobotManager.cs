using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace A_AI_Individual_Origami_Robots
{
    class IndividualOrigamiRobotManager
    {
        private Dictionary<int, List<byte>> RobotModel;

        public void TrainRobotModel(List<string> ModelIdentifiers)
        {
            Dictionary<int, List<byte>> Model = new Dictionary<int, List<byte>>();

            foreach (string Identifier in ModelIdentifiers)
            {
                byte[] AsciiBytes = Encoding.ASCII.GetBytes(Identifier);
                List<byte> asciiList = new List<byte>();

                for (int i = 0; i < AsciiBytes.Length; i++)
                {
                    asciiList.Add(AsciiBytes[i]);

                    if (!Model.TryGetValue(i, out List<byte> existingAscii))
                    {
                        existingAscii = new List<byte>(asciiList);
                        Model.Add(i, existingAscii);
                    }
                    else
                    {
                        existingAscii.AddRange(asciiList);
                        Model[i] = existingAscii;
                    }
                }
            }

            RobotModel = Model;
        }

        public void DetectIncomingObject(List<OrigamiRobot> Robots)
        {
            foreach (OrigamiRobot robot in Robots)
            {
                List<string> nearestObjects = robot.GetNearestObjects();
                string nearestObject = nearestObjects[0];

                if (!ObjectIsInSelfSpace(nearestObject))
                {
                    Console.WriteLine("Object is foreign cell");
                    if (CollidingWithObject(robot, nearestObject))
                    {
                        //Ask Goal/Reward System
                        Console.WriteLine("Collided with object, asking reward system.");
                    }
                    else
                    {
                        Console.WriteLine("No collision, reducing energy while not converged.");
                        while (!robot.IsConverged && robot.Energy != 0)
                        {
                            robot.Energy -= 10;
                        }
                    }
                }
                else
                {
                    //Continue convergence or do nothing
                    Console.WriteLine("Object is Self cell");
                }
            }
        }

        private bool ObjectIsInSelfSpace(string Identifier)
        {
            byte[] AsciiBytes = Encoding.ASCII.GetBytes(Identifier);

            for (int i = 0; i < AsciiBytes.Length; i++)
            {
                if (!RobotModel.TryGetValue(i, out List<byte> validAscii))
                {
                    return false;
                }

                if (!validAscii.Contains(AsciiBytes[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool CollidingWithObject(OrigamiRobot Robot, string NearestObject)
        {
            return false;
        }

        public void MatchRobotToCoordinates(List<OrigamiRobot> Robots, List<List<Vector3>> CoordinatesList)
        {
            int radius = 10;

            OrigamiRobot[] temp = new OrigamiRobot[Robots.Count];
            Robots.CopyTo(temp);
            List<OrigamiRobot> RobotClones = temp.ToList();

            foreach (List<Vector3> coordinates in CoordinatesList)
            {
                double smallestDistance = double.MaxValue;
                OrigamiRobot closestRobot = null;
                Vector3 centerCoordinate = GetCentroid(coordinates);

                foreach (OrigamiRobot robot in RobotClones)
                {
                    double distance = GetEuclideanDistance(robot.CurrentCoordinates, centerCoordinate);
                    Console.WriteLine("Distance: " + distance);

                    if (distance < radius && distance < smallestDistance)
                    {
                        smallestDistance = distance;
                        closestRobot = robot;
                    }
                }

                RobotClones.Remove(closestRobot);

                Console.WriteLine("Closest robot that has coordinates to center coordinate " + centerCoordinate.ToString() + " : (" + closestRobot.CurrentCoordinates.X +
                ", " + closestRobot.CurrentCoordinates.Y + ", " + closestRobot.CurrentCoordinates.Z + ")");
            }

            Console.WriteLine("Test Complete.");
        }

        private Vector3 GetCentroid(List<Vector3> Coordinates)
        {
            int centerX = (int)(Coordinates[0].X + Coordinates[1].X + Coordinates[2].Z) / 3;
            int centerY = (int)(Coordinates[0].Y + Coordinates[1].Y + Coordinates[2].Y) / 3;
            int centerZ = (int)(Coordinates[0].Z + Coordinates[1].Z + Coordinates[2].Z) / 3;

            return new Vector3(centerX, centerY, centerZ);
        }

        private double GetEuclideanDistance(Vector3 C1, Vector3 C2)
        {
            double squaredDistance = Math.Pow((C1.X - C2.X), 2) + Math.Pow((C1.Y - C2.Y), 2) + Math.Pow((C1.Z - C2.Z), 2);

            return Math.Sqrt(squaredDistance);
        }
    }
}
