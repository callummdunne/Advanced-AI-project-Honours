using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Kevin Matthew Julius 216007874
public class GameManager : MonoBehaviour
{

    // Danger Theory Model Classification
    // ==================================
    // Antibodies/Tk = Origami Swarm
    // Antigens/B = Obstacles
    // Cells = Origmi
    // Damaged Cell = Damaged Origami
    // APC/Th  = GameManager

    // Helper in Control (Bretscher & Cohn)
    // Th = GameManager
    // B = Obstacle
    // Tk =  
    // Signal two (second diagram) was introduced by Bretscher and Cohn. This helper signal 
    // comes from a T helper cell (marked Th), on receipt of signal one from the B cell. That is,
    // the B cell presents antigens to the T helper cell and awaits the T cell’s confirmation 
    // signal. If the T cell recognises the antigen (which, if negative selection has worked, 
    // should mean the antigen is non-self) then the immune response can commence.

    public const double ObstacleDZRADIUS = 10;
    public const double OrigamiDZRADIUS = 2;


    // Start is called before the first frame update
    void Start()
    {

        //// TEST CODE Starts Here
        DTObstacle obstacle1 = new DTObstacle();
        obstacle1.GameObject.transform.position = new Vector3(0, 0, 10);
        obstacle1.GameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        DTObstacle obstacle2 = new DTObstacle();
        obstacle2.GameObject.transform.position = new Vector3(4, 0, 10);
        obstacle2.GameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);

        DTObstacle obstacle3 = new DTObstacle();
        obstacle3.GameObject.transform.position = new Vector3(-4, 0, 10);
        obstacle3.GameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);

        DTOrigami origami1 = new DTOrigami();
        origami1.GameObject.transform.position = new Vector3(0, 0, 0);

        DTOrigami origami2 = new DTOrigami();
        origami2.GameObject.transform.position = new Vector3(0, 0, -2);

        DTOrigami origami3 = new DTOrigami();
        origami3.GameObject.transform.position = new Vector3(0, 0, -4);

        DTOrigami origami4 = new DTOrigami();
        origami4.GameObject.transform.position = new Vector3(4, 0, 0);

        DTOrigami origami5 = new DTOrigami();
        origami5.GameObject.transform.position = new Vector3(4, 0, -2);

        DTOrigami origami6 = new DTOrigami();
        origami6.GameObject.transform.position = new Vector3(-4, 0, -4);

        DTOrigami origami7 = new DTOrigami();
        origami7.GameObject.transform.position = new Vector3(1.5f, 0, 0);

        // test origami communication
        print("Origami communication");

        SendSignal(origami1, "Hello, I am " + origami1.Name);

        // test obstacle communication
        ActivateOrigami();
        SendSignal(obstacle1, obstacle1.Name + " is coming for you!!");

        //ActivateOrigami();
        SendSignal(obstacle2, obstacle2.Name + " is coming for you!!");

        // get signals
        foreach(DTGameObject o in DTGameObject.gameObjects)
        {
            if(o.Name.Contains("ORI"))
            {
                DTOrigami origami = (DTOrigami)o;
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
        //// TEST CODE Ends Here

    }

    // Update is called once per frame
    void Update()
    {
        //    if (Input.GetMouseButtonDown(0)) {
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //        RaycastHit hit;
        //        if (Physics.Raycast(ray, out hit)) {
        //            Destroy(hit.collider.gameObject);
        //        }
        //    }
    }

    //// Danger Theory Code

    // Check if in range using Euclidean Distance
    public static bool IsInRange(GameObject o1, GameObject o2, double range)
    {
        Vector3 point1 = o1.transform.position;
        Vector3 point2 = o2.transform.position;
        float x2 = (point1.x - point2.x) * (point1.x - point2.x);
        float y2 = (point1.y - point2.y) * (point1.y - point2.y);
        float z2 = (point1.z - point2.z) * (point1.z - point2.z);
        float distance = Mathf.Sqrt(x2 + y2 + z2);
        return (distance <= range);
    }

    public void ActivateOrigami()
    {
        foreach(DTGameObject o in DTGameObject.gameObjects)
        {
            if(o.Name.Contains("ORI"))
            {
                DTOrigami origami = (DTOrigami)o;
                origami.AddSignal("Activate");
            }
        }
    }

    // find all origami close to the given origami
    // and send the message to the origami
    public void SendSignal(DTGameObject gameObject, string signal)
    {
        foreach(DTGameObject o in DTGameObject.gameObjects)
        {
            if(o.Name.Contains("ORI"))
            {
                DTOrigami origami = (DTOrigami)o;
                if(gameObject is DTOrigami otherOrigami)
                {
                    if(IsInRange(otherOrigami.GameObject, origami.GameObject, OrigamiDZRADIUS) && (!gameObject.Name.Equals(origami.Name)))
                    {
                        origami.AddSignal(signal);
                    }
                }
                else if(gameObject is DTObstacle obstacle)
                {
                    if(IsInRange(origami.GameObject, obstacle.GameObject,ObstacleDZRADIUS))
                    {
                        o.AddSignal(signal);
                    }
                }
            }
        }

    }

}