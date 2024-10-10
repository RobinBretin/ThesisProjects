using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statioAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    GetComponent<Animator>().keepAnimatorControllerStateOnDisable = true;
        
    }

    private void OnDisable()
    {
        GetComponent<Animator>().Play("noiseMovement", -1, 0f);
        //GetComponent<Animator>().Play("normalState",0,0f);
    }
}
