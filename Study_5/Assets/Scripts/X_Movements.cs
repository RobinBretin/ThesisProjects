using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X_Movements : MonoBehaviour
{
    // Start is called before the first frame update
    public PID_Controller_X controller_X;
    public PID_Controller_Y controller_Y;
    public PID_Controller_Z controller_Z;
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
        float input_X = controller_X.UpdatePID_X(Time.fixedDeltaTime, rigidbody.position.x, targetPosition.x);
        rigidbody.AddForce(new Vector3( input_X * power,0, 0));

        float input_Y = controller_Y.UpdatePID_Y(Time.fixedDeltaTime, rigidbody.position.y, targetPosition.y);
        rigidbody.AddForce(new Vector3( 0, input_Y * power, 0));

        float input_Z = controller_Z.UpdatePID_Z(Time.fixedDeltaTime, rigidbody.position.z, targetPosition.z);
        rigidbody.AddForce(new Vector3(0,0, input_Z * power));
    }
}
