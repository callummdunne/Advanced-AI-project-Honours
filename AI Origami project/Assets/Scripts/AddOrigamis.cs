using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AddOrigamis : MonoBehaviour
{
    //the structure for a single origami
    public struct Origami
    {
        public GameObject myObject;
        public string pattern;
        public int age;
    }

    //declaring a new game and mesh object
    GameObject origami;
    Mesh myMesh;

    //creating an array of origamis
    public Origami[] origamis = new Origami[1];

    public Origami[] origamiArray
    {
        get { return origamis; }
        set { origamis = value; }
    }

    //Arrays used in the creation of the object
    Vector3[] myVertices;
    int[] myTriangles;
    Color[] colors;

    //pattern generation arrays
    int[] mainHeight = { 0, 1, 2 };
    char[] origamiPO = { 'A', 'B', 'C' };
    int[] firstYVal = { 0, 1, 2 };
    int[] secondYVal = { 0, 1, 2 };
    int[] thirdYVal = { 0, 1, 2 };

    //variables used for the object position
    Vector3 currentPosition;
    Vector3 newPosition;

    //variables used for the object orientation
    Quaternion currentOrientation;
    Quaternion newOrientation;

    //list containing all the generated models
    List<string> models = new List<string>();
    List<string> initializationList = new List<string>();

    //other necessary variables
    int r = 3;
    int marker;
    int highestMatch = 0;

    void Awake()
    {        
        initializePatterns();
    }

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            addNewOrigami(i);
        }
    }

    //this function should be called in awake to initialise the random patterns
    public void initializePatterns()
    {
        //generating random patterns and storing them in the list
        do
        {
            string thisOne = generateRandomPatterns();

            if (!models.Contains(thisOne))
            {
                models.Add(thisOne);
            }

        } while (models.Count < 150);

        do
        {
            string thisOne = generateRandomPatterns();

            if (!initializationList.Contains(thisOne))
            {
                initializationList.Add(thisOne);
            }

        } while (initializationList.Count < 20);
    }

    //function used to generate a single origami robot.. call in a loop to create many
    public void addNewOrigami(int iFromLoop)
    {
        myMesh = new Mesh();

        origami = new GameObject("origami " + Convert.ToString(iFromLoop + 1));
        origami.AddComponent<MeshFilter>().mesh = myMesh;

        Material mat = AssetDatabase.GetBuiltinExtraResource<Material>("Sprites-Default.mat");

        origami.AddComponent<MeshRenderer>().material = mat;

        origami.transform.position = new Vector3(iFromLoop * 5, iFromLoop * 5, iFromLoop * 3);

        string newRobot = "";

        if (iFromLoop > 19)
        {
            newRobot = initializationList[iFromLoop / 10];
        }
        else
        {
            newRobot = initializationList[iFromLoop];
        }



        origamis[iFromLoop].myObject = origami;
        origamis[iFromLoop].pattern = newRobot;
        origamis[iFromLoop].age = 0;

        Array.Resize(ref origamis, origamis.Length + 1);

        StringMatcher(newRobot);

        myMesh.Clear();
        myMesh.vertices = myVertices;
        myMesh.triangles = myTriangles;
        myMesh.colors = colors;

    }

    //creates the model that matched the r-contiguous bits
    void makeModel(int main, char po, int one, int two, int three)
    {
        myVertices = new Vector3[]
        {
            new Vector3(0, main, 1), new Vector3(3, main, 4), new Vector3(4, main, 0),
            new Vector3(-1, one, 5), new Vector3(7, two, 3), new Vector3(1, three, -3)
        };

        myTriangles = new int[]
        {
            0, 1, 2,
            0, 3, 1,
            2, 1, 4,
            2, 5, 0
        };

        colors = new Color[myVertices.Length];

        for (int i = 0; i < myVertices.Length; i += 3)
        {
            colors[i] = Color.yellow;
            colors[i + 1] = Color.green;
            colors[i + 2] = Color.blue;
        }
    }

    //this function generates a new pattern each time it is called
    string generateRandomPatterns()
    {
        string generatedPattern = "";

        System.Random random = new System.Random();

        int ranNum = random.Next(3);
        generatedPattern += Convert.ToString(mainHeight[ranNum]);

        int ranNumTwo = random.Next(3);
        generatedPattern += Convert.ToString(origamiPO[ranNumTwo]);

        int ranNumThree = random.Next(3);
        generatedPattern += Convert.ToString(firstYVal[ranNumThree]);

        int ranNumFour = random.Next(3);
        generatedPattern += Convert.ToString(secondYVal[ranNumFour]);

        int ranNumFive = random.Next(3);
        generatedPattern += Convert.ToString(thirdYVal[ranNumFive]);

        return generatedPattern;
    }

    //this function checks a new generated robots to see if it matches and of the defined 'self space'
    void StringMatcher(string A)
    {
        char[] inputString = A.ToCharArray();

        int IS = inputString.Length;

        int rCounter = 0;

        marker = 0;
        highestMatch = 0;

        int modelCounter = 0;

        do
        {
            string model = models[modelCounter];

            char[] modelString = model.ToCharArray();

            int MS = modelString.Length;

            //checking to see if the strings being compared are equal in length (RCB rule)
            if (IS == MS)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (inputString[j] == modelString[j])
                    {
                        rCounter += 1;

                        //checking to see if the strings match at least r contiguous places (RCB rule)
                        if (rCounter >= r)
                        {
                            if (rCounter > highestMatch)
                            {
                                //Model matched should be returned with relevant position and orientation
                                highestMatch = rCounter;
                                marker = modelCounter;
                            }
                        }
                    }
                    else
                    {
                        //Reset rCounter                      
                    }
                }
            }
            else
            {
                //Alert that strings are not equal
            }
            //Reset rCounter
            rCounter = 0;
            modelCounter += 1;
        } while (modelCounter < models.Count);

        //return the matched model
        string match = models[marker];

        char[] splitMatch = match.ToCharArray();

        int main = Convert.ToInt32(splitMatch[0]);
        char po = splitMatch[1];
        int one = Convert.ToInt32(splitMatch[2]);
        int two = Convert.ToInt32(splitMatch[3]);
        int three = Convert.ToInt32(splitMatch[4]);

        makeModel(main, po, one, two, three);
    }

    //this function is used to get the current position (x, y, z) of the robot
    public Vector3 getCurrentPosition(GameObject robot)
    {
        currentPosition = robot.transform.position;

        return currentPosition;
    }

    //this function is used to change the position (x, y, z) of the robot
    public void changePosition(GameObject robot, float xUpdate, float yUpdate, float zUpdate)
    {
        newPosition.x = xUpdate;
        newPosition.y = yUpdate;
        newPosition.z = zUpdate;

        robot.transform.position = newPosition;
    }

    //this function gets the current orientation of the robot
    public Quaternion getCurrentOrientation(GameObject robot)
    {
        currentOrientation = robot.transform.rotation;

        return currentOrientation;
    }

    //this function is used to change the orientation of the robot
    public void changeOrientation(GameObject robot, float xUpdate, float yUpdate, float zUpdate)
    {
        newOrientation.x = xUpdate;
        newOrientation.y = yUpdate;
        newOrientation.z = zUpdate;

        robot.transform.rotation = newOrientation;
    }






































    //Callums code starts here

    
    private int NumOrigamics;
    private int NumBringBack = 0;
    public int NumDied
    {
        get { return NumDied; }
        set { NumDied = value; }
    }
    private int PastObstacles = 0; 




    public void CalcGoal() //will be called as i will need data on if part of mesh could call in update if morne has function for me to get part of mesh 
    {
        NumOrigamics = origamis.Length; //todo check that this has the right number 


        Vector3 Average = AverageLocation(origamis);


        double AverageDistance = 0.00;
        double[] Distances = new double[NumOrigamics]; //storing distances so i dont have to calculate again later 

        for(int j = 0; j < NumOrigamics; j++) //Calculate average distance from average point 
        {
            Distances[j] = EuclidianDistance(origamis[j].myObject.transform.position, Average);
            AverageDistance += Distances[j];
        }

        AverageDistance /= NumOrigamics;

        int Leeway = 5; //This is how much leeway to give on the average distance
        int MaxAge = 5; //This is the maximum age that an origami can get to
        for(int O = 0; O < NumOrigamics; O++)
        {
            if(Distances[O] > (AverageDistance + Leeway))
            {
                if(origamis[O].age >= MaxAge) 
                {
                    //todo check if the origami is part of a mesh 
                    
                    //myObject.GetComponent<MyScript>().MyFunction(); create a destroy function for the origamis
                    //todo call the destroy function

                }
                else
                {
                    origamis[O].age += 2;
                }
            }
            else
            {   
                if (origamis[O].age > 0)
                {
                    origamis[O].age -= 1;
                }
            }

        }
    }


        double EuclidianDistance(Vector3 first, Vector3 second) //calculate the distance between 2 vectors
    {
        float X = first.x - second.x;
        float Y = first.y - second.y;
        float Z = first.z - second.z;

        return Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
    }

    Vector3 AverageLocation(Origami[] Origamis) //calculate the average location to find a center point
    {
        Vector3 Average = new Vector3(0, 0, 0);
        for(int a = 0; a < NumOrigamics; a++)
        {
            Average = Average + Origamis[a].myObject.transform.position;
        }

        Average = Average / NumOrigamics;
        return Average;
    }

    
    void CreateMoreOrigamis()//function to call to create more origamis as is required 
    {
        int NumberOfObstaclesBeforeAdd = 3;
        if(PastObstacles >= NumberOfObstaclesBeforeAdd)
        {
            PastObstacles = 0;
            for(int i = 0; i < NumBringBack; i++)
            {

                addNewOrigami(i);
            }

            NumOrigamics = origamis.Length;
        }
        else
        {
            if(CheckPastObstacle("")) //get string or change once we know how getting the data 
            {
                PastObstacles += 1;
            }
        }
    }



    public bool CheckPastObstacle(string NextObstacle) //Checks if all origamis are past a obstacle to see if we should add origamis
    {
        

        for(int i = 0; i < NumOrigamics; i++)
        {
            if(GetLocationObstacle(NextObstacle)[0] < origamis[i].myObject.transform.position[0]) //todo change these once we know direction
            {
                if(GetLocationObstacle(NextObstacle)[1] < origamis[i].myObject.transform.position[1])
                {
                    if(GetLocationObstacle(NextObstacle)[2] < origamis[i].myObject.transform.position[2])
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }


    private Vector3 GetLocationObstacle(string str) //look at the naming scheme of obstacles and get location
    {
        return new Vector3(0, 0, 0); //todo get the location of the obstacle so i can check
    }

}
