using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleRota : MonoBehaviour
{
    public float speedRota;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(Vector3.up * speedRota * Time.deltaTime);
    }
}
