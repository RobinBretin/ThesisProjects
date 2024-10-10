using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetRandom : MonoBehaviour
{

    [SerializeField]
    private AnimationCurve CurveGoTo;

    private float currentLerpTime;
    private float lerpTime;
    private Vector3 startingPositionGOTO;
    private Vector3 destination;
    IEnumerator coroutineGoTo;
    private float distance;
    public float MoveSpeed;

    public float timeRandom;
    private float time;
    public float timer;

    public GameObject player;

    //
    void Start()
    {
        time = 0;
        currentLerpTime = 0;
        coroutineGoTo = GoTo();
        currentLerpTime = 0;
        startingPositionGOTO = transform.position;
        float x = Random.Range(-1.5f, 1.5f);
        float z = Random.Range(2.25f, 2.65f);
        destination = new Vector3(x, player.transform.position.y, z);
        distance = Vector3.Distance(transform.position, destination);
        lerpTime = distance / MoveSpeed;
        timer = Random.Range(4f, 6f);
        time = 0;
        StartCoroutine(coroutineGoTo);

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        
        if (time > timer)
        {
            currentLerpTime = 0;
            startingPositionGOTO = transform.position;
            float x = Random.Range(-1.5f, 1.5f);
            float z = Random.Range(2.25f, 2.65f);
            destination = new Vector3(x, player.transform.position.y, z);
            distance = Vector3.Distance(transform.position, destination);
            lerpTime = distance / MoveSpeed;
            timer = Random.Range(4f, 6f);
            time = 0;
        }

    }

    private IEnumerator GoTo()
    {
        while (true)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            //lerp!
            float t = currentLerpTime / lerpTime;
            transform.position = Vector3.Lerp(startingPositionGOTO, destination, CurveGoTo.Evaluate(t));
            yield return null;
        }
    }
}
