using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ActivateController : MonoBehaviour
{
    // Start is called before the first frame update
    public float distance;
    public float minDist;
    public GameObject player;
    public GameObject leftController;
    
    void Start()
    {
        distance = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), new Vector2(transform.position.x, transform.position.z));

        leftController.GetComponent<XRRayInteractor>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), new Vector2(transform.position.x, transform.position.z));
        Debug.Log(distance);
        if (distance < minDist)
        {
            leftController.GetComponent<XRRayInteractor>().enabled=true;
        }
        else
        {

            leftController.GetComponent<XRRayInteractor>().enabled = false;
        }

    }
}
