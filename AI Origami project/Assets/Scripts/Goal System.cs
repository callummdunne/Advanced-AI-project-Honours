using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSystem : MonoBehaviour
{
    private int CheckPointsPassed;
    private int NumBringBack;
    private int PastObstacles;
    public int NumOrigamics
    {
        get
        {
            return NumOrigamics;
        }
        set
        {
            NumOrigamics = value;

        }
    }
    public int Energy
    {
        get { 
            return Energy; 
        } 
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //create the right number of origamis 
    }


    // Update is called once per frame
    void Update()
    {
        AddOrigamis();
    }


    //Calculates the energy of the ball and removes any origamis if needed
    void CalcGoal() //will be called as i will need data on if part of mesh could call in update if morne has function for me to get part of mesh 
    {
        GameObject[] Origamis = GetOrigamis();


        Vector3 Average = AverageLocation(Origamis); 
        

        double AverageDistance = 0.00;
        double[] Distances = new double[NumOrigamics]; //storing distances so i dont have to calculate again later 

        for (int j = 0; j < NumOrigamics; j++) //Calculate average distance from average point 
        {
            Distances[j] = EuclidianDistance(Origamis[j].transform.position, Average);
            AverageDistance += Distances[j];
        }

        AverageDistance /= NumOrigamics;

        int Leeway = 5; //This is how much leeway to give on the average distance
        int MaxAge = 5; //This is the maximum age that an origami can get to
        for (int O = 0; O < NumOrigamics; O++)
        {
            if (Distances[O] > AverageDistance + Leeway) 
            {
                if (Origamis[O].age > MaxAge) //need age of origami 
                {
                    //check if the origami is part of a mesh 
                    // add to their age if they are part of smaller balls so check the average distance is large then we can add to the age of the origamis
                    //myObject.GetComponent<MyScript>().MyFunction(); create a destroy function for the origamis
                }
                else
                {
                    Origamis[O].age+= 2 ; 
                }
            }
            else
            {
                Origamis[O].age -= 1 ; 
            }

        }


        //GameObject.FindGameObjectWithTag("Your_Tag_Here").transform.position; .transform.position
        //myObject.GetComponent<MyScript>().MyFunction();
        
    }

    double EuclidianDistance(Vector3 first, Vector3 second)
    {
        float X = first.x - second.x;
        float Y = first.y - second.y;
        float Z = first.z - second.z;
         
        return Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
    }

    


    private GameObject[] GetOrigamis() //get all the origamis
    {
        GameObject[] Origamis = new GameObject[NumOrigamics];
        for (int i = 0; i < NumOrigamics; i++)
        {
            Origamis[i] = GameObject.FindGameObjectWithTag("Origamis" + i);
        }
        return Origamis;
    }

    Vector3 AverageLocation(GameObject[] Origamis) //calculate the average location to find a center point
    {
        Vector3 Average = new Vector3(0, 0, 0);
        for (int a = 0; a < NumOrigamics; a++)
        {
            Average = Average + Origamis[a].transform.position;
        }

        Average = Average / NumOrigamics;
        return Average;
    }

    
    void AddOrigamis()
    {
        int NumberOfObstaclesBeforeAdd = 3;
        if (PastObstacles > NumberOfObstaclesBeforeAdd)
        {
            PastObstacles = 0;
            for (int i = 0; i < NumBringBack; i++)
            {
                
                //Call function to add origamis 
            }
        }
        else
        {
            if (CheckPastObstacle(""))
            {
                PastObstacles += 1;
            }
        }
    }


    bool CheckPastObstacle(string NextObstacle) //Checks if all origamis are past a obstacle to see if we should add origamis
    {
        GameObject[] Origamis = GetOrigamis();
        
        for (int i = 0; i < NumOrigamics; i++)
        {
            if (GetLocationObstacle(NextObstacle)[0] < Origamis[i].transform.position[0])
            {
                if (GetLocationObstacle(NextObstacle)[1] < Origamis[i].transform.position[1])
                {
                    if (GetLocationObstacle(NextObstacle)[2] < Origamis[i].transform.position[2])
                    {
                        return false; 
                    }
                }
            }


        }
        return true;
    }


    void CollisionHappened(string DangerSignal) //Maybe give the value or just call it and i can do a calculation 
    {

    }


    private Vector3 GetLocationObstacle(string str) //look at the naming scheme of obstacles and get location
    {
        return new Vector3(0, 0, 0);
    }

    private Vector3 GetLocationOrigamis(string str) //will change depending on how i get given the origamis 
    {
        return new Vector3(0, 0, 0);
    }


    //make a function to find out the number of origamis that should come back and change that values 
    // if collision then we can lower this value so as to stop as many origamis coming back








    
}
