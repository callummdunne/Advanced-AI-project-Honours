using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Kevin Matthew Julius 216007874
public class GameManager : MonoBehaviour
{

    // Game variables
    public static double TIMESPLAYED = 0;

    // Danger Zones
    public const double ObstacleDZRADIUS = 10;
    public const double OrigamiDZRADIUS = 2;

    // Global Variables
    public static List<Origami> origamis = new List<Origami>();
    public static List<Obstacle> obstacles = new List<Obstacle>();
    public static int NumberOrigami = 100;
    public static int NumberObstacles = 100;

    // Start is called before the first frame update
    void Start()
    {
        Obstacle obstacle1 = new Obstacle();
        obstacle1.GameObject.transform.position = new Vector3(0, 0, 10);
        obstacle1.GameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        Obstacle obstacle2 = new Obstacle();
        obstacle2.GameObject.transform.position = new Vector3(4, 0, 10);
        obstacle2.GameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);

        Obstacle obstacle3 = new Obstacle();
        obstacle3.GameObject.transform.position = new Vector3(-4, 0, 10);
        obstacle3.GameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);

        Origami origami1 = new Origami();
        origami1.GameObject.transform.position = new Vector3(0, 0, 0);

        Origami origami2 = new Origami();
        origami2.GameObject.transform.position = new Vector3(0, 0, -2);

        Origami origami3 = new Origami();
        origami3.GameObject.transform.position = new Vector3(0, 0, -4);

        Origami origami4 = new Origami();
        origami4.GameObject.transform.position = new Vector3(4, 0, 0);

        Origami origami5 = new Origami();
        origami5.GameObject.transform.position = new Vector3(4, 0, -2);

        Origami origami6 = new Origami();
        origami6.GameObject.transform.position = new Vector3(-4, 0, -4);

        Origami origami7 = new Origami();
        origami7.GameObject.transform.position = new Vector3(1.5f, 0, 0);

        // test origami communication
        print("");
        print("===== Origami communication");
        print("==========================================================================");

        DTGameObject.SendSignal(origami1, "Hello, I am " + origami1.Name);

        // test obstacle communication
        AwakeOrigami();
        DTGameObject.SendSignal(obstacle1, obstacle1.Name + " is coming for you!!");

        //AwakeOrigami();
        DTGameObject.SendSignal(obstacle2, obstacle2.Name + " is coming for you!!");

        // get signals
        foreach(DTGameObject o in origamis)
        {
            if(NSA.CheckIfSelfCell(o.nsaPoint))
            {
                Origami origami = (Origami)o;
                List<string> signals = o.GetSignals();
                if(signals.Count != 0)
                {
                    foreach(string message in signals)
                    {
                        print(o.Name + " <- " + message);
                        if(message.Contains("OBS100"))
                        {
                            origami.GameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                        }
                        else if(message.Contains("OBS101"))
                        {
                            origami.GameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                        }
                        else if(message.Contains("OBS102"))
                        {
                            origami.GameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                        }
                    }
                }
            }
        }
        print("==========================================================================");
    }

    // Update is called once per frame
    void Update()
    {

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
    public static void RemoveOrigami(Origami origami)
    {
        origamis.Remove(origami);
    }

    // Add obstacle from game
    public static void AddObstacle(Obstacle obstacle)
    {
        obstacles.Add(obstacle);
    }

    // Remove obstacle from game
    public static void RemoveObstacle(Obstacle obstacle)
    {
        obstacles.Remove(obstacle);
    }

    public void AwakeOrigami()
    {
        foreach(DTGameObject o in origamis)
        {
            if(o.Name.Contains("ORI"))
            {
                Origami origami = (Origami)o;
                origami.AddSignal("Awake");
            }
        }
    }

}