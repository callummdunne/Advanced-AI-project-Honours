using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Kevin Matthew Julius 216007874
public class GameManager : MonoBehaviour
{
    public GameObject GameManagerObj;
    // Game variables
    public static double TIMESPLAYED = 0;
    public int intCounter = 1;

    // Danger Zones
    public const double ObstacleDZRADIUS = 10;
    public const double OrigamiDZRADIUS = 2;

    // Global Variables
    public static List<Origami> origamis = new List<Origami>();
    public static List<Obstacle> obstacles = new List<Obstacle>();
    public static int NumberOrigami = 100;
    public static int NumberObstacles = 100;

    // 
    

    // Start is called before the first frame update
    void Start()
    {

        GameManagerObj = GameObject.FindWithTag("GameManager");
        //Generate Patterns
        GameManagerObj.GetComponent<GetUserInput>().generatePatterns();

    }

    // Update is called once per frame
    void Update()
    {
        foreach(char c in Input.inputString)
        {

            if((c == '\n') || (c == '\r')) // enter/return
            {
                ArrayList userInputs = GetComponent<GetUserInput>().userInputs;
                //Create String of User Input
                string userInput = "";
                for(int i = 0; i < userInputs.Count; i++)
                {
                    //Debug.Log(userInputs[i]);
                    userInput += userInputs[i];
                }
                //print(userInput);

                //Call function to pattern match and send results to Ruan
                //Match patterns using Rchunk pattern matching

                ArrayList returnedObstaclesPattern = GetComponent<GetUserInput>().matchPattern(userInput.ToUpper());
                if(returnedObstaclesPattern.Count > 0)
                {
                    print("Returned a matching pattern");
                    for(int i = 0; i < returnedObstaclesPattern.Count; i++)
                    {
                        //Return Matched
                        print(returnedObstaclesPattern[i]);
                        //GENERATE OBSTACLES

                        //First check difficulty
                        GetComponent<GetUserInput>().increaseDifficulty(returnedObstaclesPattern,  intCounter);
                    }
                    //Generate List of Strings
                    GetComponent<GetUserInput>().patternStringsList = new ArrayList(); //First Reset List
                    int intCounting = 0;
                    string strObstacle = "";
                    for (int j = 0; j < returnedObstaclesPattern.Count; j++)
                    {
                        if (intCounting == 5)
                        {
                            GetComponent<GetUserInput>().patternStringsList.Add(strObstacle);
                            strObstacle = "";
                            intCounting = 0;
                        }
                        strObstacle += returnedObstaclesPattern[j];
                        intCounting++;
                    }
                }
                else
                {
                    print("No matching pattern found");
                }

                //Reset list after sending
                userInputs = new ArrayList();
            }
            else
            {
                GetComponent<GetUserInput>().userInputs.Add(c);
                //Debug.Log(c);
            }
        }
    }

    // increase times played by 1
    public static void IncTimesPlaye()
    {
        ++TIMESPLAYED;
    }


    // Add origami
    public static void AddOrigami(Origami origami)
    {
        origamis.Add(origami);
    }

    // Remove origami
    public static bool RemoveOrigami(Origami origami)
    {
        return origamis.Remove(origami);
    }

    // Add obstacle from game
    public static void AddObstacle(Obstacle obstacle)
    {
        obstacles.Add(obstacle);
    }

    // Remove obstacle from game
    public static bool RemoveObstacle(Obstacle obstacle)
    {
        return obstacles.Remove(obstacle);
    }



}