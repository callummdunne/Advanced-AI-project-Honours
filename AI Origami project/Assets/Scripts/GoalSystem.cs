using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalSystem : MonoBehaviour
{

    public struct Origami
    {
        public GameObject myObject;
        public string pattern;
        public int age;
    }

    //creating an array of origamis
    public Origami[] origamis = new Origami[1];


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

        for (int j = 0; j < NumOrigamics; j++) //Calculate average distance from average point 
        {
            Distances[j] = EuclidianDistance(origamis[j].myObject.transform.position, Average);
            AverageDistance += Distances[j];
        }

        AverageDistance /= NumOrigamics;

        int Leeway = 5; //This is how much leeway to give on the average distance
        int MaxAge = 5; //This is the maximum age that an origami can get to
        for (int O = 0; O < NumOrigamics; O++)
        {
            if (Distances[O] > (AverageDistance + Leeway))
            {
                if (origamis[O].age >= MaxAge)
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
        for (int a = 0; a < NumOrigamics; a++)
        {
            Average = Average + Origamis[a].myObject.transform.position;
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

            NumOrigamics = origamis.Length;
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


        for (int i = 0; i < NumOrigamics; i++)
        {
            if (GetLocationObstacle(NextObstacle)[0] < origamis[i].myObject.transform.position[0]) //todo change these once we know direction
            {
                if (GetLocationObstacle(NextObstacle)[1] < origamis[i].myObject.transform.position[1])
                {
                    if (GetLocationObstacle(NextObstacle)[2] < origamis[i].myObject.transform.position[2])
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

