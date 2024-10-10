using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHand_interaction : MonoBehaviour
{
    public GameObject startArea;
    public GameObject forward;

    public GameObject drone;

    public GameObject Listener;

    private AudioSource audiosource;
    public AudioClip startTimer;
    public Animator anim;

    private bool playing;
    private float durationInside;
    private float decompte;
    private float decompte2;
    private bool startingD;
    public bool starting;
    public bool forwardB;


    // Start is called before the first frame update
    void Start()
    {
        durationInside=0;
        decompte=0;
        forwardB=false; 
    }

    void OnTriggerEnter(Collider other){
        durationInside=0;
    }

    void OnTriggerStay(Collider other) {
        
        if(other.gameObject==startArea){
            durationInside+=Time.deltaTime;
            if(durationInside>=3){
                if(!playing) 
            {
                Listener.GetComponent<AudioSource>().clip=startTimer;
                Listener.GetComponent<AudioSource>().Play();
                startingD=true;
                playing=true;
                startArea.SetActive(false);
                durationInside=0;
            }
                
            }   
        }

        if(other.gameObject==forward){
            durationInside+=Time.deltaTime;
            if(!drone.GetComponent<moveTest>().anim.GetCurrentAnimatorStateInfo(0).IsTag("0")) durationInside=0;
            if(durationInside>=3){
                drone.GetComponent<moveTest>().forward=true;
                durationInside=0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(startingD){
            decompte+=Time.deltaTime;
            if(decompte>=3) drone.GetComponent<moveTest>().start=true;;
        }
        
    }
}
