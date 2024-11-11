using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving_target : MonoBehaviour
{
    public float min_rotationSpeed;
    public float max_rotationSpeed;
    public float min_speed;
    public float max_speed;
    public float o;

    public float rotationSpeed;
    public float speed;

    public bool rot;


    public float timer;


    public Vector3 A;
    public Vector3 B;
    public Vector3 C;
    public Vector3 D;
    public Vector3 E;

    void Start()
    {
        A = transform.position;
        rot = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 60) { 
            rot = true;
        }
        if (!rot)
        {
            if (timer > 10)
            {
                transform.position = B;
            }

            if (timer > 20)
            {
                transform.position = C;
            }

            if (timer > 30)
            {
                transform.position = D;
            }

            if (timer > 40)
            {
                transform.position = D;
            }

            if (timer > 50)
            {
                transform.position = E;
            }

        }

        if (rot)
        {
            transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            if (rotationSpeed < max_rotationSpeed)
            {
                rotationSpeed = rotationSpeed + Time.deltaTime * o;
            }
            else rotationSpeed = min_rotationSpeed;

            if (speed < max_speed)
            {
                speed = speed + Time.deltaTime * o / 100;
            }
            else speed = min_speed;

            if (timer > 80)
            {
                timer = 0;
                rot = false;
                transform.position = E;
            }

        }
        
    }
}
