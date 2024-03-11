
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class sphereMove : MonoBehaviour
{

    //just speed for the ball
    [SerializeField]
    private float speed;
    private Vector3 direction; //direction of balls movement
    private Vector3 colVec; // Distance from ball to wall, achieved with raycast
    private List<GameObject> objectsToCollideWith = new List<GameObject>();
    private List<GameObject> goingTowardsThese = new List<GameObject>();
    private bool directinChanged;

    private void Start()
    {
        direction = Vector3.forward;
        objectsToCollideWith = GameObject.FindGameObjectsWithTag("collisionObjects").ToList();
        directinChanged = true;
    }

    private void isCollisionObjectClose()
    {
        if (directinChanged)
        {
            foreach (GameObject go in objectsToCollideWith)
            {
                colVec = gameObject.transform.position - go.transform.position;
                Debug.Log(Vector3.Dot(direction.normalized, colVec.normalized));
                if (Vector3.Dot(direction.normalized, colVec.normalized) < 0)
                {
                    goingTowardsThese.Add(go);
                }
            }
            directinChanged = false;
        }
        foreach (GameObject go in goingTowardsThese)
        {
            colVec = gameObject.transform.position - go.transform.position;
            if (Vector3.Dot(direction.normalized, colVec.normalized) > 0)
            {
                direction.x *= -1;
                direction.y *= -1;
                direction.z *= -1;
                goingTowardsThese.Clear();
                directinChanged = true;
            }
        }
    }

    //basically
    //if ball is close to objects in "collision" layer, check their normals and do dot product
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        isCollisionObjectClose();
    }
}

