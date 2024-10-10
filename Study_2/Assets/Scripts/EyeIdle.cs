using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EyeIdle : MonoBehaviour
{// Author: Anja Haumann

    [Tooltip("Pupil that will be moved in the direction of the target")]
    [SerializeField]
    private Transform pupil;
    [Tooltip("Object the eye is looking at")]
    private Transform lookAtTarget;
    [Tooltip("The distance the pupil is alowed to travel from the center of the eye.")]
    [SerializeField]
    private float movementDistance = 0.1f;
    [SerializeField]
    private float minY = 0.02f;
    [SerializeField]
    private float offZ = -0.06f;
    [SerializeField]
    private List<GameObject> targets;
    private int i;
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
    public string currentEmotion;
    private bool isBlinking;

    // Variables for looking at random targets
    private float timeToLookAtTarget;
    private float timeToReturnToInitialTarget;
    private Transform initialTarget;
    private bool lookingAtRandomTarget;
    private float minReturn;
    private float maxReturn;
    private float minLook;
    private float maxLook;
    private float[] distFromPoints;


    private GameObject randomTargetObject;

    //Participant
    [SerializeField] private GameObject participant;


    //Conditions
    [SerializeField] private GameManager GM;
    private List<List<object>> conditions;
    public bool follow;
    private int indexEmotion;
    public bool next;

    void Start()
    {
        next = false;
        conditions = GM.conditions;
        animator = GetComponent<Animator>();
        timeToBlink = 0f;
        j = 0;
        i = 0;


        indexEmotion = (int)conditions[j][0];
        follow = (bool)conditions[j][1];
        GetComponent<SpriteRenderer>().sprite = faces[indexEmotion];
        pupil.GetComponent<SpriteRenderer>().sprite = eyes[indexEmotion];


        /*GetComponent<SpriteRenderer>().sprite = faces[j];
        pupil.GetComponent<SpriteRenderer>().sprite = eyes[j];*/

        // Set initial target to look at

        if (follow)
        {
            minReturn = 1f;
            maxReturn = 2f;
            minLook = 3f;
            maxLook = 6f;
            lookAtTarget = participant.transform;
        }
        else
        {
            minReturn = 1f;
            maxReturn = 2f;
            minLook = 2f;
            maxLook = 3f;
            lookAtTarget = targets[i].transform;
        }

        initialTarget = lookAtTarget;
        timeToLookAtTarget = Time.time + Random.Range(minLook, maxLook);
        timeToReturnToInitialTarget = Time.time + Random.Range(minReturn, maxReturn);
        lookingAtRandomTarget = false;
        randomTargetObject = new GameObject("RandomTarget");
    }


    void Update()
    {


        if (follow)
        {
            initialTarget = participant.transform;
        }
        else
        {
            
            targets = targets.OrderBy(go => Vector3.Distance(participant.transform.position, go.transform.position)).ToList();
           // int random = Random.Range(0, 2);
           //initialTarget = targets[random].transform;
            
        }


        if (Time.time > timeToBlink)
        {
            blinkCoroutine = DoBlink();
            StartCoroutine(blinkCoroutine);
        }

        // Look at random targets periodically
        if (!follow)
        {
            //lookAtTarget = initialTarget;
            if (Time.time > timeToLookAtTarget)
            {
                if (!lookingAtRandomTarget)
                {
                    /*if (follow)
                    {
                        randomTargetObject.transform.position = lookAtTarget.transform.position + new Vector3(Random.Range(-.20f, .2f), Random.Range(-.2f, .1f), Random.Range(0f, .2f));
                    }
                    else*/ randomTargetObject.transform.position = lookAtTarget.transform.position + new Vector3(Random.Range(-.7f, .7f), Random.Range(-.7f, .7f), Random.Range(0f, .7f));
                    lookAtTarget = randomTargetObject.transform;
                    lookingAtRandomTarget = true;
                }
                else
                {
                    lookAtTarget = initialTarget;
                    lookingAtRandomTarget = false;
                }
                timeToLookAtTarget = Time.time + Random.Range(minLook, maxLook);
                timeToReturnToInitialTarget = Time.time + Random.Range(minReturn, maxReturn);
            }
            // Return to initial target periodically
            else if (Time.time > timeToReturnToInitialTarget)
            {
                int random = Random.Range(1, 3);
                initialTarget = targets[random].transform;
                Debug.Log(targets[0]+" "+ targets[1]+ " "+ targets[2]);
                lookAtTarget = initialTarget;

                lookingAtRandomTarget = false;
                timeToReturnToInitialTarget = Time.time + Random.Range(minReturn, maxReturn);
            }
        }
        else
        {
            lookAtTarget = initialTarget;
        }

        if (!isBlinking)
        {
            if (next)
            {
                //Changes the facial expressions
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
                next = false;
            }
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

        animator.Play(currentEmotion + "_blink");
        yield return new WaitForSeconds(0.5f);

        animator.Play(currentEmotion);
        timeToBlink = Time.time + Random.Range(0.5f, 5f);
        isBlinking = false;
    }
}
