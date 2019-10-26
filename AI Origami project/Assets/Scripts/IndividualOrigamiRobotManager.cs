using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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

                "2222R" //Foreign
                //"0A002" //Self
            };

            return objects;
        }
    }

    public class IndividualOrigamiRobotManager : MonoBehaviour
    {
        private Dictionary<int, List<byte>> RobotModel;
        public GameObject GameManager;
        public GameObject AddOrigamis;

        void Start()
        {
            GameManager = GameObject.FindWithTag("GameManager");
            AddOrigamis = GameObject.FindWithTag("GameManager");
        }

        void Update()
        {
            if (GameManager.GetComponent<Swarm_mesh>().isSwarmCalculated())
            {
                //List<List<Vector3>> coordinatesList = GameManager.GetComponent<Swarm_mesh>().getSwarmCoordinates();
                List<AddOrigamis.Origami> robots = AddOrigamis.GetComponent<AddOrigamis>().getOrigamis();

                //MatchRobotToCoordinates(robots, coordinatesList);
            }
        }

        public void TrainRobotModel(List<string> ModelIdentifiers)
        {
            Dictionary<int, List<byte>> Model = new Dictionary<int, List<byte>>();

            foreach (string Identifier in ModelIdentifiers)
            {
                byte[] AsciiBytes = Encoding.ASCII.GetBytes(Identifier);

                for (int i = 0; i < AsciiBytes.Length; i++)
                {
                    if (!Model.TryGetValue(i, out List<byte> existingAscii))
                    {
                        existingAscii = new List<byte>
                        {
                            AsciiBytes[i]
                        };
                        Model.Add(i, existingAscii);
                    }
                    else
                    {
                        existingAscii.Add(AsciiBytes[i]);
                        Model[i] = existingAscii;
                    }
                }
            }

            RobotModel = Model;
        }

        void DetectIncomingObject(List<OrigamiRobot> Robots)
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
                        //Send danger signal!
                        Console.WriteLine("Collided with object, sending danger signal.");
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
            int k = 3;
            double threshold = 0.5;
            byte[] AsciiBytes = Encoding.ASCII.GetBytes(Identifier);

            for (int i = 0; i < AsciiBytes.Length; i++)
            {
                if (!RobotModel.TryGetValue(i, out List<byte> validAscii))
                {
                    return false;
                }

                byte sampleAsciiValue = AsciiBytes[i];

                int totalDistance = 0;
                List<int> nearestValues = new List<int>();
                List<byte> tempValidAscii = new List<byte>(validAscii);

                for (int j = 0; j < k; j++)
                {
                    byte closestValidAsciiValue = 0;
                    int smallestDistance = 100;
                    foreach (byte validAsciiValue in tempValidAscii)
                    {
                        int distance = Math.Abs((validAsciiValue - sampleAsciiValue));
                        if (distance < smallestDistance)
                        {
                            smallestDistance = distance;
                            closestValidAsciiValue = validAsciiValue;
                        }
                    }

                    totalDistance += smallestDistance;
                    nearestValues.Add(smallestDistance);
                    tempValidAscii.Remove(closestValidAsciiValue);
                }

                int closestNeighbourDistance = nearestValues[0];

                double normality = (double) (closestNeighbourDistance == 0 ? 1 : closestNeighbourDistance) / (totalDistance == 0 ? 1 : totalDistance);
                Console.WriteLine("Level of normality for index " + i + ": " + normality);

                if (normality < threshold)
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

        public void MatchRobotToCoordinates(List<AddOrigamis.Origami> Robots, List<List<Vector3>> CoordinatesList)
        {
            int radius = 10;

            AddOrigamis.Origami[] temp = new AddOrigamis.Origami[Robots.Count];
            Robots.CopyTo(temp);
            List<AddOrigamis.Origami> RobotClones = temp.ToList();

            foreach (List<Vector3> coordinates in CoordinatesList)
            {
                double smallestDistance = double.MaxValue;
                AddOrigamis.Origami closestRobot = new AddOrigamis.Origami();
                Vector3 centerCoordinate = GetCentroid(coordinates);

                foreach (AddOrigamis.Origami robot in RobotClones)
                {
                    double distance = GetEuclideanDistance(robot.myObject.transform.position, centerCoordinate);
                    Console.WriteLine("Distance: " + distance);

                    if (distance < radius && distance < smallestDistance)
                    {
                        smallestDistance = distance;
                        closestRobot = robot;
                    }
                }

                RobotClones.Remove(closestRobot);

                Console.WriteLine("Closest robot that has coordinates to center coordinate " + centerCoordinate.ToString() + " : (" + closestRobot.myObject.transform.position.x +
                ", " + closestRobot.myObject.transform.position.y + ", " + closestRobot.myObject.transform.position.z + ")");

                AddOrigamis.Origami origami = Robots.Find(x => x.Equals(closestRobot));
                origami.myObject.transform.position.Set(centerCoordinate.x, centerCoordinate.y, centerCoordinate.z);
            }

            Console.WriteLine("Test Complete.");
        }

        private Vector3 GetCentroid(List<Vector3> Coordinates)
        {
            int centerX = (int)(Coordinates[0].x + Coordinates[1].x + Coordinates[2].x) / 3;
            int centerY = (int)(Coordinates[0].y + Coordinates[1].y + Coordinates[2].y) / 3;
            int centerZ = (int)(Coordinates[0].z + Coordinates[1].z + Coordinates[2].z) / 3;

            return new Vector3(centerX, centerY, centerZ);
        }

        private double GetEuclideanDistance(Vector3 C1, Vector3 C2)
        {
            double squaredDistance = Math.Pow((C1.x - C2.x), 2) + Math.Pow((C1.y - C2.y), 2) + Math.Pow((C1.z - C2.z), 2);

            return Math.Sqrt(squaredDistance);
        }
    }
}
