using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Kevin Matthew Julius 216007874
public class Obstacle : DTGameObject
{

    static int intName = 0;

    private string pattern;

    public Obstacle()
    {
        Name = "Obs" + intName;
        intName += 1;
    }

    public GameObject GameObject
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
