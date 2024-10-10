using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject RHand;
    public GameObject LHand;
    public GameObject LhandTarget;
    public GameObject RhandTarget;
    private OVRHand rightHand;
    private OVRHand leftHand;
    public GameObject playerCam;
    public SkinnedMeshRenderer playerSkin;
    
    // Start is called before the first frame update
    void Start()
    {
        rightHand=RHand.GetComponent<OVRHand>();
        leftHand=LHand.GetComponent<OVRHand>();
        
    }

    // Update is called once per frame
    void Update()
    {
       if(!rightHand.IsTracked || !leftHand.IsTracked) playerSkin.enabled=false;
       else playerSkin.enabled=true;
    }
}
