using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PID_Drone : MonoBehaviour
{

    //GameManager
    public string phase;
    public N_Back nbackScript;
    public List<float> conditionOrder;
    public float lowLevel;
    public float mediumLevel;
    public float highLevel;

    //Sound
    public SoundManagment sd;
    public AudioSource audioSource;
    public float initialPitch;
    private float finalPitch;
    public float speedSound;
    public float minMaxPitch;
    //
    //public InputAction changeReverb;


    //Level
    public float speedSoundLevel;
    public float targetLevel;

    //Animation propellers + rotation
    public GameObject[] propellers;
    public float propellerRotaSpeed;
    //

    private float timer;
    private bool start;

    /// PID
    public PID_Controller_X controller_X;
    public PID_Controller_Y controller_Y;
    public PID_Controller_Z controller_Z;
    private Rigidbody rigidbody;
    public GameObject target;
    private Vector3 targetPosition;
    public float power = 1;
    /// PID
    /// 
    public int angle;

    public GameObject player;
    private bool looking;

    void Start()
    {
        //changeReverb.Enable();

        looking = false;
        phase = "training";
        timer = 0;

        //Sound
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0;

        //Set to Initial Location
        transform.position = transform.position;

        //Initial TakeOff
        start = false;


        /// PID
        targetPosition = target.transform.position;
        rigidbody = GetComponent<Rigidbody>();
        /// PID

    }

    void FixedUpdate()
    {
        SetSoundLevel();
        timer += Time.deltaTime;

        //if (changeReverb.triggered) { GetComponent<AudioReverbFilter>().enabled = !GetComponent<AudioReverbFilter>().enabled; }

        if (timer > 3f && start == false)
        {
            start = true;
            audioSource.Play();
        }

        if (start)
        {
            //Rotate Propellers
            foreach (GameObject prop in propellers)
            {
                prop.transform.Rotate(Vector3.up * (propellerRotaSpeed * Time.deltaTime));
            }

            targetPosition = target.transform.position;
            float input_X = controller_X.UpdatePID_X(Time.fixedDeltaTime, rigidbody.position.x, targetPosition.x);
            rigidbody.AddForce(new Vector3(input_X * power, 0, 0));

            float input_Y = controller_Y.UpdatePID_Y(Time.fixedDeltaTime, rigidbody.position.y, targetPosition.y);
            rigidbody.AddForce(new Vector3(0, input_Y * power, 0));

            float input_Z = controller_Z.UpdatePID_Z(Time.fixedDeltaTime, rigidbody.position.z, targetPosition.z);
            rigidbody.AddForce(new Vector3(0, 0, input_Z * power));

            //Slightly rotate when moving
            float y = Quaternion.LookRotation(player.transform.position - transform.position, transform.TransformDirection(Vector3.up)).eulerAngles.y;
            if(looking) transform.rotation=Quaternion.Euler(new Vector3(-input_Z * angle, y, input_X * angle));
            else transform.rotation = Quaternion.Euler(new Vector3(-input_Z * angle, 180, input_X * angle));

            //Sound
            //if (sd.pitchMove) {
            float pitch = initialPitch + (Mathf.Abs(input_X + input_Y + input_Z) * minMaxPitch);

            finalPitch = Mathf.Lerp(finalPitch, pitch, Time.deltaTime * speedSound);

            audioSource.pitch = finalPitch;
            //}
            //else audioSource.pitch = 1;
        }
    }

    public void Look()
    {
        looking = !looking;
    }

    public void SetSoundLevel()
    {
        //if (phase == "training" || phase == "rest") targetLevel = 0;
        //else targetLevel = conditionOrder[nbackScript.j];
        targetLevel = conditionOrder[nbackScript.j];
        if (nbackScript.resting)
        {
            if (nbackScript.timerRestMax - nbackScript.timerRest < 5) audioSource.volume = Mathf.Lerp(audioSource.volume, targetLevel, Time.deltaTime * speedSoundLevel);
            else audioSource.volume = Mathf.Lerp(audioSource.volume, 0, Time.deltaTime * speedSoundLevel);
        }
        else audioSource.volume = Mathf.Lerp(audioSource.volume, targetLevel, Time.deltaTime * speedSoundLevel);
    }

    public void setOrder(float order)
    {
        if (order == 1)
        {
            conditionOrder = new List<float> {0f, 0f, 0f, 0f, lowLevel, mediumLevel, highLevel, highLevel, mediumLevel, lowLevel };
        }
        if (order == 2)
        {
            conditionOrder = new List<float> { 0f, 0f, 0f, 0f, mediumLevel, highLevel, lowLevel, lowLevel, highLevel, mediumLevel };
        }
        if (order == 3)
        {
            conditionOrder = new List<float> { 0f, 0f, 0f, 0f, highLevel, lowLevel, mediumLevel, mediumLevel, lowLevel, highLevel };
        }
        if (order == 4)
        {
            conditionOrder = new List<float> { 0f, 0f, 0f, 0f, lowLevel, mediumLevel, highLevel, highLevel, mediumLevel, lowLevel };
        }
        if (order == 5)
        {
            conditionOrder = new List<float> { 0f, 0f, 0f, 0f, mediumLevel, highLevel, lowLevel, lowLevel, highLevel, mediumLevel };
        }
        if (order == 6)
        {
            conditionOrder = new List<float> { 0f, 0f, 0f, 0f, highLevel, lowLevel, mediumLevel, mediumLevel, lowLevel, highLevel };
        }
    }




}
