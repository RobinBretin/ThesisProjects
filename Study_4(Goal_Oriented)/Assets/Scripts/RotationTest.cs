using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    public GameObject Participant;
    public GameObject Drone;
    private Transform line;
    private Transform test;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 relativePos = Drone.transform.position - transform.position;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        //Debug.Log((Quaternion.Inverse(transform.rotation) * rotation).eulerAngles.y);
        
        Debug.Log(Vector3.Distance(transform.position, Drone.transform.position));
        Debug.Log(Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(Drone.transform.position.x, 0, Drone.transform.position.z)));

        //Debug.Log(rotation.eulerAngles);
    }
}
