using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Kevin Matthew Julius 216007874
public class Origami : DTGameObject
{

    GameObject gameObject;

    public Origami()
    {
       
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
