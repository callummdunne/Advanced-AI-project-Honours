using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Kevin Matthew Julius 216007874
public class Origami : DTGameObject
{

    private GameObject gameObject;
    private int age;
    private string pattern;

    public Origami()
    {
        SetToOrigami();
        GameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }

    public GameObject GameObject
    {
        get => gameObject;
        set => gameObject = value;
    }

    public int Age
    {
        get => age;
        set => age = value;
    }

    public string Pattern
    {
        get => pattern;
        set => pattern = value;
    }
}
