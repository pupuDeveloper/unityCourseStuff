
using System;

using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using UnityEngine;


public class sphereMove : MonoBehaviour
{

    //just speed for the ball
    [SerializeField]
    private float speed;
    private UnityEngine.Vector3 direction; //direction of balls movement
    private UnityEngine.Vector3 colVec; // Distance from ball to wall, achieved with raycast
    private List<GameObject> objectsToCollideWith = new List<GameObject>();
    private List<GameObject> goingTowardsThese = new List<GameObject>();
    private bool directinChanged;
    private float radius;
    private UnityEngine.Vector3 edgeOfBall;
    private UnityEngine.Vector3 hitPos;

    private void Start()
    {
        //set balls direction
        direction = UnityEngine.Vector3.forward;
        //find objects we want to detect collision with
        objectsToCollideWith = GameObject.FindGameObjectsWithTag("collisionObjects").ToList();
        directinChanged = true;
        radius = gameObject.GetComponent<SphereCollider>().radius;
    }

    private void isCollisionObjectClose()
    {
        //get all objects we are going towards to with dot product
        if (directinChanged)
        {
            foreach (GameObject go in objectsToCollideWith)
            {
                colVec = gameObject.transform.position - go.transform.position;
                if (UnityEngine.Vector3.Dot(direction.normalized, colVec.normalized) < 0)
                {
                    goingTowardsThese.Add(go);
                }
            }
            directinChanged = false;
        }
        // go through each object that we are going towards to
        foreach (GameObject go in goingTowardsThese)
        {
            colVec = new UnityEngine.Vector3 (0,0,0);
            edgeOfBall = gameObject.transform.position + (direction.normalized * radius/2);
            //these if else statements check if ONE of our balls Axis are same than an object we are colliding with
            //then we se the possible hitPos Vector to be that shared axis, plus balls other 2 axis
            if (MathF.Abs(edgeOfBall.x - go.transform.position.x) < 0.01)
            {
                hitPos = new UnityEngine.Vector3 (go.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
                colVec = edgeOfBall - hitPos;
            }
            else if (MathF.Abs(edgeOfBall.y - go.transform.position.y) < 0.01)
            {
                hitPos = new UnityEngine.Vector3 (gameObject.transform.position.x, go.transform.position.y, gameObject.transform.position.z);
                colVec = edgeOfBall - hitPos;
            }
            else if (MathF.Abs(edgeOfBall.z - go.transform.position.z) < 0.01)
            {
                hitPos = new UnityEngine.Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, go.transform.position.z);
                colVec = edgeOfBall - hitPos;
            }
            //here we check if hitPos is on the bounds of the object we are heading towards, AND if the dot product of that hitpos and our movement is 1, we ricochet and switch
            //directions
            if (UnityEngine.Vector3.Dot(direction.normalized, colVec.normalized) == 1 && go.GetComponent<BoxCollider>().bounds.Contains(hitPos))
            {
                Debug.Log(UnityEngine.Vector3.Dot(direction.normalized, colVec.normalized));
                direction.x *= -1;
                direction.y *= -1;
                direction.z *= -1;
                goingTowardsThese.Clear();
                directinChanged = true;
            }
        }
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        isCollisionObjectClose();
    }
}

