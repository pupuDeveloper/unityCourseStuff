
using UnityEngine;

public class sphereMove : MonoBehaviour
{

    //just speed for the ball
    [SerializeField]
    private float speed;
    private float radius; // radius of sphere to set the raycast to a good position
    private Vector3 direction; //direction of balls movement
    private Vector3 colVec; // Distance from ball to wall, achieved with raycast
    private Ray ray; //our ray 
    private RaycastHit hit; //raycast hit info

    private void Start()
    {
        direction = Vector3.forward;
        radius = gameObject.GetComponent<SphereCollider>().radius * 0.5f;
    }

    void Update()
    {
        //moving the ball
        transform.Translate(direction * Time.deltaTime);
        //make a raycast to detect a wall, also make sure raycast starts from the EDGE of the ball, even when direction changes, so collision looks better
        Vector3 raycastOrigin = direction * radius;
        ray = new Ray(transform.position + raycastOrigin, direction);
        Debug.DrawRay(ray.origin, ray.direction, Color.green);
        //if raycasts hits something, calculate distance between ball and the hit object
        if (Physics.Raycast(ray, out hit))
        {
            colVec = hit.point - ray.origin; //distance from ball to origin
            Debug.Log(Vector3.Dot(ray.direction.normalized, colVec.normalized));
            //if dot product is less than 1, this means we are no longer right against the object the
            //raycast hit, which means we are pass it, so we change the direction vector to an opposite direction
            if (Vector3.Dot(ray.direction.normalized, colVec.normalized) < 1)
            {
                direction.x *= -1;
                direction.y *= -1;
                direction.z *= -1;
            }
        }
    }
}
//NOTE:
//There is an issue with this code, because if this didn't for some reason detect the change INSTANTLY
//the raycast would go pass the wall seen in the scene, and the raycast would not be hitting anything,
//and the ball would fly through. Could you help me understand how to prevent this? 
//my school email is saimisachiko.vilkkila@tuni.fi

