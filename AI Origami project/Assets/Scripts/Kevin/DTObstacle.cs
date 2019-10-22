using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Kevin Matthew Julius 216007874
public class DTObstacle : DTGameObject
{

    private GameObject gameObject;

    public DTObstacle()
    {
        SetToObstacle();
        GameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    public GameObject GameObject
    {
        get => gameObject;
        set => gameObject = value;
    }

}
