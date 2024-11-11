using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public Transform locationA;
    public Transform locationB;
    public float speed = 1.0f;
    public float deceleration = 0.5f;
    public float decelerationDistance = 1.0f;
    public float liftCoefficient = 0.5f;
    public float dragCoefficient = 0.5f;
    public float thrust = 10f;
    public float flyingHeight = 10f;
    public float liftSpeed = 2f;
    public float rotationSpeed = 2f;
    public Rigidbody rb; 

    public bool movingToB = true;
    private bool reachedHeight = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!reachedHeight)
        {
            if(transform.position.y < flyingHeight)
            {
                Vector3 velocity = rb.velocity;
                velocity.y = liftSpeed;
                rb.velocity = velocity;
            }
            else
            {
                reachedHeight = true;
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            if (movingToB)
            {
                Vector3 direction = (locationB.position - transform.position);
                direction.y = 0;
                direction = direction.normalized;
                Vector3 velocity = direction * speed;
                velocity += Vector3.up * liftCoefficient * speed;
                velocity -= rb.velocity * dragCoefficient;
                velocity += transform.forward * thrust;

                rb.velocity = velocity;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), rotationSpeed * Time.deltaTime);

                    if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(locationB.position.x, 0, locationB.position.z)) < decelerationDistance)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    movingToB = false;
                }
            }
            else
            {
                Vector3 direction = (locationA.position - transform.position);
                direction.y = 0;
                direction = direction.normalized;
                Vector3 velocity = direction * speed;
                velocity += Vector3.up * liftCoefficient * speed;
                velocity -= rb.velocity * dragCoefficient;
                velocity += transform.forward * thrust;

                rb.velocity = velocity;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), rotationSpeed * Time.deltaTime);

                    if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(locationA.position.x, 0, locationA.position.z)) < decelerationDistance)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    movingToB = true;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (transform.position.y < flyingHeight)
        {
            Vector3 velocity = rb.velocity;
            velocity.y = liftSpeed;
            rb.velocity = velocity;
        }
        if (transform.position.y > flyingHeight)
        {
            Vector3 velocity = rb.velocity;
            velocity.y = 0;
            rb.velocity = velocity;
        }
        //rb.velocity *= deceleration;
    }
}