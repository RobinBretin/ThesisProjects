using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.ParticleSystem;


public class DroneMovements : MonoBehaviour
{
    //TAKE_OFF--
    public float hoverHeight = 10.0f;
    public float hoverVariance = 1f;
    public float hoverVarianceSpeed = 1f;
    private float currentHeight;
    public float UpSpeed;
    private bool isHovering;
    private bool heightReached;
    private Vector3 startingPosition;
    [SerializeField]
    private MinMaxCurve CurveTO;
    //TAKE_OFF--

    //Sound
    public AudioSource audioSource;
    //

    //Animation propellers + rotation
    public GameObject[] propellers;
    public float propellerRotaSpeed;
    //

    //Actions
    public InputAction input;
    private float timer;
    private bool start;
    //

    public GameObject player;
    public PID_Controller controller;
    public float Rotationspeed = 5f; // Speed of movement along the circle
    public float speed = 1f; // Speed of movement along the circle
    private Vector3 init;


    IEnumerator coroutineTO;
    IEnumerator coroutineCM;

    void Start()
    {
        timer = 0;

        //Sound
        audioSource = GetComponent<AudioSource>();

        //Set to Initial Location
        transform.position = transform.position;


        //HOVER
        startingPosition = transform.position;
        currentHeight = startingPosition.y ;

        //InitiateCoroutines
        coroutineTO = Hover();
        coroutineCM = CircularMotion();

        //Initial TakeOff
        start = false;   
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (start == true)
        {
            foreach (GameObject prop in propellers)
            {
                prop.transform.Rotate(Vector3.up * (propellerRotaSpeed * Time.deltaTime));
            }
        }
        
        if (timer>10f && start==false)
        {
            start = true;
            StartCoroutine(coroutineTO);
            audioSource.Play();
        }

        if (timer > 15f)
        {
            init = transform.position;
            StartCoroutine(coroutineCM);
            
            //transform.Rotate(new Vector3(0,Rotationspeed,0) * Time.deltaTime);
            //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

    }

    private IEnumerator Hover()
    {
        float randomVarianceSpeedOffset = Random.Range(-0.5f, 0.5f);
        while (true)
        {

            if (Mathf.Abs(transform.position.y - hoverHeight) > 0.01 && !heightReached)
            {
                transform.position = Vector3.Lerp(transform.position,
                   new Vector3(transform.position.x, hoverHeight, transform.position.z),
                   CurveTO.Evaluate(UpSpeed * Time.deltaTime));
                isHovering = true;
                yield return null;

            }
            else if (isHovering)
            {
                heightReached = true;
                float variance = Mathf.Sin((Time.time + randomVarianceSpeedOffset) * hoverVarianceSpeed) * hoverVariance;
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, hoverHeight + variance, transform.position.z), hoverVarianceSpeed * Time.deltaTime);
                yield return null;
            }

        }
    }

    private IEnumerator CircularMotion()
    {
        yield return null;
    }




}
