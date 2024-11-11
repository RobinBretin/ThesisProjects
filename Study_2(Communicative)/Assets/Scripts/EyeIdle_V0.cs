using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeIdle_V0 : MonoBehaviour
{// Author: Anja Haumann

    [Tooltip("Pupil that will be moved in the direction of the target")]
    [SerializeField]
    private Transform pupil;
    [Tooltip("Object the eye is looking at")]
    [SerializeField]
    private Transform lookAtTarget;
    [Tooltip("The distance the pupil is alowed to travel from the center of the eye.")]
    [SerializeField]
    private float movementDistance = 0.1f;
    [SerializeField]
    private float minY = 0.02f;
    [SerializeField]
    private float offZ = -0.06f;
    [SerializeField]
    private Transform[] targets;
    private int i = 0;
    private int j;

    [SerializeField]
    private Sprite[] faces;
    [SerializeField]
    private Sprite[] eyes;
    [SerializeField]
    private Sprite[] blinks;


    private Animator animator;
    private float timeToBlink;
    private IEnumerator blinkCoroutine;
    private string currentEmotion;
    private bool isBlinking;

    // Variables for looking at random targets
    private float timeToLookAtTarget;
    private float timeToReturnToInitialTarget;
    private Transform initialTarget;
    private bool lookingAtRandomTarget;

    private GameObject randomTargetObject;

    //Participant
    [SerializeField] private GameObject participant;


    //Conditions
    [SerializeField] private GameManager GM;
    private List<List<object>> conditions;
    private bool follow;
    private int indexEmotion;



    void Start()
    {
        conditions = GM.conditions;
        animator = GetComponent<Animator>();
        timeToBlink = 0f;
        j = 0;

        indexEmotion = (int)conditions[j][0];
        follow = (bool)conditions[j][1];
        GetComponent<SpriteRenderer>().sprite = faces[indexEmotion];
        pupil.GetComponent<SpriteRenderer>().sprite = eyes[indexEmotion];

        /*GetComponent<SpriteRenderer>().sprite = faces[j];
        pupil.GetComponent<SpriteRenderer>().sprite = eyes[j];*/

        // Set initial target to look at


        initialTarget = lookAtTarget;
        timeToLookAtTarget = Time.time + Random.Range(2f, 6f);
        timeToReturnToInitialTarget = Time.time + Random.Range(3f, 10f);
        lookingAtRandomTarget = false;

        randomTargetObject = new GameObject("RandomTarget");

    }



    void Update()
    {
        if (Time.time > timeToBlink)
        {
            blinkCoroutine = DoBlink();
            StartCoroutine(blinkCoroutine);
        }


        // Look at random targets periodically
        if (Time.time > timeToLookAtTarget)
        {
            if (!lookingAtRandomTarget)
            {
                randomTargetObject.transform.position = lookAtTarget.transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(0f, 3f));
                lookAtTarget = randomTargetObject.transform;
                lookingAtRandomTarget = true;
            }
            else
            {
                lookAtTarget = initialTarget;
                lookingAtRandomTarget = false;
            }

            timeToLookAtTarget = Time.time + Random.Range(2f, 6f);
            timeToReturnToInitialTarget = Time.time + Random.Range(1f, 2f);
        }
        // Return to initial target periodically
        else if (Time.time > timeToReturnToInitialTarget)
        {
            lookAtTarget = initialTarget;
            lookingAtRandomTarget = false;
            timeToReturnToInitialTarget = Time.time + Random.Range(1f, 2f);
        }




        if (!isBlinking)
        {
           if (Input.GetKeyDown(KeyCode.Space))
            {
                initialTarget= targets[i];
                lookAtTarget = initialTarget;
                if (i + 1 >= targets.Length)
                {
                    i = 0;
                }
                else i += 1;


                if (j + 1 >= conditions.Count) j = 0;
                else
                {
                    j++;
                    indexEmotion = (int)conditions[j][0];
                    follow = (bool)conditions[j][1];
                }
                
                isBlinking = true;
                blinkCoroutine = DoBlink();
                StartCoroutine(blinkCoroutine);


            }

            /*if (Gamepad.current.leftTrigger.IsPressed()) 
            {
                initialTarget = targets[i];
                lookAtTarget = initialTarget;
                if (i + 1 >= targets.Length)
                {
                    i = 0;
                }
                else i += 1;


                if (j + 1 >= faces.Length) j = 0;
                else j++;
                Debug.Log(currentEmotion);

                isBlinking = true;
                blinkCoroutine = DoBlink();
                StartCoroutine(blinkCoroutine);
            }*/
        }

        Vector3 direction = lookAtTarget.position - transform.position;
        Vector3 offset = direction.normalized * movementDistance;
        offset.y = Mathf.Max(offset.y, minY);
        offset.z = offZ;

        // Transform the offset between world and local coordinate systems
        offset = transform.InverseTransformPoint(transform.position + offset) - transform.localPosition;


        // Transform the offset back to world coordinate system
        Vector3 newPupilPosition = transform.TransformPoint(transform.localPosition + offset);

        // Set the pupil position
        pupil.transform.position = newPupilPosition;
    }


        private IEnumerator DoBlink()
    {
        currentEmotion = faces[indexEmotion].name;
        currentEmotion = currentEmotion.Substring(0, currentEmotion.IndexOf("_"));
        GetComponent<SpriteRenderer>().sprite = faces[indexEmotion];
        pupil.GetComponent<SpriteRenderer>().sprite = eyes[indexEmotion];

        animator.Play(currentEmotion+"_blink");
        yield return new WaitForSeconds(0.5f);
        
        animator.Play(currentEmotion);
        timeToBlink = Time.time + Random.Range(0.5f, 5f);
        isBlinking = false;

        Debug.Log(currentEmotion);
        Debug.Log(follow);
    }
}
