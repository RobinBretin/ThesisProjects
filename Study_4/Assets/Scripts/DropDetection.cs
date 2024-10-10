using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDetection : MonoBehaviour
{
    public GameObject enteredObject;
    private void OnTriggerEnter(Collider other)
    {
        enteredObject = other.gameObject;


    }
}
