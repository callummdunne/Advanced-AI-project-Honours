using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSystem : MonoBehaviour
{
    private int CheckPointsPassed;
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
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    //Calculates the energy of the ball and removes any origamis if needed
    int CalcGoal() //needs the origamis 
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
            if (Distances[O] > AverageDistance + Leeway) //the five is just a place holder
            {
                if (Origamis[O].age > MaxAge) //need age of origami 
                {
                    //myObject.GetComponent<MyScript>().MyFunction(); call Julius remove origami 
                }
                else
                {
                    Origamis[O].age++; 
                }
            }
            else
            {
                Origamis[O].age = 0;
            }

        }


        //GameObject.FindGameObjectWithTag("Your_Tag_Here").transform.position; .transform.position
        //myObject.GetComponent<MyScript>().MyFunction();
        return ;
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

    
    bool CheckPastObstical()
    {
        return false;
    }








    
}
