using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

/// <summary>
/// place ramp in front of player when left mouse is clicked
/// </summary>

public class wallsNramps : MonoBehaviour
{
    // Counter for increase diffuculty functionality
    //this shows how many obstacles have been generated so far
    public int numObstaclesGenerated;
 

    //vars for internal use; I'll add comments here later but nobody should need to use these directly
    public GameObject ramp;
    public GameObject wall;
    private GameObject nextObject;

    private Transform myTransform;
    public Transform cameraHeadTransform;

    private float placeRate = 1;
    private float nextPlace = 0;

    //ramp position
    private Vector3 rampPos = new Vector3();
    private Vector3 wallPos = new Vector3();
    private Transform newWallPos;
    private ArrayList selfSet = new ArrayList();
    private Dictionary<System.String, System.Int32> detectors = new Dictionary<System.String, System.Int32>();
    private int detectorRange = 10;

    //these values should changes if balls are bigger or smaller
    private float minLeftValue = 33;
    private float scaleWidth = 20;
    // scales for obstacles
    private float obstacleScales = 1000;

    //list of next obstacles to be created
    private ArrayList nextObstacles = new ArrayList();

    //Flag to indicate when next obstacle can be generated
    private bool flagCreateNext = false;

    //string of next obstacle
    private string nextObstacle;

    // Use this for initialization
    void Start()
    {
        myTransform = transform;
        Debug.Log("Camera Pos: " + cameraHeadTransform);
        ReadString();
        getDetectors();
        Debug.Log(selfSet.Count);
        foreach (string detector in selfSet)
        {
            Debug.Log(detector.ToString());
        }
        numObstaclesGenerated = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (flagCreateNext)
        {
            createNextObstacle();
            flagCreateNext = false;
        }
        
    }

    //----------------getters and setters
    public void setFlagNext(bool flag)
    {
        flagCreateNext = flag;
    }

    public string getNextObstacleString()
    {
        return nextObstacle;
    }

    public GameObject getNextObstacle()
    {

    }
    //----------------end getters and setters

    void getDetectors()
    {
        bool stillMerging = true;
        int current = 0;

        while (stillMerging)
        {
            //find closest pair for current detector
            int minDistance = 100;
            int bestMatch = -1;
            string tempDetector = "";
            for (int next = current + 1; next < selfSet.Count; next++)
            {
                string d1 = (string)selfSet[current];
                string d2 = (string)selfSet[next];
                int tempDist = calcDistance(d1, d2);
                if ((tempDist < minDistance) && (tempDist <= detectorRange))
                {
                    bestMatch = next;
                    minDistance = tempDist;
                    tempDetector = mergeDetectors(d1, d2);
                }
                
            }
            if (bestMatch == -1)
            {
                current += 1;
                //check if this was last detector
                if (current >= selfSet.Count - 1)
                {
                    stillMerging = false;
                }
            }
            else
            {
                //add new detector in current's spot
                selfSet[current] = tempDetector;
                //remove merged detector
                selfSet.RemoveAt(bestMatch);
            }

        }
    }
    //read from file containing self set
    void ReadString()
    {
        string path = "Assets/Scripts/ObstacleGeneration/Data/selfSet.txt";

        //Read the text from directly from the test.txt file
        StreamReader sr = new StreamReader(path);
        string line;
        do
        {
            line = sr.ReadLine();
            //randomly read with 25% chance of saving string as known self
            int chance = UnityEngine.Random.Range(0, 5);
            if (chance == 1)
            {
                selfSet.Add(line);
                detectors.Add(line, 1);
            }
        } while (!sr.EndOfStream);
        Debug.Log(selfSet.Count);
    }

    int calcDistance(string s1, string s2)
    {
        int distance = 0;
        //the first char has increased weight
        char char1 = s1[0];
        char char2 = s2[0];
        if (char1 != char2)
        {
            distance += 5;
        }

        //get dist from left substring
        try
        {
            int left1 = int.Parse(s1.Substring(1, 2));
            int left2 = int.Parse(s2.Substring(1, 2));
            int result = left1-left2;
            distance += Math.Abs(result);
            //get first last two integers and calc distance
            int xSize1 = int.Parse(s1.Substring(3,1));
            int xSize2 = int.Parse(s2.Substring(3,1));
            int ySize1 = int.Parse(s1.Substring(4,1));
            int ySize2 = int.Parse(s2.Substring(4,1));
            distance += Math.Abs(xSize1 - xSize2);
            distance += Math.Abs(ySize1 - ySize2);
        }
        catch (Exception e)
        {
            Debug.Log("NaN");
        }

        return distance;
    }

    string mergeDetectors(string d1, string d2)
    {
        //if first char not the same, chose first one
        string returnString = "";
        string type = d1.Substring(0,1);
        returnString += type;
        int val1 = int.Parse(d1.Substring(1,1));
        int val2 = int.Parse(d2.Substring(1, 1));
        double tempAve = (val1+val2) / 2;
        int ave = (int)tempAve;
        //getting second val
        val1 = int.Parse(d1.Substring(2, 1));
        val2 = int.Parse(d2.Substring(2, 1));
        tempAve = (val1 + val2) / 2;
        int ave2 = (int)tempAve;
        //add to return string
        string strVal = ave.ToString();
        returnString += strVal;
        strVal = ave2.ToString();
        returnString += strVal;
        //get last two ints
        val1 = int.Parse(d1[3].ToString());
        val2 = int.Parse(d2[3].ToString());
        tempAve = (val1 + val2) / 2;
        ave = (int)tempAve;
        //getting third val
        val1 = int.Parse(d1[4].ToString());
        val2 = int.Parse(d2[4].ToString());
        tempAve = (val1 + val2) / 2;
        ave2 = (int)tempAve;
        //add to return string
        returnString += ave.ToString();
        returnString += ave2.ToString();
        return returnString;
    }


    //this method accepts an arraylist of strings representing obstacles to be generated
    public void setObstacleListOfObstacles(ArrayList obstacleStrings)
    {
        createObstacleWithCode("W3333");
        /*
        Debug.Log(obstacleStrings.Count);
        foreach (object d in obstacleStrings){
            createObstacleWithCode((string)d);
        }
        */
        
    }

    //call this method to create a new obstacle based on a given code
    public void createObstacleWithCode(string obstacleCode)
    {
        Debug.Log("Creating Obstacle with Code");
        Debug.Log(obstacleCode);
        //find closest matching detector
        //set min distance to first detector in list   
        //get the first detector in the set
        string detector = (string)selfSet[0];
        int minDistance = calcDistance(detector, obstacleCode);
        string bestDetector = detector;
        for (int d = 1; d < selfSet.Count; d++)
        {
            detector = (string)selfSet[d];
            int tempDistance = calcDistance(detector, obstacleCode);
            if (minDistance > tempDistance){
                minDistance = tempDistance;
                bestDetector = detector;
            }
        }

        //add detector to queue for next obstacle to be generated
        Debug.Log("Creating Obstacle");
        Debug.Log(detector);
        nextObstacles.Add(detector);
    }

    //call this method to add next obstacle in the list
    public void createNextObstacle() {
        if (nextObstacles.Count == 0)
        {
            return;
        }
        string detector = (string)nextObstacles[0];
        nextObstacle = detector;
        detectorToObstacle(detector);
        //remove from list
        nextObstacles.RemoveAt(0);
    }

    //convert the given detector to an obstacle and instantiate it
    private void detectorToObstacle(string detector)
    {
        Debug.Log("instantiating obstacle");
        Debug.Log(detector);
        //create wall or ramp depending on first char
        if (detector[0] == 'W')
        {
            float left = float.Parse(detector.Substring(1, 2));
            //convert to number between 0 and 4
            //given number is 33 - 69
            left = left - minLeftValue;
            left = left / 10;
            left = (float)(Math.Round((double)left));

            //width of object
            float xScale = float.Parse(detector.Substring(3, 1));
            xScale = xScale - 2;
            xScale = xScale * obstacleScales;
            //heigth of object
            float zScale = float.Parse(detector.Substring(4, 1));
            zScale = zScale - 2;
            zScale = zScale * obstacleScales;

            wallPos = cameraHeadTransform.TransformPoint(scaleWidth * left, zScale/100, 385 );
 
            GameObject newWall = Instantiate(wall, wallPos, Quaternion.Euler(270, 0, 0));
            newWall.transform.localScale = new Vector3(xScale, 500, zScale);
            newWall.transform.position = new Vector3(scaleWidth * left, zScale/100, 385);

            //store next obstacle as general game object
            nextObject = newWall;


        }
        else if (detector[0] == 'R') {
            float left = float.Parse(detector.Substring(1, 2));
            //convert to number between 0 and 4
            //given number is 33 - 67
            left = left - minLeftValue;
            left = left / 10;
            left = (float)(Math.Round((double)left));

            //width of object
            float yScale = float.Parse(detector.Substring(3, 1));
            yScale = yScale - 3;
            yScale = yScale * obstacleScales;
            //heigth of object
            float zScale = float.Parse(detector.Substring(4, 1));
            zScale = zScale - 3;
            zScale = zScale * obstacleScales;

            //create new ramp game object
            rampPos = new Vector3(scaleWidth * left, 385, zScale / obstacleScales);
            ramp.transform.localScale = new Vector3(yScale, 200f, zScale);
            GameObject newRamp = Instantiate(ramp, rampPos, Quaternion.Euler(270, 270, 0));


            //store as newest game object
            nextObject = newRamp;
            
            


        }
    }
}
