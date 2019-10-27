using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Kevin Matthew Julius 216007874
public class Origami : DTGameObject
{

    private GameObject gameObject;


    public Origami()
    {
        GameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        SetToOrigami();
    }

    public GameObject GameObject
    {
        get => gameObject;
        set => gameObject = value;
    }

}
