using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;


public class Swarm_behaviour : MonoBehaviour
{
    string[] arr_Self_Cells = { "1A120","1A011","2C102","0B212","2C022","0B102","1B110","1C022","2B000","1C202","0B220","1A001","0B110","1B221",
        "2A201","2B111","0A121","2B001","0A011","2C122","1A201","2A012","0C020","0A202","1C210","A122","1C102","2B210","0C100","1B111","1C020",
        "2B221","0A001","0C111","2A002","0A222","0A112","1C120","1B002","2A010","0C120","2A200","1C010","0A120","1C200","2B211","2A120","0C101",
        "1B212","0C021","1B102","1C211","2B022","0A002","0C212","1B222","2A000","1B112","2A220","2B002","0A110","1C121","0B000","2A011","0C121",
        "0C011","0A221","1C201","2B012","1A121","2C202","1B210","0C122","1B100","1C012","2B020","0B100","2C210","0B020","2A221","2B100","0A111",
        "1A222","0B001","1A112","0C012","2A222","2C010","1A122","2C200","0B021","1B201","0C010","0B101","2C011","1B211","2A022","1B101","2B212",
        "0A200","1A110","0C110","2B220","0A000","1C021","1A220","2C201","0B022","2B112","1A210","1A100","2C121","1C122","1A221","2C012","2C222",
        "1A111","0B010","2B101","1A101","2B021","0A012","0A022","2B120","2C220","1C110","0A212","0C201","2B202","1C000","1B022","2A110","1C011",
        "2B121","2A111","1B012","0A210","1B020","2A100","0A020","1C001","0C222","1C112","2B122","1A002","0C202","1B010","2B200","0A211","2C120",
        "2A101","0A021","0B211","1A022","2A212","0A102","2C110","0C000","1B011","0C220","0B011","0B201","1B121","2A102","0A100","1A020","2A210",
        "1C221","2C111","0C001","0C221","1C002","0B202","2C112","0B122","0A101","1A021","0B200","1A211","2C021","2C002","2B011","1A012","2C100",
        "0B121","0B221","1C111","1B202","2A020","2C101","2A021","1B122","2C211","0C112","1B200","2B110","0C002","0A201","1A212","2A122","1B220",
        "2C000","0C210","2C001","1A102","0B112","2A202","1A200","0C211","1B000","0B120","2C212","2C020","1C220","1A202","0B111","1C101","2B222",
        "0A010","0C022","1C212","0B210","0B222","1B001","0B002","0C200","2A112","1A010","0A220","2B102","1A000","2C221","0B012","1B021","2A001",
        "2B201","1B120","2A211","1C222","2B010","0C102","1C100","2A121" };

    List<Sphere> self_space;
    List<Sphere> detectors;

    public bool newObstacle;
    public string ObstaclePattern;
    
    public List<Detector_Response> Respons_Detector_Map;

    //The string patterns are 5 characters, with 26 alphabet letters and 10 numbers, thus x ( 0 - 25), y (0 - 9), z = (0 - 4)
    //int Universe_Volume = 1300;

    //Cover atleast 80% of the space between self samples and detectors, thus 80% of 1300 = 1040
    int Coverage_Threshold = 1040;

    //The detector spheres of variable size add up to a coverage volume, ths must be greater than the Coverage_Threshold
    double Detector_coverage = 0;
    int sizeOfList;

    //The maximum allowable overlap with current detectors
    //double allowed_detetor_overlap = 2;

    //The threshold for overlap with self space samples
    double allowed_samples_overlap = 0.5;

    //How many movements are allowed until it is time to terminate the detector
    int iteration_cap = 5;

    public struct Sphere
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public double radius { get; set; }

        public Sphere(double X, double Y, double Z)
        {
            x = X;
            y = Y;
            z = Z;
            radius = 1;
        }
    }

    public struct Detector_Response
    {
        public Sphere Detector { get; set; }
        public bool Activated { get; set; }
        public AssociatedResponce Responce { get; set; }

        public Detector_Response(Sphere newDetector, bool Triggered)
        {
            Detector = newDetector;
            Activated = Triggered;
            Responce = new AssociatedResponce(0, 0);
        }

        public void ActivateThis()
        {
            Activated = true;
        }
    }

    public class AssociatedResponce
    {
        //Assign reward weights to these indexes

        // Swarm choice
        // [3 linear small swarm, 3 sequenced small swarms, large swarm]
        int[] SwarmChoice { get; set; }

        // Movement choice
        // [move left, centre, move right]
        int[] MovementChoice { get; set; }

        public AssociatedResponce(int SwarmReward_Index, int MoveReward_Index)
        {
            SwarmChoice = new int[3] { 0, 1, 2 };
            MovementChoice = new int[3] { 0, 1, 0 };

            SwarmChoice[SwarmReward_Index] += 1;
            MovementChoice[MoveReward_Index] += 1;
        }

        public void Reinforcement_learning(int S_index, int S_Reward, int M_index, int M_Reward)
        {
            SwarmChoice[S_index] += S_Reward;
            MovementChoice[M_index] += M_Reward;
        }
    }



    public class Move_Info
    {
        // How much should a detctor move in x direction to avoid overlap with a specific sample or other detector
        public double x { get; set; }
        // How much should a detector move in y direction to avoid overlap with a specific sample or other detector
        public double y { get; set; }
        // How much should a detector move in z direction to avoid overlap with a specific sample or other detector
        public double z { get; set; }
        // Assign a specific direction
        public int dir { get; set; }
        // Boolean to sate if movement should be applied, if it is false detector must be discarded
        public bool apply_movement { get; set; }

        public Move_Info(double X, double Y, double Z, int direction, bool move)
        {
            x = X;
            y = Y;
            z = Z;
            dir = direction;
            apply_movement = move;
        }

        public void reset()
        {
            x = 0;
            y = 0;
            z = 0;
            dir = 0;
            apply_movement = false;
        }
    }

    // Start is called before the first frame update
    public void Start()
    {

         newObstacle = false;
         ObstaclePattern = "00000";

        print("Swarm behaviour is running");
        self_space = new List<Sphere>();
        detectors = new List<Sphere>();

        int i = 0;
        foreach (string s in arr_Self_Cells)
        {
            foreach (char c in s)
            {
                i++;
                Sphere candidate_sphere = mapToPoint(c, i);
                if (!self_space.Contains(candidate_sphere))
                {
                    self_space.Add(candidate_sphere);
                }
            }
            i = 0;
        }
        int count_self_space = self_space.Count;
        print("Self space cells: " + count_self_space);

        // Generate random detector
        Random rnd = new System.Random();

        while (Detector_coverage < Coverage_Threshold)
        {
            int radius = 1;
            int x = rnd.Next(25);  // creates a number between and including 0 and 25
            int y = rnd.Next(10);   // creates a number between and including 0 and 9
            int z = rnd.Next(5);     // creates a number between and including 0 and 4
            Sphere candidate_detector = new Sphere(x, y, z);
            Move_Info move_instructions = new Move_Info(0, 0, 0, 0, false);

            int time_to_teriminate = 0;
            bool mature_detector = false;
            bool NoClashes = true;

            foreach (Sphere s in self_space)
            {
                if ((time_to_teriminate < iteration_cap) & (NoClashes))
                {
                    mature_detector = false;
                    move_instructions.reset();
                    while ((time_to_teriminate < iteration_cap) & !mature_detector)
                    {
                        move_instructions = sample_distance_overlaps(candidate_detector, s);

                        //If detector touches self space samples or current detectors move detector
                        if (move_instructions.apply_movement)
                        {
                            if (move_instructions.x == 0 & move_instructions.y == 0 & move_instructions.z == 0)
                            {
                                //detector is matured as no collisions occured
                                mature_detector = true;
                                //print("Mature detector");
                            }
                            else
                            {
                                //Move detector away from self sample
                                time_to_teriminate += 1;
                                candidate_detector.x += move_instructions.x;
                                candidate_detector.y += move_instructions.y;
                                candidate_detector.z += move_instructions.z;
                                //print("Moving detector x-" + candidate_detector.x + " y-" + candidate_detector.y + " z-" + candidate_detector.z);
                            }
                        }
                        else
                        {
                            //No changes could be made, it will move detector out of range
                            time_to_teriminate = iteration_cap;
                            NoClashes = false;
                            //print("No changes could be made");
                        }
                    }

                    if (mature_detector)
                    {
                        time_to_teriminate = 0;
                        //print("Time to terminate");
                    }
                    else
                    {
                        NoClashes = false;
                        // print("Make no clashes false");
                    }
                }
                else
                {
                    // print("Outside the if statement");
                }

            }
            //If the detector does not clash with any of the spheres it is added as a mature detector
            if (NoClashes)
            {
                self_space.Add(candidate_detector);
                detectors.Add(candidate_detector);
                //Update the coverage volume due to new matured detector
                Detector_coverage += volume_of_sphere(radius);
            }


            //Validate if the detector does not exceed the overlap threshold with other detectors


        }

        //print_detectors();
        print("Detector coverage: " + Detector_coverage);
        sizeOfList = detectors.Count;
        print("Number of detectors: " + sizeOfList);

        Respons_Detector_Map = new List<Detector_Response>();
        foreach(Sphere d in detectors)
        {
            Detector_Response newMap = new Detector_Response(d,false);
            Respons_Detector_Map.Add(newMap);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(newObstacle)
        {
            int i = 0;
            foreach(char c in ObstaclePattern)
            {
                i++;
                Sphere Obstacle_Mapping = mapToPoint(c, i);
                foreach(Detector_Response DR in  Respons_Detector_Map)
                {
                    if (Obstacle_Detector_Distance(DR.Detector, Obstacle_Mapping) < 1.5)
                    {
                        DR.ActivateThis();
                        print("----------------- Detector ACTIVATED!!!!");
                    }
                }
            }
            newObstacle = false;
        }
    }

    Sphere mapToPoint(char C, int i)
    {
        if (48 <= (int)C && (int)C <= 57)
        {
            //print("Creating point for" + C + " as " + (int)C + " at x:" + ((int)C - 48) + " y:0" + " z:" + 2 * i);
            return new Sphere((int)C - 48, 0, 2 * i);
        }
        else
        {
           // print("Creating point for" + C + " as " + (int)C + " at x:0" + " y:" + ((int)C - 55) + " z:" + 2 * i);
            return new Sphere(0, ((int)C - 55), 2 * i);
        }
    }

    double volume_of_sphere(int r)
    {
        return (4 / 3) * Math.PI * Math.Pow(r, 3);
    }

    void print_detectors()
    {
        foreach (Sphere d in detectors)
        {
            print("Detector: x-" + d.x + " y-" + d.y + " z-" + d.z);
        }
    }

    public double Obstacle_Detector_Distance(Sphere Obstacle, Sphere Detector)
    {
        double x_dist = Detector.x - Obstacle.x;
        double y_dist = Detector.y - Obstacle.y;
        double z_dist = Detector.z - Obstacle.z;

        double distance = Math.Sqrt(Math.Pow(x_dist, 2) + Math.Pow(y_dist, 2) + Math.Pow(z_dist, 2));
        return distance;
    }

    Move_Info sample_distance_overlaps(Sphere Detector, Sphere Sample)
    {
        double x_dist = Detector.x - Sample.x;
        double y_dist = Detector.y - Sample.y;
        double z_dist = Detector.z - Sample.z;

        double distance = Math.Sqrt(Math.Pow(x_dist, 2) + Math.Pow(y_dist, 2) + Math.Pow(z_dist, 2));
        double min_distance = Detector.radius + Sample.radius;

        double adj_x = 0;
        double adj_y = 0;
        double adj_z = 0;


        if (distance + allowed_samples_overlap < min_distance)
        {
            // Deterimne how the x coordinate needs to be adjusted within the universe space
            if (x_dist < 0)
            {
                adj_x = min_distance - Math.Abs(x_dist);
            }
            else
            {
                adj_x = Math.Abs(x_dist) - min_distance;
            }

            if (Detector.x + adj_x < 0 || Detector.x + adj_x > 25)
            {
                // Do not adjust value if it will go out of range
                adj_x = 0;
            }

            // Deterimne how the y coordinate needs to be adjusted within the universe space
            if (y_dist < 0)
            {
                adj_y = min_distance - Math.Abs(y_dist);
            }
            else
            {
                adj_y = Math.Abs(y_dist) - min_distance;
            }

            if (Detector.y + adj_y < 0 || Detector.y + adj_y > 25)
            {
                // Do not adjust value if it will go out of range
                adj_y = 0;
            }

            // Deterimne how the x coordinate needs to be adjusted within the universe space
            if (z_dist < 0)
            {
                adj_z = min_distance - Math.Abs(z_dist);
            }
            else
            {
                adj_z = Math.Abs(z_dist) - min_distance;
            }

            if (Detector.z + adj_z < 0 || Detector.z + adj_z > 25)
            {
                // Do not adjust value if it will go out of range
                adj_z = 0;
            }

            // If no adjustments can be made, the changes should not be applied and the detector must be discarded
            if (adj_x == 0 & adj_y == 0 & adj_z == 0)
            {
                return new Move_Info(0, 0, 0, 0, false);
            }
            else
            {
                return new Move_Info(adj_x, adj_y, adj_z, 0, true);
            }

        }
        else
        {
            return new Move_Info(0, 0, 0, 0, true);
        }
    }

}
