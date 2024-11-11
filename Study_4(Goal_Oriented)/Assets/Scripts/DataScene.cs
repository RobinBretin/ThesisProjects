using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataScene : MonoBehaviour
{
    public static GameObject participant;
    void Awake()
    {
        if (participant != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        participant = this.gameObject;
        DontDestroyOnLoad(this.gameObject);
    }
}
