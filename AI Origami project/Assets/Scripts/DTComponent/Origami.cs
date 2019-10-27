using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Kevin Matthew Julius 216007874
public class Origami : DTGameObject
{

    static int intName= 0;

    GameObject gameObject;

    public Vector3 oldPosition;
    public Vector3 newPosition;

    public bool hasMoved;

    public Origami()
    {
       Name = "Ori" + intName;
       intName += 1;
    }

    public GameObject GameObject
    {
        get => gameObject;
        set => gameObject = value;
    }

    public GameObject myObject
    {
        get => gameObject;
        set => gameObject = value;
    }

    public int Age
    {
        get;
        set;
    }

    public string Pattern
    {
        get;
        set;
    }
    
}
