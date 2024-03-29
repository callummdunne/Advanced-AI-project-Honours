﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveObstacle : MonoBehaviour
{
    public Transform myTransform;
    private float speed = 100f;
    private float lifeTime = 5;
    public GameObject obstacle;
    GameObject manager;
    wallsNramps wNr;
    GoalSystem gs;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
        GameObject manager = GameObject.Find("GameManager");
        wNr = manager.GetComponent<wallsNramps>();

        gs = manager.GetComponent<GoalSystem>();


    }

    // Update is called once per frame
    void Update()
    {
        if(myTransform == null)
        {
            return;
        }
        if (myTransform.position.z > -100){
            myTransform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        //this allows us to keeep a copy of the obstacle at < -200 that does not move but allows others to be created
        if ((myTransform.position.z < -100) && (myTransform.position.z > -200))
        {
            wNr.setFlagNext(true);
            Destroy(obstacle);
        }
    }
}
