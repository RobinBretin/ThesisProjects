using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class droneMoveAnimations : MonoBehaviour
{

    public GameObject init_position;

    //Parameters 
    public float timeBaseline;
    public float timeResting;
    public float timeTakeOff;
    public float timeInvasion;
    public string[] triggers_order;

    public GameObject hand;

    //Variables
    private float dist;
    private string phase;
    private float time;
    private int i;
    private int k;
    private bool start;
    private Animator anim;
    

    void Start()
    {
        phase="None";
        time=0;
        anim=GetComponent<Animator>();
    }

    void Update()
    {
        time+=Time.deltaTime;

        if(start){
            if(k==0){
                time=0;
                phase="Baseline";
                k=1;
            }

            if(time>=timeBaseline && i==0 && phase== "Baseline"){

            }

            if(time>=timeBaseline && i==1 && phase== "Take_Off"){
                
            }

            if(time>=timeBaseline && i==2 && phase== "Approach"){
                
            }

            if(time>=timeBaseline && i==0 && phase== "Invasion"){
                
            }

            if(time>=timeBaseline && i==0 && phase== "Return"){
                
            }

            if(time>=timeBaseline && i==0 && phase== "Resting"){
                
            }
            
        }

        
    }
}
