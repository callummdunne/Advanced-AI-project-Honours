using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Kevin Matthew Julius 216007874
public class Obstacle : DTGameObject
{

    private GameObject gameObject;


    public Obstacle()
    {
        GameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        SetToObstacle();
    }

    public GameObject GameObject
    {
        get => gameObject;
        set => gameObject = value;
    }

}
