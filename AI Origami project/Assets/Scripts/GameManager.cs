using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Kevin Matthew Julius 216007874
public class GameManager : MonoBehaviour
{
    public GameObject GameManagerObj;
    // Game variables
    public double TIMESPLAYED = 0;
    public int intCounter = 1;

    // Danger Zones
    public const double ObstacleDZRADIUS = 10;
    public const double OrigamiDZRADIUS = 2;

    // Global Variables
    public List<Origami> origamis = new List<Origami>();
    public List<Obstacle> obstacles = new List<Obstacle>();
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
                ArrayList userInputs = GameManagerObj.GetComponent<GetUserInput>().userInputs;
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

                ArrayList returnedObstaclesPattern = GameManagerObj.GetComponent<GetUserInput>().matchPattern(userInput.ToUpper());
                if(returnedObstaclesPattern.Count > 0)
                {
                    print("Returned a matching pattern");
                    //for(int i = 0; i < returnedObstaclesPattern.Count; i++)
                    //{
                    //    //Return Matched
                    //    print(returnedObstaclesPattern[i]);
                      
                    //    //First check difficulty
                    //    GetComponent<GetUserInput>().increaseDifficulty(returnedObstaclesPattern,  intCounter);
                    //}

                    //Generate List of Strings
                    GetComponent<GetUserInput>().PatternStringsList = new ArrayList(); //First Reset List
                    //int intCounting = 0;
                    string strObstacle = "";
                    //for(int j = 0; j < returnedObstaclesPattern.Count; j++)
                    //{
                    //    if (intCounting < 5)
                    //    {
                    //        strObstacle += returnedObstaclesPattern[j];
                    //        intCounting++;
                    //    }
                    //    else {
                    //        GetComponent<GetUserInput>().PatternStringsList.Add(strObstacle);
                    //        strObstacle = "";
                    //        intCounting = 0;
                    //    }                        
                    //}

                    for (int j = 0; j < 5; j++)
                    {
                        strObstacle += returnedObstaclesPattern[j];
                    }

                    //Increase difficulty if necessary
                    for (int i = 0; i < GetComponent<GetUserInput>().PatternStringsList.Count; i++)
                    {
                        //ArrayList to Increase Difficulty of
                        ArrayList toIncrease = new ArrayList();
                        //Valid String
                        string validString = (string)GetComponent<GetUserInput>().PatternStringsList[i];
                        //Current Obstacle Combo
                        for (int k = 0; k < validString.Length; k++)
                        {
                            //Return Matched
                            print(validString[k]);
                            toIncrease.Add(validString[k]);
                        }

                        //First check difficulty
                        string result = GetComponent<GetUserInput>().increaseDifficulty(toIncrease, intCounter);
                        GetComponent<GetUserInput>().PatternStringsList[i] = result;
                    }

                    //Send List
                    //GameManagerObj.GetComponent<wallsNramps>().setObstacleListOfObstacles(GetComponent<GetUserInput>().PatternStringsList);
                    GameManagerObj.GetComponent<wallsNramps>().setObstacleListOfObstacles(strObstacle);
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
                GameManagerObj.GetComponent<GetUserInput>().userInputs.Add(c);
                //Debug.Log(c);
            }
        }
    }

    // increase times played by 1
    public void IncTimesPlaye()
    {
        ++TIMESPLAYED;
    }


    // Add origami
    public void AddOrigami(Origami origami)
    {
        origamis.Add(origami);
    }

    // Remove origami
    public bool RemoveOrigami(Origami origami)
    {
        return origamis.Remove(origami);
    }

    // Add obstacle from game
    public void AddObstacle(Obstacle obstacle)
    {
        obstacles.Add(obstacle);
    }

    // Remove obstacle from game
    public bool RemoveObstacle(Obstacle obstacle)
    {
        return obstacles.Remove(obstacle);
    }



}