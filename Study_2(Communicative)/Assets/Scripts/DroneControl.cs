using UnityEngine;
using System.Collections;
using TMPro;
using static UnityEngine.ParticleSystem;

public class DroneControl : MonoBehaviour
{

    public GameManager GM;

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

    //VARIABLES--
    [SerializeField] private GameObject Participant;
    [SerializeField] private float rotationSpeed;
    private Quaternion targetRotation;
    private Quaternion initialRotation;
    private float randomAngle;
    private float timeChangeAngle;
    private float timer;

    //CONDITIONS
    [SerializeField] private EyeIdle eyes;
    private bool follow;
    private bool toofar;



    //Sound
    private AudioSource audio;
    //

    //Animation propellers + rotation
    public GameObject[] propellers;
    public float propellerRotaSpeed;
    //

    IEnumerator coroutineTO;
    private bool CR;

    void Start()
    {
        CR = false;
        toofar = false;
        initialRotation = transform.rotation;
        follow = eyes.follow;
        //Sound
        audio = GetComponent<AudioSource>();

        //Set to Initial Location
        //transform.position = startingPosition;
        targetRotation = Quaternion.LookRotation(new Vector3(Participant.transform.position.x, transform.position.y, Participant.transform.position.z) - transform.position);

        currentHeight = startingPosition.y + hoverHeight;
        randomAngle = Random.Range(-10.0f, 10.0f);
        timeChangeAngle = Random.Range(2f, 4.0f);

        //InitiateCoroutines
        coroutineTO = Hover();

        //Initial TakeOff
        //StartCoroutine(coroutineTO);
        //audio.Play();
    }

    private void FixedUpdate()
    {
        follow = eyes.follow;

        if (GM.start)
        {
            if (!CR) StartCoroutine(coroutineTO);
            if (!audio.isPlaying) audio.Play();

            //Rotating propellers
            foreach (GameObject prop in propellers)
            {
                prop.transform.Rotate(Vector3.up * (propellerRotaSpeed * Time.deltaTime * 1000));
            }

            //Fix participant as rotation target in follow condition.
            if (follow)
            {
                Quaternion angle = Quaternion.LookRotation(new Vector3(Participant.transform.position.x, transform.position.y, Participant.transform.position.z) - transform.position);
                Debug.Log(angle.eulerAngles.y);
                if (angle.eulerAngles.y > 280 || angle.eulerAngles.y < 80)
                {
                    targetRotation = angle;
                }
                else targetRotation = initialRotation;

                /*float diffAngles = Mathf.Abs(transform.rotation.eulerAngles.y - initialRotation.eulerAngles.y);
                if (diffAngles < 80)
                {
                    targetRotation = Quaternion.LookRotation(new Vector3(Participant.transform.position.x, transform.position.y, Participant.transform.position.z) - transform.position);
                    toofar = false;
                }
                else
                {
                    toofar = true;
                    targetRotation = transform.rotation;
                    Quaternion angle = Quaternion.LookRotation(new Vector3(Participant.transform.position.x, transform.position.y, Participant.transform.position.z) - transform.position);
                    if (!(angle.eulerAngles.y > 260 || angle.eulerAngles.y < 100))
                    {
                        targetRotation = angle;
                    }

                targetRotation = initialRotation;

            }*/
            }
            else
            {
                targetRotation = initialRotation;
            }

            if (!follow)
            {
                timer += Time.deltaTime;
                if (timer > timeChangeAngle)
                {
                    timer = 0;
                    randomAngle = Random.Range(-10.0f, 10.0f);
                    timeChangeAngle = Random.Range(2.5f, 5.0f);
                }
            }
        }

    }


    private IEnumerator Hover()
    {
        float randomVarianceSpeedOffset = Random.Range(-0.5f, 0.5f);
        CR = true;

        while (true)
        {
            //ROTATION
            if (!follow)
            {
                Vector3 randomAxis = new Vector3(0.0f, 1.0f, 0.0f); // Set the random axis to the y-axis
                Quaternion randomRotation = Quaternion.AngleAxis(randomAngle, randomAxis); // Create a random rotation quaternion on the y-axis
                Quaternion finalRotation = targetRotation * randomRotation; // Combine the original rotation with the random rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * rotationSpeed);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

            //HEIGHT
            hoverHeight = Participant.transform.position.y - 0.15f;
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


}

