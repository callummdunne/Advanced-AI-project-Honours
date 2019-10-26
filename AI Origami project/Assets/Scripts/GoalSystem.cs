using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalSystem : MonoBehaviour
{

    //Origami origamis;
    // Start is called before the first frame update
    private GameManager MasterScript; //Kevins script
    private GameObject ManagerOfTheSystem; //access to all other scripts
     private int NumOrigamics; //how many origamis 
    private int NumBringBack = 0; //how many will be brought back after next obstacle 
    public int NumDied { get; set; } = 0; //How many are set you die
    
    private int PastObstacles = 3; //how many obstacles we have passed 

    private float ObstacleLocation = 380; 

    void Start()
    {
        ManagerOfTheSystem = GameObject.Find("GameManager");
        MasterScript = ManagerOfTheSystem.GetComponent<GameManager>();
        //origamis = MasterScript.origamis;
        //origamis = GameManager.origamis;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        CalcGoal();
        
        CreateMoreOrigamis();
        
    }



    public void CalcGoal() //will be called as i will need data on if part of mesh could call in update if morne has function for me to get part of mesh 
    {
        
        NumOrigamics = MasterScript.origamis.Count; 


        Vector3 Average = AverageLocation();


        double AverageDistance = 0.00;
        double[] Distances = new double[NumOrigamics]; //storing distances so i dont have to calculate again later 
        int position = 0;
        foreach (Origami Ori in MasterScript.origamis) //Calculate average distance from average point 
        {
            Distances[position] = EuclidianDistance(Ori.GameObject.transform.position, Average);
            AverageDistance += Distances[position];
            position +=1;
        }

        AverageDistance /= NumOrigamics;

        int Leeway = 5; //This is how much leeway to give on the average distance
        int MaxAge = 5; //This is the maximum age that an origami can get to
        position = 0; 
        List<Origami> ToBedestroyed = new List<Origami>();
        foreach (Origami Ori in MasterScript.origamis)
        {
            if (Distances[position] > (AverageDistance + Leeway))
            {
                if (Ori.Age >= MaxAge)
                {
                    
                    ToBedestroyed.Add(Ori); //add to list to be destroyed all as one
                    
                    Debug.Log("Killed Origami");
                }
                else
                {
                    Ori.Age += 2;
                    Debug.Log("Added to age");
                }
            }
            else
            {
                if (Ori.Age > 0)
                {
                    Ori.Age -= 1;
                    Debug.Log("Took away from Age");
                }
            }
        position +=1;
        }

        for (int i = 0 ; i< ToBedestroyed.Count; i++)
        {
            MasterScript.RemoveOrigami(ToBedestroyed[i]);
            NumDied +=1;
        }
        ToBedestroyed = null;
    }


    double EuclidianDistance(Vector3 first, Vector3 second) //calculate the distance between 2 vectors
    {
        float X = first.x - second.x;
        float Y = first.y - second.y;
        float Z = first.z - second.z;

        return Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
    }

    Vector3 AverageLocation() //calculate the average location to find a center point
    {
        Vector3 Average = new Vector3(0, 0, 0);
        foreach (Origami Ori in MasterScript.origamis)
        {
            Average = Average + Ori.GameObject.transform.position;
        }

        Average = Average / NumOrigamics;
        return Average;
    }


    void CreateMoreOrigamis()//function to call to create more origamis as is required 
    {
        int NumberOfObstaclesBeforeAdd = 3;
        if (PastObstacles >= NumberOfObstaclesBeforeAdd)
        {
            NumDied =0;
            CalcAddOrigamis();
            PastObstacles = 0;
            for (int i = 0; i < NumBringBack; i++)
            {

                //addNewOrigami(i); call nsikas addOrigami function 
                ManagerOfTheSystem.GetComponent<AddOrigamis>().addNewOrigami(i);
                Debug.Log("Added new Origami");
            }

            NumOrigamics = MasterScript.origamis.Count;
        }
        else
        {
            if (CheckPastObstacle(ObstacleLocation)) //get string or change once we know how getting the data 
            {
                Debug.Log("Passed obstacle");
                PastObstacles += 1;
            }
        }
    }



    public bool CheckPastObstacle(float  NextObstacle) //Checks if all origamis are past a obstacle to see if we should add origamis
    {
        //get the location of the next obstacle
        ObstacleLocation = NextObstacle;
        if(NextObstacle < 0) // get the location of the obstacle 
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void CalcAddOrigamis()
    {
        int BallsDied = NumDied / 40; 

        if (BallsDied  < 2) 
        {
            NumBringBack = 30;
        }
        else if(BallsDied < 3)
        {
            NumBringBack =20;
        }
        else if(BallsDied < 4)
        {
            NumBringBack =10;
        }
        else if (BallsDied < 1 )
        {
            NumBringBack =40;
        }
        else {
            NumBringBack = 0;
        }



        
    }



}

