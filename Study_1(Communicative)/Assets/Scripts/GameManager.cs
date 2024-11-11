using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool save;
    public string[] height_conditions_order;
    public bool social_framing;

    // Start is called before the first frame update
    void Awake()
    {
        if (!save)
        {
            GetComponent<Save>().enabled=false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (save)
        {
            GetComponent<Save>().enabled=true;
        }
        else GetComponent<Save>().enabled=false;
    }
}
