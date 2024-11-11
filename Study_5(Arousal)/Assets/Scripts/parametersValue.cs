using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class parametersValue : MonoBehaviour
{
    public Slider slider;

    public void Change()
    {
        GetComponent<TextMeshProUGUI>().text = slider.value.ToString();
    }
}
