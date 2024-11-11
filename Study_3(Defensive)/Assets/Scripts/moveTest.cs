using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class moveTest : MonoBehaviour
{
    //PROXEMIC
    public GameObject[] proxemicColliders;
    public string proxemicSpace;

    //RIGIDBODY
    public GameObject droneRB;
    private Rigidbody rb;

    //SPEED
    private float[] speeds;
    public float speed;
    private float frein;

    //OTHERS
    private float dist;
    private float distToStop;
    public string phase;
    public GameObject starter;
    public bool start;
    public bool forward;
    private int i;
    private int k;
    private int j;
    private int l; //nb of returns
    private int m;//nb of proxemic personal frontier passed
    public bool ended;
    public Save save;
    private string[] animStrings;

    //AUDIO
    private AudioSource audiosource;
    public AudioClip flyClip;
    public AudioClip landClip;

    //ANIMATIONS
    public Animator anim;
    public Animator animFly;
    public Animator[] animPropellers;

    //RETURN BUTTONS
    public GameObject FButton;

    //OBJECT TARGET
    public GameObject end;
    public GameObject head;
    private float height;
    public GameObject init;
    public GameObject targetTakeoff;

    //DURATIONS
    private float duration;
    public float duration_baseline;
    public float duration_takeOff ;
    public float duration_invasion ;
    public float duration_Rperiod;
    private float duration_blueArrow;

    void Start()
    {



        animFly.keepAnimatorControllerStateOnDisable=true;
        speeds=save.speeds;

        ended=false;
        start=false;
        forward=false;
        rb=droneRB.GetComponent<Rigidbody>();
        duration_blueArrow=10;
        l=0;
        j=0;
        m=0;
        speed=speeds[j];
        phase="None";
        proxemicSpace="Public";
        audiosource= GetComponent<AudioSource>();
        duration=0;
        frein=speed*100f;
        anim=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        speed=speeds[j];
        if(speed==1) {
            frein=35f;
            distToStop=0.8f;}
        else {
            frein=10f;
            distToStop=0.3f;
            }
        Debug.Log(phase);
        Debug.Log(proxemicSpace);

        if(start && !ended){
            if(k==0){
                phase="Baseline";
                duration=0;
                k=k+1;
                height=head.transform.position.y-0.05f;
            }

            dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(end.transform.position.x, end.transform.position.z));
            duration+=Time.deltaTime;

            if((duration>=duration_baseline && i==0 && j==0) || (duration>=duration_Rperiod && i==0 && j==1)){

                phase="Take_off";
                anim.SetTrigger("Take_off");
                Flying(true);
                Audio(flyClip,true);

                if(anim.GetCurrentAnimatorStateInfo(0).IsTag("0")){
                    duration=0;
                    //anim.ResetTrigger("Take_off");
                    i=i+1;
                }

            }

            if(duration>=duration_takeOff && i==1){
                phase="Approach";
                //if(transform.position.y<height) transform.position = transform.up*Time.deltaTime;
                //if(transform.position.y>height) transform.position = -transform.up*Time.deltaTime;   

                if(dist<distToStop){
                    if(rb.velocity.z >=0){
                        rb.AddForce(-transform.forward*Time.deltaTime*frein);
                    }
                    else {
                        rb.velocity=new Vector3(0,0,0);
                        phase="Invasion";
                        i=i+1;
                        duration=0;
                    }
                } 
                else{
                    if(rb.velocity.z <= speed){ 
                        rb.AddForce(transform.forward*Time.deltaTime*25);
                        if(((droneRB.transform.position.y+0.9)-height) > 0.01f) droneRB.transform.position-= droneRB.transform.up*Time.deltaTime*0.05f;
                        if(((droneRB.transform.position.y+0.9)-height) < -0.01f) droneRB.transform.position+= droneRB.transform.up*Time.deltaTime*0.05f;
                        } 
                    else rb.velocity = new Vector3(0,0,speed);
                    }
            }

            if(duration>=duration_invasion && i==2){
                Debug.Log(l);
                FButton.SetActive(true);
                phase="Return";

                if (speed==1) animStrings= new string[5]{"Bhigh", "Bhigh", "BhighV2", "BhighV2","BhighV3"};
                else animStrings= new string[5]{"Blow", "Blow","BlowV2","BlowV2","BlowV3"};

                if(anim.GetCurrentAnimatorStateInfo(0).IsTag("0")) {
                        duration_blueArrow+=Time.deltaTime;
                        if (duration_blueArrow>=10f) FButton.GetComponent<MeshRenderer>().enabled=true;
                    }
                else {
                    FButton.GetComponent<MeshRenderer>().enabled=false;
                    duration_blueArrow=0;}

                if(forward){
                    if(anim.GetCurrentAnimatorStateInfo(0).IsTag("0")) {
                        //FButton.GetComponent<MeshRenderer>().enabled=true;
                        if(l<=4) anim.SetTrigger(animStrings[l]);
                        if(l<6)l=l+1;
                    }

                    forward=false;

                   /* if (l<2){
                        anim.SetTrigger(animStrings[0]);
                        //FButton.GetComponent<Collider>().enabled=false;
                        if(anim.GetCurrentAnimatorStateInfo(0).IsTag("0")) {
                            l=l+1;
                            //FButton.GetComponent<Collider>().enabled=true;
                        }
                    }
                    else{
                        if(l<4){
                            anim.SetTrigger(animStrings[1]);
                           // FButton.GetComponent<Collider>().enabled=false;
                            if(anim.GetCurrentAnimatorStateInfo(0).IsTag("0")) {
                            l=l+1;
                           // FButton.GetComponent<Collider>().enabled=true;
                        }
                        }
                        else{
                            if(l==4){
                                anim.SetTrigger(animStrings[2]);
                              //  FButton.GetComponent<Collider>().enabled=false;
                                if(anim.GetCurrentAnimatorStateInfo(0).IsTag("0")) {
                                    l=l+1;
                                  // FButton.GetComponent<Collider>().enabled=true;
                        }
                            }
                            else{
                                l=l+1;
                            }

                        }
                    }*/
                    
                }

                if(l==6 && anim.GetCurrentAnimatorStateInfo(0).IsTag("0")){
                    if(droneRB.transform.position.y < -0.001f) droneRB.transform.position+= droneRB.transform.up*Time.deltaTime*0.05f;
                    else{
                        if(droneRB.transform.position.y > 0.001f) droneRB.transform.position-= droneRB.transform.up*Time.deltaTime*0.05f;
                    }
                    if(droneRB.transform.position.y > -0.001f && droneRB.transform.position.y < 0.001f) l=l+1;
                        }

                if(l==7 && anim.GetCurrentAnimatorStateInfo(0).IsTag("0")) {
                    FButton.SetActive(false);
                    Approach(init,speed);
                }    
            }
            if(j==2) ended = true;
        }
    }

    void Approach(GameObject target, float speed)
    {
        if((((droneRB.transform.position.z)-(0.42+0.42+1.20+1.20+1.50)*(j+1))-target.transform.position.z) < 0.1f){
        //if((transform.position.z-target.transform.position.z ) < 0.1f){
            anim.SetTrigger("Land");
            Audio(landClip,false);

            /*audiosource.loop=false;
            audiosource.clip=landClip;
            audiosource.Play();*/

            if(anim.GetCurrentAnimatorStateInfo(0).IsTag("0")){
                    duration=0;
                    i=0;
                    j=j+1;
                    l=0;
                    m=0;
                    Flying(false);
                    forward=false;
                    phase="Resting";
                }
        }
        else droneRB.transform.position = Vector3.MoveTowards(droneRB.transform.position, target.transform.position, Time.deltaTime*0.4f);
    }

    void Flying(bool F){
        foreach (Animator animProp in animPropellers)
                {

                    animProp.enabled=F;
                    animProp.SetBool("Flying 0",F);
                    
                }

                if(!F) animFly.Play("noiseMovement", -1, 0f);
                animFly.enabled=F;
                animFly.SetBool("Flying",F);
                

    }

    void Audio(AudioClip clip, bool loop){
        audiosource.loop=loop;
        audiosource.clip=clip;
        audiosource.Play();
    }


    void OnTriggerEnter(Collider other) {
        if(other.gameObject==proxemicColliders[1]) m=m+1;   
    }

    void OnTriggerStay(Collider other) {
        if(other.gameObject==proxemicColliders[2]) proxemicSpace="Intimate";
        if(other.gameObject==proxemicColliders[1]) proxemicSpace="Personal";
        if(other.gameObject==proxemicColliders[0]) proxemicSpace="Social";

    }

    void OnTriggerExit(Collider other) {
        if(other.gameObject==proxemicColliders[2]) proxemicSpace="Personal";
        if(other.gameObject==proxemicColliders[1] && m==2) proxemicSpace="Social";
        if(other.gameObject==proxemicColliders[0] && m==2) proxemicSpace="Public";

    }
}


/* if(l<2) {
                        if(speed==1) anim.SetTrigger("Bhigh");
                        else anim.SetTrigger("Blow");
                        l=l+1;
                    }
                    else{
                        if(l<4) {
                        if(speed==1) anim.SetTrigger("BhighV2");
                        else anim.SetTrigger("BlowV2");
                        l=l+1;
                        }
                        else {
                            if(l==5){
                                if(speed==1) anim.SetTrigger("BhighV3");
                                else anim.SetTrigger("BlowV3");
                                l=l+1;
                            }
                            else {
                                if(l==6){
                                if(droneRB.transform.position.y < -0.01f) droneRB.transform.position+= droneRB.transform.up*Time.deltaTime*0.05f;
                                else {
                                 if(droneRB.transform.position.y > 0.01f) droneRB.transform.position-= droneRB.transform.up*Time.deltaTime*0.05f;
                                 else l=l+1;
                                }
                            }   
                        }

                        }
                        

                    }*/