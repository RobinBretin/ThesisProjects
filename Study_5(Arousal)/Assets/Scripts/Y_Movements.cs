using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_Movements : MonoBehaviour
{
    // Start is called before the first frame update
    public PID_Controller controller;
    private Rigidbody rigidbody;
    public GameObject target;
    private Vector3 targetPosition;
    public float power=1;
    void Start()
    {
        targetPosition = target.transform.position;
        rigidbody = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        targetPosition = target.transform.position;
        float input = controller.UpdatePID(Time.fixedDeltaTime, rigidbody.position.y, targetPosition.y);
        rigidbody.AddForce(new Vector3(0, input * power, 0));


    }
}
