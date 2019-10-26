using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveObstacle : MonoBehaviour
{
    public Transform myTransform;
    private float speed = 100f;
    private float lifeTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.Translate(Vector3.up * speed * Time.deltaTime);
        if (GoalSystem.CheckPastObstacle(myTransform.position.z))
        {
            wallsNramps.setFlagNext(true);
        }
        if (myTransform.position.z < -100)
        {
            Destroy(myTransform);
        }
    }
}
