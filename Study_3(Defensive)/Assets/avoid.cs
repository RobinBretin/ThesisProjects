using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class avoid : MonoBehaviour
{
    public GameObject init_position;
    public GameObject target_position;
    public float height;
    public float speed;
    public float timeBaseline;
    public float timeResting;
    public float timeTakeOff;
    public float timeInvasion;
    public float min;
    public float max;
    public GameObject player;
    private Transform positionPlayer;
    private float dist;
    public string status;
    private float time;
    private float durationApproach;
    private float timeToPhase;
    private float invasionTime;
    private int i;
    private List<string> phases;

    public Vector3 initialPos;
    public bool inside;
    // Start is called before the first frame update
    void Start()
    {
        positionPlayer = target_position.transform;
        initialPos = init_position.transform.position;
        durationApproach = 0;
        status = "Baseline";
    }

    /*void OnTriggerEnter(Collider other) {
        inside=true;
        Debug.Log("yes");
    }
    void OnTriggerExit(Collider other) {
        inside=false;
    }*/
    // Update is called once per frame
    void FixedUpdate()
    {
        dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(positionPlayer.position.x, positionPlayer.position.z));
        Debug.Log(status);

        time += Time.deltaTime;

        if (status == "Baseline")
        {
            if (time > timeBaseline)
            {
                time = 0;
                status = "TakeOff";
            }
        }

        if (status == "TakeOff")
        {
            TakeOff(height);
            if (time > timeTakeOff)
            {
                status = "Approach";
            }
        }

        if (status == "Approach")
        {
            Approach(target_position, max);
            if (dist <= max)
            {
                status = "Invasion";
                time = 0;
            }
        }

        if (status == "Invasion")
        {
            if (time > timeInvasion)
            {
                status = "Return";
                time = 0;
            }
        }

        if (status == "Return")
        {
            Approach(init_position, 0);
            if (dist <= max)
            {
                time=0;
                status="Land";
               TakeOff(0);
            }
        }

        void TakeOff(float height)
        {
            if (height > transform.position.y)
            {
                transform.position += Vector3.up * 1 * Time.deltaTime;
            }
            if (transform.position.y > height) transform.position += Vector3.up * -1 * Time.deltaTime;
        }

    }

    void Approach(GameObject target, float maxi)
    {
        max = maxi;
        positionPlayer = target.transform;
        if (dist > max)
        {
            transform.position = new Vector3(Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.z), new Vector2(positionPlayer.position.x, positionPlayer.position.z), 1 * speed * Time.fixedDeltaTime).x, transform.position.y, Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.z), new Vector2(positionPlayer.position.x, positionPlayer.position.z), 1 * speed * Time.deltaTime).y);
        }
    }
}

