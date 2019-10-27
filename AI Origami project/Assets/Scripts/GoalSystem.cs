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
    private int NumDiedStub;
    
    private int PastObstacles = 3; //how many obstacles we have passed 

    private float ObstacleLocation = 380; 

    private List<StubOrigami>  StubOrigamis; 
    private int NumOrigamicstub;
    
    

    

    void Start()
    {
        ManagerOfTheSystem = GameObject.Find("GameManager");
        MasterScript = ManagerOfTheSystem.GetComponent<GameManager>();
        //stubs();
        //origamis = MasterScript.origamis;
        //origamis = GameManager.origamis;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //CalcGoalStub();
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
                    
                    Debug.Log("<color=red>" +"Killed Origami" + "</color>");
                }
                else
                {
                    Ori.Age += 2;
                    Debug.Log("<color=orange>"+"Added to age" + "</color>");
                }
            }
            else
            {
                if (Ori.Age > 0)
                {
                    Ori.Age -= 1;
                    Debug.Log("<color=green>"+ "decreased Age" +"</color>");
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
                Debug.Log("<color=green>"+"Added new Origami" + "</color>");
            }

            NumOrigamics = MasterScript.origamis.Count;
        }
        else
        {
            if (CheckPastObstacle(ObstacleLocation)) //get string or change once we know how getting the data 
            {
                Debug.Log("<color=green>"+"Passed obstacle" +"</color>");
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


    void stubs() 
    {
        int NumOrigamisStub = 160; 
        StubOrigamis = new List<StubOrigami>(); 
        System.Random rnd = new System.Random();
        
        for (int i = 0; i <NumOrigamisStub -3; i++)
        {
            StubOrigamis.Add(new StubOrigami(new Vector3(rnd.Next(1,100),rnd.Next(1,100),rnd.Next(1,100)), 0));
            
        }
        StubOrigamis.Add(new StubOrigami(AverageLocationStub(), 0));
        StubOrigamis.Add(new StubOrigami(AverageLocationStub(), 0));
        StubOrigamis.Add(new StubOrigami(AverageLocationStub(), 0));

        Debug.Log("Stub origamis made");
        Debug.Log(StubOrigamis.Count);

        

    }




    public void CalcGoalStub() //will be called as i will need data on if part of mesh could call in update if morne has function for me to get part of mesh 
    {
        
        NumOrigamicstub = StubOrigamis.Count; 


        Vector3 Average = AverageLocationStub();
        System.Random rnd = new System.Random();

        double AverageDistance = 0.00;
        double[] Distances = new double[NumOrigamicstub]; //storing distances so i dont have to calculate again later 
        int position = 0;
        foreach (StubOrigami Ori in StubOrigamis) //Calculate average distance from average point 
        {
            Distances[position] = EuclidianDistance(Ori.Location, Average);
            AverageDistance += Distances[position];
            position +=1;
        }

        AverageDistance /= NumOrigamicstub;
        
        int Leeway = 10; //This is how much leeway to give on the average distance
        int MaxAge = 5; //This is the maximum age that an origami can get to
        position = 0; 
        List<StubOrigami> ToBedestroyed = new List<StubOrigami>();
        int ageDescrease = 0;
        int AgeIncrease = 0; 
        int deleted = 0;
        int ageZero = 0;
        for (int i = 0 ; i<NumOrigamicstub;i++)
        {
            if (Distances[position] > (AverageDistance + Leeway))
            {
                if (StubOrigamis[i].Age >= MaxAge)
                {
                    
                    ToBedestroyed.Add(StubOrigamis[i]); //add to list to be destroyed all as one
                    deleted+= 1;
                    
                }
                else
                {   
                    AgeIncrease +=1;
                    StubOrigamis[i].Age += 2;
                    
                }
            }
            else
            {
                if (StubOrigamis[i].Age > 0)
                {
                    ageDescrease += 1;
                    StubOrigamis[i].Age -= 1;
                    
                }else 
                {
                    ageZero +=1; 
                    
                    Debug.Log(StubOrigamis[i].Location);
                    StubOrigamis[i].Location =  new Vector3(rnd.Next(1,1000),rnd.Next(1,1000),rnd.Next(1,1000));
                    Debug.Log(StubOrigamis[i].Location);
                }

            }
            Debug.Log("<color=green>"+ "Age descreased " +ageDescrease +"</color>");
            Debug.Log("<color=orange>"+ "Age increased " +AgeIncrease + "</color>");
            Debug.Log("<color=red>" + "Deleted " +deleted + "</color>");
            Debug.Log("<color=green>" + "Age at zero " +ageZero + "</color>");
            ageDescrease = 0;
            AgeIncrease = 0; 
            deleted = 0;
            ageZero = 0; 

        position +=1;
        }

        for (int i = 0 ; i< ToBedestroyed.Count; i++)
        {
            
            StubOrigamis.Remove(ToBedestroyed[i]);
            NumDiedStub +=1;
        }
        NumOrigamicstub -= ToBedestroyed.Count;
        ToBedestroyed = null;
    }

    Vector3 AverageLocationStub() //calculate the average location to find a center point
    {
        Vector3 Average = new Vector3(0, 0, 0);
        foreach (StubOrigami Ori in StubOrigamis)
        {
            Average = Average + Ori.Location;
            //Average[1] = Average[1] + Ori.Location[1];
            //Average[2] = Average[2] + Ori.Location[2];
            
        }
        
        Average = Average / NumOrigamicstub;
        //Average[1] = Average[1] / NumOrigamicstub;
        //Average[2] = Average[2] / NumOrigamicstub;
        
        return Average;
    }
 // todo test for collision

}

public class StubOrigami
{
        public Vector3 Location;
        public int Age;

        public StubOrigami(Vector3 vect, int age)
        {
            Location = vect; 
            Age = age;
        }
}

