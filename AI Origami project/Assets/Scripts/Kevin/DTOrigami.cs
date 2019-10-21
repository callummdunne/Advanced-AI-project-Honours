using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Kevin Matthew Julius 216007874
public class DTOrigami : DTGameObject
{

    private GameObject gameObject;

    public DTOrigami()
    {
        SetToOrigami();
        GameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }

    public GameObject GameObject
    {
        get => gameObject;
        set => gameObject = value;
    }
}
