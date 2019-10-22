using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Kevin Matthew Julius 216007874
public class DTGameObject
{

    public static List<DTGameObject> gameObjects = new List<DTGameObject>();
    private static int NumberOrigami = 100;
    private static int NumberObstacles = 100;

    private string name;
    private bool activated;
    private bool sentasignal;
    private List<string> receivedSignals;

    public string Name
    {
        get => name;
    }
    public bool Activated
    {
        get => activated;
    }

    public DTGameObject()
    {
        name = "";
        activated = false;
        sentasignal = true;
        receivedSignals = new List<string>();
    }

    public void SetToOrigami()
    {
        string name = "ORI" + NumberOrigami;
        ++NumberOrigami;
        this.name = name;
        DTGameObject.AddGameObject(this);
    }

    public void SetToObstacle()
    {
        string name = "OBS" + NumberObstacles;
        ++NumberObstacles;
        this.name = name;
        DTGameObject.AddGameObject(this);
    }

    public void AddSignal(string signal)
    {
        if(signal.Equals("Activate"))
        {
            activated = true;
            sentasignal = false;
        }
        else if(activated)
        {
            receivedSignals.Add(signal);
            activated = false;
            if(!sentasignal)
            {
                sentasignal = true;
                foreach(DTGameObject o in DTGameObject.gameObjects)
                {
                    if(o.Name.Contains("ORI") && this is DTOrigami)
                    {
                        DTOrigami otherOrigami = (DTOrigami)o;
                        DTOrigami origami = (DTOrigami)this;
                        if(GameManager.IsInRange(origami.GameObject, otherOrigami.GameObject, GameManager.OrigamiDZRADIUS) && (!origami.Equals(otherOrigami)))
                        {
                            otherOrigami.AddSignal(signal);
                        }
                    }
                }
            }
        }
    }

    public List<string> GetSignals()
    {
        List<string> signals = new List<string>();
        foreach(string s in receivedSignals)
        {
            signals.Add(s);
        }
        receivedSignals.Clear();
        return signals;
    }

    public static void AddGameObject(DTGameObject gameObject)
    {
        gameObjects.Add(gameObject);
    }

    public static bool RemoveGameObject(DTGameObject gameObject)
    {
        return gameObjects.Remove(gameObject);
    }

}
