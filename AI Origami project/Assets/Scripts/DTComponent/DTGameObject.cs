﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Kevin Matthew Julius 216007874
public class DTGameObject
{
    /// Danger Theory Model Classification
    /// ========================================
    ///
    /// Helper in Control (Bretscher & Cohn)
    /// ========================================
    /// Th = GameManager
    /// B = Obstacle or Origami
    /// Tk = Origami
    /// 
    /// Description:
    /// ========================================
    /// Signal two (second diagram) was introduced by Bretscher and Cohn. This helper signal 
    /// comes from a T helper cell (marked Th), on receipt of signal one from the B cell. That is,
    /// the B cell presents antigens to the T helper cell and awaits the T cell’s confirmation 
    /// signal. If the T cell recognises the antigen (which, if negative selection has worked, 
    /// should mean the antigen is non-self) then the immune response can commence.


    private string name;
    private bool signal1;
    private bool sentasignal;
    private List<string> receivedSignals;

    public string Name
    {
        get => name;
    }
    public bool Awake
    {
        get => signal1;
    }

    public DTGameObject()
    {
        name = "";
        signal1 = false;
        sentasignal = true;
        receivedSignals = new List<string>();
    }

    public void SetToOrigami()
    {
        string name = "ORI" + GameManager.NumberOrigami;
        ++GameManager.NumberOrigami;
        this.name = name;
        GameManager.AddOrigami((Origami) this);
    }

    public void SetToObstacle()
    {
        string name = "OBS" + GameManager.NumberObstacles;
        ++GameManager.NumberObstacles;
        this.name = name;
        GameManager.AddObstacle((Obstacle) this);
    }

    public void AddSignal(string signal)
    {
        if(signal.Equals("Awake"))
        {
            signal1 = true;
            sentasignal = false;
        }
        else if(signal1)
        {
            receivedSignals.Add(signal);
            signal1 = false;
            if(!sentasignal)
            {
                sentasignal = true;
                foreach(DTGameObject o in GameManager.origamis)
                {
                    if(o.Name.Contains("ORI") && this is Origami)
                    {
                        Origami otherOrigami = (Origami)o;
                        Origami origami = (Origami)this;
                        if(IsInRange(origami.GameObject, otherOrigami.GameObject, GameManager.OrigamiDZRADIUS) && (!origami.Equals(otherOrigami)))
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

    //// Danger Theory Helper Code

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

    public static void AwakeOrigami()
    {
        foreach(DTGameObject o in GameManager.origamis)
        {
            if(o.Name.Contains("ORI"))
            {
                Origami origami = (Origami)o;
                origami.AddSignal("Awake");
            }
        }
    }

    // find all origami close to the given origami
    // and send the message to the origami
    public static void SendSignal(DTGameObject gameObject, string signal)
    {
        foreach(DTGameObject o in GameManager.origamis)
        {
            if(o.Name.Contains("ORI"))
            {
                Origami origami = (Origami)o;
                if(gameObject is Origami otherOrigami)
                {
                    if(IsInRange(otherOrigami.GameObject, origami.GameObject, GameManager.OrigamiDZRADIUS) && (!gameObject.Name.Equals(origami.Name)))
                    {
                        origami.AddSignal(signal);
                    }
                }
                else if(gameObject is Obstacle obstacle)
                {
                    if(IsInRange(origami.GameObject, obstacle.GameObject, GameManager.ObstacleDZRADIUS))
                    {
                        o.AddSignal(signal);
                    }
                }
            }
        }

    }

}
