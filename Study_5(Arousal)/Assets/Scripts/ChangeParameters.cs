using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeParameters : MonoBehaviour
{
    public OrderID or;
    public Slider idP;
    public Slider order;
    public GameObject CanvaParameters;
    public N_Back nbackScript;
    public Save saveF;
    public PID_Drone droneScript;

    public void SetParameters()
    {
        saveF.id_participant= idP.value;
        saveF.order= order.value;
        saveF.InitSave();
        //or.id_participant = idP.value;
        //or.order = order.value;
        nbackScript.setOrder(order.value);
        droneScript.setOrder(order.value);
        CanvaParameters.SetActive(false);
    }

    
}
