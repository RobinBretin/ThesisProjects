using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class proxemicShape : MonoBehaviour
{
    public GameObject participant;
    private Vector3 p_position;
    public GameObject drone;
    private Vector3 d_position;
    private float participant_drone_distance;
    private float participant_drone_angle;
    // Start is called before the first frame update
    void Start()
    {
        p_position=participant.transform.position;
        d_position=drone.transform.position;
        participant_drone_distance=Vector3.Distance(p_position,d_position);
        participant_drone_angle=Vector3.SignedAngle(p_position,d_position,Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        participant_drone_distance=Vector3.Distance(p_position,d_position);
        participant_drone_angle=Vector3.SignedAngle(p_position,d_position,Vector3.up);

    }
}
