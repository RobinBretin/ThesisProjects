using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using TMPro;




public class SpeechRecog : MonoBehaviour
{
    public GameObject GameManager;
    public GameObject drone;
    public GameObject drone_;
    public GameObject[] ailes;
    public GameObject[] pivots;
    public TextMeshPro text_color;
    public string[] sequences_colors; 
    public float speedRota;
    private bool activated;
    public bool landed;
    private Animator anim;
    private Material mat;
    private bool triggerOn;
    private AudioSource audiosource;

    public AudioClip flyClip;
    public AudioClip landClip;
    public AudioClip activateClip;
    private bool playing;

    public string[] triggers_order;
    private int i;

    void OnNext ()
    { 
        //Valve.VR.OpenVR.System.ResetSeatedZeroPose();
         if(!triggerOn){
            landed=false;
            anim.SetTrigger(triggers_order[i]);
            text_color.text=sequences_colors[i].Replace("\\n", "\n");
            anim.ResetTrigger("Down");
            triggerOn=true;
            activated=true;
            
        }
        else
        {
            landed=false;
            anim.ResetTrigger(triggers_order[i]);   
            anim.SetTrigger("Down");
            if(i<2) i++;
            else i=0;
            triggerOn=false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        speedRota=0;
        playing=false;
        audiosource=GetComponent<AudioSource>();

        activated=false;
        triggers_order=GameManager.GetComponent<GameManager>().height_conditions_order;
        i=0;
        triggerOn=false;
        anim=drone.GetComponent<Animator>();
        mat=drone_.GetComponent<Renderer>().material;
    }

    void Update()
    {
        if(activated)
        {
            if (landed) speedRota=0.5f;
            else speedRota=100f;
        }

        int j=0;
        foreach (var item in ailes)
        {
            GameObject pivot=pivots[j];
            item.transform.RotateAround(pivot.transform.position, Vector3.up, 360*Time.deltaTime*10*speedRota);
            j+=1;
        }
 

        if(anim.GetCurrentAnimatorStateInfo(0).IsTag("1"))
        {
            audiosource.loop=true;
            audiosource.clip=flyClip;
            if(!playing) 
            {
                audiosource.Play();
                playing=true;
            }
            drone_.GetComponent<Animator>().SetTrigger("flying");
        }
        else drone_.GetComponent<Animator>().ResetTrigger("flying");


        if(anim.GetCurrentAnimatorStateInfo(0).IsTag("2"))
        {
            audiosource.clip=landClip;
            if(playing) 
            {
                audiosource.Play();
                audiosource.loop=false;
                playing=false;
            }
        }

    }
} 
