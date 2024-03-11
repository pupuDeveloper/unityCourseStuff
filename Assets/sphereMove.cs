
using UnityEngine;

public class sphereMove : MonoBehaviour
{

    //just speed for the ball
    [SerializeField]
    private float speed;
    private Vector3 direction; //direction of balls movement
    private Vector3 colVec; // Distance from ball to wall, achieved with raycast
    private bool incontact;

    private void Start()
    {
        direction = Vector3.forward;
    }

    private void OnCollisionEnter(Collision col)
    {
        colVec = col.contacts[0].normal;
        incontact = true;
    }
    private void OnCollisionExit(Collision col)
    {
        incontact = false;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        if (incontact)
        {
            Debug.Log(Vector3.Dot(direction.normalized, colVec.normalized));
            if (Vector3.Dot(direction.normalized, colVec.normalized) < 1)
            {
                direction.x *= -1;
                direction.y *= -1;
                direction.z *= -1;
            }
        }
    }
}

