using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AddOrigamis : MonoBehaviour
{

    public GameObject GameManagerObj;

    //declaring a new game and mesh object
    Mesh myMesh;

    public bool origamisGenerated = false;

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
        GameManagerObj = GameObject.FindWithTag("GameManager");

        for (int i = 0; i < 5; i++)
        {
            addNewOrigami(i);
        }

        origamisGenerated = true;
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

    public bool isOrigamisGenerated()
    {
        return origamisGenerated;
    }

    //function used to generate a single origami robot.. call in a loop to create many
    public void addNewOrigami(int iFromLoop)
    {
        myMesh = new Mesh();

        Origami origami = new Origami();

        origami.GameObject = new GameObject("origami " + Convert.ToString(iFromLoop + 1));
        origami.GameObject.AddComponent<MeshFilter>().mesh = myMesh;

        Material mat = AssetDatabase.GetBuiltinExtraResource<Material>("Sprites-Default.mat");

        origami.GameObject.AddComponent<MeshRenderer>().material = mat;

        origami.GameObject.transform.position = new Vector3(iFromLoop * 5, iFromLoop * 5, iFromLoop * 3);

        string newRobot = "";

        if (iFromLoop > 19)
        {
            newRobot = initializationList[iFromLoop / 10];
        }
        else
        {
            newRobot = initializationList[iFromLoop];
        }

        origami.Pattern = newRobot;
        origami.Age = 0;
        //origamis[iFromLoop].inPosition = origami.transform.position;
        GameManagerObj.GetComponent<GameManager>().AddOrigami(origami);

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

    //one triangle origami function
    void makeModelOneTriangle(int main, char po, int one, int two, int three)
    {
        myVertices = new Vector3[]
        {
            new Vector3(0, main, 1), new Vector3(3, main, 4), new Vector3(4, main, 0),
            //new Vector3(-1, one, 5), new Vector3(7, two, 3), new Vector3(1, three, -3)
        };

        myTriangles = new int[]
        {
            0, 1, 2
            //0, 3, 1,
            //2, 1, 4,
            //2, 5, 0
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

        makeModelOneTriangle(main, po, one, two, three);
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

}
