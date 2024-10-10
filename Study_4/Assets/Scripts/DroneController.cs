using UnityEngine;
using System.Collections;
using TMPro;
using static UnityEngine.ParticleSystem;

public class DroneController : MonoBehaviour
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

    //LAND--
    [SerializeField]
    private MinMaxCurve CurveLand;
    [SerializeField]
    private float Lspeed;
    //LAND--


    //GOTO--
    [SerializeField]
    public GameObject[] destinations;
    [SerializeField]
    private GameObject[] dropLocations;
    public Vector3 destination;
    private Quaternion targetRotation;
    public int i;
    private Transform StartPoint;
    [SerializeField]
    private float MoveSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private MinMaxCurve Curve;
    [SerializeField]
    private AnimationCurve CurveGoTo;

    private float currentLerpTime;
    private float lerpTime;

    private float distance;
    private float remainingDistance;
    private bool moving;
    private Vector3 startingPositionGOTO;
    //GOTO--

    //PICK&DROP--
    private Rigidbody pickedObject;
    public bool grabbed = false;
    private bool grabbing = false;
    [SerializeField]
    private float GrabSpeed;
    private Vector3 startingPositionGrab;
    [SerializeField]
    private AnimationCurve CurveGrab;
    private float currentLerpTimeGrab;
    //PICK&DROP--

    //GameManager
    public bool start;
    private bool objectReached;
    public bool dropReached;

    //Check Participant Grab
    public GameManager GM;
    public Save save;


    //Sound
    private AudioSource audio;
    //

    //Animation propellers + rotation
    public GameObject[] propellers;
    public float propellerRotaSpeed;
    //

    IEnumerator coroutineTO;
    IEnumerator coroutineLand;
    IEnumerator coroutineGoTo;
    IEnumerator coroutineGrab;
    IEnumerator coroutineGrabV2;

    void Start()
    {

        //Sound
        audio = GetComponent<AudioSource>();

        //Set to Initial Location
        transform.position = destinations[0].transform.position;


        //HOVER
        startingPosition = transform.position;
        currentHeight = startingPosition.y + hoverHeight;

        //InitiateCoroutines
        coroutineTO = Hover();
        coroutineGoTo = GoTo();
        coroutineLand = Land();
        coroutineGrab = GrabObject();
        //coroutineGrabV2= GrabObjectV2();

        //Initial TakeOff
        StartCoroutine(coroutineTO);
        audio.Play();
        i = 1;

        start = false;
    }

    void FixedUpdate()
    {
        foreach (GameObject prop in propellers){
            prop.transform.Rotate(Vector3.up * (propellerRotaSpeed * Time.deltaTime));
        }
            
        if (start)
        {
            //StartPoint.position = transform.position;
            destination = new Vector3(destinations[i].transform.position.x, transform.position.y, destinations[i].transform.position.z);
            targetRotation = Quaternion.LookRotation(destination - transform.position);
            distance = Vector3.Distance(transform.position, destination);
            remainingDistance = distance;

            if (!objectReached)
            {
                if (!moving)
                {
                    currentLerpTime = 0;
                    lerpTime = distance / MoveSpeed;
                    startingPositionGOTO = transform.position;
                    StartCoroutine(coroutineGoTo);
                    moving = true;
                }

                if (dropReached)
                {
                    if (2*GM.NB_dropped == i)
                    {
                        StopCoroutine(coroutineGoTo);
                        moving = false;
                        DropObject();
                        i++;
                    }
                        
                    
                    
                }

            }
            else
            {
                if (grabbed)
                {
                    /*if (dropReached)
                    {
                        StopCoroutine(coroutineGoTo);
                        moving = false;
                        Debug.Log("Dropping");
                        DropObject();
                        i++;
                    }
                    else
                    {
                        Debug.Log("Move to drop");
                        StopCoroutine(coroutineGrab);
                        grabbing = false;
                        i++;
                        startingPosition = transform.position;
                        StartCoroutine(coroutineTO);
                        if (!moving)
                        {
                            StartCoroutine(coroutineGoTo);
                            moving = true;
                        }
                    }*/
                    if (!dropReached)
                    {
                        StopCoroutine(coroutineGrab);
                        grabbing = false;
                        startingPosition = transform.position;
                        StartCoroutine(coroutineTO);
                        if (!moving)
                        {
                            currentLerpTime = 0;
                            startingPositionGOTO = transform.position;
                            lerpTime = distance / MoveSpeed;
                            StartCoroutine(coroutineGoTo);
                            moving = true;
                        }
                    }



                }
                else
                {
                    StopCoroutine(coroutineTO);
                    StopCoroutine(coroutineGoTo);
                    moving = false;
                    if (!grabbing)
                    {
                        startingPositionGrab = transform.position;
                        currentLerpTimeGrab = 0;
                        StartCoroutine(coroutineGrab);
                        grabbing = true;     
                    }

                }
            }
        }
        else
        {
            if (save.started == "Done")
            {
                DropObject();
                StopAllCoroutines();
                StartCoroutine(coroutineLand);
                audio.Stop();
            }
        }

        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startingPosition = transform.position;
            //currentHeight = startingPosition.y + hoverHeight;
            StopCoroutine(coroutineTO);
            StartCoroutine(coroutineLand);
            
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            startingPosition = transform.position;
           // currentHeight = startingPosition.y + hoverHeight;
            StopCoroutine(coroutineLand);
            StopCoroutine(coroutineGrab);
            StartCoroutine(coroutineTO);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
           // StartPoint.position = transform.position;
            destination = new Vector3(destinations[i].transform.position.x, transform.position.y, destinations[i].transform.position.z);
            targetRotation = Quaternion.LookRotation(destination- transform.position);
            distance = Vector3.Distance(transform.position, destination);
            remainingDistance = distance;
            StartCoroutine(coroutineGoTo);

        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            i++;
           // StartPoint.position = transform.position;
            destination = new Vector3(destinations[i].transform.position.x, transform.position.y, destinations[i].transform.position.z);
            targetRotation = Quaternion.LookRotation(destination - transform.position);
            distance = Vector3.Distance(transform.position, destination);
            remainingDistance = distance;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            StopCoroutine(coroutineTO);
            StopCoroutine(coroutineGoTo);
            StartCoroutine(coroutineGrab);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            DropObject();
        }
        */
    }

    private void DropObject()
    {
        pickedObject.useGravity = true;
        pickedObject.isKinematic = false;
        pickedObject.transform.SetParent(null);
        pickedObject = null;
        grabbed = false;
    }

    private IEnumerator GrabObject()
    {
        while (true)
        {
            while (!grabbed)
            {
                //transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, destinations[i].transform.position.y, CurveLand.Evaluate(Lspeed * Time.deltaTime)), transform.position.z);

                //transform.position = Vector3.Slerp(transform.position, destinations[i].transform.position, CurveLand.Evaluate(Lspeed * Time.deltaTime));

                if (GM.grabbed)
                {
                    transform.position = Vector3.Slerp(startingPositionGrab, destinations[i].transform.position, CurveGrab.Evaluate(currentLerpTimeGrab));
                    currentLerpTimeGrab += Time.deltaTime * GrabSpeed;
                }
                yield return null;
            }

            yield return null;
        }
    }

    private IEnumerator GoTo()
    {
        while (true)
        {
            //transform.position = Vector3.Lerp(StartPoint.position, new Vector3(EndPoint.transform.position.x, transform.position.y, EndPoint.transform.position.z), Curve.Evaluate(1 - (remainingDistance / distance)));
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(destination, Vector3.up), rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);


            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            //lerp!
            float t = currentLerpTime / lerpTime;
            //t = t * t * t * (t * (6f * t - 15f) + 10f);
            //t = t * t * (3f - 2f * t);
            //transform.position = Vector3.Lerp(transform.position, destination, CurveGoTo.Evaluate(Time.deltaTime*MoveSpeed));

            transform.position = Vector3.Lerp(new Vector3(startingPositionGOTO.x, transform.position.y, startingPositionGOTO.z), destination, CurveGoTo.Evaluate(t));
            yield return null;


            //remainingDistance -= MoveSpeed * Time.deltaTime;

            remainingDistance = Vector3.Distance(transform.position, destination);

            if (remainingDistance < .25 & !grabbed) objectReached = true;
            else objectReached = false;

            if (remainingDistance < 0.1 & grabbed) dropReached= true;
            else dropReached = false;

            yield return null;
        }
    }

    /*private void GoTo_f()
    {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            transform.position = Vector3.Lerp(transform.position, destination, Curve.Evaluate(1 - (remainingDistance / distance)));
            remainingDistance -= MoveSpeed * Time.deltaTime;

            if (remainingDistance < 0.01 && !grabbed) objectReached = true;
            else objectReached = false;

            if (remainingDistance < 0.01 && grabbed) dropReached = true;
            else dropReached = false;

    }*/

    private IEnumerator Land()
    {
        while (true)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, 0, CurveLand.Evaluate(Lspeed*Time.deltaTime)), transform.position.z);
            yield return null;
        }
    }

    /*private IEnumerator GrabObjectV2()
    {
        while (true)
        {
            while (!grabbed)
            {
                //transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, destinations[i].transform.position.y, CurveLand.Evaluate(Lspeed * Time.deltaTime)), transform.position.z);

                //transform.position = Vector3.Slerp(transform.position, destinations[i].transform.position, CurveLand.Evaluate(Lspeed * Time.deltaTime));
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, destinations[i].transform.position.y, transform.position.z), CurveTO.Evaluate(UpSpeed * Time.deltaTime));
                yield return null;
            }
            yield return null;
        }
    }*/



    private IEnumerator Hover()
    {
        float randomVarianceSpeedOffset = Random.Range(-0.5f, 0.5f);
        while (true)
        {

            if (Mathf.Abs(transform.position.y- hoverHeight) >0.01 && !heightReached)
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
                float variance = Mathf.Sin((Time.time+ randomVarianceSpeedOffset) * hoverVarianceSpeed) * hoverVariance;
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, hoverHeight + variance, transform.position.z), hoverVarianceSpeed * Time.deltaTime);
                yield return null;
            }
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == destinations[i].name)
        {
            pickedObject = other.GetComponent<Rigidbody>();
            pickedObject.useGravity = false;
            pickedObject.isKinematic = true;
            pickedObject.transform.SetParent(transform);
            grabbed = true;
            i++;
        }
    }




    /* private IEnumerator UpDown (float height, float stiff)
     {
         if (!isTakenOff)
         {
             rb.AddForce(Vector3.up * takeOffForce);
             if (rb.velocity.y > 0)
             {
                 isTakenOff = true;
             }
         }
         else
         {   while(hoverHeight - transform.position.y >= 0.05)
             {
                 float heightDifference = height - transform.position.y;
                 Vector3 force = Vector3.up * heightDifference * stiff - rb.velocity * hoverForce;
                 rb.AddForce(force);
             }
         }
         yield return null;
     }*/

}
