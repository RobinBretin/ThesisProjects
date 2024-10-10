using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class T_Movements : MonoBehaviour
{
    public InputAction Axis;

    void Start()
    {
        Axis.Enable();
    }

    void Update()
    {
        transform.position += new Vector3(Axis.ReadValue<Vector2>().x*Time.deltaTime, 0, Axis.ReadValue<Vector2>().y * Time.deltaTime);
    }
}
