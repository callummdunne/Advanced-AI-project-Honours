using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalSystem : MonoBehaviour
{

    //Origami origamis;
    // Start is called before the first frame update
    GameManager MasterScript;
    void Start()
    {
        GameObject ManagerOfTheSystem = GameObject.Find("GameManager");
        GameManager MasterScript = ManagerOfTheSystem.GetComponent<GameManager>();
        //origamis = MasterScript.origamis;
        //origamis = GameManager.origamis;
        Debug.Log("It worked");
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    


    private int NumOrigamics; //how many origamis 
    private int NumBringBack = 0; //how many will be brought back after next obstacle 
    public int NumDied
    {
        get { return NumDied; }
        set { NumDied = value; }
    }
    private int PastObstacles = 0; //how many obstacles we have pasted 




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
        foreach (Origami Ori in MasterScript.origamis)
        {
            if (Distances[position] > (AverageDistance + Leeway))
            {
                if (Ori.Age >= MaxAge)
                {
                    //todo check if the origami is part of a mesh 

                    //myObject.GetComponent<MyScript>().MyFunction(); create a destroy function for the origamis
                    //todo call the destroy function
                    NumDied += 1;
                }
                else
                {
                    Ori.Age += 2;
                }
            }
            else
            {
                if (Ori.Age > 0)
                {
                    Ori.Age -= 1;
                }
            }
        position +=1;
        }
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
            PastObstacles = 0;
            for (int i = 0; i < NumBringBack; i++)
            {

                //addNewOrigami(i); call nsikas addOrigami function 
            }

            NumOrigamics = MasterScript.origamis.Count;
        }
        else
        {
            if (CheckPastObstacle("")) //get string or change once we know how getting the data 
            {
                PastObstacles += 1;
            }
        }
    }



    public bool CheckPastObstacle(string NextObstacle) //Checks if all origamis are past a obstacle to see if we should add origamis
    {
        //get the location of the next obstacle

        if(GetLocationObstacle(NextObstacle)[0] < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private Vector3 GetLocationObstacle(string str) //look at the naming scheme of obstacles and get location
    {
        return new Vector3(0, 0, 0); //todo get the location of the obstacle so i can check
    }

}

