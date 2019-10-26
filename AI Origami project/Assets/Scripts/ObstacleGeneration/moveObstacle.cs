using System.Collections;
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
        Debug.Log("THis is the managesre");
        Debug.Log(manager);
       
        wNr = manager.GetComponent<wallsNramps>();
        Debug.Log("The wnr");
        Debug.Log(wNr);
        gs = manager.GetComponent<GoalSystem>();
        Debug.Log(gs);

    }

    // Update is called once per frame
    void Update()
    {
        if(myTransform == null)
        {
            return;
        }
        myTransform.Translate(Vector3.up * speed * Time.deltaTime);
        Debug.Log(gs);
        if (gs.CheckPastObstacle(myTransform.position.z))
        {
            wNr.setFlagNext(true);
        }
        if (myTransform.position.z < -100)
        {
            Destroy(obstacle);
        }
    }
}
